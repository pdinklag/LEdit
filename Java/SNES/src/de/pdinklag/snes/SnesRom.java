package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.ByteBuffer;

import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.io.Serializable;

/**
 * Base class for SNES rom loaders.
 */
public class SnesRom implements Serializable {
    private static final long serialVersionUID = -5541709373986405124L;

    private static enum Layout {
        LOROM_HEADERLESS(0x7FC0, 0),
        LOROM(0x81C0, 0x200),
        HIROM_HEADERLESS(0xFFC0, 0),
        HIROM(0x101C0, 0x200);

        private final int snesHeaderAddress, romHeaderSize;

        private Layout(int snesHeaderAddress, int romHeaderSize) {
            this.snesHeaderAddress = snesHeaderAddress;
            this.romHeaderSize = romHeaderSize;
        }
    }

    /**
     * File extension for Super MagiCom ROM files.
     */
    public static final String FILE_EXT_SMC = ".smc";

    private static boolean isASCII(byte[] bytes) {
        for (byte b : bytes) {
            if (b < 0x20)
                return false;
        }
        return true;
    }

    /**
     * Calculates the SNES ROM checksum.
     *
     * @param romBuffer  The full ROM image wrapped into a {@link ByteBuffer}.
     * @param headerSize The size of the ROM header, which is not taken into account for checksum calculation.
     * @return The calculated checksum for the SNES ROM.
     * @throws java.io.IOException In case an I/O error occurs on the buffer.
     */
    public static int calculateChecksum(ByteBuffer romBuffer, int headerSize) throws IOException {
        int bodySize = romBuffer.getSize() - headerSize;
        int checksum = 0;

        romBuffer.setPosition(headerSize);

        int chunkSize;
        if (bodySize < 0x100000)
            chunkSize = 0x80000; //4 MBit
        else if (bodySize < 0x200000)
            chunkSize = 0x100000; //8 MBit
        else if (bodySize < 0x400000)
            chunkSize = 0x200000; //16 MBit
        else
            chunkSize = 0x400000; //32 MBit

        BinaryInputStream in = new BinaryInputStream(romBuffer.in);

        int left = bodySize;
        while (left > 0) {
            long chunkChecksum = 0;
            for (int i = 0; i < Math.min(left, chunkSize); i++)
                chunkChecksum += in.read();

            while (left < chunkSize) {
                left <<= 1;
                chunkChecksum <<= 1;
            }

            left -= chunkSize;

            checksum = (int) ((checksum + chunkChecksum) & 0xFFFF);
        }

        return checksum;
    }

    protected transient File romFile;

    protected long romHeaderSize;
    protected long romBodySize;

    private String romName;
    private int romChecksum;
    private int calculatedChecksum;

    /**
     * All addresses should be channeled through this method to ensure that the ROM file header
     * size is considered.
     *
     * @param address The address to be potentially adjusted.
     * @return The adjusted address.
     */
    public long $a(long address) {
        return address + romHeaderSize;
    }

    /**
     * Loads a ROM file.
     *
     * @param romFile The file to load.
     * @throws IOException In case an I/O error occurs.
     */
    public void load(File romFile) throws IOException {
        if (!romFile.exists())
            throw new IOException("File not found: " + romFile);

        setRomFile(romFile);

        //read ROM image
        RandomAccessFile raf = new RandomAccessFile(romFile, "r");
        long fileSize = raf.length();

        ByteBuffer romBuffer = new ByteBuffer((int) fileSize);
        byte[] bufferBytes = romBuffer.getBytes();

        raf.read(bufferBytes);
        raf.close();

        //find SNES header by checking all possible locations for the ASCII name
        BinaryInputStream in = new BinaryInputStream(romBuffer.in);
        byte[] nameBytes = new byte[0x15];

        for (Layout layout : Layout.values()) {
            romBuffer.setPosition(layout.snesHeaderAddress);

            in.read(nameBytes);
            if (isASCII(nameBytes)) {
                //header found
                romName = new String(nameBytes);

                romBuffer.setPosition(layout.snesHeaderAddress + 0x1E);
                romChecksum = in.readUnsignedShort();

                romHeaderSize = layout.romHeaderSize;
                break;
            }
        }

        romBodySize = fileSize - romHeaderSize;

        //calculate actual ROM checksum
        calculatedChecksum = calculateChecksum(romBuffer, (int) romHeaderSize);
    }

    public File getRomFile() {
        return romFile;
    }

    public void setRomFile(File romFile) {
        this.romFile = romFile;
    }

    /**
     * Gets the ROM file header size in bytes.
     * <p/>
     * Note that this is not to be confused with the size of the SNES header containing
     * information about the ROM, which is always 0x40 bytes.
     * <p/>
     * The only valid values for the ROM file header size are 0 or 512.
     *
     * @return The size of the ROM file header.
     */
    public long getRomHeaderSize() {
        return romHeaderSize;
    }

    /**
     * Gets the ROM body size in bytes.
     * <p/>
     * Note that this is not to be confused with the ROM size as determined in the SNES header,
     * but is the actual size of the body as found in the image file.
     *
     * @return The size of the ROM body.
     */
    public long getRomBodySize() {
        return romBodySize;
    }

    /**
     * Gets the name of the ROM.
     * <p/>
     * This is the actual name of the ROM as described in the SNES header and should be
     * 21 ASCII characters long.
     *
     * @return The name of the ROM.
     */
    public String getRomName() {
        return romName;
    }

    /**
     * Gets the checksum of the ROM claimed by the SNES header.
     * <p/>
     * Note that this might not be the actual checksum of the ROM.
     *
     * @return The ROM checksum claimed by the SNES header.
     * @see de.pdinklag.snes.SnesRom#getCalculatedChecksum()
     */
    public int getRomChecksum() {
        return romChecksum;
    }

    /**
     * Gets the actual calculated checksum of the ROM.
     * <p/>
     * This can be used to verify the integrity of the ROM image by comparing against
     * the checksum saved in the SNES header.
     *
     * @return The actual, calculated checksum of the ROM.
     * @see de.pdinklag.snes.SnesRom#getRomChecksum
     */
    public int getCalculatedChecksum() {
        return calculatedChecksum;
    }
}

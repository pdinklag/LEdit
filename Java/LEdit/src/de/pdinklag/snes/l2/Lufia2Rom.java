package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.snes.SnesRom;
import de.pdinklag.util.Hex;

import java.io.*;
import java.nio.channels.FileChannel;
import java.util.ArrayList;
import java.util.logging.Logger;

/**
 * Base class for all Lufia 2 ROMs.
 */
public abstract class Lufia2Rom extends SnesRom {
    private static final long serialVersionUID = 2395639887686301635L;
    private static final Logger logger = Logger.getLogger("l2.Lufia2Rom");

    public static final String L2_FILE_EXT = ".bin";

    public static enum Version {
        GERMAN(Lufia2RomGerman.class, 0xBC15),
        ENGLISH(Lufia2RomEnglish.class, 0x349D);

        private final Class<? extends Lufia2Rom> romClass;
        private final int checksum;

        private Version(Class<? extends Lufia2Rom> romClass, int checksum) {
            this.romClass = romClass;
            this.checksum = checksum;
        }
    }

    /**
     * Attempts to detect the ROM version.
     *
     * @param file The file to check.
     * @return The respective ROM loader class, or <tt>null</tt> if the version could not be detected.
     * @throws java.io.IOException In case an I/O error occurs.
     */
    public static Class<? extends Lufia2Rom> detectVersion(File file) throws IOException {
        SnesRom rom = new SnesRom();
        rom.load(file);

        for (Version v : Version.values()) {
            if (v.checksum == rom.getCalculatedChecksum())
                return v.romClass;
        }

        return null;
    }

    public abstract long getFileTableAddress();

    public int getFileCount() {
        return 0x2A8;
    }

    public abstract long getMonsterSpriteDimensionAddress();

    public int getMonsterSpriteFileIdOffset() {
        return 0x1B8;
    }

    public int getMonsterSpriteCount() {
        return 0x86;
    }

    public abstract long getMonsterTableAddress();

    public int getMonsterDataSize() {
        return 0x49A9;
    }

    public int getMonsterCount() {
        return 0xE0;
    }

    public abstract long getItemTableAddress();

    public abstract long getItemNamesAddress();

    public int getItemCount() {
        return 0x1D3;
    }

    private transient File filesDir = new File("files");

    private final ArrayList<MonsterSprite> monsterSprites = new ArrayList<MonsterSprite>();
    private final ArrayList<Monster> monsters = new ArrayList<Monster>();
    private final ArrayList<Item> items = new ArrayList<Item>();

    @Override
    public void load(File romFile) throws IOException {
        super.load(romFile);

        //Extract files
        for (int i = 0; i < getFileCount(); i++) {
            File file = getFile(i);
            if (!file.exists()) {
                try {
                    byte[] data = extractFile(i);
                    FileOutputStream out = new FileOutputStream(file);
                    out.write(data);
                    out.close();
                } catch (IOException ex) {
                    ex.printStackTrace();
                }
            }
        }

        try {
            FileInputStream fis = new FileInputStream(romFile);
            FileChannel channel = fis.getChannel();
            BinaryInputStream rom = new BinaryInputStream(fis);

            //Load MonsterSprites
            channel.position($a(getMonsterSpriteDimensionAddress()));
            for (int i = 0; i < getMonsterSpriteCount(); i++) {
                //dimensions
                MonsterSprite monsterSprite = new MonsterSprite();
                monsterSprite.readDimensions(rom);

                //file
                try {
                    FileInputStream in = new FileInputStream(
                            getFile(getMonsterSpriteFileIdOffset() + i));

                    monsterSprite.readSnes(new BinaryInputStream(in));
                    in.close();
                } catch (IOException ex) {
                    ex.printStackTrace();
                }

                //add
                monsterSprites.add(monsterSprite);
            }

            //Load Monsters
            for (int i = 0; i < getMonsterCount(); i++) {
                channel.position($a(getMonsterTableAddress()) + 2 * i);
                int offset = rom.readUnsignedShort();
                channel.position($a(getMonsterTableAddress()) + offset);

                Monster monster = new Monster();
                monster.readSnes(rom);
                monsters.add(monster);
            }

            //Load Items
            for (int i = 0; i < getItemCount(); i++) {
                Item item = new Item();

                channel.position($a(getItemNamesAddress()) + 12 * i);
                item.getName().readSnes(rom);

                channel.position($a(getItemTableAddress()) + 2 * i);
                int offset = rom.readUnsignedShort();
                channel.position($a(getItemTableAddress()) + offset);

                item.readSnes(rom);
                items.add(item);
            }

            fis.close();

        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    public void setFilesDir(File filesDir) {
        this.filesDir = filesDir;
    }

    private File getFile(int i) {
        return new File(filesDir, i + L2_FILE_EXT);
    }

    public MonsterSprite getMonsterSprite(int i) {
        return monsterSprites.get(i);
    }

    public Monster getMonster(int i) {
        return monsters.get(i);
    }

    public Item getItem(int i) {
        return items.get(i);
    }

    private static int signedShort(int u) {
        if ((u & 0x8000) == 0)
            return (short) (u & 0x7FFF);
        else
            return (short) (-(((~u) + 1) & 0xFFFF));
    }

    /**
     * Extracts a Lufia 2 compressed romFile.
     *
     * @param id               The romFile ID.
     * @param fileTableAddress The address of the romFile table.
     * @return An array of bytes containing the uncompressed romFile.
     * @throws IOException In case an I/O error occurs.
     */
    public byte[] extractFile(int id, long fileTableAddress) throws IOException {
        byte[] file;
        FileInputStream romFis = new FileInputStream(this.romFile);
        BinaryInputStream rom = new BinaryInputStream(romFis);

        //get romFile address
        romFis.getChannel().position(fileTableAddress + id * 3);
        romFis.getChannel().position($a(getFileTableAddress()) +
                rom.readUnsignedShort() + (rom.readByte() << 16)); //offset from original table

        //read romFile size
        int fileSize = rom.readUnsignedShort();
        int filePointer = 0;
        int fileWritten = 0;

        file = new byte[fileSize];

        logger.info("Extracting file 0x " + Hex.format(id, 3) +
                " (" + fileSize + " bytes) ...");

        //decompress and read romFile
        int x65 = rom.readByte();
        int x66 = 0x8;

        int b;
        int L, copy, copyOffset, copyAmount;

        while (fileWritten < fileSize) {
            b = rom.readByte();
            if ((b & 0x80) == 0) {
                file[filePointer++] = (byte) b;
                ++fileWritten;
            } else {
                L = x65;
                x65 = (byte) ((x65 << 1) & 0xFF);

                if ((L & 0x80) == 0) {
                    file[filePointer++] = (byte) b;
                    ++fileWritten;
                } else {
                    L = (b << 8) & 0xFFFF;
                    b = rom.readByte();

                    L |= b;
                    b &= 0xF;

                    if (b == 0) {
                        b = rom.readByte();

                        int x5A = (b << 8) & 0xFFFF;

                        copyAmount = (b & 0x3F) + 2;

                        copyOffset = (((((L >> 4) | 0xF000) << 1) | ((x5A & 0x8000) >> 15)) & 0xFFFF);
                        x5A = ((x5A << 1) & 0xFFFF);
                        copyOffset = (((copyOffset << 1) | ((x5A & 0x8000) >> 15)) & 0xFFFF);
                        //x5A = ((x5A << 1) & 0xFFFF); //not sure why
                    } else {
                        copyAmount = b + 1;
                        copyOffset = ((L >> 4) | 0xF000) & 0xFFFF;
                    }

                    copy = filePointer + signedShort(copyOffset);

                    while (copyAmount >= 0 && fileWritten < fileSize) {
                        b = file[copy++];
                        file[filePointer++] = (byte) b;

                        ++fileWritten;
                        --copyAmount;
                    }
                }

                if (--x66 == 0) {
                    x65 = rom.readByte();
                    x66 = 0x8;
                }
            }
        }

        rom.close();
        return file;
    }

    /**
     * Extracts a Lufia 2 compressed romFile using the original romFile table.
     *
     * @param id The romFile ID.
     * @return An array of bytes containing the uncompressed romFile.
     * @throws IOException In case an I/O error occurs.
     */
    public byte[] extractFile(int id) throws IOException {
        return extractFile(id, $a(getFileTableAddress()));
    }
}

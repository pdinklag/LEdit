package de.pdinklag.ledit;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.io.ByteBuffer;
import de.pdinklag.io.FileUtils;
import de.pdinklag.snes.SnesRom;
import de.pdinklag.snes.l2.Lufia2Rom;

import java.io.*;
import java.util.logging.Logger;
import java.util.zip.GZIPInputStream;
import java.util.zip.GZIPOutputStream;

public class LEdit implements Serializable {
    private static final long serialVersionUID = -1100366388450228241L;
    private static final Logger logger = Logger.getLogger("ledit");

    public static final int PROJECT_FILE_VERSION = 1;

    public static final int LPATCH_ROM_SIZE = 0x800000; //ExLoROM without amy header

    public static final int LPATCH_ADDRESS_CODE_FILEREADER = 0x300000;
    public static final int LPATCH_ADDRESS_TABLE_FILES = 0x300080;
    public static final int LPATCH_ADDRESS_TABLE_MONSTERS = 0x408000;
    public static final int LPATCH_ADDRESS_FILES = 0x440000;

    public static final byte[] LPATCH_ROM_SIGNATURE = "ledit".getBytes();
    public static final byte[] LPATCH_FILE_SIGNATURE = "unp".getBytes();

    public static final String DIR_FILES = "files";
    public static final String FILE_SOURCE = "l2.rom";

    public static final String PROJECT_FILE_EXT = ".l2project";

    public static LEdit create(File romFile, File projectDir)
            throws IOException, IllegalAccessException, InstantiationException {

        Class<? extends Lufia2Rom> version = Lufia2Rom.detectVersion(romFile);
        if (version == null)
            throw new IOException("Unsupported ROM");

        File filesDir = new File(projectDir, DIR_FILES);
        filesDir.mkdirs();

        File sourceFile = new File(filesDir, FILE_SOURCE);
        FileUtils.copyFile(romFile, sourceFile);

        Lufia2Rom rom = version.newInstance();
        rom.setFilesDir(new File(projectDir, DIR_FILES));
        rom.load(sourceFile);

        LEdit app = new LEdit(rom);
        app.projectDir = projectDir;
        app.projectFileName = projectDir.getName() + PROJECT_FILE_EXT;

        app.save();

        return app;
    }

    public static LEdit load(File file) throws ClassNotFoundException, IOException {
        ObjectInputStream in = new ObjectInputStream(new GZIPInputStream(new FileInputStream(file)));
        LEdit app = (LEdit) in.readObject();
        in.close();

        app.projectDir = file.getParentFile();
        app.projectFileName = file.getName();
        app.rom.setRomFile(new File(new File(app.projectDir, DIR_FILES), FILE_SOURCE));

        return app;
    }

    private final Lufia2Rom rom;
    private int projectFileVersion = PROJECT_FILE_VERSION;

    private transient File projectDir;
    private transient String projectFileName;

    public LEdit(Lufia2Rom rom) {
        this.rom = rom;
    }

    private int $a(int address) {
        return address; //no header, but maybe in the future
    }

    private int $a(long address) {
        return $a((int) address);
    }

    public void save() throws IOException {
        save(new File(projectDir, projectFileName));
    }

    public void save(File outFile) throws IOException {
        ObjectOutputStream out = new ObjectOutputStream(new GZIPOutputStream(new FileOutputStream(outFile)));
        out.writeObject(this);
        out.close();
    }

    public Lufia2Rom getRom() {
        return rom;
    }

    private static int writeFile(BinaryOutputStream out, byte[] file) throws IOException {
        out.write(LPATCH_FILE_SIGNATURE); //signature for unpacked files, recognized by code hack
        out.writeShort(file.length);
        out.writeByte(file.length >> 16);
        out.write(file);

        return file.length + LPATCH_FILE_SIGNATURE.length + 3;
    }

    public void createRom(File outFile) throws IOException {
        File srcFile = rom.getRomFile();
        if (!srcFile.exists())
            throw new IOException("Original ROM file does no longer exist: " + srcFile);

        ByteBuffer outBuffer = new ByteBuffer(LPATCH_ROM_SIZE);
        BinaryOutputStream out = new BinaryOutputStream(outBuffer.out);
        BinaryInputStream in = new BinaryInputStream(outBuffer.in);

        byte[] buffer;
        int filesOffset = $a(LPATCH_ADDRESS_FILES);

        //copy original ROM body
        {
            RandomAccessFile src = new RandomAccessFile(srcFile, "r");
            buffer = new byte[(int) rom.getRomBodySize()];

            src.seek(rom.$a(0));
            src.read(buffer);

            outBuffer.setPosition($a(0));
            out.write(buffer);

            src.close();
        }

        //set ExLoROM flag
        {
            outBuffer.setPosition($a(0x7FD7));
            out.writeByte(0x0D);
        }

        //copy SNES header for ExLoROM
        {
            buffer = new byte[0x8000];

            outBuffer.setPosition($a(0));
            in.read(buffer);

            outBuffer.setPosition($a(0x400000));
            out.write(buffer);
        }

        //file reader hack
        {
            //hook
            outBuffer.setPosition($a(0xECF));
            out.writeInt(0xE080005C);
            outBuffer.setPosition($a(0xF52));
            out.writeInt(0xE0802C5C);
            outBuffer.setPosition($a(0xFF6));
            out.writeInt(0xE080485C);
        }
        {
            //code
            outBuffer.setPosition($a(LPATCH_ADDRESS_CODE_FILEREADER));
            out.writeLong(0xC818D075C95DB75AL);
            out.writeLong(0xB7C811D06EC95DB7L);
            out.writeLong(0x8501A90AD070C95DL);
            out.writeLong(0x640380C8C8C87A5CL);
            out.writeLong(0xD35C58855DB77A5CL);
            out.writeLong(0x000000000000808EL);
            outBuffer.setPosition($a(LPATCH_ADDRESS_CODE_FILEREADER + 0x2C));
            out.writeLong(0x6506680DD05CA548L);
            out.writeLong(0x225C808F565C0490L);
            out.writeLong(0x00808F105C68808FL);
            outBuffer.setPosition($a(LPATCH_ADDRESS_CODE_FILEREADER + 0x48));
            out.writeLong(0x6506680DD05CA548L);
            out.writeLong(0xC65C808FFA5C0490L);
            out.writeLong(0x00808FB45C68808FL);
        }

        //move file table
        {
            //code hack
            outBuffer.setPosition($a(0xEBC));
            out.writeInt(0x09E00080);
            outBuffer.setPosition($a(0xEC4));
            out.writeInt(0x0AE00081);
        }
        {
            //copy original
            buffer = new byte[0x7FE];

            outBuffer.setPosition($a(rom.getFileTableAddress()));
            in.read(buffer);

            outBuffer.setPosition($a(LPATCH_ADDRESS_TABLE_FILES));
            out.write(buffer);
        }
        {
            //TODO patch addresses
        }

        //move monster table
        {
            //code hack
            outBuffer.setPosition($a(0xFC00));
            out.writeInt(0x018000BF);
            outBuffer.setPosition($a(0xFC05));
            out.writeInt(0x01800FBF);
            outBuffer.setPosition($a(0xFC17));
            out.writeInt(0x018000BF);
            outBuffer.setPosition($a(0xFC1E));
            out.writeInt(0x01801DBF);
            outBuffer.setPosition($a(0xFC25));
            out.writeInt(0x01801EBF);
            outBuffer.setPosition($a(0xFC2C));
            out.writeInt(0x01801FBF);
            outBuffer.setPosition($a(0xFC33));
            out.writeInt(0x018020BF);
            outBuffer.setPosition($a(0xFC4B));
            out.writeByte(0x01);
            outBuffer.setPosition($a(0xFC63));
            out.writeShort(0x8000);
            outBuffer.setPosition($a(0xFC67));
            out.writeShort(0x8000);
            outBuffer.setPosition($a(0xFD1F));
            out.writeByte(0x01);
            outBuffer.setPosition($a(0xFD25));
            out.writeShort(0x8000);
            outBuffer.setPosition($a(0xFD29));
            out.writeShort(0x8000);
            outBuffer.setPosition($a(0xB20F));
            out.writeByte(0x01);
        }
        {
            //write monster data
            int[] monsterOffset = new int[rom.getMonsterCount()];

            outBuffer.setPosition($a(LPATCH_ADDRESS_TABLE_MONSTERS + 2 * monsterOffset.length));
            for (int i = 0; i < monsterOffset.length; i++) {
                monsterOffset[i] = outBuffer.getPosition() - $a(LPATCH_ADDRESS_TABLE_MONSTERS);
                rom.getMonster(i).writeSnes(out);
            }

            outBuffer.setPosition($a(LPATCH_ADDRESS_TABLE_MONSTERS));
            for (int offset : monsterOffset) {
                out.writeShort(offset);
            }
        }

        //write monster sprites
        {
            //write monster sprite files
            int[] msFileOffset = new int[rom.getMonsterSpriteCount()];

            outBuffer.setPosition($a(filesOffset));
            for (int i = 0; i < msFileOffset.length; i++) {
                msFileOffset[i] = outBuffer.getPosition() - $a(rom.getFileTableAddress());

                ByteArrayOutputStream msBuffer = new ByteArrayOutputStream();
                rom.getMonsterSprite(i).writeSnes(new BinaryOutputStream(msBuffer));

                filesOffset += writeFile(out, msBuffer.toByteArray());
            }

            //update file table
            outBuffer.setPosition($a(LPATCH_ADDRESS_TABLE_FILES + 3 * rom.getMonsterSpriteFileIdOffset()));
            for (int i = 0; i < msFileOffset.length; i++) {
                out.writeShort(msFileOffset[i]);
                out.writeByte(msFileOffset[i] >> 16);
            }
        }
        {
            //write dimensions
            outBuffer.setPosition($a(rom.getMonsterSpriteDimensionAddress()));
            for (int i = 0; i < rom.getMonsterSpriteCount(); i++) {
                rom.getMonsterSprite(i).writeDimensions(out);
            }
        }
        
        //write ROM signature
        {
            outBuffer.setPosition(outBuffer.getSize() - LPATCH_ROM_SIGNATURE.length);
            out.write(LPATCH_ROM_SIGNATURE);
        }

        //write new checksum
        {
            int chk = SnesRom.calculateChecksum(outBuffer, $a(0));
            
            outBuffer.setPosition($a(0x7FDC));
            out.writeShort(~chk);
            out.writeShort(chk);
            
            outBuffer.setPosition($a(0x407FDC));
            out.writeShort(~chk);
            out.writeShort(chk);
        }

        //write file
        FileOutputStream fileOutput = new FileOutputStream(outFile);
        fileOutput.write(outBuffer.getBytes());
        fileOutput.close();
    }

    private void readObject(ObjectInputStream ois) throws ClassNotFoundException, IOException {
        ois.defaultReadObject();

        if (projectFileVersion < PROJECT_FILE_VERSION) {
            logger.info("Project file version is outdated!");
        }
    }
}

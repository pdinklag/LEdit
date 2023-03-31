package de.pdinklag.snes.l2;

import java.io.File;
import java.io.IOException;

/**
 * German Lufia 2 ROM addresses.
 */
public class Lufia2RomGerman extends Lufia2Rom {
    private static final long serialVersionUID = -4972658681909108863L;

    public static void main(String[] args) throws IOException {
        Lufia2Rom rom = new Lufia2RomGerman();
        rom.load(new File("L2-G.smc"));
    }

    @Override
    public long getFileTableAddress() {
        return 0x1B8000;
    }

    @Override
    public long getMonsterSpriteDimensionAddress() {
        return 0x1AD140;
    }

    @Override
    public long getMonsterTableAddress() {
        return 0x1A05C0;
    }

    @Override
    public long getItemTableAddress() {
        return 0x1A4F69;
    }

    @Override
    public long getItemNamesAddress() {
        return 0xF5ED5;
    }
}

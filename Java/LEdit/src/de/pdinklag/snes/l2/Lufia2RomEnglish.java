package de.pdinklag.snes.l2;

import java.io.File;
import java.io.IOException;

public class Lufia2RomEnglish extends Lufia2Rom {
    private static final long serialVersionUID = -5198003729879648135L;

    public static void main(String[] args) throws IOException {
        Lufia2Rom rom = new Lufia2RomEnglish();
        rom.load(new File("L2-E.smc"));
    }

    @Override
    public long getFileTableAddress() {
        return 0x138000;
    }

    @Override
    public long getMonsterSpriteDimensionAddress() {
        return 0xBCAAA;
    }

    @Override
    public long getMonsterTableAddress() {
        return 0xB05C0;
    }

    @Override
    public long getItemTableAddress() {
        return 0xB4F69;
    }

    @Override
    public long getItemNamesAddress() {
        return 0xF5DD3;
    }
}

package de.pdinklag.ledit.gui;

import de.pdinklag.snes.l2.Lufia2Rom;
import de.pdinklag.util.Hex;

public class MonsterSpriteComboBox extends L2ComboBox {
    private static final long serialVersionUID = 648574910689032996L;

    @Override
    protected int getRomItemCount(Lufia2Rom rom) {
        return rom.getMonsterSpriteCount();
    }

    @Override
    protected String getRomItemName(Lufia2Rom rom, int i) {
        return "[0x" + Hex.formatByte(i) + "]"; //TODO database
    }
}

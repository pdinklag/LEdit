package de.pdinklag.ledit.gui;

import de.pdinklag.snes.l2.Lufia2Rom;
import de.pdinklag.util.Hex;

public class ItemComboBox extends L2ComboBox {
    private static final long serialVersionUID = -2728942815056698714L;

    public ItemComboBox() {
        super();
    }

    public ItemComboBox(boolean hasNull) {
        super(hasNull);
    }

    @Override
    protected int getRomItemCount(Lufia2Rom rom) {
        return rom.getItemCount();
    }

    @Override
    protected String getRomItemName(Lufia2Rom rom, int i) {
        return "[0x" + Hex.format(i, 3) + "] " + rom.getItem(i).getName().getValue().trim();
    }
}

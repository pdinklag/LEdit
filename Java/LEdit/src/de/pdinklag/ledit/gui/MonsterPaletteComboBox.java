package de.pdinklag.ledit.gui;

import de.pdinklag.snes.l2.Lufia2Rom;
import de.pdinklag.snes.l2.MonsterSprite;
import de.pdinklag.util.Localizer;

public class MonsterPaletteComboBox extends L2ComboBox {
    private static final long serialVersionUID = -5838154898459604130L;

    private transient MonsterSprite sprite;

    public void initialize(Lufia2Rom rom) {
        throw new UnsupportedOperationException("Use initialize(Lufia2Rom, MonsterSprite)!");
    }

    public void initialize(MonsterSprite sprite) {
        this.sprite = sprite;
        super.initialize(null);
    }

    @Override
    protected int getRomItemCount(Lufia2Rom rom) {
        return sprite.getPaletteCount();
    }

    @Override
    protected String getRomItemName(Lufia2Rom rom, int i) {
        return Localizer.localize("monster.display.palette") + " " + (i + 1);
    }
}

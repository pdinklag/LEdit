package de.pdinklag.ledit.gui;

import de.pdinklag.snes.l2.Lufia2Rom;
import de.pdinklag.util.Localizer;

import javax.swing.*;

public abstract class L2ComboBox extends JComboBox {
    private static final long serialVersionUID = -2728942815056698714L;
    private static final int ITEM_NONE = -1; //selected item "None"
    private static final int ITEM_INVALID = -2; //no selected item id

    private final boolean hasNull;

    public L2ComboBox() {
        this(false);
    }

    public L2ComboBox(boolean hasNull) {
        super(new DefaultComboBoxModel());
        this.hasNull = hasNull;
    }

    public void initialize(Lufia2Rom rom) {
        DefaultComboBoxModel model = (DefaultComboBoxModel) getModel();
        model.removeAllElements();

        if (hasNull)
            model.addElement(Localizer.localize("ui.none"));

        for (int i = 0; i < getRomItemCount(rom); i++) {
            model.addElement(getRomItemName(rom, i));
        }

        setEditable(false);
    }

    protected abstract int getRomItemCount(Lufia2Rom rom);

    protected abstract String getRomItemName(Lufia2Rom rom, int i);

    public int getSelectedItemId() {
        int x = getSelectedIndex();

        if (x == -1) {
            return ITEM_INVALID;
        } else {
            return x - (hasNull ? 1 : 0);
        }
    }

    public void setSelectedItemId(int id) {
        if (id == ITEM_INVALID) {
            setSelectedIndex(-1);
        } else if (id == ITEM_NONE) {
            setSelectedIndex(hasNull ? 0 : -1);
        } else {
            setSelectedIndex(id + (hasNull ? 1 : 0));
        }
    }
}

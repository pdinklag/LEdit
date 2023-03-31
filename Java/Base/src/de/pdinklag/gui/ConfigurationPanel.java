package de.pdinklag.gui;

import javax.swing.*;
import java.awt.*;

/**
 * A panel designed to hold configuration field arrays assorted in two columns.
 */
public class ConfigurationPanel extends JPanel {
    private static final long serialVersionUID = -8993961492546159632L;

    /**
     * Designates an empty component spot.
     */
    public final static Component NONE = null;

    private GridBagConstraints gbc = new GridBagConstraints();

    public ConfigurationPanel(int hgap, int vgap) {
        super(new GridBagLayout());

        gbc.gridy = 0;
        gbc.anchor = GridBagConstraints.NORTHWEST;
        gbc.insets = new Insets(vgap, hgap, vgap, hgap);
    }

    public ConfigurationPanel() {
        this(0, 0);
    }

    public GridBagConstraints getGridBagConstraints() {
        return gbc;
    }

    public void setGridBagConstraints(GridBagConstraints gbc) {
        this.gbc = gbc;
    }

    /**
     * Adds a new configuration field pair.
     * <p/>
     * Use {@link ConfigurationPanel#NONE} to designate that a spot should remain empty.
     *
     * @param label The label, appearing on the left side.
     * @param value The actual editable field, appearing on the right side.
     */
    public void add(Component label, Component value) {
        if (label != null) {
            gbc.gridx = 0;
            gbc.fill = GridBagConstraints.NONE;
            gbc.weightx = 0.0;
            add(label, gbc);
        }

        if (value != null) {
            gbc.gridx = 1;
            gbc.fill = GridBagConstraints.HORIZONTAL;
            gbc.weightx = 1.0;
            add(value, gbc);
        }

        gbc.gridy++;
    }

    /**
     * Adds a bottom filler component.
     *
     * @param c The component to add as a bottom filler.
     */
    public void addBottom(Component c) {
        gbc.gridx = 0;
        gbc.gridwidth = 2;
        gbc.weighty = 1.0;
        gbc.fill = GridBagConstraints.BOTH;
        add(c, gbc);
    }

    /**
     * Adds an empty bottom filler.
     */
    public void addBottom() {
        addBottom(new JPanel());
    }
}

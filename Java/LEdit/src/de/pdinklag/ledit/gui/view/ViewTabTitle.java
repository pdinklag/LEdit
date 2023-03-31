package de.pdinklag.ledit.gui.view;

import javax.swing.*;
import java.awt.*;

public class ViewTabTitle extends JPanel {
    private static final long serialVersionUID = 6840592447667069045L;

    private final JLabel label = new JLabel();

    public ViewTabTitle() {
        super(new FlowLayout(FlowLayout.LEFT));

        setOpaque(false);
        add(label);
    }

    public ViewTabTitle(String text) {
        this();
        label.setText(text);
    }

    public void setText(String text) {
        label.setText(text);
    }

    public String getText() {
        return label.getText();
    }

    public void setIcon(Icon icon) {
        label.setIcon(icon);
    }

    public Icon getIcon() {
        return label.getIcon();
    }
}

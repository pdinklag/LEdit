package de.pdinklag.snes.gui;

import de.pdinklag.snes.Palette;

import javax.swing.*;
import java.awt.*;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.util.LinkedList;
import java.util.List;

/**
 * Simple GUI element drawing the colors of a SNES palette.
 */
public class PaletteBox extends JPanel implements MouseListener {
    private static final long serialVersionUID = -7579011811058577155L;
    private static final JTextField BORDER_REF = new JTextField();
    private static final int SELECTION_STROKE_WIDTH = 5;

    private Palette palette;

    private boolean editable = true;
    private int selectedColor = -1;
    private final JColorChooser colorChooser = new JColorChooser();

    private List<PaletteListener> listeners = new LinkedList<PaletteListener>();

    public PaletteBox() {
        super();

        addMouseListener(this);

        setPreferredSize(new Dimension(getPreferredSize().width, BORDER_REF.getPreferredSize().height));
        setBorder(BORDER_REF.getBorder());
    }

    public PaletteBox(Palette palette) {
        this();
        setPalette(palette);
    }

    public Palette getPalette() {
        return palette;
    }

    public void setPalette(Palette palette) {
        this.palette = palette;
        repaint();
    }

    public void addPaletteListener(PaletteListener l) {
        if (!listeners.contains(l))
            listeners.add(l);
    }

    public void removePaletteListener(PaletteListener l) {
        listeners.remove(l);
    }

    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);

        if (palette != null) {
            float w = (float) getWidth() / (float) Palette.NUM_COLORS;
            int h = getHeight();

            for (int i = 0; i < Palette.NUM_COLORS; i++) {
                g.setColor(palette.getColor(i));
                g.fillRect((int) ((float) i * w), 0, (int) w + 1, h);

                if (editable && selectedColor == i) {
                    //TODO highlight
                }
            }
        }
    }

    public JColorChooser getColorChooser() {
        return colorChooser;
    }

    public boolean isEditable() {
        return editable;
    }

    public void setEditable(boolean editable) {
        this.editable = editable;
    }

    public int getSelectedColor() {
        return selectedColor;
    }

    public void setSelectedColor(int selectedColor) {
        this.selectedColor = selectedColor;
        repaint();
    }

    @Override
    public void mouseClicked(MouseEvent e) {
        if (editable && selectedColor >= 0 && e.getClickCount() > 1) {
            Color old = palette.getColor(selectedColor);
            Color color = JColorChooser.showDialog(this, "", old);
            if (color != null) {
                palette.setColor(selectedColor, color);

                for (PaletteListener l : listeners)
                    l.colorChanged(this, selectedColor, old);

                repaint();
            }
        }
    }

    @Override
    public void mousePressed(MouseEvent e) {
        if (editable) {
            float w = (float) getWidth() / (float) Palette.NUM_COLORS;
            selectedColor = (int) ((float) e.getX() / w);
            repaint();
        }
    }

    @Override
    public void mouseReleased(MouseEvent e) {
    }

    @Override
    public void mouseEntered(MouseEvent e) {
    }

    @Override
    public void mouseExited(MouseEvent e) {
    }
}

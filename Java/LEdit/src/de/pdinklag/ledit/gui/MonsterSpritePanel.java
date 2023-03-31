package de.pdinklag.ledit.gui;

import de.pdinklag.snes.Palette;
import de.pdinklag.snes.l2.MonsterSprite;

import javax.swing.*;
import javax.swing.border.BevelBorder;
import java.awt.*;
import java.awt.event.MouseWheelEvent;
import java.awt.event.MouseWheelListener;
import java.awt.image.BufferedImage;

public class MonsterSpritePanel extends JPanel implements MouseWheelListener {
    private static final long serialVersionUID = -3673683433427384078L;
    private static final Color GRADIENT_COLOR1 = new Color(128, 255, 255);
    private static final Color GRADIENT_COLOR2 = new Color(64, 128, 255);

    private MonsterSprite sprite;
    private Palette palette;
    private int zoom = 1;

    public MonsterSpritePanel() {
        super();
        setBorder(new BevelBorder(BevelBorder.LOWERED));
        addMouseWheelListener(this);
    }

    public MonsterSpritePanel(MonsterSprite sprite, int palette) {
        this.sprite = sprite;
        this.palette = sprite.getPalette(palette);
    }

    public MonsterSpritePanel(MonsterSprite sprite, Palette palette) {
        this.sprite = sprite;
        this.palette = palette;
    }

    public MonsterSprite getSprite() {
        return sprite;
    }

    public void setSprite(MonsterSprite sprite) {
        this.sprite = sprite;
        repaint();
    }

    public Palette getPalette() {
        return palette;
    }

    public void setPalette(int palette) {
        this.palette = sprite.getPalette(palette);
        repaint();
    }

    public void setPalette(Palette palette) {
        this.palette = palette;
        repaint();
    }

    public int getZoom() {
        return zoom;
    }

    public void setZoom(int zoom) {
        this.zoom = Math.max(1, zoom);
        repaint();
    }

    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);

        if (isEnabled()) {
            GradientPaint gradient = new GradientPaint(0, 0, GRADIENT_COLOR1, 0, getHeight(), GRADIENT_COLOR2);
            ((Graphics2D) g).setPaint(gradient);
            g.fillRect(0, 0, getWidth(), getHeight());

            if (sprite != null) {
                BufferedImage image = sprite.getImage(palette);

                int x = (getWidth() - image.getWidth() * zoom) / 2;
                int y = (getHeight() - image.getHeight() * zoom) / 2;

                g.drawImage(image, x, y, image.getWidth() * zoom, image.getHeight() * zoom, null);
            }
        }
    }

    @Override
    public void mouseWheelMoved(MouseWheelEvent e) {
        setZoom(zoom + (e.getUnitsToScroll() > 0 ? -1 : 1));
    }
}

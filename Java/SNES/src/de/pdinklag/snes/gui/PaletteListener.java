package de.pdinklag.snes.gui;

import java.awt.*;

/**
 * Interface for palette box editing listeners.
 */
public interface PaletteListener {
    /**
     * Invoked when a color was edited.
     *
     * @param source The palette box causing the event.
     * @param color  The index of the color that was changed.
     * @param old    The old value of the color, before it was changed.
     */
    public void colorChanged(PaletteBox source, int color, Color old);
}

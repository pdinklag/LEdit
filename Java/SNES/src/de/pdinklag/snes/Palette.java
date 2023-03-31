package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;

import java.awt.*;
import java.io.IOException;
import java.io.Serializable;

/**
 * Class to work with SNES palettes of 16 colors.
 */
public class Palette implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -7276153325369706668L;
    public final static int NUM_COLORS = 16;

    private final Color[] color = new Color[NUM_COLORS];

    /**
     * Decodes a SNES encoded 16 bit color.
     *
     * @param snesValue The encoded color value.
     * @return An AWT color representing the encoded color.
     */
    public static Color decodeColor(int snesValue) {
        return new Color(
                (snesValue & 0x1F) << 3,
                ((snesValue >> 5) & 0x1F) << 3,
                ((snesValue >> 10) & 0x1F) << 3
        );
    }

    /**
     * Encodes an AWT color into SNES 16 bit format.
     *
     * @param color The color to encode.
     * @return The encoded value of the provided color.
     */
    public static int encodeColor(Color color) {
        return (color.getRed() >> 3) |
                ((color.getGreen() >> 3) << 5) |
                ((color.getBlue() >> 3) << 10);
    }

    /**
     * Creates a new palette with only black colors.
     */
    public Palette() {
        for (int i = 0; i < NUM_COLORS; i++)
            color[i] = new Color(0, false);
    }

    /**
     * Creates a new palette from another palette.
     *
     * @param palette The palette to copy.
     */
    public Palette(Palette palette) {
        for (int i = 0; i < NUM_COLORS; i++)
            color[i] = new Color(palette.color[i].getRGB());
    }

    /**
     * Retrieves a color from the palette.
     *
     * @param i The color index, which must be between 0 and 15.
     * @return The color at the given index.
     */
    public Color getColor(int i) {
        return color[i];
    }

    /**
     * Sets a color in the palette.
     *
     * @param i     The color index, which must be between 0 and 15.
     * @param color The color to store at the given index.
     */
    public void setColor(int i, Color color) {
        this.color[i] = color;
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        for (int i = 0; i < NUM_COLORS; i++)
            color[i] = decodeColor(in.readUnsignedShort());

        return NUM_COLORS * 2;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        for (int i = 0; i < NUM_COLORS; i++)
            out.writeShort(encodeColor(color[i]));

        return NUM_COLORS * 2;
    }
}

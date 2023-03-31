package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.io.Serializable;

/**
 * An SNES 8x8 8bpp tile.
 * <p/>
 * The low 4 bits represent the {@link Palette} color index from 0 to 15, the high bits represent
 * a palette index in a set of 8 palettes.
 */
public class Tile8 implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -2470399980896612692L;

    public static final int SIZE = 8;

    private static final BufferedImage imageBuffer = new BufferedImage(SIZE, SIZE, BufferedImage.TYPE_4BYTE_ABGR);

    private final int[][] pixel = new int[SIZE][SIZE];

    /**
     * Creates a new, completely black (or transparent) tile.
     */
    public Tile8() {
    }

    /**
     * Creates a new tile from another tile.
     *
     * @param tile The tile to copy.
     */
    public Tile8(Tile8 tile) {
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++)
                pixel[x][y] = tile.pixel[x][y];
        }
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        byte[] data = new byte[64];
        in.read(data);

        int p;
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++) {
                pixel[x][y] = data[(y << 3) + x];
            }
        }
        return data.length;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        byte[] data = new byte[64];
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++) {
                data[(y << 3) + x] = (byte) (pixel[x][y] & 0xFF);
            }
        }
        out.write(data);
        return data.length;
    }

    /**
     * Retrieves the pixel information for the given coordinates.
     *
     * @param x The X coordinate, ranging between 0 and 7.
     * @param y The Y coordinate, ranging between 0 and 7.
     * @return The palette color index for the specified pixel.
     */
    public int getPixel(int x, int y) {
        return pixel[x][y] & 0x0F;
    }

    /**
     * Retrieves the palette index for the given coordinates.
     *
     * @param x The X coordinate, ranging between 0 and 7.
     * @param y The Y coordinate, ranging between 0 and 7.
     * @return The palette index for the specified pixel.
     */
    public int getPixelPalette(int x, int y) {
        return (pixel[x][y] >> 4) & 0x07;
    }

    /**
     * Sets the pixel information for the given coordinates.
     *
     * @param x       The X coordinate, ranging between 0 and 7.
     * @param y       The Y coordinate, ranging between 0 and 7.
     * @param pixel   The new palette color index for the specified pixel.
     * @param palette The palette index.
     */
    public void setPixel(int x, int y, int pixel, int palette) {
        this.pixel[x][y] = (pixel & 0x0F) | ((palette & 0x07) << 4);
    }

    /**
     * Draws this tile.
     *
     * @param g           The Graphics object to draw this tile to.
     * @param palettes    An array of 8 palettes.
     * @param dx          The destination X coordinate (position of the left edge).
     * @param dy          The destination Y coordinate (position of the top edge).
     * @param zoom        The zoom level. 1 means no zoom, 2 means 2x, etc.
     * @param transparent Whether or not to interpret zero pixels as transparent pixels.
     * @param flipX       Whether or not to flip the tile horizontally.
     * @param flipY       Whether or not to flip the tile vertically.
     */
    public void draw(
            Graphics g, Palette[] palettes, int dx, int dy, int zoom, boolean transparent, boolean flipX, boolean flipY) {

        if (zoom < 1)
            zoom = 1;

        int p;
        Palette pal;

        synchronized (imageBuffer) {
            for (int y = 0; y < SIZE; y++) {
                for (int x = 0; x < SIZE; x++) {
                    p = pixel[flipX ? SIZE - 1 - x : x][flipY ? SIZE - 1 - y : y];

                    pal = palettes[(p >> 4) & 0x07];
                    p &= 0x0F;

                    if (transparent && p == 0)
                        imageBuffer.setRGB(x, y, 0);
                    else
                        imageBuffer.setRGB(x, y, pal.getColor(p).getRGB());
                }
            }

            g.drawImage(imageBuffer, dx, dy, dx + SIZE * zoom, dy + SIZE * zoom, 0, 0, SIZE, SIZE, null);
        }
    }
}

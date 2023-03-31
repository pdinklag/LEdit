package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.io.Serializable;

/**
 * An SNES 8x8 4bpp tile.
 * <p/>
 * The 4 bits represent the {@link Palette} color index from 0 to 15.
 */
public class Tile4 implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -8726748777953096407L;

    public static final int SIZE = 8;

    private static final BufferedImage imageBuffer = new BufferedImage(SIZE, SIZE, BufferedImage.TYPE_4BYTE_ABGR);

    private final int[][] pixel = new int[SIZE][SIZE];

    /**
     * Creates a new, completely black (or transparent) tile.
     */
    public Tile4() {
    }

    /**
     * Creates a new tile from another tile.
     *
     * @param tile The tile to copy.
     */
    public Tile4(Tile4 tile) {
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++)
                pixel[x][y] = tile.pixel[x][y];
        }
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        byte[] data = new byte[32];
        in.read(data);

        int p;
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++) {
                p = 0;

                if ((data[(y << 1) + 0x00] & (1 << (7 - x))) != 0)
                    p |= 0x1;
                if ((data[(y << 1) + 0x01] & (1 << (7 - x))) != 0)
                    p |= 0x2;
                if ((data[(y << 1) + 0x10] & (1 << (7 - x))) != 0)
                    p |= 0x4;
                if ((data[(y << 1) + 0x11] & (1 << (7 - x))) != 0)
                    p |= 0x8;

                pixel[x][y] = p;
            }
        }

        return data.length;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        byte[] data = new byte[32];
        int p;
        for (int y = 0; y < SIZE; y++) {
            for (int x = 0; x < SIZE; x++) {
                p = pixel[x][y];

                if ((p & 0x01) != 0)
                    data[(y << 1) + 0x00] |= (byte) (1 << (7 - x));
                if ((p & 0x02) != 0)
                    data[(y << 1) + 0x01] |= (byte) (1 << (7 - x));
                if ((p & 0x04) != 0)
                    data[(y << 1) + 0x10] |= (byte) (1 << (7 - x));
                if ((p & 0x08) != 0)
                    data[(y << 1) + 0x11] |= (byte) (1 << (7 - x));
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
        return pixel[x][y];
    }

    /**
     * Sets the pixel information for the given coordinates.
     *
     * @param x     The X coordinate, ranging between 0 and 7.
     * @param y     The Y coordinate, ranging between 0 and 7.
     * @param pixel The new palette color index for the specified pixel.
     */
    public void setPixel(int x, int y, int pixel) {
        this.pixel[x][y] = pixel & 0x0F;
    }

    /**
     * Draws this tile.
     *
     * @param g           The Graphics object to draw this tile to.
     * @param pal         The palette to use for drawing.
     * @param dx          The destination X coordinate (position of the left edge).
     * @param dy          The destination Y coordinate (position of the top edge).
     * @param zoom        The zoom level. 1 means no zoom, 2 means 2x, etc.
     * @param transparent Whether or not to interpret zero pixels as transparent pixels.
     * @param flipX       Whether or not to flip the tile horizontally.
     * @param flipY       Whether or not to flip the tile vertically.
     */
    public void draw(
            Graphics g, Palette pal, int dx, int dy, int zoom, boolean transparent, boolean flipX, boolean flipY) {

        if (zoom < 1)
            zoom = 1;

        int p;
        synchronized (imageBuffer) {
            for (int y = 0; y < SIZE; y++) {
                for (int x = 0; x < SIZE; x++) {
                    p = pixel[flipX ? SIZE - 1 - x : x][flipY ? SIZE - 1 - y : y];
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

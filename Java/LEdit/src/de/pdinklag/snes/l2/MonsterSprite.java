package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.snes.Palette;
import de.pdinklag.snes.SnesSerializable;
import de.pdinklag.snes.Tile4;

import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.HashMap;

/**
 * Represents a monster sprite that has a tile map and one or multiple palettes.
 */
public class MonsterSprite implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -1821759927981742750L;

    private static final HashMap<MonsterSprite, HashMap<Palette, BufferedImage>> imageCache =
            new HashMap<MonsterSprite, HashMap<Palette, BufferedImage>>();

    private int tilesX, tilesY, unknown;
    private final ArrayList<Palette> palettes = new ArrayList<Palette>();
    private final ArrayList<Tile4> tiles = new ArrayList<Tile4>();

    public void readDimensions(BinaryInputStream in) throws IOException {
        tilesX = in.readByte();
        int y = in.readByte();

        unknown = y & 0xE0;
        tilesY = y & 0x1F;
    }

    public void writeDimensions(BinaryOutputStream out) throws IOException {
        out.writeByte(tilesX);
        out.writeByte(tilesY | unknown);
    }

    public int readSnes(BinaryInputStream in) throws IOException {
        int numRead = 0;

        tiles.clear();
        palettes.clear();

        if (imageCache.containsKey(this))
            imageCache.get(this).clear();

        if (tilesX > 0 && tilesY > 0) {
            int numPals = in.readByte();
            numRead++;

            for (int i = 0; i < numPals; i++) {
                Palette pal = new Palette();
                numRead += pal.readSnes(in);
                palettes.add(pal);
            }

            int numTiles = tilesX * tilesY;
            for (int i = 0; i < numTiles; i++) {
                Tile4 tile = new Tile4();
                numRead += tile.readSnes(in);
                tiles.add(tile);
            }
        }
        return numRead;
    }

    public int writeSnes(BinaryOutputStream out) throws IOException {
        int numWritten = 0;

        out.writeByte(palettes.size());
        numWritten++;

        for (Palette pal : palettes)
            numWritten += pal.writeSnes(out);

        for (Tile4 tile : tiles)
            numWritten += tile.writeSnes(out);

        return numWritten;
    }

    public int getTilesX() {
        return tilesX;
    }

    public void setTilesX(int tilesX) {
        this.tilesX = tilesX;
    }

    public int getTilesY() {
        return tilesY;
    }

    public void setTilesY(int tilesY) {
        this.tilesY = tilesY;
    }

    public int getTileCount() {
        return tiles.size();
    }

    public Tile4 getTile(int i) {
        return tiles.get(i);
    }

    public void addTile(Tile4 tile) {
        tiles.add(tile);
    }

    public void removeTile(int i) {
        tiles.remove(i);
    }

    public int getPaletteCount() {
        return palettes.size();
    }

    public Palette getPalette(int i) {
        return palettes.get(i);
    }

    public void setPalette(int i, Palette pal) {
        palettes.set(i, pal);
    }

    public void addPalette(Palette palette) {
        palettes.add(palette);
    }

    public void removePalette(int i) {
        palettes.remove(i);
    }

    public void clearImageCache() {
        if (imageCache.containsKey(this))
            imageCache.get(this).clear();
    }

    public Tile4[][] getTileMap() {
        Tile4[][] map = new Tile4[tilesX][tilesY];
        for (int blockY = 0; blockY < (tilesY >> 1); blockY++) {
            for (int blockX = 0; blockX < (tilesX >> 1); blockX++) {
                for (int y = 0; y < 2; y++) {
                    for (int x = 0; x < 2; x++) {
                        int t = ((y + (blockX << 1) + blockY * tilesX) << 1) + x;
                        map[x + (blockX << 1)][y + (blockY << 1)] = tiles.get(t);
                    }
                }
            }
        }
        return map;
    }

    public BufferedImage getImage(Palette pal) {
        BufferedImage image;

        if (imageCache.containsKey(this)) {
            image = imageCache.get(this).get(pal);
        } else {
            imageCache.put(this, new HashMap<Palette, BufferedImage>());
            image = null;
        }

        if (image == null) {
            BufferedImage bufferedImage = new BufferedImage(
                    tilesX * Tile4.SIZE, tilesY * Tile4.SIZE, BufferedImage.TYPE_4BYTE_ABGR);

            Graphics gImage = bufferedImage.createGraphics();
            Tile4[][] map = getTileMap();
            for (int y = 0; y < tilesY; y++) {
                for (int x = 0; x < tilesX; x++) {
                    map[x][y].draw(gImage, pal, x * Tile4.SIZE, y * Tile4.SIZE, 1, true, false, false);
                }
            }

            imageCache.get(this).put(pal, image);
            image = bufferedImage;
        }

        return image;
    }
}

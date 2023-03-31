using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public class L2MapBlock
    {
        struct TileData
        {
            public short tile;
            public byte palette;
            public bool draw_over_sprites;
            public bool flip_x;
            public bool flip_y;
        }

        Bitmap bmp;
        bool drawn;

        TileData[,] tiles = new TileData[2, 2];

        ~L2MapBlock()
        {
            if (bmp != null) bmp.Dispose();
        }

        public L2MapBlock(BinaryReader br) { drawn = false; FromStream(br); }

        public void FromStream(BinaryReader br)
        {
            if (br != null)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        tiles[x, y].draw_over_sprites = false;
                        tiles[x, y].flip_x = false;
                        tiles[x, y].flip_y = false;
                        tiles[x, y].palette = 0;
                        tiles[x, y].tile = (short)br.ReadByte();

                        byte flags = br.ReadByte();

                        if ((flags & 0x01) != 0) tiles[x, y].tile |= 0x100;
                        if ((flags & 0x02) != 0) tiles[x, y].tile |= 0x200;
                        bool bit4 = ((flags & 0x04) != 0);
                        bool bit8 = ((flags & 0x08) != 0);
                        bool bit10 = ((flags & 0x10) != 0);
                        if ((flags & 0x20) != 0) tiles[x, y].draw_over_sprites = true;
                        if ((flags & 0x40) != 0) tiles[x, y].flip_x = true;
                        if ((flags & 0x80) != 0) tiles[x, y].flip_y = true;

                        if (!bit10 && !bit8 && bit4)
                            tiles[x, y].palette = 0x5;//0x1;
                        else if (bit10 && !bit8 && !bit4)
                            tiles[x, y].palette = 0x2;
                        else if (bit10 && !bit8 && bit4)
                            tiles[x, y].palette = 0x3;
                        else if (bit10 && bit8 && !bit4)
                            tiles[x, y].palette = 0x4;
                        else if (bit10 && bit8 && bit4)
                            tiles[x, y].palette = 0x5;
                        else if (!bit10 && bit8 && bit4)
                            tiles[x, y].palette = 0x1;
                        else if (!bit10 && bit8 && !bit4)
                            tiles[x, y].palette = 0x0;
                        else
                            tiles[x, y].palette = 0x0;

                    }
                }
            }
        }

        public void PreDraw(SNESTileset set, SNESPalette[] pals)
        {
            if (!drawn)
            {
                bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        set.tiles[tiles[x, y].tile].Draw(
                            g,
                            pals[tiles[x, y].palette],
                            x << 3,
                            y << 3,
                            1,
                            true,
                            tiles[x, y].flip_x,
                            tiles[x, y].flip_y);
                    }
                    drawn = true;
                }
            }
        }

        public void Draw(Graphics dst, SNESTileset set, SNESPalette[] pals, int dx, int dy)
        { Draw(dst, set, pals, dx, dy, 1); }

        public void Draw(Graphics dst, SNESTileset set, SNESPalette[] pals, int dx, int dy, int zoom)
        {
            if (zoom < 1) zoom = 1;

            PreDraw(set,pals);

            if (zoom == 1 && drawn)
                dst.DrawImage(bmp, dx, dy);
            else
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        set.tiles[tiles[x, y].tile].Draw(
                            dst,
                            pals[tiles[x, y].palette],
                            dx + x * (zoom << 3),
                            dy + y * (zoom << 3),
                            zoom,
                            true,
                            tiles[x, y].flip_x,
                            tiles[x, y].flip_y);
                    }
                }
            }
        }
    }
}

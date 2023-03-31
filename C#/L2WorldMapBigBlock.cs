using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace LEdit
{
    //This is both idiotic and pathetic...
    public class L2WorldMapBigBlock
    {
        #region STATIC
        public const long Offset = 0xD0000;
        public const int Count = 0xF2E;

        static L2WorldMapBigBlock[] WMBB = new L2WorldMapBigBlock[Count];

        public static L2WorldMapBigBlock Get(int n)
        {
            if (n >= 0 && n < Count)
                return WMBB[n];
            else
                return null;
        }

        public static void InitROM(BinaryReader br)
        {
            br.BaseStream.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
                WMBB[i] = new L2WorldMapBigBlock(br);
        }

        #endregion


        Bitmap bmp;
        bool drawn;

        ushort[,] blocks = new ushort[2, 2];

        public L2WorldMapBigBlock(BinaryReader br)
        {
            drawn = false;
            FromStream(br);
        }

        ~L2WorldMapBigBlock()
        {
            if (bmp != null) bmp.Dispose();
        }

        public void FromStream(BinaryReader br)
        {
            if (br != null)
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                        blocks[x, y] = br.ReadUInt16();
                }
            }
        }

        public void PreDraw(SNESTileset8 set, SNESPalette[] pals)
        {
            if (!drawn)
            {
                bmp = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        L2WorldMapBlock.Get(blocks[x, y]).Draw(
                            g,
                            set,
                            pals,
                            x << 4,
                            y << 4,
                            1);
                    }
                    drawn = true;
                }
            }
        }

        public void Draw(Graphics dst, SNESTileset8 set, SNESPalette[] pals, int dx, int dy)
        { Draw(dst, set, pals, dx, dy, 1); }

        public void Draw(Graphics dst, SNESTileset8 set, SNESPalette[] pals, int dx, int dy, int zoom)
        {
            if (zoom < 1) zoom = 1;

            PreDraw(set, pals);

            if (zoom == 1 && drawn)
                dst.DrawImage(bmp, dx, dy);
            else
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        L2WorldMapBlock.Get(blocks[x, y]).Draw(
                            dst,
                            set,
                            pals,
                            dx + x * (zoom << 4),
                            dy + y * (zoom << 4),
                            zoom);
                    }
                }
            }
        }
    }
}

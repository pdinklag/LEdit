using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2WorldMapBlock
    {
        #region STATIC
        public const long Offset = 0xF3000;
        public const int Count = 0x4C8;

        static L2WorldMapBlock[] WMB = new L2WorldMapBlock[Count];

        public static L2WorldMapBlock Get(int n)
        {
            if (n >= 0 && n < Count)
                return WMB[n];
            else
                return null;
        }

        public static void InitROM(BinaryReader br)
        {
            br.BaseStream.Seek(Offset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
                WMB[i] = new L2WorldMapBlock(br);
        }

        #endregion

        public enum PassabilityType
        {
            Plain, Desert, Forest, Water, Mountain
        };

        Bitmap bmp;
        bool drawn;

        byte[,] tiles = new byte[2, 2];
        PassabilityType pass;

        public PassabilityType Passability { get { return pass; } set { pass = value; } }

        ~L2WorldMapBlock()
        {
            if (bmp != null) bmp.Dispose();
        }

        public L2WorldMapBlock(BinaryReader br) 
        {
            drawn = false; FromStream(br); 
        }

        public void FromStream(BinaryReader br)
        {
            if (br != null)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                        tiles[x, y] = br.ReadByte();
                }

                byte p = br.ReadByte();

                switch (p)
                {
                    case 0x00:
                        pass = PassabilityType.Plain;
                        break;

                    case 0x10:
                        pass = PassabilityType.Desert;
                        break;

                    case 0x20:
                        pass = PassabilityType.Forest;
                        break;

                    case 0x30:
                        pass = PassabilityType.Water;
                        break;

                    case 0x40:
                        pass = PassabilityType.Mountain;
                        break;

                    default:
                        Console.WriteLine("Unknown PassabilityType 0x" + p.ToString("X2") + " at 0x" + (br.BaseStream.Position - 1).ToString("X6"));
                        break;
                }
            }
        }

        public void PreDraw(SNESTileset8 set, SNESPalette[] pals)
        {
            if (!drawn)
            {
                bmp = new Bitmap(16, 16, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(bmp);

                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        set.tiles8[tiles[x, y]].Draw(
                            g,
                            pals,
                            x << 3,
                            y << 3,
                            1,
                            false,
                            false,
                            false);
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

            PreDraw(set,pals);

            if (zoom == 1 && drawn)
                dst.DrawImage(bmp, dx, dy);
            else
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        set.tiles8[tiles[x, y]].Draw(
                            dst,
                            pals,
                            dx + x * (zoom << 3),
                            dy + y * (zoom << 3),
                            zoom,
                            false,
                            false, //tiles[x, y].flip_x,
                            false); //tiles[x, y].flip_y);
                    }
                }
            }
        }
    }
}

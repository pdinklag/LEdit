using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2WorldMap
    {
        public const int TileSetFile = 0x28C;
        public const int MapFile = 0x28D;
        public const long PalSetAddress = 0x133400;

        public struct L2WorldMapTile
        {
            public ushort block;
        };

        SNESPalette[] pals = new SNESPalette[8];
        SNESTileset8 set;
        L2MapHeader header;

        L2WorldMapTile[,] tiles = new L2WorldMapTile[128, 128];

        public byte Width { get { return 128; } }
        public byte Height { get { return 128; } }
        public L2WorldMapTile[,] Tiles { get { return tiles; } }
        public L2MapHeader Header { get { return header; } }

        public SNESTileset8 TileSet { get { return set; } }
        public SNESPalette[] Palettes { get { return pals; } }

        public L2WorldMap()
        {
            header = L2MapHeader.Get(0);

            FileStream fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            //Load PaletteSet
            br.BaseStream.Seek(PalSetAddress, SeekOrigin.Begin);
            for (int i = 0; i < 8; i++)
                pals[i] = new SNESPalette(br);

            br.Close();
            fs.Close();

            //Load TileSet
            string FileName = L2FileManager.ExtractFile(TileSetFile);

            set = new SNESTileset8(new FileInfo(FileName));

            //Load Map
            FileName =  L2FileManager.ExtractFile(MapFile);

            fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            br = new BinaryReader(fs);

            br.BaseStream.Seek(0x40, SeekOrigin.Begin); //skip header
            for (int y = 0; y < 128; y++)
            {
                for (int x = 0; x < 128; x++)
                    tiles[x, y].block = br.ReadUInt16();
            }

            //more unknown data follows in the file...

            br.Close();
            fs.Close();
        }

        public void Draw(Graphics dst, int dx, int dy, int mx, int my, int mw, int mh, int zoom, L2Map.DrawInfo draw)
        {
            if (zoom < 1) zoom = 1;
            if (mw == 0) mw = 128;
            if (mh == 0) mh = 128;

            Pen grid_pen = new Pen(Color.FromArgb(0x40, Color.White));

            //Layers
            for (int y = my; (y < my + mh) && (y < 128); y++)
            {
                for (int x = mx; (x < mx + mw) && (x < 128); x++)
                {
                    int final_x = dx + zoom * ((x - mx) << 5);
                    int final_y = dy + zoom * ((y - my) << 5);

                    L2WorldMapBigBlock.Get(tiles[x, y].block).Draw(
                        dst,
                        set,
                        pals,
                        final_x,
                        final_y,
                        zoom);

                    if (draw.grid)
                    {
                        dst.DrawLine(grid_pen,
                            final_x,
                            final_y,
                            final_x + (zoom << 5) - 1,
                            final_y);

                        dst.DrawLine(grid_pen,
                            final_x,
                            final_y,
                            final_x,
                            final_y + (zoom << 5) - 1);
                    }
                }
            }
        }
    }
}

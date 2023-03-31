using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LEdit
{
	/*
	 * Lufia 2 - MapSprite Class
	 */
	public class L2MapSprite
    {
        #region STATIC
        public const byte Bank = 0xDF;
        public const int Count = 0xFB;
        public const long PalOffset = 0xF7EB9;
        public const long TableOffset = 0x2FF000;

        static SNESPalette[] MSPal = new SNESPalette[8];
        static L2MapSprite[] MS = new L2MapSprite[Count];

        public static SNESPalette[] GetPalettes()
        {
            return MSPal;
        }
        public static SNESPalette GetPalette(int n)
        {
            if (n >= 0 && n < 8)
                return MSPal[n];
            else return
                null;
        }

        public static L2MapSprite Get(int n)
        {
            if (n >= 0 && n < Count)
                return MS[n];
            else
                return null;
        }
        public static void Set(int n, L2MapSprite sprite)
        {
            if (n >= 0 && n < Count)
                MS[n] = sprite;
        }

        public static void InitROM(BinaryReader br)
        {
            br.BaseStream.Seek(PalOffset, SeekOrigin.Begin);
            for (int i = 0; i < 8; i++) MSPal[i] = new SNESPalette(br);

            SNESExLoROMAddress rom_addr = new SNESExLoROMAddress();

            br.BaseStream.Seek(TableOffset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
            {
                rom_addr.Set(Bank, br.ReadUInt16());

                long ret_addr = br.BaseStream.Position;
                br.BaseStream.Seek(rom_addr.ToPCAddress(), SeekOrigin.Begin);

                MS[i] = new L2MapSprite(br);

                br.BaseStream.Seek(ret_addr, SeekOrigin.Begin);
            }
        }

        #endregion

        byte flags;
		byte paln;
		SNESExLoROMAddress tl_addr;
		
		int ntiles;
        SNESTileset tl;

        public L2MapSprite() { }

        public L2MapSprite(BinaryReader br)
        {
            this.FromStream(br);
        }

        public void FromStream(BinaryReader br)
        {
            flags = br.ReadByte();
            paln = (byte)(br.ReadByte() & 0x7);

            ushort offs = br.ReadUInt16();
            byte bnk = br.ReadByte();
            tl_addr = new SNESExLoROMAddress(bnk, offs);

            long ret_addr = br.BaseStream.Position;
            br.BaseStream.Seek(tl_addr.ToPCAddress(), SeekOrigin.Begin);

            if ((flags & 0x2) != 0) ntiles = 0x10;
            else if ((flags & 0x1) != 0) ntiles = 0x8;
            else ntiles = 0x4;

            if ((flags & 0x8) != 0) ntiles *= 2;
            else if ((flags & 0x10) != 0) ntiles *= 6;

            tl = new SNESTileset(br, ntiles);

            br.BaseStream.Seek(ret_addr, SeekOrigin.Begin);
        }

        public void ToStream(BinaryWriter bw)
        {
            bw.Write(flags);
            bw.Write(paln);
            tl_addr.ToStream(bw);
        }

        public void Draw(Graphics dst, int dx, int dy)
        { this.Draw(dst, dx, dy, 1, paln); }

        public void Draw(Graphics dst, int dx, int dy, int zoom)
        { this.Draw(dst, dx, dy, zoom, paln); }

        public void Draw(Graphics dst, int dx, int dy, int zoom, byte palette)
        {
            int n, nx, ny, fx, fy, fn, nf;

            nx = ((flags & 0x2) != 0) ? 4 : 2;
            ny = ((flags & 0x1) != 0) ? 4 : 2;

            fx = nx; fy = ny;
            fn = fx * fy;

            if ((flags & 0x8) != 0) nf = 2;
            else if ((flags & 0x10) != 0) nf = 6;
            else nf = 1;

            nx *= nf;
            if ((nx * ny) == ntiles)
            {
                for (int f = 0; f < nf; f++)
                {
                    for (int y = 0; y < fy; y++)
                    {
                        for (int x = 0; x < fx; x++)
                        {
                            n = f * fn + (y & 1) * (fn >> 1) + (y >> 1) * fx + x;

                            tl.tiles[n].Draw(dst,
                                       MSPal[(int)palette],
                                       dx + ((((f * fx) + x) * zoom) << 3),
                                       dy + ((y * zoom) << 3),
                                       zoom,
                                       true);
                        }
                    }
                }
            }
        }

        public void DrawFrame(Graphics dst, int frame, int dx, int dy)
        {
            DrawFrame(dst, frame, dx, dy, 1, paln);
        }

        public void DrawFrame(Graphics dst, int frame, int dx, int dy, int zoom)
        {
            DrawFrame(dst, frame, dx, dy, zoom, paln);
        }

        public void DrawFrame(Graphics dst, int frame, int dx, int dy, int zoom, byte palette)
        {
            int n, nx, ny, fx, fy, fn, nf;

            nx = ((flags & 0x2) != 0) ? 4 : 2;
            ny = ((flags & 0x1) != 0) ? 4 : 2;

            fx = nx; fy = ny;
            fn = fx * fy;

            if ((flags & 0x8) != 0) nf = 2;
            else if ((flags & 0x10) != 0) nf = 6;
            else nf = 1;

            if (frame < 0) frame = 0;
            if (frame > nf) frame = nf - 1;

            nx *= nf;
            if ((nx * ny) == ntiles)
            {
                for (int y = 0; y < fy; y++)
                {
                    for (int x = 0; x < fx; x++)
                    {
                        n = frame * fn + (y & 1) * (fn >> 1) + (y >> 1) * fx + x;

                        tl.tiles[n].Draw(dst,
                                   MSPal[(int)palette],
                                   dx + ((x * zoom) << 3),
                                   dy + ((y * zoom) << 3),
                                   zoom,
                                   true);
                    }
                }
            }
        }

        public void ExportBMP(FileInfo File)
        {
            this.ExportBMP(File, paln); 
        }

        public void ExportBMP(FileInfo File, byte palette)
        {
            int n, nx, ny, fx, fy, fn, nf;

            nx = ((flags & 0x2) != 0) ? 4 : 2;
            ny = ((flags & 0x1) != 0) ? 4 : 2;

            fx = nx; fy = ny;
            fn = fx * fy;

            if ((flags & 0x8) != 0) nf = 2;
            else if ((flags & 0x10) != 0) nf = 6;
            else nf = 1;

            nx *= nf;
            if ((nx * ny) == ntiles)
            {
                //Draw to array
                byte[,] p = new byte[nx << 3, ny << 3];

                for (int f = 0; f < nf; f++)
                {
                    for (int y = 0; y < fy; y++)
                    {
                        for (int x = 0; x < fx; x++)
                        {
                            n = f * fn + (y & 1) * (fn >> 1) + (y >> 1) * fx + x;
                            tl.tiles[n].To2DArray(p, ((f * fx) + x) << 3, y << 3);
                        }
                    }
                }

                //Save
                BMP4bppIO.Export2DArray(
                    File, p, MSPal[(int)palette], nx << 3, ny << 3);
            }
        }

        public void ImportBMP(FileInfo File)
        {
            int n, nx, ny, fx, fy, fn, nf;

            nx = ((flags & 0x2) != 0) ? 4 : 2;
            ny = ((flags & 0x1) != 0) ? 4 : 2;

            fx = nx; fy = ny;
            fn = fx * fy;

            if ((flags & 0x8) != 0) nf = 2;
            else if ((flags & 0x10) != 0) nf = 6;
            else nf = 1;

            nx *= nf;
            if ((nx * ny) == ntiles)
            {
                //Load
                byte[,] p = new byte[nx << 3, ny << 3];

                if (BMP4bppIO.Import2DArray(File, p))
                {
                    //Read from array
                    for (int f = 0; f < nf; f++)
                    {
                        for (int y = 0; y < fy; y++)
                        {
                            for (int x = 0; x < fx; x++)
                            {
                                n = f * fn + (y & 1) * (fn >> 1) + (y >> 1) * fx + x;
                                tl.tiles[n].From2DArray(p, ((f * fx) + x) << 3, y << 3);
                            }
                        }
                    }
                }
                else MessageBox.Show("ERROR!!!");
            }
        }

        public int TileCount { get { return ntiles; } }
        public SNESTileset Tileset { get { return tl; } }

        public byte Flags { get { return flags; } set { flags = value; } }

        public SNESExLoROMAddress TileAddress { get { return tl_addr; } set { tl_addr = value; } }

        public byte Palette
        {
            get { return paln; }
            set { paln = (byte)(value & 0x7); }
        }
	}
	
	
}

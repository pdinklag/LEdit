using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

#region SNESPalette

/*
 * SNES Palette - 16 colors
 */
public class SNESPalette
{
    Color[] c;

    public SNESPalette()
    {
        c = new Color[16];
    }

    public SNESPalette(BinaryReader br)
    {
        c = new Color[16];
        this.FromStream(br);
    }

    public SNESPalette(ushort[] src)
    {
        c = new Color[16];
        this.FromData(src);
    }

    public SNESPalette(SNESPalette src)
    {
        c = new Color[16];
        this.FromPalette(src);
    }

    public void FromPalette(SNESPalette src)
    {
        if (src != null)
        {
            for (byte i = 0; i < 16; i++)
                c[i] = src.GetColor(i);
        }
    }

    public void FromStream(BinaryReader br)
    {
        if (br != null)
        {
            ushort[] data = new ushort[16];
            for (int i = 0; i < 16; i++) data[i] = br.ReadUInt16();

            this.FromData(data);
        }
    }

    public void ToStream(BinaryWriter bw)
    {
        if (bw != null)
        {
            for (int i = 0; i < 16; i++)
            {
                ushort u = (ushort)(
                    (c[i].R >> 3) | ((c[i].G >> 3) << 5) | ((c[i].B >> 3) << 10)
                    );

                bw.Write(u);
            }
        }
    }

    public void FromData(ushort[] src)
    {
        if (src.Length >= 16)
        {
            int r, g, b;

            for (int i = 0; i < 16; i++)
            {
                r = (src[i] & 0x1F) << 3;
                g = ((src[i] >> 5) & 0x1F) << 3;
                b = ((src[i] >> 10) & 0x1F) << 3;

                c[i] = Color.FromArgb(r, g, b);
            }
        }
    }

    public Color GetColor(byte n)
    {
        if (n >= 0 && n < 16)
            return c[n];
        else
            return Color.Black;
    }

    public void SetColor(byte n, Color color)
    {
        if (n >= 0 && n < 16) c[n] = color;
    }
}

#endregion

#region SNESTile
/*
 * SNES Tile - 8x8 4bpp
 */
public class SNESTile
{
    public byte[,] p;

    public SNESTile()
    {
        p = new byte[8, 8];
    }

    public SNESTile(BinaryReader br)
    {
        p = new byte[8, 8];
        this.FromStream(br);
    }

    public SNESTile(byte[] src)
    {
        p = new byte[8, 8];
        this.FromData(src);
    }

    public SNESTile(SNESTile src)
    {
        p = new byte[8, 8];
        this.FromTile(src);
    }

    public void FromTile(SNESTile src)
    {
        if (src != null)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                    p[x, y] = src.GetPixel(x, y);
            }
        }
    }

    public void FromStream(BinaryReader br)
    {
        if (br != null)
        {
            byte[] data = br.ReadBytes(32);
            this.FromData(data);
        }
    }

    public void ToStream(BinaryWriter bw)
    {
        byte[] data = new byte[32];

        byte _p;
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _p = p[x, y];

                if ((_p & 0x01) != 0)
                    data[y << 1] |= (byte)(1 << (7 - x));
                if ((_p & 0x02) != 0)
                    data[(y << 1) + 0x1] |= (byte)(1 << (7 - x));
                if ((_p & 0x04) != 0)
                    data[(y << 1) + 0x10] |= (byte)(1 << (7 - x));
                if ((_p & 0x08) != 0)
                    data[(y << 1) + 0x11] |= (byte)(1 << (7 - x));
            }
        }

        bw.Write(data);
    }

    public void To2DArray(byte[,] dst, int dx, int dy)
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
                dst[dx + x, dy + y] = p[x, y];
        }
    }

    public void From2DArray(byte[,] src, int dx, int dy)
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
                p[x, y] = src[dx + x, dy + y];
        }
    }

    public void FromData(byte[] src)
    {
        if (src.Length >= 32)
        {
            byte _p;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    _p = 0;

                    if ((src[y << 1] & (1 << (7 - x))) != 0)
                        _p |= 0x1;
                    if ((src[(y << 1) + 0x1] & (1 << (7 - x))) != 0)
                        _p |= 0x2;
                    if ((src[(y << 1) + 0x10] & (1 << (7 - x))) != 0)
                        _p |= 0x4;
                    if ((src[(y << 1) + 0x11] & (1 << (7 - x))) != 0)
                        _p |= 0x8;

                    p[x, y] = _p;
                }
            }
        }
    }

    public void SetPixel(byte c, int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            p[x, y] = c;
    }

    public byte GetPixel(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            return p[x, y];
        else
            return 0;
    }

    public void Draw(Graphics dst, SNESPalette pal, int dx, int dy)
    { this.Draw(dst, pal, dx, dy, 1, false); }
    public void Draw(Graphics dst, SNESPalette pal, int dx, int dy, int zoom)
    { this.Draw(dst, pal, dx, dy, zoom, false); }
    public void Draw(Graphics dst, SNESPalette pal, int dx, int dy, int zoom, bool transparent)
    { this.Draw(dst, pal, dx, dy, zoom, transparent, false, false); }

    public void Draw(Graphics dst, SNESPalette pal, int dx, int dy, int zoom, bool transparent, bool flipx, bool flipy)
    {
        if (zoom < 1) zoom = 1;
        if (pal != null && dst != null)
        {
            byte cc;
            Color c;

            byte[] data = new byte[(zoom * zoom) << 8];

            int current = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int px = flipx ? (7 - x) : x;
                    int py = flipy ? (7 - y) : y;

                    cc = p[px, py];
                    if (!(transparent && cc == 0))
                    {
                        c = pal.GetColor(cc);
                        if (zoom == 1)
                        {
                            data[current + 3] = 0xFF;
                            data[current + 2] = c.R;
                            data[current + 1] = c.G;
                            data[current + 0] = c.B;
                        }
                        else
                        {
                            for (int zy = 0; zy < zoom; zy++)
                            {
                                for (int zx = 0; zx < zoom; zx++)
                                {
                                    int current_zoomed =
                                        current + ((zy * zoom) << 5) + (zx << 2);
                                    data[current_zoomed + 3] = 0xFF;
                                    data[current_zoomed + 2] = c.R;
                                    data[current_zoomed + 1] = c.G;
                                    data[current_zoomed + 0] = c.B;
                                }
                            }
                        }
                    }
                    current += zoom << 2;
                }
                if (zoom > 1) current += (zoom - 1) * (zoom << 5);
            }

            Bitmap bmp = new Bitmap(zoom << 3, zoom << 3,
                                    PixelFormat.Format32bppArgb);

            BitmapData bmp_data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly,
                bmp.PixelFormat);

            Marshal.Copy(data, 0, bmp_data.Scan0, data.Length);

            bmp.UnlockBits(bmp_data);

            dst.DrawImage(bmp, dx, dy);
        }
    }
}
#endregion

#region SNESTile8
/*
 * SNES Tile - 8x8 8bpp (4 bits for color, 4 bits for palette)
 */
public class SNESTile8
{
    public byte[,] p;

    public SNESTile8()
    {
        p = new byte[8, 8];
    }

    public SNESTile8(BinaryReader br)
    {
        p = new byte[8, 8];
        this.FromStream(br);
    }

    public SNESTile8(byte[] src)
    {
        p = new byte[8, 8];
        this.FromData(src);
    }

    public SNESTile8(SNESTile8 src)
    {
        p = new byte[8, 8];
        this.FromTile(src);
    }

    public void FromTile(SNESTile8 src)
    {
        if (src != null)
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                    p[x, y] = src.GetPixel(x, y);
            }
        }
    }

    public void FromStream(BinaryReader br)
    {
        if (br != null)
        {
            byte[] data = br.ReadBytes(64);
            this.FromData(data);
        }
    }

    public void FromData(byte[] src)
    {
        if (src.Length >= 64)
        {
            //byte _p;

            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                    p[x, y] = src[(y << 3) + x];
            }
        }
    }

    public void SetPixel(byte pc, int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            p[x, y] = pc;
    }

    public byte GetPixel(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
            return p[x, y];
        else
            return 0;
    }

    public void Draw(Graphics dst, SNESPalette[] pals, int dx, int dy)
    { this.Draw(dst, pals, dx, dy, 1, false); }
    public void Draw(Graphics dst, SNESPalette[] pals, int dx, int dy, int zoom)
    { this.Draw(dst, pals, dx, dy, zoom, false); }
    public void Draw(Graphics dst, SNESPalette[] pals, int dx, int dy, int zoom, bool transparent)
    { this.Draw(dst, pals, dx, dy, zoom, transparent, false, false); }

    public void Draw(Graphics dst, SNESPalette[] pals, int dx, int dy, int zoom, bool transparent, bool flipx, bool flipy)
    {
        if (zoom < 1) zoom = 1;
        if (pals != null && pals.Length >= 8 && dst != null)
        {
            byte cc;
            byte pal;
            Color c;

            byte[] data = new byte[(zoom * zoom) << 8];

            int current = 0;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    int px = flipx ? (7 - x) : x;
                    int py = flipy ? (7 - y) : y;

                    pal = (byte)((p[px, py] & 0xF0) >> 4);
                    cc = (byte)(p[px, py] & 0x0F);
                    if (!(transparent && cc == 0))
                    {
                        c = pals[pal].GetColor(cc);
                        if (zoom == 1)
                        {
                            data[current + 3] = 0xFF;
                            data[current + 2] = c.R;
                            data[current + 1] = c.G;
                            data[current + 0] = c.B;
                        }
                        else
                        {
                            for (int zy = 0; zy < zoom; zy++)
                            {
                                for (int zx = 0; zx < zoom; zx++)
                                {
                                    int current_zoomed =
                                        current + ((zy * zoom) << 5) + (zx << 2);
                                    data[current_zoomed + 3] = 0xFF;
                                    data[current_zoomed + 2] = c.R;
                                    data[current_zoomed + 1] = c.G;
                                    data[current_zoomed + 0] = c.B;
                                }
                            }
                        }
                    }
                    current += zoom << 2;
                }
                if (zoom > 1) current += (zoom - 1) * (zoom << 5);
            }

            Bitmap bmp = new Bitmap(zoom << 3, zoom << 3,
                                    PixelFormat.Format32bppArgb);

            BitmapData bmp_data = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.WriteOnly,
                bmp.PixelFormat);

            Marshal.Copy(data, 0, bmp_data.Scan0, data.Length);

            bmp.UnlockBits(bmp_data);

            dst.DrawImage(bmp, dx, dy);
        }
    }
}
#endregion

#region SNESTileset
/*
 * Set of SNES Tiles
 */
public class SNESTileset
{
    public Collection<SNESTile> tiles = new Collection<SNESTile>();

    public SNESTileset() { }

    public SNESTileset(BinaryReader br, int num)
    { FromStream(br, num); }

    public SNESTileset(FileInfo File)
    { FromFile(File); }

    public SNESTileset(SNESTileset src)
    { FromSNESTileset(src); }

    public void FromSNESTileset(SNESTileset src)
    {
        tiles.Clear();
        if (src != null)
        {
            SNESTile current_tile;
            for (int i = 0; i < src.tiles.Count; i++)
            {
                current_tile = new SNESTile(src.tiles[i]);
                tiles.Add(current_tile);
            }
        }
    }

    public void FromStream(BinaryReader br, int num)
    {
        tiles.Clear();
        if (br != null)
        {
            SNESTile current_tile;
            for (int i = 0; i < num; i++)
            {
                current_tile = new SNESTile(br);
                tiles.Add(current_tile);
            }
        }
    }

    public void ToStream(BinaryWriter bw)
    {
        if (bw != null)
        {
            for (int i = 0; i < tiles.Count; i++)
                tiles[i].ToStream(bw);
        }
    }

    public void FromFile(FileInfo File)
    {
        tiles.Clear();
        if (File.Exists)
        {
            BinaryReader br = new BinaryReader(File.OpenRead());
            FromStream(br, (int)(br.BaseStream.Length / 0x20));
            br.Close();
        }
    }
}

#endregion

#region SNESTileset8
/*
 * Set of SNES 8bpp Tiles
 */
public class SNESTileset8
{
    public Collection<SNESTile8> tiles8 = new Collection<SNESTile8>();

    public SNESTileset8() { }

    public SNESTileset8(BinaryReader br, int num)
    { FromStream(br, num); }

    public SNESTileset8(FileInfo File)
    { FromFile(File); }

    public SNESTileset8(SNESTileset8 src)
    { FromSNESTileset8(src); }

    public void FromSNESTileset8(SNESTileset8 src)
    {
        tiles8.Clear();
        if (src != null)
        {
            SNESTile8 current_tile;
            for (int i = 0; i < src.tiles8.Count; i++)
            {
                current_tile = new SNESTile8(src.tiles8[i]);
                tiles8.Add(current_tile);
            }
        }
    }

    public void FromStream(BinaryReader br, int num)
    {
        tiles8.Clear();
        if (br != null)
        {
            SNESTile8 current_tile;
            for (int i = 0; i < num; i++)
            {
                current_tile = new SNESTile8(br);
                tiles8.Add(current_tile);
            }
        }
    }

    public void ToStream(BinaryWriter bw)
    {
        if (bw != null)
        {
            for (int i = 0; i < tiles8.Count; i++)
            { }//tiles[i].ToStream(bw);
        }
    }

    public void FromFile(FileInfo File)
    {
        tiles8.Clear();
        if (File.Exists)
        {
            BinaryReader br = new BinaryReader(File.OpenRead());
            FromStream(br, (int)(br.BaseStream.Length / 0x40));
            br.Close();
        }
    }
}

#endregion

#region SNESExLoROMAddress
/*
 * SNES ExLoROM Address
 */
public class SNESExLoROMAddress
{
    byte bnk;
    ushort offs;

    public SNESExLoROMAddress()
    { Set((byte)0x80, (ushort)0x8000); }

    public SNESExLoROMAddress(long pc)
    { FromPCAddress(pc); }

    public SNESExLoROMAddress(byte bank, ushort offset)
    { Set(bank, offset); }

    public SNESExLoROMAddress(SNESExLoROMAddress src)
    {
        if (src != null) Set(src.GetBank(), src.GetOffset());
    }

    public SNESExLoROMAddress(BinaryReader src)
    {
        this.FromStream(src);
    }

    public override string ToString()
    {
        return "$" + bnk.ToString("X") + ":" + offs.ToString("X");
    }

    public ulong Get() { return (ulong)((bnk << 16) | offs); }
    public byte GetBank() { return bnk; }

    public void SetBank(byte bank) { bnk = bank; }
    public void SetOffset(ushort offset) { offs = (ushort)(offset); }

    public ushort GetOffset() { return offs; }

    public void Set(byte bank, ushort offset)
    { bnk = bank; offs = (ushort)(offset); }

    public void Set(ulong abs)
    {
        bnk = (byte)((abs >> 16) & 0xFF);
        offs = (ushort)((abs & 0xFFFF));
    }

    public void FromStream(BinaryReader br)
    {
        offs = (ushort)(br.ReadUInt16());
        bnk = br.ReadByte();
    }

    public void ToStream(BinaryWriter dest)
    {
        dest.Write(offs);
        dest.Write(bnk);
    }

    public void FromPCAddress(long pc)
    {
        byte bnk_pc = (byte)((pc >> 16) & 0xFF);
        ushort offs_pc = (ushort)(pc & 0xFFFF);

        if (bnk_pc <= 0x7E)
        {
            if ((bnk_pc & 0x40) != 0)
                bnk = (byte)(((bnk_pc & 0x3F) << 1) | ((offs_pc & 0x8000) >> 0xF));
            else
                bnk = (byte)(0x80 | (bnk_pc << 1) | ((offs_pc & 0x8000) >> 0xF));


            offs = (ushort)(0x8000 | (offs_pc & 0x7FFF));
        }
    }

    public long ToPCAddress()
    {
        byte bnk_pc;
        ushort offs_pc;

        if ((bnk & 0x80) != 0)
            bnk_pc = (byte)((bnk & 0x7F) >> 1);
        else
            bnk_pc = (byte)((bnk | 0x80) >> 1);

        offs_pc = (ushort)((offs & 0x7FFF) | ((bnk & 1) << 0xF));

        return (long)((bnk_pc << 16) | offs_pc);
    }
}
#endregion

#region BMP4bppIO
/*
    4bpp Bitmap In/Out
*/
public class BMP4bppIO
{
    public static SNESPalette ImportPalette(FileInfo File)
    {
        if (File.Exists)
        {
            SNESPalette pal = new SNESPalette();

            BinaryReader br = new BinaryReader(File.OpenRead());

            br.BaseStream.Seek(0x1C, SeekOrigin.Begin);
            short bpp = br.ReadInt16();

            if (bpp == 4)
            {
                br.BaseStream.Seek(0x2E, SeekOrigin.Begin);
                int ncolors = br.ReadInt32();

                if (ncolors == 0x00 || ncolors == 0x10)
                {
                    br.BaseStream.Seek(0x36, SeekOrigin.Begin);

                    //Read palette
                    uint col32;
                    Color c;
                    for (byte i = 0; i < 0x10; i++)
                    {
                        col32 = br.ReadUInt32();
                        c = Color.FromArgb((int)((col32 >> 16) & 0xFF),
                                         (int)((col32 >> 8) & 0xFF),
                                         (int)(col32 & 0xFF));

                        pal.SetColor(i, c);
                    }

                    return pal;
                }
            }

            br.Close();
        }

        return null;
    }

    public static Rectangle GetDimensions(FileInfo File)
    {
        Rectangle rResult = new Rectangle(0, 0, 0, 0);
        if (File.Exists)
        {
            BinaryReader br = new BinaryReader(File.OpenRead());

            br.BaseStream.Seek(0x12, SeekOrigin.Begin);
            rResult.Width = br.ReadInt32();
            rResult.Height = br.ReadInt32();

            br.Close();
        }
        return rResult;
    }

    public static bool Import2DArray(FileInfo File, byte[,] dst)
    {
        if (File.Exists)
        {
            BinaryReader br = new BinaryReader(File.OpenRead());

            br.BaseStream.Seek(0x1C, SeekOrigin.Begin);
            short bpp = br.ReadInt16();

            if (bpp == 4)
            {
                br.BaseStream.Seek(0x12, SeekOrigin.Begin);
                int w = br.ReadInt32();
                int h = br.ReadInt32();

                if (dst.GetUpperBound(0) >= (w - 1) &&
                   dst.GetUpperBound(1) >= (h - 1))
                {
                    br.BaseStream.Seek(0xA, SeekOrigin.Begin);
                    uint offs = br.ReadUInt32();

                    br.BaseStream.Seek((long)offs, SeekOrigin.Begin);

                    byte c = 0;
                    for (int y = h - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < w; x += 2)
                        {
                            c = br.ReadByte();

                            dst[x, y] = (byte)((c >> 4) & 0xF);
                            dst[x + 1, y] = (byte)(c & 0xF);
                        }
                    }

                    return true;
                }
            }

            br.Close();
        }

        return false;
    }

    public static void Export2DArray(FileInfo File, byte[,] src, SNESPalette pal, int w, int h)
    {
        //Create file
        if (File.Exists)
            File.Delete();

        BinaryWriter bw = new BinaryWriter(File.OpenWrite());

        //Write header
        bw.Write((ushort)0x4D42);			//"BM"
        bw.Write((int)(0x76 + ((w * h) >> 1)));	//File Size
        bw.Write((uint)0x0);				//Reserved
        bw.Write((uint)0x76);				//Data offset
        bw.Write((int)0x28);				//Info size
        bw.Write(w);						//Width
        bw.Write(h);						//Height
        bw.Write((short)0x1);				//Planes
        bw.Write((short)0x4);				//BPP
        bw.Write((int)0x0);					//Compression
        bw.Write((int)0x0);					//Image size
        bw.Write((int)0x0);					//Pixels per meter X
        bw.Write((int)0x0);					//Pixels per meter Y
        bw.Write((int)0x10);				//NumColors
        bw.Write((int)0x0);					//NumImportantColors

        //Write palette
        Color c;
        for (byte i = 0; i < 0x10; i++)
        {
            c = pal.GetColor(i);
            bw.Write((uint)((c.R << 16) | (c.G << 8) | c.B));
        }

        //Write bitmap
        for (int y = h - 1; y >= 0; y--)
        {
            for (int x = 0; x < w; x += 2)
            {
                bw.Write((byte)((src[x, y] << 4) | src[x + 1, y]));
            }
        }

        bw.Close();
    }
}
#endregion
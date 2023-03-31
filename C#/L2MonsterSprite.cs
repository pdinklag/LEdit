using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System;

namespace LEdit
{
    public class L2MonsterSprite : Content
    {
        #region STATIC
        public const int InitialCount = 0x86;
        public const long DimensionTableAddress = 0x1AD140;
        public const string DimensionFileExt = ".d";
        public const int FileIDOffset = 0x1B8;

        public static Collection<L2MonsterSprite> Array = new Collection<L2MonsterSprite>();

        public static void ExtractAll(ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Extrahiere MonsterSprites...");
                progress.SetMaxProgress(InitialCount);
                progress.Reset();
            }

            FileStream ROMStream = new FileStream(Project.ROMFile.FullName, FileMode.Open, FileAccess.Read);
            BinaryReader ROMReader = new BinaryReader(ROMStream);

            Project.GetOriginalFolder(ContentType.MonsterSprite).Create();
            for (int i = 0; i < InitialCount; i++)
            {
                FileInfo File = Project.RequestOriginalSource(ContentType.MonsterSprite, i);

                if (File.Exists)
                    File.Delete();

                ROMStream.Seek(DimensionTableAddress + (i << 1), SeekOrigin.Begin);
                ushort dim = ROMReader.ReadUInt16();

                L2ROM.ExtractL2File(ROMStream, (ushort)(i + FileIDOffset), File.FullName);

                BinaryWriter FileWriter = new BinaryWriter(
                    new FileInfo(File.FullName + DimensionFileExt).OpenWrite());

                FileWriter.Write(dim);
                FileWriter.Close();

                if (progress != null)
                    progress.Progress(1);
            }

            ROMStream.Close();
        }

        public static void StaticInit(ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Lade MonsterSprites...");
                progress.SetMaxProgress(InitialCount);
                progress.Reset();
            }

            for (int i = 0; i < InitialCount; i++)
            {
                FileInfo File = Project.RequestSource(ContentType.MonsterSprite, i);

                if (!File.Exists)
                {
                    //TODO: Error!
                }

                L2MonsterSprite MonsterSprite = new L2MonsterSprite();
                MonsterSprite.ID = i;
                MonsterSprite.Load(File);
                Array.Add(MonsterSprite);

                if (progress != null)
                    progress.Progress(1);
            }

            PrepareContentSelectionDialog();
        }

        public static void StaticUnload()
        {

        }

        public static void PrepareContentSelectionDialog()
        {
            ContentSelectionDialog dlg;

            try
            {
                dlg = ContentSelectionDialog.Map[ContentType.MonsterSprite];
                dlg.Dispose();
            }
            catch (Exception)
            {
            }

            dlg = new ContentSelectionDialog("MonsterSprite auswählen");
            foreach (L2MonsterSprite monsprite in Array)
                dlg.AddItem(monsprite);

            ContentSelectionDialog.Map.Remove(ContentType.MonsterSprite);
            ContentSelectionDialog.Map.Add(ContentType.MonsterSprite, dlg);
        }

        public static void Compile(FileStream rom, ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Füge MonsterSprites ein...");
                progress.SetMaxProgress(Array.Count);
                progress.Reset();
            }

            BinaryWriter romWriter = new BinaryWriter(rom);

            for (int i = 0; i < Array.Count; i++)
            {
                FileInfo src = Project.HasMod(ContentType.MonsterSprite, i);
                if (src != null)
                {
                    L2ROM.InsertL2File(rom, (ushort)(i + FileIDOffset), src);

                    BinaryReader DimensionReader = new BinaryReader(
                        new FileInfo(src.FullName + DimensionFileExt).OpenRead());

                    rom.Seek(DimensionTableAddress + (i << 1), SeekOrigin.Begin);
                    romWriter.Write(DimensionReader.ReadUInt16());

                    DimensionReader.Close();
                }

                if (progress != null)
                    progress.Progress(1);
            }
        }

        #endregion

        public byte TilesX;
        public byte TilesY;
        public byte Unknown;

        public Collection<SNESPalette> Palettes = new Collection<SNESPalette>();
        public SNESTileset Tileset;

        private bool Drawn;
        private Collection<Bitmap> Bitmaps = new Collection<Bitmap>();

        public Bitmap GetBitmap(int Palette)
        {
            if (!Drawn)
                Redraw();

            if (Palette >= 0 && Palette < Bitmaps.Count)
                return Bitmaps[Palette];
            else
                return null;
        }

        ~L2MonsterSprite()
        {
            DisposeOfBitmaps();
        }

        public L2MonsterSprite()
        {
        }

        public L2MonsterSprite(BinaryReader br)
        {
            FromStream(br);
        }

        public override string GetName()
        {
            return Content.FormatID(ID);
        }

        public override Bitmap GetIcon()
        {
            return GetBitmap(0);
        }

        private void DimensionFromStream(BinaryReader br)
        {
            TilesX = br.ReadByte();

            byte y = br.ReadByte();
            Unknown = (byte)(y & 0xE0);
            TilesY = (byte)(y & 0x1F);
        }

        private void DimensionToStream(BinaryWriter bw)
        {
            bw.Write((byte)TilesX);
            bw.Write((byte)(TilesY | Unknown));
        }

        public override void Load(FileInfo File)
        {
            BinaryReader br = new BinaryReader(new FileInfo(File.FullName + DimensionFileExt).OpenRead());
            DimensionFromStream(br);
            br.Close();

            br = new BinaryReader(File.OpenRead());
            FromStream(br);
            br.Close();
        }

        public override void FromStream(BinaryReader br)
        {
            if (TilesX == 0 || TilesY == 0)
                return;

            byte NumPals = br.ReadByte();

            Palettes.Clear();
            for (int i = 0; i < NumPals; i++)
                Palettes.Add(new SNESPalette(br));

            int NumTiles = TilesX * TilesY;
            Tileset = new SNESTileset(br, NumTiles);

            DisposeOfBitmaps();
        }

        public override void Save(FileInfo File)
        {
            BinaryWriter bw = new BinaryWriter(new FileInfo(File.FullName + DimensionFileExt).OpenWrite());
            DimensionToStream(bw);
            bw.Close();

            bw = new BinaryWriter(File.OpenWrite());
            ToStream(bw);
            bw.Close();
        }

        public override void ToStream(BinaryWriter bw)
        {
            bw.Write((byte)Palettes.Count);
            for (int i = 0; i < Palettes.Count; i++)
                Palettes[i].ToStream(bw);

            Tileset.ToStream(bw);
        }

        public void AddPalette(SNESPalette pal)
        {
            Palettes.Add(pal);
            Redraw();
        }

        public void RemovePalette(int n)
        {
            if (Palettes.Count > 1 && n >= 0 && n < Palettes.Count)
            {
                Palettes.RemoveAt(n);
                Redraw();
            }
        }

        public void SetPalette(int n, SNESPalette pal)
        {
            if (n >= 0 && n < Palettes.Count)
            {
                Palettes[n] = pal;

                Redraw();
            }
        }

        public void Draw(Graphics dst, byte paln, int dx, int dy)
        {
            if (!Drawn)
                Redraw();

            if (paln >= 0 && paln < Palettes.Count)
                dst.DrawImage(Bitmaps[paln], new Point(dx, dy));
        }

        public void ExportPNG(FileInfo File, int Palette)
        {
            if (!Drawn)
                Redraw();

            if (Palette >= 0 && Palette < Bitmaps.Count)
            {
                if (File.Exists)
                    File.Delete();

                FileStream stm = File.Create();

                if (Bitmaps.Count > 0)
                    Bitmaps[Palette].Save(stm, ImageFormat.Png);

                stm.Close();
            }
        }

        private void DisposeOfBitmaps()
        {
            if (!Drawn)
                return;

            for (int p = 0; p < Palettes.Count; p++)
            {
                if (Bitmaps[p] != null)
                    Bitmaps[p].Dispose();
            }
            Bitmaps.Clear();

            Drawn = false;
        }

        public void Redraw()
        {
            DisposeOfBitmaps(); 

            for (int p = 0; p < Palettes.Count; p++)
            {
                Bitmap current_bmp = new Bitmap(TilesX << 3, TilesY << 3, PixelFormat.Format32bppArgb);
                Bitmaps.Add(current_bmp);
            }

            for (int cBY = 0; cBY < (TilesY >> 1); cBY++)
            {
                for (int cBX = 0; cBX < (TilesX >> 1); cBX++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            int t = ((y + (cBX << 1) + cBY * TilesX) << 1) + x;

                            for (int p = 0; p < Palettes.Count; p++)
                            {
                                Graphics g = Graphics.FromImage(Bitmaps[p]);

                                Tileset.tiles[t].Draw(
                                    g,
                                    Palettes[p],
                                    (x + (cBX << 1)) << 3,
                                    (y + (cBY << 1)) << 3,
                                    1, true);
                            }
                        }
                    }
                }
            }

            Drawn = true;
        }

        public bool ImportBMP(FileInfo File)
        {
            Rectangle r = BMP4bppIO.GetDimensions(File);
            if (r.Width > 0 && r.Width <= 128 && (r.Width % 8 == 0) &&
                r.Height > 0 && r.Height <= 128 && (r.Height % 8 == 0))
            {
                TilesX = (byte)(r.Width / 8);
                TilesY = (byte)(r.Height / 8);

                byte[,] p = new byte[r.Width, r.Height];
                if (BMP4bppIO.Import2DArray(File, p))
                {
                    Tileset.tiles.Clear();

                    for (int cBY = 0; cBY < (TilesY >> 1); cBY++)
                    {
                        for (int cBX = 0; cBX < (TilesX >> 1); cBX++)
                        {
                            for (int y = 0; y < 2; y++)
                            {
                                for (int x = 0; x < 2; x++)
                                {
                                    int t = ((y + (cBX << 1) + cBY * TilesX) << 1) + x;

                                    SNESTile tile = new SNESTile();
                                    tile.From2DArray(
                                        p,
                                        (x + (cBX << 1)) << 3,
                                        (y + (cBY << 1)) << 3);

                                    Tileset.tiles.Add(tile);
                                }
                            }
                        }
                    }

                    Redraw();
                    return true;
                }
            }

            return false;
        }

        public void ExportBMP(FileInfo File, int palette)
        {
            byte[,] p = new byte[TilesX << 3, TilesY << 3];

            for (int cBY = 0; cBY < (TilesY >> 1); cBY++)
            {
                for (int cBX = 0; cBX < (TilesX >> 1); cBX++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        for (int x = 0; x < 2; x++)
                        {
                            int t = ((y + (cBX << 1) + cBY * TilesX) << 1) + x;

                            Tileset.tiles[t].To2DArray(
                                p,
                                (x + (cBX << 1)) << 3,
                                (y + (cBY << 1)) << 3);
                        }
                    }
                }
            }

            //Save
            BMP4bppIO.Export2DArray(
                File, p, Palettes[(int)palette], TilesX << 3, TilesY << 3);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace LEdit
{
    /*
     * Lufia 2 - Monster Class
     */
    public class L2Monster : Content
    {
        #region STATIC
        public const int InitialCount = 0xE0;
        public const long TableAddress = 0x1A05C0;
        public const long TableAddress_LPatch = 0x408000;

        public static Collection<L2Monster> Array = new Collection<L2Monster>();

        public static void ExtractAll(ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Extrahiere Monster...");
                progress.SetMaxProgress(InitialCount);
                progress.Reset();
            }

            BinaryReader rom = new BinaryReader(
                new FileStream(Project.ROMFile.FullName, FileMode.Open, FileAccess.Read));

            Project.GetOriginalFolder(ContentType.Monster).Create();
            for (int i = 0; i < InitialCount; i++)
            {
                FileInfo file = Project.RequestOriginalSource(ContentType.Monster, i);

                if (file.Exists)
                    file.Delete();

                rom.BaseStream.Seek(TableAddress + (i << 1), SeekOrigin.Begin);
                rom.BaseStream.Seek(TableAddress + (long)rom.ReadUInt16(), SeekOrigin.Begin);

                new L2Monster(rom).Save(file);

                if(progress != null)
                    progress.Progress(1);
            }

            rom.Close();
        }

        public static void StaticInit(ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Lade Monster...");
                progress.SetMaxProgress(InitialCount);
                progress.Reset();
            }

            for (int i = 0; i < InitialCount; i++)
            {
                FileInfo file = Project.RequestSource(ContentType.Monster, i);

                if (!file.Exists)
                {
                    //TODO: Error!
                }

                L2Monster monster = new L2Monster();
                monster.ID = i;
                monster.Load(file);
                Array.Add(monster);

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
                dlg = ContentSelectionDialog.Map[ContentType.Monster];
                dlg.Dispose();
            }
            catch (Exception)
            {
            }

            dlg = new ContentSelectionDialog("Monster auswählen");
            foreach (L2Monster monster in Array)
                dlg.AddItem(monster);

            ContentSelectionDialog.Map.Remove(ContentType.Monster);
            ContentSelectionDialog.Map.Add(ContentType.Monster, dlg);
        }

        public static void Compile(FileStream rom, ProgressForm progress)
        {
            if (progress != null)
            {
                progress.SetText("Füge Monster ein...");
                progress.SetMaxProgress(Array.Count);
                progress.Reset();
            }

            BinaryWriter romWriter = new BinaryWriter(rom);

            long offset = Array.Count << 1;
            for (int i = 0; i < Array.Count; i++)
            {
                rom.Seek(TableAddress_LPatch + (i << 1), SeekOrigin.Begin);
                romWriter.Write((ushort)offset);

                rom.Seek(TableAddress_LPatch + offset, SeekOrigin.Begin);

                BinaryReader src = new BinaryReader(new FileStream(
                    Project.RequestSource(ContentType.Monster, i).FullName,
                    FileMode.Open, FileAccess.Read));

                romWriter.Write(src.ReadBytes((int)src.BaseStream.Length));
                src.Close();

                offset = rom.Position - TableAddress_LPatch;

                if (progress != null)
                    progress.Progress(1);
            }
        }

        /*
        public static void SaveAllToROM()
        {
            if (File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                int sz = (int)(fs.Length - 1);
                if (sz <= 0x8000)
                {
                    BinaryReader br = new BinaryReader(fs);
                    br.BaseStream.Seek(0x01, SeekOrigin.Begin);
                    byte[] data = br.ReadBytes(sz);

                    br.Close();
                    fs.Close();

                    fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Seek((int)TableOffset_LPatch, SeekOrigin.Begin);
                    bw.Write(data, 0, sz);

                    bw.Close();
                    fs.Close();
                }
                else
                    MessageBox.Show("Die Monsterdaten übersteigen die maximale Größe von 32KB!",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);


            }
        }
         */

        #endregion

        public L2String Name;
        public byte Level;
        public byte Unknown1;

        public int MonsterSpriteID;

        public byte Palette;

        public int HP;
        public int MP;
        public int ANG;
        public int ABW;

        public byte BWG;
        public byte INT;
        public byte MUT;
        public byte MGA;

        public int Experience;
        public int Gold;

        public int GiftItem = -1;
        public int GiftProbability;

        private ushort offset_ex7, offset_ex8;

        public L2BattleScript AttackScript = new L2BattleScript();
        public L2BattleScript DefendScript = new L2BattleScript();

        public L2Monster()
        {
        }

        public L2Monster(BinaryReader br)
        {
            FromStream(br);
        }

        public override string GetName()
        {
            return Name.Value.TrimEnd();
        }

        public override Bitmap GetIcon()
        {
            return L2MonsterSprite.Array[MonsterSpriteID - 1].GetBitmap(Palette);
        }

        public override void FromStream(BinaryReader br)
        {
            if (br != null)
            {
                long address = br.BaseStream.Position;

                Name = new L2String(br, 0xD);

                Level = br.ReadByte();
                Unknown1 = br.ReadByte();

                MonsterSpriteID = (int)br.ReadByte();
                //TODO: Sprite = L2MonsterSprite.Get(SpriteFile);

                Palette = br.ReadByte();

                HP = br.ReadUInt16();
                MP = br.ReadUInt16();
                ANG = br.ReadUInt16();
                ABW = br.ReadUInt16();
                BWG = br.ReadByte();
                INT = br.ReadByte();
                MUT = br.ReadByte();
                MGA = br.ReadByte();
                Experience = br.ReadUInt16();
                Gold = br.ReadUInt16();

                //Extra Data - CODE REPLICATION
                offset_ex7 = 0;
                offset_ex8 = 0;

                for (byte b = br.ReadByte(); b != 0; b = br.ReadByte())
                {
                    if (b == 0x03)
                    {
                        GiftItem = (byte)br.ReadByte();
                        GiftProbability = (int)br.ReadByte();

                        if ((GiftProbability & 0x01) != 0)
                        {
                            GiftItem |= 0x100;
                            GiftProbability &= 0xFE;
                        }
                    }
                    else if (b == 0x07)
                        offset_ex7 = br.ReadUInt16();
                    else if (b == 0x08)
                        offset_ex8 = br.ReadUInt16();
                    else
                    {
                        Console.WriteLine("??? - 0x" + b.ToString("X2"));
                        br.ReadUInt16();    //skip, unknown
                    }
                }

                //ex7 - ATTACK SCRIPT
                if (offset_ex7 != 0)
                {
                    br.BaseStream.Seek(address + (long)offset_ex7, SeekOrigin.Begin);
                    AttackScript = new L2BattleScript(br, offset_ex7);
                }

                //ex8 - DEFEND SCRIPT
                if (offset_ex8 != 0)
                {
                    br.BaseStream.Seek(address + (long)offset_ex8, SeekOrigin.Begin);
                    DefendScript = new L2BattleScript(br, offset_ex8);
                }
            }
        }

        public override void Load(FileInfo File)
        {
            BinaryReader br = new BinaryReader(File.OpenRead());

            FromStream(br);

            br.Close();
        }

        public override void ToStream(BinaryWriter bw)
        {
            ushort offset = 0;
            ushort next_free_offset = 0;

            Name.ToStream(bw, 0x0D, false);

            bw.Write(Level);
            bw.Write(Unknown1);
            bw.Write((byte)MonsterSpriteID);
            bw.Write(Palette);
            bw.Write((ushort)HP);
            bw.Write((ushort)MP);
            bw.Write((ushort)ANG);
            bw.Write((ushort)ABW);
            bw.Write((byte)BWG);
            bw.Write((byte)INT);
            bw.Write((byte)MUT);
            bw.Write((byte)MGA);
            bw.Write((ushort)Experience);
            bw.Write((ushort)Gold);

            next_free_offset += 0x21;

            if (GiftItem >= 0)
            {
                bw.Write((byte)0x03);

                int item = GiftItem;
                int probability = GiftProbability;

                if ((item & 0x100) != 0)
                {
                    item &= 0xFF;
                    probability |= 0x01;
                }

                bw.Write((byte)item);
                bw.Write((byte)probability);

                next_free_offset += 0x03;
            }

            ushort as_size = (ushort)AttackScript.Bytes.Count;
            ushort ds_size = (ushort)DefendScript.Bytes.Count;

            ushort as_offset = 0;
            if (as_size > 0)
            {
                as_offset = (ushort)(next_free_offset + 0x04);

                if (ds_size > 0)
                    as_offset += 0x03;

                AttackScript.Offset = as_offset - offset;

                bw.Write((byte)0x07);
                bw.Write((ushort)(as_offset - offset));
                next_free_offset += 0x03;
            }

            ushort ds_offset = 0;
            if (ds_size > 0)
            {
                ds_offset = (ushort)(next_free_offset + 0x04 + as_size);

                DefendScript.Offset = ds_offset - offset;

                bw.Write((byte)0x08);
                bw.Write((ushort)(ds_offset - offset));
                next_free_offset += 0x03;
            }

            bw.Write((byte)0x00);
            next_free_offset += 0x01;

            for (int k = 0; k < as_size; k++)
                bw.Write(AttackScript.Bytes[k]);

            for (int k = 0; k < ds_size; k++)
                bw.Write(DefendScript.Bytes[k]);

            next_free_offset += (ushort)(as_size + ds_size);
        }

        public override void Save(FileInfo File)
        {
            BinaryWriter bw = new BinaryWriter(File.OpenWrite());

            ToStream(bw);
            bw.Close();
        }
    }
}

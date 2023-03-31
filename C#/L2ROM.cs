using System;
using System.Collections.ObjectModel;
using System.IO;

namespace LEdit
{
    public class L2ROM
    {
        public const int L2FileCount = 0x2A8;
        public const long L2FileTableAddress = 0x1B8000;
        public const long L2FileTableAddressLPatch = 0x300080;

        public class L2ROMFragment
        {
            public long Address;
            public long Size;

            public L2ROMFragment(long x, long sz)
            {
                Address = x;
                Size = sz;
            }
        }
        public static Collection<L2ROMFragment> Fragments = new Collection<L2ROMFragment>();

        //=== Current File ===
        [Obsolete("New system")]
        public static string file_name;
        [Obsolete("New system")]
        public static string FileName { get { return file_name; } }

        //=== Make directories ===
        [Obsolete("New system")]
        static Collection<string> mkdir = new Collection<string>();

        //=== MapSprite Names ===
        static string[] MSName = new string[L2MapSprite.Count];

        public static string GetMapSpriteName(int n)
        {
            if (n >= 0 && n < L2MapSprite.Count) return MSName[n];
            else return string.Empty;
        }

        //=== BattleAnim Names ===
        static string[] BAnimName = new string[0x100];

        public static string GetBattleAnimName(int n)
        {
            if (n >= 0 && n < L2MapSprite.Count)
                return BAnimName[n];
            else
                return string.Empty;
        }

        //=== World Map ===
        public static L2WorldMap world;

        public static L2WorldMap GetWorld()
        {
            return world;
        }

        //=== Init ===
        public static void InitLPatchFragments()
        {
            Fragments.Clear();
            Fragments.Add(new L2ROMFragment(0x600000, 0x200000)); //2 MB
        }

        //=== Apply LPatch to a ROM ===
        public static bool ApplyLPatch(string FileName)
        {
            if(!File.Exists(FileName))
                return false;

            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            BinaryWriter bw = new BinaryWriter(fs);

            //MAKE EXLOROM
            //Set Flag
            bw.BaseStream.Seek(0x7FD7, SeekOrigin.Begin);
            bw.Write((byte)0xD);

            //Copy header bank
            fs.Seek(0, SeekOrigin.Begin);
            byte[] header = br.ReadBytes(0x8000);
            fs.Seek(0x400000, SeekOrigin.Begin);
            bw.Write(header);

            //Expand to 8MB, last two bytes are "pd" ;)
            fs.Seek(0x7FFFFE, SeekOrigin.Begin);
            bw.Write((ushort)0x6470);

            //LOADING UNCOMPRESSED FILES
            //Hook
            fs.Seek(0xECF, SeekOrigin.Begin);
            bw.Write((uint)0xE080005C);
            fs.Seek(0xF52, SeekOrigin.Begin);
            bw.Write((uint)0xE0802C5C);
            fs.Seek(0xFF6, SeekOrigin.Begin);
            bw.Write((uint)0xE080485C);
            //Install
            fs.Seek(0x300000, SeekOrigin.Begin);
            bw.Write((ulong)0xC818D075C95DB75A);
            bw.Write((ulong)0xB7C811D06EC95DB7);
            bw.Write((ulong)0x8501A90AD070C95D);
            bw.Write((ulong)0x640380C8C8C87A5C);
            bw.Write((ulong)0xD35C58855DB77A5C);
            bw.Write((ulong)0x000000000000808E);
            fs.Seek(0x30002C, SeekOrigin.Begin);
            bw.Write((ulong)0x6506680DD05CA548);
            bw.Write((ulong)0x225C808F565C0490);
            bw.Write((ulong)0x00808F105C68808F);
            fs.Seek(0x300048, SeekOrigin.Begin);
            bw.Write((ulong)0x6506680DD05CA548);
            bw.Write((ulong)0xC65C808FFA5C0490);
            bw.Write((ulong)0x00808FB45C68808F);

            //MOVE FILE TABLE
            //Code hack
            fs.Seek(0xEBC, SeekOrigin.Begin);
            bw.Write((uint)0x09E00080);
            fs.Seek(0xEC4, SeekOrigin.Begin);
            bw.Write((uint)0x0AE00081);

            //Copy table
            fs.Seek(0x1B8000, SeekOrigin.Begin);
            byte[] file_table = br.ReadBytes(0x7FE);
            fs.Seek(0x300080, SeekOrigin.Begin);
            bw.Write(file_table, 0, 0x7FE);

            //MOVE MONSTER DATA
            //Code hack
            fs.Seek(0xFC00, SeekOrigin.Begin);
            bw.Write((uint)0x018000BF);
            fs.Seek(0xFC05, SeekOrigin.Begin);
            bw.Write((uint)0x01800FBF);
            fs.Seek(0xFC17, SeekOrigin.Begin);
            bw.Write((uint)0x018000BF);
            fs.Seek(0xFC1E, SeekOrigin.Begin);
            bw.Write((uint)0x01801DBF);
            fs.Seek(0xFC25, SeekOrigin.Begin);
            bw.Write((uint)0x01801EBF);
            fs.Seek(0xFC2C, SeekOrigin.Begin);
            bw.Write((uint)0x01801FBF);
            fs.Seek(0xFC33, SeekOrigin.Begin);
            bw.Write((uint)0x018020BF);
            fs.Seek(0xFC4B, SeekOrigin.Begin);
            bw.Write((byte)0x01);
            fs.Seek(0xFC63, SeekOrigin.Begin);
            bw.Write((ushort)0x8000);
            fs.Seek(0xFC67, SeekOrigin.Begin);
            bw.Write((ushort)0x8000);
            fs.Seek(0xFD1F, SeekOrigin.Begin);
            bw.Write((byte)0x01);
            fs.Seek(0xFD25, SeekOrigin.Begin);
            bw.Write((ushort)0x8000);
            fs.Seek(0xFD29, SeekOrigin.Begin);
            bw.Write((ushort)0x8000);
            fs.Seek(0xB20F, SeekOrigin.Begin);
            bw.Write((byte)0x01);

            //Copy data
            fs.Seek(0x1A05C0, SeekOrigin.Begin);
            byte[] monster_data = br.ReadBytes(0x49A9);
            fs.Seek(0x408000, SeekOrigin.Begin);
            bw.Write(monster_data, 0, 0x49A9);

            //MOVE MAP HEADERS
            for (int i = 0; i < L2MapHeader.Count; i++)
            {
                //Read old address
                fs.Seek(L2MapHeader.InfoTableOffset + i * 3, SeekOrigin.Begin);

                SNESExLoROMAddress addr = new SNESExLoROMAddress(br);
                long offs = addr.ToPCAddress();

                //Write new address
                fs.Seek(L2MapHeader.InfoTableOffset + i * 3, SeekOrigin.Begin);

                long noffs = L2MapHeader.NewInfoOffset + i * 0x600;
                SNESExLoROMAddress naddr = new SNESExLoROMAddress(noffs);

                naddr.ToStream(bw);

                //Copy data to new address
                fs.Seek(offs, SeekOrigin.Begin);

                ushort sz = br.ReadUInt16();
                fs.Seek(offs, SeekOrigin.Begin);

                byte[] data = br.ReadBytes(sz);

                fs.Seek(noffs, SeekOrigin.Begin);
                bw.Write(data);
            }

            fs.Close();

            return true;
        }

        public static string ExtractL2File(FileInfo ROMFile, ushort FileNumber, string DestFileName, bool bOverride)
        {
            FileStream ROMStream = ROMFile.OpenRead();

            string result = ExtractL2File(ROMStream, FileNumber, DestFileName);

            ROMStream.Close();

            return result;
        }

        public static string ExtractL2File(FileStream ROMStream, ushort FileNumber, string DestFileName)
        {
            //Open destination
            //if (DestFileName == string.Empty)
            //    DestFileName = "files\\" + Files[FileNumber].FileName;
            BinaryReader ROMReader = new BinaryReader(ROMStream);

            if (File.Exists(DestFileName))
                File.Delete(DestFileName);

            FileStream dst_fs = new FileStream(DestFileName, FileMode.CreateNew, FileAccess.ReadWrite);
            BinaryWriter dst = new BinaryWriter(dst_fs);
            BinaryReader dstr = new BinaryReader(dst_fs);

            //Setup
            dst.BaseStream.Seek(0, SeekOrigin.Begin);

            //Get source offset
            ROMStream.Seek(L2FileTableAddress + FileNumber * 3, SeekOrigin.Begin);
            ROMStream.Seek(
                L2FileTableAddress +
                ROMReader.ReadUInt16() + (ROMReader.ReadByte() << 16),
                SeekOrigin.Begin);

            //Variables
            ushort x65, x66;
            int nWritten, nToWrite;

            //Get file size
            nToWrite = (int)(ROMReader.ReadUInt16());
            nWritten = 0;

            //Load x65 and x66...
            x65 = ROMReader.ReadByte();
            x66 = 0x8;

            byte b;
            ushort L;
            ushort copyOffset;
            long write, copy;
            int copyAmount;

            //Load next byte
            while (nWritten < nToWrite)
            {
                //??????????? when and WHY did I do this?!
                //if (dst.BaseStream.Position == 0x1F1)
                //    b = 0;

                b = ROMReader.ReadByte();
                if ((b & 0x80) == 0)
                {
                    dst.Write(b);
                    ++nWritten;
                }
                else
                {
                    L = x65;
                    x65 = (byte)((x65 << 1) & 0xFF);

                    if ((L & 0x80) == 0)
                    {
                        dst.Write(b);
                        ++nWritten;
                    }
                    else
                    {
                        L = (ushort)(b << 8);
                        b = ROMReader.ReadByte();

                        L |= b;
                        b &= 0xF;

                        if (b == 0)
                        {
                            b = ROMReader.ReadByte();

                            ushort x5A = (ushort)(b << 8);

                            copyAmount = (b & 0x3F) + 2;

                            copyOffset = (ushort)(((((L >> 4) | 0xF000) << 1) | ((x5A & 0x8000) >> 15)) & 0xFFFF);
                            x5A = (ushort)((x5A << 1) & 0xFFFF);
                            copyOffset = (ushort)(((copyOffset << 1) | ((x5A & 0x8000) >> 15)) & 0xFFFF);
                            x5A = (ushort)((x5A << 1) & 0xFFFF);
                        }
                        else
                        {
                            copyAmount = b + 1;
                            copyOffset = (ushort)((L >> 4) | 0xF000);
                        }

                        copy = dst.BaseStream.Position +
                            (long)SignConverter.MakeSigned(copyOffset);

                        while (copyAmount >= 0 && nWritten < nToWrite)
                        {
                            write = dst.BaseStream.Position;

                            dstr.BaseStream.Seek(copy, SeekOrigin.Begin);
                            b = dstr.ReadByte();
                            ++copy;

                            dst.BaseStream.Seek(write, SeekOrigin.Begin);
                            dst.Write(b);

                            ++nWritten;
                            --copyAmount;
                        }
                    }

                    if (--x66 == 0)
                    {
                        x65 = ROMReader.ReadByte();
                        x66 = 0x8;
                    }
                }
            }

            //Close destination
            dstr.Close();
            dst.Close();
            dst_fs.Close();

            //Return file name
            return DestFileName;
        }

        //=== Insert File into an LPatched (!!!) ROM ===
        public static bool InsertL2File(FileInfo ROMFile, ushort FileNumber, FileInfo SourceFile)
        {
            FileStream fs_rom = ROMFile.Open(FileMode.Open, FileAccess.ReadWrite);

            bool result = InsertL2File(fs_rom, FileNumber, SourceFile);

            fs_rom.Close();

            return result;
        }

        public static bool InsertL2File(FileStream ROMStream, ushort FileNumber, FileInfo SourceFile)
        {
            bool bResult = false;
            if (FileNumber >= 0 && FileNumber < L2FileCount)
            {
                //if (FileName == string.Empty)
                //    FileName = "files\\" + Files[FileNumber].FileName;

                if (SourceFile.Exists)
                {
                    //Open extracted file
                    BinaryReader SourceFileReader = new BinaryReader(SourceFile.OpenRead());

                    long size = SourceFileReader.BaseStream.Length;
                    long tsize = size + 6;

                    //Find Fragment
                    long dest = 0;
                    for (int i = 0; i < Fragments.Count; i++)
                    {
                        if (Fragments[i].Size >= tsize)
                        {
                            dest = Fragments[i].Address;

                            //New Fragment
                            if (Fragments[i].Size > tsize)
                            {
                                L2ROMFragment fragment = new L2ROMFragment(
                                    Fragments[i].Address + tsize,
                                    Fragments[i].Size - tsize);

                                Fragments.Add(fragment);
                            }

                            Fragments.RemoveAt(i);
                            break;
                        }
                    }

                    if (dest > 0)
                    {
                        Console.WriteLine("Writing to 0x" + dest.ToString("X8"));

                        BinaryReader ROMReader = new BinaryReader(ROMStream);
                        BinaryWriter ROMWriter = new BinaryWriter(ROMStream);

                        //Copy file
                        byte[] buffer = SourceFileReader.ReadBytes((int)size);
                        ROMStream.Seek(dest, SeekOrigin.Begin);

                        ROMWriter.Write('u');  //Signature for unpacked files
                        ROMWriter.Write('n');
                        ROMWriter.Write('p');
                        ROMWriter.Write((ushort)size);
                        ROMWriter.Write((byte)0);
                        ROMWriter.Write(buffer);

                        //Update file table
                        ROMStream.Seek(L2FileTableAddressLPatch + FileNumber * 3, SeekOrigin.Begin);

                        long offset = dest - L2FileTableAddress;
                        byte offset_bank = (byte)((offset >> 16) & 0xFF);
                        ushort offset_addr = (ushort)(offset & 0xFFFF);

                        ROMWriter.Write(offset_addr);
                        ROMWriter.Write(offset_bank);

                        //Success
                        bResult = true;
                    }
                    else
                    {
                        //TODO: ROM is FULL!!! D=
                    }

                    SourceFileReader.Close();
                }
            }

            return bResult;
        }

        //=== Load Lufia2 ROM ===
        /*
        public static bool LoadROM(string fn)
        {
            if (File.Exists(fn))
            {
                file_name = new FileInfo(fn).FullName;

                SNESExLoROMAddress rom_addr = new SNESExLoROMAddress();

                //Open
                BinaryReader br = new BinaryReader(new FileStream(file_name, FileMode.Open, FileAccess.Read));

                //Load ROM Information file
                LoadInfoFile();

                //Init ROM
                L2MonsterAttackName.InitROM(br);
                L2MapSprite.InitROM(br);
                L2MonsterSprite.InitROM(br);

                L2IP.InitROM(br);
                L2Spell.InitROM(br);
                L2Item.InitROM(br);
                L2Monster.InitROM(br);
                L2MonsterHorde.InitROM(br);
                L2MapHeader.InitROM(br);

                L2WorldMapBlock.InitROM(br);
                L2WorldMapBigBlock.InitROM(br);

                //Close
                br.Close();

                world = new L2WorldMap();

                return true;
            }
            return false;
        }
        */

        /*
        public static void LoadInfoFile()
        {
            if (!File.Exists(file_name + ".xml"))
                File.Copy("default.xml", file_name + ".xml");

            if (File.Exists(file_name + ".xml"))
            {
                XmlTextReader xr = new XmlTextReader(file_name + ".xml");

                xr.Read();
                if (xr.NodeType == XmlNodeType.Element && xr.Name == "ledit") //root element
                {
                    for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                    {
                        if (xr.NodeType == XmlNodeType.Element)
                        {
                            L2FileManager.LoadFragments(xr);
                            L2FileManager.LoadFileInfos(xr);

                            //Make directories
                            if (xr.Name == "mkdirs")
                            {
                                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                                {
                                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "mkdir")
                                    {
                                        if (xr.MoveToAttribute("name"))
                                            Directory.CreateDirectory(xr.ReadContentAsString());
                                    }
                                }
                            }

                            //MapSpriteNames
                            if (xr.Name == "msnames")
                            {
                                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                                {
                                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "msname")
                                    {
                                        if (xr.MoveToAttribute("index"))
                                        {
                                            int idx = xr.ReadContentAsInt();

                                            if (idx >= 0 && idx < L2MapSprite.Count)
                                            {
                                                xr.MoveToAttribute("value");
                                                MSName[idx] = xr.ReadContentAsString();
                                            }
                                        }
                                    }
                                }
                            }

                            //BattleAnimNames
                            if (xr.Name == "banimnames")
                            {
                                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                                {
                                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "banimname")
                                    {
                                        if (xr.MoveToAttribute("index"))
                                        {
                                            int idx = xr.ReadContentAsInt();

                                            if (idx >= 0 && idx < 0x100)
                                            {
                                                xr.MoveToAttribute("value");
                                                BAnimName[idx] = xr.ReadContentAsString();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                xr.Close();
            }
        }
         */

        /*
        public static void SaveInfoFile()
        {
            XmlTextWriter xw = new XmlTextWriter(file_name + ".xml", Encoding.Unicode);

            xw.WriteStartElement("ledit");
            {
                //Save MKDirs
                xw.WriteWhitespace("\r\n\t");
                xw.WriteStartElement("mkdirs");
                xw.WriteWhitespace("\r\n");
                for (int i = 0; i < mkdir.Count; i++)
                {
                    xw.WriteWhitespace("\t\t");
                    xw.WriteStartElement("mkdir");
                    xw.WriteStartAttribute("name");
                    xw.WriteValue(mkdir[i]);
                    xw.WriteEndElement();
                    xw.WriteWhitespace("\r\n");
                }
                xw.WriteWhitespace("\t");
                xw.WriteEndElement();
                xw.WriteWhitespace("\r\n");

                //Save MSNames
                xw.WriteWhitespace("\r\n\t");
                xw.WriteStartElement("msnames");
                xw.WriteWhitespace("\r\n");
                for (int i = 0; i < L2MapSprite.Count; i++)
                {
                    xw.WriteWhitespace("\t\t");
                    xw.WriteStartElement("msname");
                    xw.WriteStartAttribute("index");
                    xw.WriteValue(i);
                    xw.WriteStartAttribute("hex");
                    xw.WriteValue("0x" + i.ToString("X2"));
                    xw.WriteAttributeString("value", MSName[i]);
                    xw.WriteEndElement();
                    xw.WriteWhitespace("\r\n");
                }
                xw.WriteWhitespace("\t");
                xw.WriteEndElement();
                xw.WriteWhitespace("\r\n");

                //Save BAnimNames
                xw.WriteWhitespace("\r\n\t");
                xw.WriteStartElement("banimnames");
                xw.WriteWhitespace("\r\n");
                for (int i = 0; i < 0x100; i++)
                {
                    xw.WriteWhitespace("\t\t");
                    xw.WriteStartElement("banimname");
                    xw.WriteStartAttribute("index");
                    xw.WriteValue(i);
                    xw.WriteStartAttribute("hex");
                    xw.WriteValue("0x" + i.ToString("X2"));
                    xw.WriteAttributeString("value", BAnimName[i]);
                    xw.WriteEndElement();
                    xw.WriteWhitespace("\r\n");
                }
                xw.WriteWhitespace("\t");
                xw.WriteEndElement();
                xw.WriteWhitespace("\r\n");

                L2FileManager.SaveFragments(xw);
                L2FileManager.SaveFileInfos(xw);
            }
            xw.WriteEndElement();

            xw.Close();
        }
         */
    }
}

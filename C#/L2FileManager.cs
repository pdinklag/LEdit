using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace LEdit
{
    [Obsolete("New system")]
    public class L2FileManager
    {
        public struct L2ROMFragment
        {
            public long Address;
            public long Size;
        }

        public enum L2FileType
        {
            Unknown, Music, Map, MapTileSet, MapBlockSet, MonsterSprite,
        }

        public struct L2FileInfo
        {
            public string FileName;
            public bool Inserted;
            public long OriginalAddress;
            public long Address;
            public long Size;
        }

        public const int File_Count = 0x2A8;
        public const long File_Table = 0x1B8000;
        public const long File_Table_New = 0x300080;

        //File Infos
        static L2FileInfo[] Files = new L2FileInfo[File_Count];

        //Fragments
        static Collection<L2ROMFragment> Fragments = new Collection<L2ROMFragment>();

        //File Names
        public static string GetFileName(ushort FileNumber)
        {
            return Project.Directory.FullName + "\\" + FileNumber.ToString("X3");
        }

        //=== Load FileInfos ===
        public static void LoadFileInfos(XmlTextReader xr)
        {
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "fileinfos")
            {
                for (int i = 0; i < File_Count; i++)
                {
                    Files[i] = new L2FileInfo();
                    Files[i].FileName = i.ToString("D3");
                    Files[i].Inserted = false;
                }

                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "fileinfo")
                    {
                        if (xr.MoveToAttribute("index"))
                        {
                            int idx = xr.ReadContentAsInt();
                            if (idx >= 0 && idx < File_Count)
                            {
                                if (xr.MoveToAttribute("name"))
                                    Files[idx].FileName = xr.ReadContentAsString();
                                if (xr.MoveToAttribute("inserted"))
                                    Files[idx].Inserted = xr.ReadContentAsBoolean();
                                if (xr.MoveToAttribute("oaddress"))
                                    Files[idx].OriginalAddress = xr.ReadContentAsLong();
                                if (xr.MoveToAttribute("address"))
                                    Files[idx].Address = xr.ReadContentAsLong();
                                if (xr.MoveToAttribute("size"))
                                    Files[idx].Size = xr.ReadContentAsLong();
                            }
                        }
                    }
                }
            }
        }

        //=== Save FileInfos ===
        public static void SaveFileInfos(XmlTextWriter xw)
        {
            xw.WriteWhitespace("\t");
            xw.WriteStartElement("fileinfos");
            xw.WriteWhitespace("\r\n");
            for (int i = 0; i < File_Count; i++)
            {
                xw.WriteWhitespace("\t\t");
                xw.WriteStartElement("fileinfo");
                xw.WriteStartAttribute("index");
                xw.WriteValue(i);
                xw.WriteStartAttribute("hex");
                xw.WriteValue("0x" + i.ToString("X3"));
                xw.WriteStartAttribute("name");
                xw.WriteValue(Files[i].FileName);
                if (Files[i].Inserted)
                {
                    xw.WriteStartAttribute("inserted");
                    xw.WriteValue(Files[i].Inserted);
                    xw.WriteStartAttribute("oaddress");
                    xw.WriteValue(Files[i].OriginalAddress);
                    xw.WriteStartAttribute("address");
                    xw.WriteValue(Files[i].Address);
                    xw.WriteStartAttribute("size");
                    xw.WriteValue(Files[i].Size);
                }
                xw.WriteEndElement();
                xw.WriteWhitespace("\r\n");
            }
            xw.WriteWhitespace("\t");
            xw.WriteEndElement();
            xw.WriteWhitespace("\r\n");
        }

        //=== Load Fragments ===
        public static void LoadFragments(XmlTextReader xr)
        {
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "fragments")
            {
                Fragments.Clear();
                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element && xr.Name == "fragment")
                    {
                        L2ROMFragment fragment = new L2ROMFragment();

                        if (xr.MoveToAttribute("address"))
                        {
                            fragment.Address = xr.ReadContentAsLong();
                            if (xr.MoveToAttribute("size"))
                            {
                                fragment.Size = xr.ReadContentAsLong();
                                Fragments.Add(fragment);
                            }
                        }
                    }
                }
            }
        }

        //=== Save Fragments ===
        public static void SaveFragments(XmlTextWriter xw)
        {
            xw.WriteWhitespace("\t");
            xw.WriteStartElement("fragments");
            xw.WriteWhitespace("\r\n");
            for (int i = 0; i < Fragments.Count; i++)
            {
                xw.WriteWhitespace("\t\t");
                xw.WriteStartElement("fragment");
                xw.WriteStartAttribute("address");
                xw.WriteValue(Fragments[i].Address);
                xw.WriteStartAttribute("size");
                xw.WriteValue(Fragments[i].Size);
                xw.WriteEndElement();
                xw.WriteWhitespace("\r\n");
            }
            xw.WriteWhitespace("\t");
            xw.WriteEndElement();
            xw.WriteWhitespace("\r\n");
        }

        //=== Remove inserted file ===
        public static void RemoveInsertedFile(ushort FileNumber)
        {
            if (FileNumber >= 0 && FileNumber < File_Count)
            {
                if (Files[FileNumber].Inserted)
                {
                    FileStream fs_rom = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Write);
                    BinaryWriter bw_rom = new BinaryWriter(fs_rom);

                    //Update file info
                    Files[FileNumber].Inserted = false;

                    //Update file table
                    bw_rom.BaseStream.Seek(File_Table_New + FileNumber * 3, SeekOrigin.Begin);

                    long offset = Files[FileNumber].OriginalAddress - File_Table;
                    byte offset_bank = (byte)((offset >> 16) & 0xFF);
                    ushort offset_addr = (ushort)(offset & 0xFFFF);

                    bw_rom.Write(offset_addr);
                    bw_rom.Write(offset_bank);

                    //Nullify space
                    long tsize = Files[FileNumber].Size + 6;

                    bw_rom.BaseStream.Seek(Files[FileNumber].Address, SeekOrigin.Begin);
                    for (int i = 0; i < tsize; i++)
                        bw_rom.Write((byte)0);

                    //Update Fragments
                    long start = Files[FileNumber].Address;
                    long end = start + tsize;

                    bool bMerged = false;
                    bool bLoop = true;

                    //Find fragment to merge with start
                    while (bLoop)
                    {
                        bLoop = false;
                        for (int f = 0; f < Fragments.Count; f++)
                        {
                            if (Fragments[f].Address + Fragments[f].Size == start)
                            {
                                L2ROMFragment fragment = new L2ROMFragment();
                                fragment.Address = Fragments[f].Address;
                                fragment.Size = Fragments[f].Size + tsize;

                                Fragments.RemoveAt(f);
                                Fragments.Add(fragment);

                                bMerged = true;
                                bLoop = true;
                                break;
                            }
                            if (Fragments[f].Address == end)
                            {
                                L2ROMFragment fragment = new L2ROMFragment();
                                fragment.Address = start;
                                fragment.Size = Fragments[f].Size + tsize;

                                Fragments.RemoveAt(f);
                                Fragments.Add(fragment);

                                bMerged = true;
                                bLoop = true;
                                break;
                            }
                        }
                    }

                    if (!bMerged)
                    {
                        L2ROMFragment fragment = new L2ROMFragment();
                        fragment.Address = start;
                        fragment.Size = tsize;
                        Fragments.Add(fragment);
                    }

                    //Save the ROM Info, in case the app crashes
                    //L2ROM.SaveInfoFile();

                    bw_rom.Close();
                    fs_rom.Close();
                }
            }
        }

        //=== Insert File ===
        public static bool InsertFile(ushort FileNumber)
        {
            return InsertFile(FileNumber, string.Empty);
        }

        public static bool InsertFile(ushort FileNumber, string FileName)
        {
            bool bResult = false;
            if (FileNumber >= 0 && FileNumber < File_Count)
            {
                if (FileName == string.Empty)
                    FileName = GetFileName(FileNumber);

                if (File.Exists(FileName))
                {
                    //Remove inserted file
                    if (Files[FileNumber].Inserted)
                        RemoveInsertedFile(FileNumber);

                    //Open extracted file
                    FileStream fs_in = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br_in = new BinaryReader(fs_in);

                    long size = br_in.BaseStream.Length;
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
                                L2ROMFragment fragment = new L2ROMFragment();

                                fragment.Address = Fragments[i].Address + tsize;
                                fragment.Size = Fragments[i].Size - tsize;

                                Fragments.Add(fragment);
                            }

                            Fragments.RemoveAt(i);
                            break;
                        }
                    }

                    if (dest > 0)
                    {
                        FileStream fs_rom = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.ReadWrite);
                        BinaryReader br_rom = new BinaryReader(fs_rom);
                        BinaryWriter bw_rom = new BinaryWriter(fs_rom);

                        //Copy file
                        byte[] buffer = br_in.ReadBytes((int)size);
                        bw_rom.BaseStream.Seek(dest, SeekOrigin.Begin);

                        bw_rom.Write('u');  //Signature for unpacked files
                        bw_rom.Write('n');
                        bw_rom.Write('p');
                        bw_rom.Write((ushort)size);
                        bw_rom.Write((byte)0);
                        bw_rom.Write(buffer);

                        //Load original file info
                        br_rom.BaseStream.Seek(File_Table_New + FileNumber * 3, SeekOrigin.Begin);

                        //Update file info
                        Files[FileNumber].OriginalAddress = File_Table + br_rom.ReadUInt16() + (br_rom.ReadByte() << 16);
                        Files[FileNumber].Address = dest;
                        Files[FileNumber].Size = size;
                        Files[FileNumber].Inserted = true;

                        //Update file table
                        bw_rom.BaseStream.Seek(File_Table_New + FileNumber * 3, SeekOrigin.Begin);

                        long offset = dest - File_Table;
                        byte offset_bank = (byte)((offset >> 16) & 0xFF);
                        ushort offset_addr = (ushort)(offset & 0xFFFF);

                        bw_rom.Write(offset_addr);
                        bw_rom.Write(offset_bank);

                        //Success
                        bResult = true;

                        //Save the ROM Info, in case the app crashes
                        //L2ROM.SaveInfoFile();

                        bw_rom.Close();
                        br_rom.Close();
                        fs_rom.Close();
                    }

                    br_in.Close();
                    fs_in.Close();
                }
            }

            return bResult;
        }

        //=== Extract File ===
        public static string ExtractFile(ushort FileNumber)
        {
            return ExtractFile(FileNumber, string.Empty, false);
        }

        public static string ExtractFile(ushort FileNumber, bool bOverride)
        {
            return ExtractFile(FileNumber, string.Empty, bOverride);
        }

        public static string ExtractFile(ushort FileNumber, string DestFileName)
        {
            return ExtractFile(FileNumber, DestFileName, false);
        }

        public static string ExtractFile(ushort FileNumber, string DestFileName, bool bOverride)
        {
            //Open source
            FileStream src_fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Read);
            BinaryReader src = new BinaryReader(src_fs);

            //Open destination
            if (DestFileName == string.Empty)
                DestFileName = GetFileName(FileNumber);

            if (File.Exists(DestFileName))
            {
                if (bOverride)
                    File.Delete(DestFileName);
                else
                {
                    src.Close();
                    src_fs.Close();
                    return DestFileName;
                }
            }

            FileStream dst_fs = new FileStream(DestFileName, FileMode.CreateNew, FileAccess.ReadWrite);
            BinaryWriter dst = new BinaryWriter(dst_fs);
            BinaryReader dstr = new BinaryReader(dst_fs);

            //Setup
            dst.BaseStream.Seek(0, SeekOrigin.Begin);

            //Get source offset
            src.BaseStream.Seek(File_Table + FileNumber * 3, SeekOrigin.Begin);
            src.BaseStream.Seek(
                File_Table +
                src.ReadUInt16() + (src.ReadByte() << 16),
                SeekOrigin.Begin);

            //Variables
            ushort x65, x66;
            int nWritten, nToWrite;

            //Get file size
            nToWrite = (int)(src.ReadUInt16());
            nWritten = 0;

            //Load x65 and x66...
            x65 = src.ReadByte();
            x66 = 0x8;

            byte b;
            ushort L;
            ushort copyOffset;
            long write, copy;
            int copyAmount;

            //Load next byte
            while (nWritten < nToWrite)
            {
                if (dst.BaseStream.Position == 0x1F1)
                    b = 0;

                b = src.ReadByte();
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
                        b = src.ReadByte();

                        L |= b;
                        b &= 0xF;

                        if (b == 0)
                        {
                            b = src.ReadByte();

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
                        x65 = src.ReadByte();
                        x66 = 0x8;
                    }
                }
            }

            //Close destination
            dstr.Close();
            dst.Close();
            dst_fs.Close();

            //Close source
            src.Close();
            src_fs.Close();

            //Return file name
            return DestFileName;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LEdit
{
    public class Project
    {
        public const long Version = 1000;
        public const string FOLDER_ORIGINAL = "lufia2";
        public const string FOLDER_CONTENT = "content";

        public class Mod
        {
            public Content.ContentType Type;
            public int ID;
            public FileInfo File;
        }

        public static Collection<Mod> Mods = new Collection<Mod>();

        public static bool Initialized = false;

        public static string Name;
        public static FileInfo ROMFile;
        public static DirectoryInfo Directory;

        private static string ROMHash;

        public static FileInfo GetROMFile()
        {
            return new FileInfo(Directory.FullName + "\\" + FOLDER_ORIGINAL + "\\source.smc");
        }

        public static FileInfo GetInfoFile()
        {
            return new FileInfo(Directory.FullName + "\\" + Name + ".l2project");
        }

        public static FileInfo GetProductFile()
        {
            return new FileInfo(Directory.FullName + "\\" + Name + ".smc");
        }

        public static FileInfo HasMod(Content.ContentType Type, int ID)
        {
            foreach(Mod x in Mods)
            {
                if (x.Type == Type && x.ID == ID)
                    return x.File;
            }

            return null;
        }

        public static FileInfo RequestSource(Content.ContentType Type, int ID)
        {
            return RequestSource(Type, ID, true);
        }

        public static FileInfo RequestSource(Content.ContentType Type, int ID, bool Extension)
        {
            foreach (Mod x in Mods)
            {
                if (x.Type == Type && x.ID == ID)
                {
                    return Extension ?
                        x.File :
                        new FileInfo(x.File.FullName.Substring(
                            0,
                            x.File.FullName.Length - x.File.Extension.Length));
                }
            }

            return new FileInfo(
                GetOriginalFolder(Type).FullName + "\\" +
                Content.FormatID(ID) +
                (Extension ? Content.GetFileExtension(Type) : string.Empty));
        }

        public static FileInfo RequestContentSource(Content.ContentType Type, int ID, bool Extension)
        {
            return new FileInfo(
                GetContentFolder(Type).FullName + "\\" +
                Content.FormatID(ID) +
                (Extension ? Content.GetFileExtension(Type) : string.Empty));
        }

        public static FileInfo RequestOriginalSource(Content.ContentType Type, int ID)
        {
            return new FileInfo(
                GetOriginalFolder(Type).FullName + "\\" +
                Content.FormatID(ID) +
                Content.GetFileExtension(Type));
        }

        public static DirectoryInfo GetOriginalFolder(Content.ContentType Type)
        {
            return new DirectoryInfo(Directory.FullName + "\\" + FOLDER_ORIGINAL + "\\" + Content.GetContentFolderName(Type));
        }

        public static DirectoryInfo GetContentFolder(Content.ContentType Type)
        {
            return new DirectoryInfo(Directory.FullName + "\\" + FOLDER_CONTENT + "\\" + Content.GetContentFolderName(Type));
        }

        public static void Compile()
        {
            if (!Initialized)
                return;

            FileInfo Product = GetProductFile();

            if (Product.Exists)
                Product.Delete();

            //Apply LPatch
            ROMFile.CopyTo(Product.FullName);
            if (!L2ROM.ApplyLPatch(Product.FullName))
            {
                MessageBox.Show(
                    "LPatch konnte nicht installiert werden!",
                    "Fehler",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Product.Delete();
                return;
            }
            L2ROM.InitLPatchFragments();

            //Compile
            ProgressForm progress = new ProgressForm(null, "Projekt wird kompiliert...");
            progress.Show();

            FileStream rom = Product.Open(FileMode.Open, FileAccess.ReadWrite);

            L2MonsterSprite.Compile(rom, progress);
            L2Monster.Compile(rom, progress);

            rom.Close();
            progress.Close();
            progress.Dispose();

            MessageBox.Show(
                "Das Projekt wurde kompiliert!" + Environment.NewLine +
                Product.FullName,
                "Erfolg",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static FileInfo RegisterMod(Content.ContentType Type, int ID)
        {
            return RegisterMod(Type, ID, true);
        }

        public static FileInfo RegisterMod(Content.ContentType Type, int ID, bool Override)
        {
            foreach (Mod x in Mods)
            {
                if (x.Type == Type && x.ID == ID)
                {
                    if (Override)
                        Mods.Remove(x);
                    else
                        return null;
                }
            }

            Mod mod = new Mod();

            mod.Type = Type;
            mod.ID = ID;
            mod.File = RequestContentSource(Type, ID, true);

            Mods.Add(mod);

            mod.File.Directory.Create();
            return mod.File;
        }

        public static void Close()
        {
            Save();

            ContentSelectionDialog.StaticUnload();

            L2Monster.StaticUnload();
            L2MonsterSprite.StaticUnload();

            Mods.Clear();
            Initialized = false;
            Name = string.Empty;
            ROMFile = null;
            Directory = null;
        }

        private static void VersionUpdate(int OldVersion, bool ShowMessage)
        {
            if (OldVersion >= Version)
                return;

            if (ShowMessage)
            {
                MessageBox.Show(
                    "Das Projekt liegt in einem älterem Format vor und wird nun auf den neusten Stand gebracht.",
                    "Projekt-Update",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            ProgressForm progress = new ProgressForm(null, "Projekt wird erstellt...");
            progress.Show();

            if (OldVersion < 1000)
            {
                L2MonsterSprite.ExtractAll(progress);
                L2Monster.ExtractAll(progress);
            }

            progress.Close();
            progress.Dispose();
        }

        public static bool Open(FileInfo File)
        {
            Close();

            //=== LOAD PROJECT INFO ===
            int v = 0;
            XmlTextReader xr = new XmlTextReader(File.FullName);

            xr.Read();
            if (xr.NodeType == XmlNodeType.Element && xr.Name == "l2project") //root element
            {
                for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        //Project Information
                        if (xr.Name == "project")
                        {
                            if (xr.MoveToAttribute("name"))
                                Name = xr.ReadContentAsString();

                            if (xr.MoveToAttribute("location"))
                                Directory = new DirectoryInfo(xr.ReadContentAsString());

                            if (xr.MoveToAttribute("hash"))
                                ROMHash = xr.ReadContentAsString();

                            if (xr.MoveToAttribute("version"))
                                v = xr.ReadContentAsInt();
                        }

                        if (xr.Name == "mods")
                        {
                            for (xr.Read(); xr.NodeType != XmlNodeType.EndElement; xr.Read())
                            {
                                if (xr.Name == "content")
                                {
                                    Mod mod = new Mod();

                                    if (xr.MoveToAttribute("type"))
                                        mod.Type = Content.GetContentType(xr.ReadContentAsString());

                                    if (xr.MoveToAttribute("id"))
                                        mod.ID = xr.ReadContentAsInt();

                                    if (xr.MoveToAttribute("file"))
                                        mod.File = new FileInfo(xr.ReadContentAsString());

                                    if(mod.File.Exists)
                                        Mods.Add(mod);
                                }
                            }
                        }
                    }
                }
            }
            xr.Close();

            if (Directory == null)
            {
                //TODO: ERROR
                return false;
            }

            ROMFile = GetROMFile();

            if (!ROMFile.Exists)
            {
                //TODO: ERROR
                return false;
            }

            if (ROMHash != Utility.ComputeFileHash(ROMFile.FullName))
            {
                MessageBox.Show(
                    "Der Quell-ROM wurde modifiziert!" + Environment.NewLine +
                    "Dies kann möglicherweise zu schwerwiegenden Problemen führen.",
                    "Warnung",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            //=== NEW VERSION CHECK ===
            VersionUpdate(v, true);

            //=== LOAD CONTENT ===
            ProgressForm progress = new ProgressForm(null, "Projekt wird geöffnet...");
            progress.Show();

            //order DOES matter
            L2MonsterSprite.StaticInit(progress);
            L2Monster.StaticInit(progress);

            // BEGIN Restoring map and item editors, 7 November 2018
            {
                L2ROM.file_name = ROMFile.FullName;

                BinaryReader br = new BinaryReader(
                    new FileStream(ROMFile.FullName, FileMode.Open, FileAccess.Read));

                L2MonsterAttackName.InitROM(br);
                L2MapSprite.InitROM(br);

                L2IP.InitROM(br);
                L2Spell.InitROM(br);
                L2Item.InitROM(br);
                L2MonsterHorde.InitROM(br);
                L2MapHeader.InitROM(br);

                L2WorldMapBlock.InitROM(br);
                L2WorldMapBigBlock.InitROM(br);

                br.Close();

                L2ROM.world = new L2WorldMap();
            }
            // END Restoring map and item editors, 7 November 2018

            progress.Close();
            progress.Dispose();

            Initialized = true;
            return true;
        }

        public static void Save()
        {
            if (!Initialized)
                return;

            FileInfo InfoFile = GetInfoFile();

            if (InfoFile.Exists)
                InfoFile.Delete();

            XmlTextWriter xw = new XmlTextWriter(InfoFile.FullName, Encoding.Unicode);

            xw.WriteStartElement("l2project");
            xw.WriteWhitespace(Environment.NewLine);
            {
                //Project Information
                xw.WriteWhitespace(Utility.Tab);
                xw.WriteStartElement("project");
                xw.WriteStartAttribute("name");
                xw.WriteValue(Name);
                xw.WriteStartAttribute("location");
                xw.WriteValue(Directory.FullName);
                xw.WriteStartAttribute("hash");
                xw.WriteValue(ROMHash);
                xw.WriteStartAttribute("version");
                xw.WriteValue(Version);
                xw.WriteEndElement();

                //Mods
                if (Mods.Count > 0)
                {
                    xw.WriteWhitespace(Environment.NewLine + Utility.Tab);
                    xw.WriteStartElement("mods");
                    foreach(Mod mod in Mods)
                    {
                        xw.WriteWhitespace(Environment.NewLine + Utility.Tab + Utility.Tab);
                        xw.WriteStartElement("content");
                        xw.WriteStartAttribute("type");
                        xw.WriteValue(Content.GetContentText(mod.Type));
                        xw.WriteStartAttribute("id");
                        xw.WriteValue(mod.ID.ToString());
                        xw.WriteStartAttribute("file");
                        xw.WriteValue(mod.File.FullName);
                        xw.WriteEndElement();
                    }
                    xw.WriteWhitespace(Environment.NewLine + Utility.Tab);
                    xw.WriteEndElement();
                }
            }
            xw.WriteWhitespace(Environment.NewLine);
            xw.WriteEndElement();
            xw.Close();
        }

        public static bool NewProject()
        {
            NewProjectDialog dlg = new NewProjectDialog();
            if (dlg.ShowDialog() != DialogResult.Cancel)
            {
                Close();

                Name = dlg.ProjectName;
                Directory = new DirectoryInfo(dlg.ProjectFolder);
                FileInfo OriginalROMFile = new FileInfo(dlg.ROMFile);

                //TODO: Check whether project already exists
                //TODO: Check whether ROM is valid

                Directory.Create();
                Directory.CreateSubdirectory(FOLDER_ORIGINAL);
                Directory.CreateSubdirectory(FOLDER_CONTENT);

                ROMFile = GetROMFile();

                //Remove ROM header if present
                if (OriginalROMFile.Length == 0x300200)
                {
                    BinaryReader OriginalReader = new BinaryReader(
                        new FileStream(OriginalROMFile.FullName, FileMode.Open, FileAccess.Read));

                    BinaryWriter NewWriter = new BinaryWriter(
                        new FileStream(ROMFile.FullName, FileMode.CreateNew, FileAccess.Write));

                    OriginalReader.BaseStream.Seek(0x200, SeekOrigin.Begin);
                    NewWriter.Write(OriginalReader.ReadBytes(0x300000));

                    NewWriter.Close();
                    OriginalReader.Close();
                }
                else
                {
                    OriginalROMFile.CopyTo(ROMFile.FullName);
                }

                ROMHash = Utility.ComputeFileHash(ROMFile.FullName);

                Initialized = true;
                Save();

                //Update from version "0"
                VersionUpdate(0, false);

                return Open(GetInfoFile());
            }
            else
            {
                return false;
            }
        }
    }
}

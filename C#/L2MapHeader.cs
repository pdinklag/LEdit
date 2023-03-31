using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public class L2MapHeader
    {
        #region STATIC
        public const long TableOffset = 0x138000;
        public const int Count = 0xF2;
        public const long InfoTableOffset = 0x2FFCBC;
        public const long ChestTableOffset = 0x8B808;

        public const long NewInfoOffset = 0x308000; //0x800 bytes per map

        public const long BlockSetFileListOffset = 0x2FF72E;
        public const long TileSetFileListOffset = 0x2FF81E;
        public const long PaletteSetListOffset = 0x2FFA08;
        public const long PaletteSetAddressListOffset = 0x2FFAFA;

        public const long SomeIndexTableOffset = 0x2FF90E; //indices per map to SomeData
        public const long SomeDataOffset = 0x2FF9FE; //list of 2 byte structs

        public const long MonsterHordeIndexTableOffset = 0x1AC088;

        static L2MapHeader[] MH = new L2MapHeader[Count];

        public static L2MapHeader Get(int n)
        {
            if (n >= 0 && n < Count)
                return MH[n];
            else
                return null;
        }

        public static void InitROM(BinaryReader br)
        {
            br.BaseStream.Seek(TableOffset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
                MH[i] = new L2MapHeader(br, i);
        }

        #endregion

        #region CLASSES
        //MAP INFO
        public class Identifiable
        {
            public byte id;

            public void FromStream(BinaryReader br)
            {
                //This is already done, usually
                //id = br.ReadByte();
            }

            public void ToStream(BinaryWriter bw)
            {
                bw.Write(id);
            }
        }

        public class Spot : Identifiable
        {
            public byte x1;
            public byte y1;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                x1 = br.ReadByte();
                y1 = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(x1);
                bw.Write(y1);
            }
        }

        public class Area : Spot
        {
            public byte x2;
            public byte y2;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                x2 = br.ReadByte();
                y2 = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(x2);
                bw.Write(y2);
            }
        }

        public class Door : Area
        {
            public byte area1_x1;
            public byte area1_y1;
            public byte area1_x2;
            public byte area1_y2;
            public byte area2_x1;
            public byte area2_y1;
            public byte area2_x2;
            public byte area2_y2;
            public byte unknown1;
            public byte unknown2;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);

                area1_x1 = br.ReadByte();
                area1_y1 = br.ReadByte();
                area1_x2 = br.ReadByte();
                area1_y2 = br.ReadByte();
                area2_x1 = br.ReadByte();
                area2_y1 = br.ReadByte();
                area2_x2 = br.ReadByte();
                area2_y2 = br.ReadByte();
                unknown1 = br.ReadByte();
                unknown2 = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(area1_x1);
                bw.Write(area1_y1);
                bw.Write(area1_x2);
                bw.Write(area1_y2);
                bw.Write(area2_x1);
                bw.Write(area2_y1);
                bw.Write(area2_x2);
                bw.Write(area2_y2);
                bw.Write(unknown1);
                bw.Write(unknown2);
            }
        }

        public class UnknownStruct04 : Identifiable
        {
            public byte[] unknown; //9 bytes, no idea

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);

                unknown = br.ReadBytes(9);
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(unknown, 0, 9);
            }
        }

        public class CharacterStart : Spot
        {
            public byte move_area_x1; //marks the area this character is allowed to move in
            public byte move_area_y1;
            public byte move_area_x2;
            public byte move_area_y2;
            public byte unknown; //possibly connected script of sorts?

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                move_area_x1 = br.ReadByte();
                move_area_y1 = br.ReadByte();
                move_area_x2 = br.ReadByte();
                move_area_y2 = br.ReadByte();
                unknown = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(move_area_x1);
                bw.Write(move_area_y1);
                bw.Write(move_area_x2);
                bw.Write(move_area_y2);
                bw.Write(unknown);
            }
        }

        public class WarpDoor : Area
        {
            public byte unknown;
            public byte dest_x;
            public byte dest_y;
            public byte dest_map;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                unknown = br.ReadByte();
                dest_x = br.ReadByte();
                dest_y = br.ReadByte();
                dest_map = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(unknown);
                bw.Write(dest_x);
                bw.Write(dest_y);
                bw.Write(dest_map);
            }
        }

        public class Treasure : Spot
        {
            public byte unknown;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                unknown = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(unknown);
            }
        }

        public class CharacterNode : Spot
        {
            public byte unknown1;
            public byte unknown2;
            public byte unknown3;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);
                unknown1 = br.ReadByte();
                unknown2 = br.ReadByte();
                unknown3 = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(unknown1);
                bw.Write(unknown2);
                bw.Write(unknown3);
            }
        }

        public class UnknownStruct22 : Spot
        {
            public byte unknown1;
            public byte unknown2;
            public byte unknown3;

            new public void FromStream(BinaryReader br)
            {
                base.FromStream(br);

                unknown1 = br.ReadByte();
                unknown2 = br.ReadByte();
                unknown3 = br.ReadByte();
            }

            new public void ToStream(BinaryWriter bw)
            {
                base.ToStream(bw);
                bw.Write(unknown1);
                bw.Write(unknown2);
                bw.Write(unknown3);
            }
        }

        //MAP INSTANCES
        public class Character
        {
            public byte id;
            public CharacterStart start;
            public int map_sprite;
        }

        //CHESTS
        public class Chest
        {
            public Treasure treasure;
            public int global_id;
            public int item;
        }
        #endregion

        //Map Info
        public Collection<Door> Doors = new Collection<Door>();
        public Collection<UnknownStruct04> Unknown04 = new Collection<UnknownStruct04>();
        public Collection<WarpDoor> WarpDoors = new Collection<WarpDoor>();
        //0x08 is unused
        public Collection<Area> SecretAlleys = new Collection<Area>();
        public Collection<Area> CutsceneTriggers = new Collection<Area>();
        public Collection<Spot> Unknown0E = new Collection<Spot>(); //Character path nodes for movie scenes?
        public Collection<CharacterStart> CharacterStarts = new Collection<CharacterStart>();
        public Collection<CharacterNode> CharacterNodes = new Collection<CharacterNode>();
        public Collection<Area> Unknown14 = new Collection<Area>();
        public Collection<UnknownStruct04> Unknown16 = new Collection<UnknownStruct04>();
        public Collection<Spot> Unknown18 = new Collection<Spot>(); //ONLY on map 0x05
        public Collection<Spot> BombSpots = new Collection<Spot>(); //Spots where bombs should be placed
        public Collection<Spot> SwordSpots = new Collection<Spot>(); //Spots you should use your sword at
        public Collection<Spot> ArrowSpots = new Collection<Spot>(); //Spots you should fire arrows at
        public Collection<Spot> ActionSpots = new Collection<Spot>(); //Spots you should press the action button
        public Collection<UnknownStruct22> Unknown22 = new Collection<UnknownStruct22>(); //Triggers?
        public Collection<Area> Unknown24 = new Collection<Area>();
        public Collection<Treasure> Treasures = new Collection<Treasure>(); //Character path nodes for movie scenes?
        public Collection<Spot> MovableObjects = new Collection<Spot>();
        public Collection<Spot> HammerSpots = new Collection<Spot>(); //Spots you should use your hammer at

        //Map Script results
        public Collection<Character> Characters = new Collection<Character>();
        public Collection<Chest> Chests = new Collection<Chest>();

        public CharacterStart FindCharacterStart(byte ID)
        {
            for (int i = 0; i < CharacterStarts.Count; i++)
            {
                if (CharacterStarts[i].id == ID)
                    return CharacterStarts[i];
            }
            return null;
        }

        public Treasure FindTreasure(byte ID)
        {
            for (int i = 0; i < Treasures.Count; i++)
            {
                if (Treasures[i].id == ID)
                    return Treasures[i];
            }
            return null;
        }

        int number;

        ushort map_file;
        ushort tileset_file;
        ushort blockset_file;

        long palset_address;

        L2String name;
        byte battle_bg;
        byte music;

        public ushort MapFile { get { return map_file; } }
        public ushort TileSetFile { get { return tileset_file; } }
        public ushort BlockSetFile { get { return blockset_file; } }

        public long PalSetAddress { get { return palset_address; } }

        public L2String Name { get { return name; } }
        public int BattleBG { get { return (int)battle_bg; } }
        public int MusicFile { get { return (int)music; } }

        public L2MapHeader(BinaryReader br, int n)
        {
            FromStream(br,n);
        }

        public void SaveToROM()
        {
            //Currently "only" the MapInfo can be saved
            FileStream fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            long offset = NewInfoOffset + number * 0x600;
            ushort none_offset = 0x2C;
            ushort next_free_offset = 0x2D;

            fs.Seek(offset + none_offset, SeekOrigin.Begin);
            bw.Write((byte)0xFF); //"no info" byte!

            #region Save... It's all the same

            //0x02 - Doors
            fs.Seek(offset + 0x02, SeekOrigin.Begin);
            if (Doors.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write((ushort)next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Doors.Count; i++)
                    Doors[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x04 - Unknown04
            fs.Seek(offset + 0x04, SeekOrigin.Begin);
            if (Unknown04.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown04.Count; i++)
                    Unknown04[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x06 - Unknown06
            fs.Seek(offset + 0x06, SeekOrigin.Begin);
            if (WarpDoors.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < WarpDoors.Count; i++)
                    WarpDoors[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x08 - Unused
            fs.Seek(offset + 0x08, SeekOrigin.Begin);
            bw.Write(none_offset);

            //0x0A - Secret Alleys
            fs.Seek(offset + 0x0A, SeekOrigin.Begin);
            if (SecretAlleys.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < SecretAlleys.Count; i++)
                    SecretAlleys[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x0C - CutsceneTriggers
            fs.Seek(offset + 0x0C, SeekOrigin.Begin);
            if (CutsceneTriggers.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < CutsceneTriggers.Count; i++)
                    CutsceneTriggers[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x0E - Unknown0E
            fs.Seek(offset + 0x0E, SeekOrigin.Begin);
            if (Unknown0E.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown0E.Count; i++)
                    Unknown0E[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x10 - CharacterStarts
            fs.Seek(offset + 0x10, SeekOrigin.Begin);
            if (CharacterStarts.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < CharacterStarts.Count; i++)
                    CharacterStarts[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x12 - CharacterNodes
            fs.Seek(offset + 0x12, SeekOrigin.Begin);
            if (CharacterNodes.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < CharacterNodes.Count; i++)
                    CharacterNodes[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x14 - Unknown14
            fs.Seek(offset + 0x14, SeekOrigin.Begin);
            if (Unknown14.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown14.Count; i++)
                    Unknown14[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x16 - Unknown16
            fs.Seek(offset + 0x16, SeekOrigin.Begin);
            if (Unknown16.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown16.Count; i++)
                    Unknown16[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x18 - Unknown18
            fs.Seek(offset + 0x18, SeekOrigin.Begin);
            if (Unknown18.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown18.Count; i++)
                    Unknown18[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x1A - BombSpots
            fs.Seek(offset + 0x1A, SeekOrigin.Begin);
            if (BombSpots.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < BombSpots.Count; i++)
                    BombSpots[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x1C - SwordSpots
            fs.Seek(offset + 0x1C, SeekOrigin.Begin);
            if (SwordSpots.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < SwordSpots.Count; i++)
                    SwordSpots[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x1E - ArrowSpots
            fs.Seek(offset + 0x1E, SeekOrigin.Begin);
            if (ArrowSpots.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < ArrowSpots.Count; i++)
                    ArrowSpots[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x20 - ActionSpots
            fs.Seek(offset + 0x20, SeekOrigin.Begin);
            if (ActionSpots.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < ActionSpots.Count; i++)
                    ActionSpots[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x22 - Unknown22
            fs.Seek(offset + 0x22, SeekOrigin.Begin);
            if (Unknown22.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown22.Count; i++)
                    Unknown22[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x24 - Unknown24
            fs.Seek(offset + 0x24, SeekOrigin.Begin);
            if (Unknown24.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Unknown24.Count; i++)
                    Unknown24[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x26 - Treasures
            fs.Seek(offset + 0x26, SeekOrigin.Begin);
            if (Treasures.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < Treasures.Count; i++)
                    Treasures[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x28 - MovableObjects
            fs.Seek(offset + 0x28, SeekOrigin.Begin);
            if (MovableObjects.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < MovableObjects.Count; i++)
                    MovableObjects[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            //0x2A - HammerSpots
            fs.Seek(offset + 0x2A, SeekOrigin.Begin);
            if (HammerSpots.Count == 0)
                bw.Write(none_offset);
            else
            {
                bw.Write(next_free_offset);
                fs.Seek(offset + next_free_offset, SeekOrigin.Begin);
                for (int i = 0; i < HammerSpots.Count; i++)
                    HammerSpots[i].ToStream(bw);

                bw.Write((byte)0xFF);
                next_free_offset = (ushort)(fs.Position - offset);
            }

            #endregion

            fs.Seek(offset, SeekOrigin.Begin);
            bw.Write(next_free_offset);

            bw.Close();
            fs.Close();
        }

        public void FromStream(BinaryReader br, int n)
        {
            if (br != null && n >= 0 && n < Count)
            {
                number = n;

                long restore_pos = br.BaseStream.Position;

                CharacterStarts.Clear();
                Characters.Clear();

                br.BaseStream.Seek(
                    TableOffset + 0x10 + (n << 3),
                    SeekOrigin.Begin);

                long offset_dialogs = (long)br.ReadUInt16();
                offset_dialogs |= (long)((long)br.ReadByte() << 16);
                offset_dialogs += TableOffset;

                long offset_settings = (long)br.ReadUInt16();
                offset_settings |= (long)((long)br.ReadByte() << 16);
                offset_settings += TableOffset;

                long offset_name = (long)br.ReadUInt16();
                offset_name += TableOffset;

                //Map Info
                br.BaseStream.Seek(
                    InfoTableOffset + n * 3,
                    SeekOrigin.Begin);

                SNESExLoROMAddress snes_offset_info =
                    new SNESExLoROMAddress(br);

                long offset_mapinfo = snes_offset_info.ToPCAddress();

                br.BaseStream.Seek(offset_mapinfo, SeekOrigin.Begin);

                ushort sizeof_mapinfo = br.ReadUInt16();

                /* 0x02 - Doors */
                long offset_doors = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x04 - UnknownStruct04 */
                long offset_unknown_04 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x06 - WarpDoors */
                long offset_warpdoors = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x08 - Unused! */
                long offset_unused_08 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x0A - Secret Alleys  */
                long offset_secretalleys = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x0C - Cutscene Triggers  */
                long offset_cutscene_triggers = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x0E - Character path nodes for movie scenes? */
                long offset_unknown_0E = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x10 - Character starting points and movement areas */
                long offset_char_starts = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x12 - Character path nodes for movie scenes */
                long offset_character_nodes = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x14 - ??? (Area) */
                long offset_unknown_14 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x16 - ??? (0xA bytes like Unknown04, prolly not the same though) */
                long offset_unknown_16 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x18 - ??? (Spot, ONLY in Höhle der Geheimnisse) */
                long offset_unknown_18 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x1A - Bomb Spots */
                long offset_bombspots = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x1C - Sword Spots */
                long offset_swordspots = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x1E - Arrow Spots*/
                long offset_arrowspots = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x20 - Action Spots */
                long offset_actionspots = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x22 - Trigger positions of some sort... */
                long offset_unknown_22 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x24 - ??? (Area)*/
                long offset_unknown_24 = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x26 - Treasures */
                long offset_treasures = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x28 - Movable Objects */
                long offset_movable = offset_mapinfo + (long)br.ReadUInt16();
                /* 0x2A - Hammer Spots */
                long offset_hammerspots = offset_mapinfo + (long)br.ReadUInt16();

                byte id;

                //0x02 - Doors
                br.BaseStream.Seek(offset_doors, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Door area = new Door();
                    area.id = id;
                    area.FromStream(br);
                    Doors.Add(area);

                    id = br.ReadByte();
                }

                //0x04 - Unknown04
                br.BaseStream.Seek(offset_unknown_04, SeekOrigin.Begin);
                id = br.ReadByte();

                while (id != 0xFF)
                {
                    UnknownStruct04 x = new UnknownStruct04();
                    x.id = id;
                    x.FromStream(br);
                    Unknown04.Add(x);

                    id = br.ReadByte();
                }

                //0x06 - WarpDoors
                br.BaseStream.Seek(offset_warpdoors, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    WarpDoor current_warpdoor = new WarpDoor();
                    current_warpdoor.id = id;
                    current_warpdoor.FromStream(br);
                    WarpDoors.Add(current_warpdoor);

                    id = br.ReadByte();
                }

                //0x08 - Unused!

                //0x0A - SecretAlleys
                br.BaseStream.Seek(offset_secretalleys, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Area area = new Area();
                    area.id = id;
                    area.FromStream(br);
                    SecretAlleys.Add(area);

                    id = br.ReadByte();
                }

                //0x0C - CutsceneTriggers
                br.BaseStream.Seek(offset_cutscene_triggers, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Area area = new Area();
                    area.id = id;
                    area.FromStream(br);
                    CutsceneTriggers.Add(area);

                    id = br.ReadByte();
                }

                //0x0E - Unknown0E
                br.BaseStream.Seek(offset_unknown_0E, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot spot = new Spot();
                    spot.id = id;
                    spot.FromStream(br);
                    Unknown0E.Add(spot);

                    id = br.ReadByte();
                }

                //0x10 - CharacterStarts
                br.BaseStream.Seek(offset_char_starts, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    CharacterStart current_charstart = new CharacterStart();
                    current_charstart.id = id;
                    current_charstart.FromStream(br);
                    CharacterStarts.Add(current_charstart);

                    id = br.ReadByte();
                }

                //0x12 - CharacterNodes
                br.BaseStream.Seek(offset_character_nodes, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    CharacterNode cn = new CharacterNode();
                    cn.id = id;
                    cn.FromStream(br);
                    CharacterNodes.Add(cn);

                    id = br.ReadByte();
                }

                //0x14 - Unknown14
                br.BaseStream.Seek(offset_unknown_14, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Area area = new Area();
                    area.id = id;
                    area.FromStream(br);
                    Unknown14.Add(area);

                    id = br.ReadByte();
                }

                //0x16 - Unknown16
                br.BaseStream.Seek(offset_unknown_16, SeekOrigin.Begin);
                id = br.ReadByte();

                while (id != 0xFF)
                {
                    UnknownStruct04 x = new UnknownStruct04();
                    x.id = id;
                    x.FromStream(br);
                    Unknown16.Add(x);

                    id = br.ReadByte();
                }

                //0x18 - Unknown18
                br.BaseStream.Seek(offset_unknown_18, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot spot = new Spot();
                    spot.id = id;
                    spot.FromStream(br);
                    Unknown18.Add(spot);

                    id = br.ReadByte();
                }

                //0x1A - BombSpots
                br.BaseStream.Seek(offset_bombspots, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    BombSpots.Add(current_spot);

                    id = br.ReadByte();
                }

                //0x1C - SwordSpots
                br.BaseStream.Seek(offset_swordspots, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    SwordSpots.Add(current_spot);

                    id = br.ReadByte();
                }

                //0x1E - ArrowSpots
                br.BaseStream.Seek(offset_arrowspots, SeekOrigin.Begin);
                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    ArrowSpots.Add(current_spot);

                    id = br.ReadByte();
                }

                //0x20 - ActionSpots
                br.BaseStream.Seek(offset_actionspots, SeekOrigin.Begin);
                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    ActionSpots.Add(current_spot);

                    id = br.ReadByte();
                }

                //0x22 - Unknown22
                br.BaseStream.Seek(offset_unknown_22, SeekOrigin.Begin);
                id = br.ReadByte();

                while (id != 0xFF)
                {
                    UnknownStruct22 x = new UnknownStruct22();
                    x.id = id;
                    x.FromStream(br);
                    Unknown22.Add(x);

                    id = br.ReadByte();
                }

                //0x24 - Unknown24
                br.BaseStream.Seek(offset_unknown_24, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Area area = new Area();
                    area.id = id;
                    area.FromStream(br);
                    Unknown24.Add(area);

                    id = br.ReadByte();
                }

                //0x26 - Treasures
                br.BaseStream.Seek(offset_treasures, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Treasure current_treasure = new Treasure();
                    current_treasure.id = id;
                    current_treasure.FromStream(br);
                    Treasures.Add(current_treasure);

                    id = br.ReadByte();
                }

                //0x28 - MovableObjects
                br.BaseStream.Seek(offset_movable, SeekOrigin.Begin);
                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    MovableObjects.Add(current_spot);

                    id = br.ReadByte();
                }

                //0x2A - HammerSpots
                br.BaseStream.Seek(offset_hammerspots, SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Spot current_spot = new Spot();
                    current_spot.id = id;
                    current_spot.FromStream(br);
                    HammerSpots.Add(current_spot);

                    id = br.ReadByte();
                }

                //Chests
                br.BaseStream.Seek(ChestTableOffset + (n << 1), SeekOrigin.Begin);
                br.BaseStream.Seek(ChestTableOffset + br.ReadUInt16(), SeekOrigin.Begin);

                id = br.ReadByte();
                while (id != 0xFF)
                {
                    Chest current_chest = new Chest();

                    bool bFlag20 = ((id & 0x20) != 0); //Can be opened from all sides?
                    //Reliquary on 0x20:
                    //"Dragon Egg can't appear in that chest"

                    bool bFlag40 = ((id & 0x40) != 0); //Item |= 0x100

                    id &= 0x1F;

                    current_chest.treasure = FindTreasure(id);
                    if (n > 1 && current_chest.treasure == null)
                        Console.WriteLine("CHEST ERROR: Could not find treasure with ID 0x" +
                            id.ToString("X2") + " in map header 0x" + n.ToString("X2"));

                    current_chest.global_id = br.ReadByte();
                    current_chest.item = br.ReadByte();

                    if (bFlag40)
                        current_chest.item |= 0x100;

                    Chests.Add(current_chest);

                    id = br.ReadByte();
                }

                //Dialogs
                br.BaseStream.Seek(offset_dialogs, SeekOrigin.Begin);
                //can't load yet

                //Commands
                br.BaseStream.Seek(offset_settings, SeekOrigin.Begin);
                byte cmd = br.ReadByte();
                while (cmd != 0)
                {
                    switch (cmd)
                    {
                        case 0x14:  //Unknown
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            break;

                        case 0x15:  //Unknown
                            br.ReadByte();
                            break;

                        case 0x1C:
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            br.ReadByte();
                            break;

                        case 0x4B:  //Music
                            music = br.ReadByte();
                            break;

                        case 0x54:  //Unknown
                            br.ReadByte();
                            break;

                        case 0x7B:
                        case 0x68:  //Character
                            Character current_character = new Character();

                            current_character.id = br.ReadByte();
                            //byte preset_id = br.ReadByte();
                            byte preset_id = (byte)(id - 0x4F);

                            current_character.start = FindCharacterStart(preset_id);
                            current_character.map_sprite = (int)br.ReadByte();

                            //if(n == 3)
                            //Console.WriteLine("0x" + current_character.id.ToString("X2") + " with MapSprite 0x" + current_character.map_sprite.ToString("X2"));

                            Characters.Add(current_character);
                            break;

                        case 0x69:  //Settings
                            br.ReadByte();
                            break;

                        case 0x74:
                            battle_bg = br.ReadByte();
                            break;
                    }
                    cmd = br.ReadByte();
                }

                //Name
                br.BaseStream.Seek(offset_name, SeekOrigin.Begin);
                name = new L2String(br);

                //Files
                br.BaseStream.Seek(TileSetFileListOffset + n, SeekOrigin.Begin);
                tileset_file = (ushort)(br.ReadByte() + 0x14C);

                br.BaseStream.Seek(BlockSetFileListOffset + n, SeekOrigin.Begin);
                blockset_file = (ushort)(br.ReadByte() + 0x131);

                map_file = (ushort)(n + 0x40);

                //Palette Set
                br.BaseStream.Seek(PaletteSetListOffset + n, SeekOrigin.Begin);
                byte palset_index = (byte)(br.ReadByte() - 1);

                br.BaseStream.Seek(PaletteSetAddressListOffset + (palset_index << 1), SeekOrigin.Begin);

                SNESExLoROMAddress palset_snes_address = new SNESExLoROMAddress(0x9B, br.ReadUInt16());

                int offset = 0x10;
                
                //This is wicked...
                br.BaseStream.Seek(SomeIndexTableOffset + n, SeekOrigin.Begin);
                br.BaseStream.Seek(SomeDataOffset + (br.ReadByte() << 1), SeekOrigin.Begin);

                byte v28 = br.ReadByte();
                byte v29 = br.ReadByte();

                if ((v28 >> 4) != 0)
                {
                    //The game now would load file [0x160 + (v28 >> 4)]
                }

                if ((v29 >> 4) != 0)
                {
                    //The game now would load file [0x165 + (v29 >> 4)]
                    offset = 0x20; //important for palette choosing
                }

                palset_address = palset_snes_address.ToPCAddress() + offset;

                //Restore Stream position
                br.BaseStream.Position = restore_pos;
            }
        }
    }
}

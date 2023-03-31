using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2Item
    {
        #region STATIC
        public const int Count = 0x1D3;
        public const long TableOffset = 0x1A4F69;

        public const long NamesOffset = 0xF5ED5;

        static L2Item[] I = new L2Item[Count];

        public static L2Item Get(int n)
        {
            if (n >= 0 && n < Count)
                return I[n];
            else
                return null;
        }
        public static void Set(int n, L2Item item)
        {
            if (n >= 0 && n < Count)
                I[n] = item;
        }

        public static void InitROM(BinaryReader br)
        {
            for (int i = 0; i < Count; i++)
            {
                br.BaseStream.Seek(TableOffset + (i << 1), SeekOrigin.Begin);
                br.BaseStream.Seek(TableOffset + br.ReadUInt16(), SeekOrigin.Begin);
                I[i] = new L2Item(br);

                br.BaseStream.Seek(NamesOffset + (i * 12), SeekOrigin.Begin);
                I[i].name = new L2String(br, 12);

                if (I[i].Script_Weapon.Bytes.Contains(0xFF))
                    Console.WriteLine("(0x" + i.ToString("X3") + " - " + I[i].Name.Value.TrimEnd() + ")");

                if (I[i].Script_Armor.Bytes.Count > 0)
                    Console.WriteLine("Armor Script: 0x" + i.ToString("X3"));
            }
        }

        #endregion

        L2String name;
        public L2String Name { get { return name; } set { name = value; } }

        byte usability;
        public byte Usability { get { return usability; } set { usability = value; } }

        byte unknown;
        public byte Unknown { get { return unknown; } set { unknown = value; } }

        byte targeting_mode;
        public byte TargetingMode { get { return targeting_mode; } set { targeting_mode = value; } }

        byte icon;
        public byte Icon { get { return icon; } set { icon = value; } }

        byte mapsprite;
        public byte MapSprite { get { return mapsprite; } set { mapsprite = value; } }

        ushort cost;
        public ushort Cost { get { return cost; } set { cost = value; } }

        byte type;
        public byte Type { get { return type; } set { type = value; } }

        byte equipable_by;
        public byte EquipableBy { get { return equipable_by; } set { equipable_by = value; } }

        ushort property_flags;
        public ushort PropertyFlags { get { return property_flags; } set { property_flags = value; } }

        L2BattleScript script_menu = new L2BattleScript();
        public L2BattleScript Script_Menu { get { return script_menu; } set { script_menu = value; } }

        L2BattleScript script_battle = new L2BattleScript();
        public L2BattleScript Script_Battle { get { return script_battle; } set { script_battle = value; } }

        L2BattleScript script_weapon = new L2BattleScript();
        public L2BattleScript Script_Weapon { get { return script_weapon; } set { script_weapon = value; } }

        L2BattleScript script_armor = new L2BattleScript();
        public L2BattleScript Script_Armor { get { return script_armor; } set { script_armor = value; } }

        int bonus_ang;
        public int Bonus_ANG { get { return bonus_ang; } set { bonus_ang = value; } }

        int bonus_abw;
        public int Bonus_ABW { get { return bonus_abw; } set { bonus_abw = value; } }

        int bonus_str;
        public int Bonus_STR { get { return bonus_str; } set { bonus_str = value; } }

        int bonus_bwg;
        public int Bonus_BWG { get { return bonus_bwg; } set { bonus_bwg = value; } }

        int bonus_int;
        public int Bonus_INT { get { return bonus_int; } set { bonus_int = value; } }

        int bonus_mut;
        public int Bonus_MUT { get { return bonus_mut; } set { bonus_mut = value; } }

        int bonus_mga;
        public int Bonus_MGA { get { return bonus_mga; } set { bonus_mga = value; } }

        int battle_anim = -1;
        public int BattleAnim { get { return battle_anim; } set { battle_anim = value; } }

        int unknown_property;
        public int UnknownProperty { get { return unknown_property; } set { unknown_property = value; } }

        int ip_attack = -1;
        public int IPAttack { get { return ip_attack; } set { ip_attack = value; } }

        public L2Item(BinaryReader br)
        {
            FromStream(br);
        }

        public void FromStream(BinaryReader br)
        {
            long offset = br.BaseStream.Position;

            usability = br.ReadByte();
            unknown = br.ReadByte();
            targeting_mode = br.ReadByte();
            icon = br.ReadByte();
            mapsprite = br.ReadByte();
            cost = br.ReadUInt16();
            type = br.ReadByte();
            equipable_by = br.ReadByte();
            property_flags = br.ReadUInt16();

            br.ReadUInt16(); //unused zeros

            long offset_script_menu = -1;
            long offset_script_battle = -1;
            long offset_script_weapon = -1;
            long offset_script_armor = -1;

            if ((property_flags & 0x0001) != 0) //Menu Script
                offset_script_menu = br.ReadUInt16();
            if ((property_flags & 0x0002) != 0) //Battle Script
                offset_script_battle = br.ReadUInt16();
            if ((property_flags & 0x0004) != 0) //Weapon Script
                offset_script_weapon = br.ReadUInt16();
            if ((property_flags & 0x0008) != 0) //Armor Script
                offset_script_armor = br.ReadUInt16();
            if ((property_flags & 0x0010) != 0) //ANG Bonus
                bonus_ang = br.ReadUInt16();
            if ((property_flags & 0x0020) != 0) //ABW Bonus
                bonus_abw = br.ReadUInt16();
            if ((property_flags & 0x0040) != 0) //STR Bonus
                bonus_str = br.ReadUInt16();
            if ((property_flags & 0x0080) != 0) //BWG Bonus
                bonus_bwg = br.ReadUInt16();
            if ((property_flags & 0x0100) != 0) //INT Bonus
                bonus_int = br.ReadUInt16();
            if ((property_flags & 0x0200) != 0) //MUT Bonus
                bonus_mut = br.ReadUInt16();
            if ((property_flags & 0x0400) != 0) //MGA Bonus
                bonus_mga = br.ReadUInt16();
            if ((property_flags & 0x0800) != 0) //Unused
            { }
            if ((property_flags & 0x1000) != 0) //Unused
            { }
            if ((property_flags & 0x2000) != 0) //Battle Animation
                battle_anim = br.ReadUInt16();
            if ((property_flags & 0x4000) != 0) //Unknown
                unknown_property = br.ReadUInt16();
            if ((property_flags & 0x8000) != 0) //IP Attack
                ip_attack = br.ReadUInt16();

            //Menu Script
            if (offset_script_menu >= 0)
            {
                br.BaseStream.Seek(offset + offset_script_menu, SeekOrigin.Begin);
                script_menu = new L2BattleScript(br, (int)offset_script_menu);
            }

            //Battle Script
            if (offset_script_battle >= 0)
            {
                br.BaseStream.Seek(offset + offset_script_battle, SeekOrigin.Begin);
                script_battle = new L2BattleScript(br, (int)offset_script_battle);
            }

            //Weapon Script
            if (offset_script_weapon >= 0)
            {
                br.BaseStream.Seek(offset + offset_script_weapon, SeekOrigin.Begin);
                script_weapon = new L2BattleScript(br, (int)offset_script_weapon);
            }

            //Armor Script
            if (offset_script_armor >= 0)
            {
                br.BaseStream.Seek(offset + offset_script_armor, SeekOrigin.Begin);
                script_weapon = new L2BattleScript(br, (int)offset_script_armor);
            }
        }
    }
}

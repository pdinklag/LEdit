using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2Spell
    {
        #region STATIC
        public const int Count = 0x28;
        public const long TableOffset = 0x19FA5B;
        //public const long TableOffset_New = 0x308000;

        static L2Spell[] S = new L2Spell[Count];

        public static L2Spell Get(int n)
        {
            if (n >= 0 && n < Count)
                return S[n];
            else
                return null;
        }
        public static void Set(int n, L2Spell spell)
        {
            if (n >= 0 && n < Count)
                S[n] = spell;
        }

        public static void InitROM(BinaryReader br)
        {
            for (int i = 0; i < Count; i++)
            {
                br.BaseStream.Seek(TableOffset + (i << 1), SeekOrigin.Begin);
                br.BaseStream.Seek(TableOffset + br.ReadUInt16(), SeekOrigin.Begin);
                S[i] = new L2Spell(br);
            }
        }

        #endregion

        L2String name;
        public L2String Name { get { return name; } set { name = value; } }

        byte usability;
        public byte Usability { get { return usability; } set { usability = value; } }

        byte unknown1;
        public byte Unknown1 { get { return unknown1; } set { unknown1 = value; } }

        byte usable_by;
        public byte UsableBy { get { return usable_by; } set { usable_by = value; } }

        byte unknown2;
        public byte Unknown2 { get { return unknown2; } set { unknown2 = value; } }

        ushort mpcost;
        public ushort MPCost { get { return mpcost; } set { mpcost = value; } }

        byte unknown3;
        public byte Unknown3 { get { return unknown3; } set { unknown3 = value; } }

        ushort cost;
        public ushort Cost { get { return cost; } set { cost = value; } }

        ushort script_offset;
        public ushort ScriptOffset { get { return script_offset; } set { script_offset = value; } }

        L2BattleScript script = new L2BattleScript();
        public L2BattleScript Script { get { return script; } set { script = value; } }

        public L2Spell(BinaryReader br)
        {
            FromStream(br);
        }

        public void FromStream(BinaryReader br)
        {
            name = new L2String(br, 8);
            usability = br.ReadByte();
            unknown1 = br.ReadByte();
            usable_by = br.ReadByte();
            unknown2 = br.ReadByte();
            mpcost = br.ReadUInt16();
            unknown3 = br.ReadByte();
            cost = br.ReadUInt16();
            script_offset = br.ReadUInt16();

            script = new L2BattleScript(br, script_offset);
        }
    }
}

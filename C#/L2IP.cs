using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2IP
    {
        #region STATIC
        public const int Count = 0xA8;
        public const long TableOffset = 0x20F11;
        //public const long TableOffset_New = 0x308000;
        public const byte Bank = 0x84;

        static L2IP[] IP = new L2IP[Count];

        public static L2IP Get(int n)
        {
            if (n >= 0 && n < Count)
                return IP[n];
            else
                return null;
        }
        public static void Set(int n, L2IP ip)
        {
            if (n >= 0 && n < Count)
                IP[n] = ip;
        }

        public static void InitROM(BinaryReader br)
        {
            for (int i = 0; i < Count; i++)
            {
                br.BaseStream.Seek(TableOffset + (i << 1), SeekOrigin.Begin);

                SNESExLoROMAddress rom_addr = new SNESExLoROMAddress(Bank, br.ReadUInt16());
                br.BaseStream.Seek(rom_addr.ToPCAddress(), SeekOrigin.Begin);
                IP[i] = new L2IP(br);
            }
        }

        #endregion

        ushort effect;
        public ushort Effect { get { return effect; } set { effect = value; } }

        byte anim;
        public byte Animation { get { return anim; } set { anim = value; } }

        byte targeting_cursor;
        public byte TargetingCursor { get { return targeting_cursor; } set { targeting_cursor = value; } }

        byte targeting_mode;
        public byte TargetingMode { get { return targeting_mode; } set { targeting_mode = value; } }

        byte ip_cost;
        public byte IPCost { get { return ip_cost; } set { ip_cost = value; } }

        L2String name;
        public L2String Name { get { return name; } set { name = value; } }

        public L2IP(BinaryReader br)
        {
            FromStream(br);
        }

        public void FromStream(BinaryReader br)
        {
            effect = br.ReadUInt16();
            anim = br.ReadByte();
            targeting_cursor = br.ReadByte();
            targeting_mode = br.ReadByte();
            ip_cost = br.ReadByte();
            name = new L2String(br);
        }
    }
}

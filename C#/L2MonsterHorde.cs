using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2MonsterHorde
    {
        #region STATIC
        public const long DataOffset = 0x1AC56F;
        public const int Count = 0xC0;

        static L2MonsterHorde[] MH = new L2MonsterHorde[Count];

        public static L2MonsterHorde Get(int n)
        {
            if (n >= 0 && n < Count)
                return MH[n];
            else
                return null;
        }
        public static void Set(int n, L2MonsterHorde horde)
        {
            if (n >= 0 && n < Count)
                MH[n] = horde;
        }

        public static void InitROM(BinaryReader br)
        {
            br.BaseStream.Seek(DataOffset, SeekOrigin.Begin);
            for (int i = 0; i < Count; i++)
                MH[i] = new L2MonsterHorde(br);
        }

        #endregion

        byte[] monsters;
        public byte[] Monsters { get { return monsters; } set { monsters = value; } }

        public L2MonsterHorde(BinaryReader br)
        {
            FromStream(br);
        }

        public void FromStream(BinaryReader br)
        {
            monsters = br.ReadBytes(8);
        }
    }
}

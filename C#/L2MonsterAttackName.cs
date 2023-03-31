using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2MonsterAttackName
    {
        #region STATIC
        /*private static byte count;
        public static byte Count { get { return count; } set { count = value; } }

        public const byte InitialCount = 0xE0;*/
        public const byte InitialCount = 0x8A;
        public const long TableOffset = 0x2F308;
        public const byte Bank = 0x85;
        //public const long TableOffset_LPatch = 0x408000;

        public const string FileName = "files\\monster_attack_names.l2";
        public const int FileSize = 0x7BD; //original value

        static L2String[] MAttN = new L2String[InitialCount];
        public static L2String Get(int n)
        {
            if (n >= 0 && n < InitialCount)
                return MAttN[n];
            else
                return new L2String("ERROR: 0x" + n.ToString("X2") + " is out of range!");
        }
        public static void Set(int n, L2String str)
        {
            if (n >= 0 && n < InitialCount)
                MAttN[n] = str;
        }

        public static void InitROM(BinaryReader rom)
        {
            for (int i = 0; i < InitialCount; i++)
            {
                rom.BaseStream.Seek(TableOffset + (i << 1), SeekOrigin.Begin);

                SNESExLoROMAddress address =
                    new SNESExLoROMAddress(Bank, rom.ReadUInt16());

                rom.BaseStream.Seek(address.ToPCAddress(), SeekOrigin.Begin);
                MAttN[i] = new L2String(rom);
            }
        }
        #endregion
    }
}

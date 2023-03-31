using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2String
    {
        string val;
        public string Value { get { return val; } set { val = value; } }

        public L2String()
        {
            val = string.Empty;
        }

        public L2String(string value)
        {
            val = value;
        }

        public L2String(BinaryReader br)
        {
            FromStream(br);
        }

        public L2String(BinaryReader br, int len)
        {
            FromStream(br, len);
        }

        public void ToStream(BinaryWriter bw, int len, bool zero_termination)
        {
            char[] str = val.ToCharArray();

            if (len == 0 || len > str.Length)
                len = str.Length;

            for (int i = 0; i < len; i++)
            {
                switch (str[i])
                {
                    case '\n':
                        break;

                    case '\r':
                        bw.Write((byte)0x03);
                        break;

                    case 'ö':
                        bw.Write((byte)0x28);
                        break;

                    case 'ü':
                        bw.Write((byte)0x3B);
                        break;

                    case 'ä':
                        bw.Write((byte)0x3C);
                        break;

                    case 'ß':
                        bw.Write((byte)0x7C);
                        break;

                    default:
                        bw.Write((byte)str[i]);
                        break;
                }
            }

            if (zero_termination)
                bw.Write((byte)0x00);
        }

        public void FromStream(BinaryReader br)
        {
            long pos = br.BaseStream.Position;

            int len = 0;
            for (byte b = br.ReadByte(); b != 0; b = br.ReadByte())
                ++len;

            ++len;

            br.BaseStream.Position = pos;
            FromStream(br, len);
        }

        public void FromStream(BinaryReader br, int len)
        {
            byte[] str = br.ReadBytes(len);

            val = string.Empty;
            for (int i = 0; i < len; i++)
            {
                switch (str[i])
                {
                    case 0x00:      //End of string
                    case 0x05:      //Read from table
                    case 0xE0:      //Unknown
                        break;

                    case 0x03:
                        val += "\r\n";
                        break;

                    case 0x28:      //'('
                        val += "ö";
                        break;

                    case 0x3B:      //';'
                        val += "ü";
                        break;

                    case 0x3C:      //'<'
                        val += "ä";
                        break;

                    case 0x7C:      //'|'
                        val += "ß";
                        break;

                    default:
                        val += (char)str[i];
                        break;
                }
            }
        }
    }
}

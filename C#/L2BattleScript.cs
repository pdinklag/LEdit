using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2BattleScript
    {
        public static int GetArgumentByteCount(byte cmd)
        {
            switch (cmd)
            {
                case 0x00:
                case 0x01:
                case 0x02:
                case 0x1D:
                case 0x28:
                case 0x29:
                case 0x2A:
                case 0x2E:
                case 0x37:
                case 0x3C:
                case 0x41:
                case 0x4F:
                case 0x50:
                case 0x51:
                case 0x58:
                case 0x5B:
                case 0x5C:
                    return 0;

                case 0x11:
                case 0x19:
                case 0x1A:
                case 0x1B:
                case 0x1C:
                case 0x1E:
                case 0x2D:
                case 0x2C:
                case 0x2F:
                case 0x30:
                case 0x32:
                case 0x35:
                case 0x3D:
                case 0x3E:
                case 0x40:
                case 0x4B:
                case 0x4C:
                case 0x54:
                case 0x5A:
                    return 1;

                case 0x03:
                case 0x04:
                case 0x12:
                case 0x13:
                case 0x14:
                case 0x15:
                case 0x1F:
                case 0x20:
                case 0x23:
                case 0x25:
                case 0x26:
                case 0x27:
                case 0x2B:
                case 0x42:
                case 0x44: //used by Pierre
                case 0x56:
                    return 2;

                case 0x05:
                case 0x0C:
                case 0x0D:
                case 0x0E:
                case 0x0F:
                case 0x10:
                case 0x16:
                case 0x17:
                case 0x18:
                case 0x46: //used by Daniele and several shield and helmets etc
                case 0x55:
                case 0x57:
                    return 3;

                case 0x24:
                    return 4;

                case 0x06:
                case 0x07:
                case 0x08:
                case 0x09:
                case 0x0A:
                case 0x0B:
                case 0x21:
                case 0x22:
                    return 5;

                case 0x31:
                case 0x33:
                case 0x34:
                case 0x36:
                case 0x38:
                case 0x39:
                case 0x3A:
                case 0x3B:
                case 0x3F: //"for capsule monsters"
                case 0x43:
                case 0x45:
                case 0x47: //magic spell for IPs
                case 0x48:
                case 0x49:
                case 0x4A:
                case 0x4D:
                case 0x4E:
                case 0x52:
                case 0x53:
                case 0x59:
                default:
                    return -1;
            }
        }

        Collection<byte> script = new Collection<byte>();
        int offset;

        public int Size { get { return script.Count; } }
        public Collection<byte> Bytes { get { return script; } set { script = value; } }
        public int Offset { get { return offset; } set { offset = value; } }

        public L2BattleScript()
        {

        }

        public L2BattleScript(BinaryReader br, int offset)
        {
            FromStream(br, offset);
        }

        public L2BattleScript(Collection<byte> bytes, int ex_offset)
        {
            script = bytes;
            offset = ex_offset;
        }

        public void FromStream(BinaryReader br, int ex_offset)
        {
            offset = ex_offset;

            bool bContinue = true;
            int coffset = offset;
            int index, jump_offset;
            int highest_jump_offset = offset;

            byte b;
            while (bContinue)
            {
                b = br.ReadByte();

                index = script.Count;
                script.Add(b);

                int x = L2BattleScript.GetArgumentByteCount(b);
                if (x >= 0)
                {
                    for (int i = 0; i < L2BattleScript.GetArgumentByteCount(b); i++)
                        script.Add(br.ReadByte());

                    coffset += 1 + x;

                    if (b == 0x00 || b == 0x2A || b == 0x41 || b == 0x4F) //0x29 ?
                    {
                        if (coffset > highest_jump_offset)
                            bContinue = false;
                    }
                    else
                    {
                        jump_offset = offset;

                        if (b == 0x03 || b == 0x04)
                            jump_offset = script[index + 1] | (script[index + 2] << 8);
                        else if (b == 0x05)
                            jump_offset = script[index + 2] | (script[index + 3] << 8);
                        else if (b >= 0x06 && b <= 0x0B)
                            jump_offset = script[index + 4] | (script[index + 5] << 8);

                        if (jump_offset > highest_jump_offset)
                            highest_jump_offset = jump_offset;
                    }
                }
                else
                {
                    Console.WriteLine("SCRIPT ERROR: Could not parse OpCode 0x" + b.ToString("X2") +
                        ", Address = 0x" + (br.BaseStream.Position - 1).ToString("X8"));

                    script.Add(0xFF);
                    break;
                }
            }
        }

        public string Disassemble()
        {
            string text = string.Empty;

            int o = 0;
            while (o < script.Count)
            {
                text += "0x" + (o + offset).ToString("X4") + ": ";

                byte cmd = script[o++];

                if (o + GetArgumentByteCount(cmd) - 1 >= script.Count)
                {
                    text += "ERROR: Incomplete instruction!\r\n";
                    return text;
                }

                byte arg8, arg8_2;
                ushort arg16, arg16_2;

                switch (cmd)
                {
                    case 0x00:
                        text += "DONE\r\n";
                        break;

                    case 0x01:
                        text += "CONTINUE\r\n";
                        break;

                    case 0x03:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "GOTO (offset = 0x" + arg16.ToString("X4") + 
                            ")\r\n";
                        break;

                    case 0x04:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "ON_FAILURE_GOTO (offset = 0x" + arg16.ToString("X4") + 
                            ")\r\n";
                        break;

                    case 0x05:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "ON_CHANCE_GOTO (chance = 0x" + arg8.ToString("X2") + 
                            ", offset = 0x" + arg16.ToString("X4") + 
                            ")\r\n";
                        break;

                    case 0x06:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_EQUAL_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x07:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_NOT_EQUAL_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x08:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_GREATER_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x09:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_LESS_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0A:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_GREATER_OR_EQUAL_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0B:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "IF_LESS_OR_EQUAL_GOTO (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ", offset = 0x" + arg16_2.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0C:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_SET (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0D:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_ADD (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0E:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_SUB (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x0F:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_MUL (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x10:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_DIV_SIGNED (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x11:
                        arg8 = script[o++];

                        text += "REGISTER_RAND (register = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x13:
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "REGISTER_SET_STAT (register = 0x" + arg8.ToString("X2") +
                            ", stat = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x14:
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "STAT_SET_REGISTER (stat = 0x" + arg8.ToString("X2") +
                            ", register = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x15:
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "REGISTER_SET_STAT_SELF (register = 0x" + arg8.ToString("X2") +
                            ", stat = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x16:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_AND (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x17:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_OR (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x18:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_XOR (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x19:
                        arg8 = script[o++];

                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ", arg = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x1A:
                        arg8 = script[o++];

                        text += "REGISTER_NEG (register = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x1E:
                        arg8 = script[o++];

                        text += "DISPLAY_ATTACK_NAME (id = 0x" + arg8.ToString("X2") + 
                            " [" + 
                            L2MonsterAttackName.Get(arg8 - 1).Value +
                            "] )\r\n";
                        break;

                    case 0x1F:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "ELEMENTAL_POWER (power = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x20:
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "CRITICAL_HIT_CHANCE (chance = 0x" + arg8.ToString("X2") +
                            ", damagemult = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x21:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg8 = script[o++];

                        text += "PHYSICAL_DAMAGE (element = 0x" + arg16.ToString("X4") + 
                            ", amount = 0x" + arg16_2.ToString("X4") + 
                            ", unknown = 0x" + arg8.ToString("X2") + 
                            ")\r\n";
                        break;

                    case 0x22:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg16_2 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg8 = script[o++];

                        text += "MAGICAL_DAMAGE (element = 0x" + arg16.ToString("X4") +
                            ", amount = 0x" + arg16_2.ToString("X4") +
                            ", unknown = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x23:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "BUNNY_SWORD (arg = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x24:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg8_2 = script[o++];

                        text += "RESTORE_STAT (type = 0x" + arg8.ToString("X2") +
                            ", amount = 0x" + arg16.ToString("X4") +
                            ", fluct = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x25:
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "AFFECT_STAT (register = 0x" + arg8.ToString("X2") +
                            ", percent = " + arg8_2.ToString() +
                            "%)\r\n";
                        break;

                    case 0x26:
                        arg8 = script[o++];
                        arg8_2 = script[o++];
                        text += "RECOVER_FROM (effect = 0x" + arg8.ToString("X2") + 
                            ", chance = " + arg8_2.ToString() + 
                            "%)\r\n";
                        break;

                    case 0x27:
                        arg8 = script[o++];
                        arg8_2 = script[o++];
                        text += "APPLY_EFFECT (effect = 0x" + arg8.ToString("X2") + 
                            ", chance = " + arg8_2.ToString() + 
                            "%)\r\n";
                        break;

                    case 0x28:
                        text += "PHYSICAL_ATTACK\r\n";
                        break;

                    case 0x29:
                        text += "BLOCK\r\n";
                        break;

                    case 0x2A:
                        //++o;
                        text += "FLEE\r\n";
                        break;

                    case 0x2B:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "USE_ITEM (item = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x2C:
                        arg8 = script[o++];

                        text += "MONSTER_CAST_SPELL (spell = 0x" + arg8.ToString("X2") +
                            " [" + L2Spell.Get(arg8).Name.Value.TrimEnd() + "] )\r\n";
                        break;

                    case 0x2D:
                        //FIXME
                        /*
                        arg8 = script[o++];
                        text += "CALL_REINFORCEMENT (monster = 0x" + arg8.ToString("X2") +
                            " [" + L2Monster.Get(arg8).Name.Value.TrimEnd() + "] )\r\n";
                         */
                        break;

                    case 0x32:
                        arg8 = script[o++];
                        text += "TARGET (code = #0x" + arg8.ToString("X2") + 
                            ")\r\n";
                        break;

                    case 0x35:
                        arg8 = script[o++];
                        text += "SPECIAL_ITEM (type = #0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x37:
                        text += "WEAPON_PHYSICAL_ATTACK\r\n";
                        break;

                    case 0x3C:
                        text += "WAIT_UNTIL_EFFECT_DONE\r\n";
                        break;

                    case 0x3E:
                        arg8 = script[o++];

                        text += "DISPLAY_CM_ATTACK_NAME (id = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x41:
                        text += "WAIT\r\n";
                        break;

                    case 0x42:
                        arg8 = script[o++];
                        arg8_2 = script[o++]; //always 0

                        text += "ELEMENTAL_CHARACTER (character = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x4F:
                        text += "QUIT\r\n";
                        break;

                    case 0x51:
                        text += "DARK_REFLECTOR\r\n";
                        break;

                    case 0x54:
                        arg8 = script[o++];

                        text += "CAST_SPELL_IP (spell = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x55:
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "REGISTER_DIV_UNSIGNED (register = 0x" + arg8.ToString("X2") +
                            ", value = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x56:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "ATTACK_NAME_DURATION (duration = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x57:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;
                        arg8 = script[o++];

                        text += "ITEM_EFFECTIVENESS (item = 0x" + arg16.ToString("X4") +
                            ", increment = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x58:
                        text += "EERIE_LIGHT\r\n";
                        break;

                    case 0x5A:
                        arg8 = script[o++];

                        text += "BATTLE_ANIMATION (anim = 0x" + arg8.ToString("X2") +
                            " [" +
                            L2ROM.GetBattleAnimName(arg8 + 1) + 
                            "] )\r\n";
                        break;

                    case 0x5B:
                        text += "LOSE_ON_ENEMY_SUICIDE\r\n";
                        break;

                    case 0x5C:
                        text += "DISABLE_DAMAGE_DISPLAY\r\n";
                        break;

                    case 0x46: //Daniele
                        arg8 = script[o++];
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ", arg = 0x" + arg8.ToString("X2") +
                            ", arg = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x44: //Pierre
                        arg8 = script[o++];
                        arg8_2 = script[o++];

                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ", arg = 0x" + arg8.ToString("X2") +
                            ", arg = 0x" + arg8_2.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x12:
                        arg16 = (ushort)(script[o] | (script[o + 1] << 8)); o += 2;

                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ", arg = 0x" + arg16.ToString("X4") +
                            ")\r\n";
                        break;

                    case 0x1B:
                    case 0x1C:
                    case 0x2F:
                    case 0x30:
                    case 0x3D:
                    case 0x40:
                    case 0x4B:
                    case 0x4C:
                        arg8 = script[o++];

                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ", arg = 0x" + arg8.ToString("X2") +
                            ")\r\n";
                        break;

                    case 0x1D:
                    case 0x2E:
                    case 0x50:
                    default:
                        text += "UNKNOWN (opcode = 0x" + cmd.ToString("X2") +
                            ")\r\n";
                        break;
                        //Unknown

                    case 0xFF:
                        text += "[ERROR!]\r\n";
                        break;
                }
            }

            return text;
        }
    }
}

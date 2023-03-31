package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.snes.SnesSerializable;
import de.pdinklag.util.Hex;

import java.io.IOException;
import java.io.Serializable;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Logger;

public class BattleScript implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -5632990947823334463L;
    private static final Logger logger = Logger.getLogger("l2.BattleScript");

    public static int getParameterByteCount(int cmd) {
        switch (cmd) {
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


    private final ArrayList<Integer> bytes = new ArrayList<Integer>();
    private transient int offsetBase = 0;

    public void setOffsetBase(int offsetBase) {
        this.offsetBase = offsetBase;
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        boolean done = false;

        int currentOffset = offsetBase;
        int index, jumpOffset, paramBytes;
        int highestJumpOffset = offsetBase;
        int b;

        while (!done) {
            b = in.readByte();

            index = bytes.size();
            bytes.add(b);

            paramBytes = getParameterByteCount(b);
            if (paramBytes >= 0) {
                for (int i = 0; i < paramBytes; i++)
                    bytes.add(in.readByte());

                currentOffset += 1 + paramBytes;

                if (b == 0x00 || b == 0x2A || b == 0x41 || b == 0x4F /* || 0x29 ? */) {
                    if (currentOffset > highestJumpOffset)
                        done = true;
                } else {
                    jumpOffset = 0;

                    if (b == 0x03 || b == 0x04)
                        jumpOffset = bytes.get(index + 1) | (bytes.get(index + 2) << 8);
                    else if (b == 0x05)
                        jumpOffset = bytes.get(index + 2) | (bytes.get(index + 3) << 8);
                    else if (b >= 0x06 && b <= 0x0B)
                        jumpOffset = bytes.get(index + 4) | (bytes.get(index + 5) << 8);

                    if (jumpOffset > highestJumpOffset)
                        highestJumpOffset = jumpOffset;
                }
            } else {
                logger.warning("Failed to parse opcode 0x" + Hex.format(b, 2));
            }
        }
        return bytes.size();
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        for (int b : bytes)
            out.writeByte(b);

        return bytes.size();
    }

    public List<Integer> getBytes() {
        return bytes;
    }
}

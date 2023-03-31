package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.snes.SnesSerializable;

import java.io.IOException;
import java.io.Serializable;

public class L2String implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -8419349726990456686L;

    private String value;
    private int fixedLength;

    public L2String() {
        this.value = "";
        this.fixedLength = 0;
    }

    public L2String(String value) {
        this.value = value;
    }

    public L2String(int fixedLength) {
        this.fixedLength = fixedLength;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        StringBuilder valueBuilder = new StringBuilder();

        if (fixedLength > 0) {
            if (value.length() > fixedLength) {
                value = value.substring(0, fixedLength);
            }
        }

        valueBuilder.append(value);

        if (fixedLength > 0) {
            while (valueBuilder.length() < fixedLength) {
                valueBuilder.append(" ");
            }
        }

        this.value = valueBuilder.toString();
    }

    public int getFixedLength() {
        return fixedLength;
    }

    public void setFixedLength(int fixedLength) {
        this.fixedLength = fixedLength;
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        StringBuilder str = new StringBuilder();

        for (int i = 0; i < fixedLength; i++) {
            int c = in.readByte();
            switch (c) {
                case 0x00: //End of string
                case 0x05: //Read from table
                case 0xE0: //Unknown
                    break;

                case 0x03:
                    str.append('\n');
                    break;

                //'('
                case 0x28:
                    str.append('ö');
                    break;

                //';'
                case 0x3B:
                    str.append('ü');
                    break;

                //'<'
                case 0x3C:
                    str.append('ä');
                    break;

                //'|'
                case 0x7C:
                    str.append('ß');
                    break;

                default:
                    str.append((char) c);
            }
        }

        this.value = str.toString();
        return fixedLength;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        char[] chars = value.toCharArray();
        for (char c : chars) {
            switch (c) {
                case '\n':
                    out.writeByte(0x03);
                    break;

                case 'ö':
                    out.writeByte(0x28);
                    break;

                case 'ü':
                    out.writeByte(0x3B);
                    break;

                case 'ä':
                    out.writeByte(0x3C);
                    break;

                case 'ß':
                    out.writeByte(0x7C);
                    break;

                default:
                    out.writeByte(c);
                    break;
            }
        }
        return chars.length;
    }
}

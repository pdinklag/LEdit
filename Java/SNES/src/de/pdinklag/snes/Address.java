package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.util.Hex;

import java.io.IOException;
import java.io.Serializable;

/**
 * Abstract base class for 24 bit addresses (8 bits for bank, 16 bits for offset).
 */
public abstract class Address implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -1151063383239453149L;

    protected int bank, offset;

    public int getBank() {
        return bank;
    }

    public void setBank(int bank) {
        this.bank = bank;
    }

    public int getOffset() {
        return offset;
    }

    public void setOffset(int offset) {
        this.offset = offset;
    }

    /**
     * Converts a PC ROM file address.
     *
     * @param romAddress The address inside the ROM file.
     */
    public abstract void setRomAddress(long romAddress);


    /**
     * Converts this address to a PC ROM file address.
     *
     * @return The address inside the ROM file.
     */
    public abstract long getRomAddress();

    @Override
    public String toString() {
        StringBuilder str = new StringBuilder(8);
        str.append("$").append(Hex.format(bank, 2));
        str.append(":").append(Hex.format(offset, 4));
        return str.toString();
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;

        Address that = (Address) o;

        if (bank != that.bank) return false;
        if (offset != that.offset) return false;

        return true;
    }

    @Override
    public int hashCode() {
        int result = bank;
        result = 31 * result + offset;
        return result;
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        this.bank = in.readByte();
        this.offset = in.readUnsignedShort();
        return 3;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        out.writeByte(bank);
        out.writeShort(offset);
        return 3;
    }
}

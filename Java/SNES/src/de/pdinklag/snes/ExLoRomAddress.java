package de.pdinklag.snes;

/**
 * Class to convert between SNES ExLoROM addresses and PC ROM file addresses.
 */
public class ExLoRomAddress extends Address {
    private static final long serialVersionUID = -1151063383239453149L;
    public final static int SNES_SIZE = 3;

    public ExLoRomAddress() {
    }

    public ExLoRomAddress(int bank, int offset) {
        this.bank = bank & 0xFF;
        this.offset = offset & 0xFFFF;
    }

    public ExLoRomAddress(long romAddress) {
        setRomAddress(romAddress);
    }

    @Override
    public void setRomAddress(long romAddress) {
        int bank = (int) ((romAddress >> 16) & 0xFF);
        int offset = (int) (romAddress & 0xFFFF);

        if (bank <= 0x7E) {
            if ((bank & 0x40) != 0)
                this.bank = (((bank & 0x3F) << 1) | ((offset & 0x8000) >> 0xF));
            else
                this.bank = (0x80 | (bank << 1) | ((offset & 0x8000) >> 0xF));

            this.offset = (0x8000 | (offset & 0x7FFF));
        }
    }

    /**
     * Converts this address to a PC ROM file address.
     *
     * @return The address inside the ROM file.
     */
    @Override
    public long getRomAddress() {
        int bank, offset;

        if ((this.bank & 0x80) != 0)
            bank = (byte) ((this.bank & 0x7F) >> 1);
        else
            bank = (byte) ((this.bank | 0x80) >> 1);

        offset = ((this.offset & 0x7FFF) | ((this.bank & 1) << 0xF));

        return (long) ((bank << 16) | offset);
    }
}

package de.pdinklag.snes;

/**
 * Class to convert between SNES LoROM addresses and PC ROM file addresses.
 */
public class LoRomAddress extends Address {
    private static final long serialVersionUID = -5128831595779514084L;
    public final static int SNES_SIZE = 3;

    public LoRomAddress() {
    }

    public LoRomAddress(int bank, int offset) {
        this.bank = bank & 0xFF;
        this.offset = offset & 0xFFFF;
    }

    public LoRomAddress(long romAddress) {
        setRomAddress(romAddress);
    }

    @Override
    public void setRomAddress(long romAddress) {
        int bank = (int) ((romAddress >> 16) & 0xFF);
        int offset = (int) (romAddress & 0xFFFF);

        if (bank <= 0x7E) {
            this.bank = (0x80 | (bank << 1) | ((offset & 0x8000) >> 0xF));
            this.offset = (0x8000 | (offset & 0x7FFF));
        }
    }

    @Override
    public long getRomAddress() {
        int bank, offset;

        bank = (byte) ((this.bank & 0x7F) >> 1);
        offset = ((this.offset & 0x7FFF) | ((this.bank & 1) << 0xF));

        return (long) ((bank << 16) | offset);
    }
}

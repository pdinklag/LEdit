package de.pdinklag.io;

import java.io.IOException;
import java.io.OutputStream;

/**
 * Little-endian binary output stream.
 */
public class BinaryOutputStream extends OutputStream {
    private final OutputStream wrapped;

    public BinaryOutputStream(OutputStream out) {
        this.wrapped = out;
    }

    @Override
    public void write(int b) throws IOException {
        wrapped.write(b);
    }

    @Override
    public void flush() throws IOException {
        wrapped.flush();
    }

    @Override
    public void close() throws IOException {
        wrapped.close();
    }

    public synchronized void writeByte(int x) throws IOException {
        write(x);
    }

    public synchronized void writeShort(int x) throws IOException {
        write(x);
        write(x >> 8);
    }

    public synchronized void writeInt(int x) throws IOException {
        write(x);
        write(x >> 8);
        write(x >> 16);
        write(x >> 24);
    }

    public synchronized void writeLong(long x) throws IOException {
        writeInt((int) x);
        writeInt((int) (x >> 32));
    }

    public synchronized void writeFloat(float x) throws IOException {
        writeInt(Float.floatToIntBits(x));
    }
}

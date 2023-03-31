package de.pdinklag.io;

import java.io.IOException;
import java.io.InputStream;

/**
 * Little-endian binary input stream.
 */
public class BinaryInputStream extends InputStream {
    private final InputStream wrapped;

    public BinaryInputStream(InputStream in) {
        this.wrapped = in;
    }

    @Override
    public int read() throws IOException {
        return wrapped.read();
    }

    @Override
    public void close() throws IOException {
        wrapped.close();
    }

    @Override
    public long skip(long n) throws IOException {
        return wrapped.skip(n);
    }

    @Override
    public int available() throws IOException {
        return wrapped.available();
    }

    @Override
    public void mark(int readlimit) {
        wrapped.mark(readlimit);
    }

    @Override
    public void reset() throws IOException {
        wrapped.reset();
    }

    @Override
    public boolean markSupported() {
        return wrapped.markSupported();
    }

    public synchronized int readByte() throws IOException {
        return read();
    }

    public synchronized int readUnsignedShort() throws IOException {
        return read() | (read() << 8);
    }

    public synchronized int readInt() throws IOException {
        return read() | (read() << 8) | (read() << 16) | (read() << 24);
    }

    public synchronized long readUnsignedInt() throws IOException {
        int i = readInt();
        if (i >= 0)
            return (long) i;
        else
            return 0x100000000L + (long) i;
    }

    public synchronized long readLong() throws IOException {
        return readUnsignedInt() | (readUnsignedInt() << 32);
    }

    public synchronized float readFloat() throws IOException {
        return Float.intBitsToFloat(readInt());
    }
}

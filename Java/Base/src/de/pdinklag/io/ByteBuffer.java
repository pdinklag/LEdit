package de.pdinklag.io;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.io.Serializable;

/**
 * A byte array wrapper with seek, input stream and output stream support.
 * <p/>
 * This class is <i>not</i> responsible of resizing the buffer if the read / write position reached the end.
 */
public class ByteBuffer implements Serializable {
    private static final long serialVersionUID = -1737308960640778815L;

    private final byte[] buffer;
    private transient int position;

    /**
     * An {@link OutputStream} that can be used to write to the wrapped byte array.
     */
    public final OutputStream out = new OutputStream() {
        @Override
        public void write(int b) throws IOException {
            synchronized (ByteBuffer.this) {
                buffer[position++] = (byte) b;
            }
        }
    };

    /**
     * An {@link InputStream} that can be used to read from the wrapped byte array.
     */
    public final InputStream in = new InputStream() {
        @Override
        public int read() throws IOException {
            int b;
            synchronized (ByteBuffer.this) {
                b = buffer[position++];
            }
            return (b >= 0 ? b : 256 + b);
        }
    };

    /**
     * Creates a new byte buffer.
     *
     * @param size The size of the underlying byte array.
     */
    public ByteBuffer(int size) {
        buffer = new byte[size];
    }

    /**
     * Creates a new byte buffer.
     *
     * @param wrap The byte array to wrap in.
     */
    public ByteBuffer(byte[] wrap) {
        this.buffer = wrap;
    }

    /**
     * Yields the current read / write position of the streams.
     *
     * @return The current read / write position of the streams.
     */
    public int getPosition() {
        return position;
    }

    /**
     * Sets the read / write position of the streams.
     *
     * @param position The new read / write position of the streams.
     */
    public void setPosition(int position) {
        this.position = position;
    }

    public int getSize() {
        return buffer.length;
    }

    public byte[] getBytes() {
        return buffer;
    }
}

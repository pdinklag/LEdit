package de.pdinklag.snes;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;

import java.io.IOException;

/**
 * Interface for SNES encoded data sets.
 */
public interface SnesSerializable {
    /**
     * Initializes this object from a stream of SNES serialized data.
     *
     * @param in The stream to read from.
     * @return The amount of bytes read.
     * @throws IOException In case an I/O error occurs.
     */
    public abstract int readSnes(BinaryInputStream in) throws IOException;

    /**
     * Writes this object to a stream in SNES serialized form.
     *
     * @param out The stream to write to.
     * @return The amount of bytes written.
     * @throws IOException In case an I/O error occurs.
     */
    public abstract int writeSnes(BinaryOutputStream out) throws IOException;
}

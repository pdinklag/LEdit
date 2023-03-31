package de.pdinklag.io;

import java.io.*;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * File operation utility class.
 */
public abstract class FileUtils {
    private static final Logger logger = Logger.getLogger(FileUtils.class.getName());

    private static final int COPY_BUFFER_SIZE = 8192;

    /**
     * Extracts a resource to a file.
     *
     * @param ref      The reference class, the classpath of which is used to lookup the resource.
     * @param resource The resource key.
     * @param outFile  The file to extract this resource into.
     * @throws java.io.IOException In case an I/O error occurs.
     */
    public static void extractResource(Class ref, String resource, File outFile) throws IOException {
        InputStream in = ref.getResourceAsStream(resource);
        byte[] b = new byte[in.available()];
        in.read(b);
        in.close();

        OutputStream out = new FileOutputStream(outFile);
        out.write(b);
        out.close();
    }

    /**
     * Extracts a resource from a classpath to a temporary file.
     *
     * @param ref      The reference class, the classpath of which is used to lookup the resource.
     * @param resource The resource key.
     * @return The file the resource has been extracted to.
     */
    public static File extractResource(Class ref, String resource) {
        try {
            File outFile = File.createTempFile(ref.getName(), "extract");
            extractResource(ref, resource, outFile);
            return outFile;
        } catch (IOException ex) {
            logger.log(Level.WARNING, "Failed to extract resource " + resource, ex);
            return null;
        }
    }

    /**
     * Copies a file without overriding.
     *
     * @param file    The source file.
     * @param newFile The target file.
     * @return <tt>true</tt> if the file was successfully copied, <tt>false</tt> if an error occured or the
     *         target file already exists.
     */
    public static boolean copyFile(File file, File newFile) {
        return copyFile(file, newFile, false);
    }

    /**
     * Copies a file.
     *
     * @param file      The source file.
     * @param newFile   The target file.
     * @param bOverride Whether or not to override the target file if it already exists.
     * @return <tt>true</tt> if the file was successfully copied, <tt>false</tt> if an error occured or the
     *         target file already exists and <tt>bOverride</tt> is set to <tt>false</tt>.
     */
    public static boolean copyFile(File file, File newFile, boolean bOverride) {
        boolean success = false;

        if (bOverride || !newFile.exists()) {
            FileInputStream in = null;
            FileOutputStream out = null;

            try {
                byte[] buffer = new byte[COPY_BUFFER_SIZE];

                in = new FileInputStream(file);
                out = new FileOutputStream(newFile);

                for (int n = in.read(buffer); n > 0; n = in.read(buffer))
                    out.write(buffer, 0, n);

                success = true;
            } catch (IOException ex) {
                logger.log(Level.WARNING, "Failed to copy file " + file + " to " + newFile, ex);
            }

            try {
                if (out != null)
                    out.close();
            } catch (IOException ex) {
                //
            }

            try {
                if (in != null)
                    in.close();
            } catch (IOException ex) {
                //
            }
        }
        return success;
    }
}

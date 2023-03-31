package de.pdinklag.util;

/**
 * Utility class to format integer values as hexadecimal strings.
 */
public class Hex {
    public static String format(int n, int numDigits) {
        return String.format("%0" + numDigits + "X", n);
    }

    public static String formatByte(int n) {
        return String.format("%02X", n);
    }

    public static String formatWord(int n) {
        return String.format("%04X", n);
    }

    public static String formatDword(int n) {
        return String.format("%08X", n);
    }

    public static String formatDword(long n) {
        return String.format("%08X", n);
    }
}

package de.pdinklag.util;

/**
 * Interface for readable configuration nodes.
 */
public interface ReadableConfiguration {
    /**
     * Checks whether this configuration node contains a certain key.
     *
     * @param key The configuration key to look up.
     * @return <tt>true</tt> if this configuration node contains they key, <tt>false</tt> otherwise.
     */
    public boolean containsKey(String key);

    /**
     * Retrieves a string value from the configuration node.
     *
     * @param key The configuration key.
     * @param def The default value.
     * @return The string value associated to the given key, or the default value if the key does not exist.
     */
    public String getString(String key, String def);

    /**
     * Retrieves a string value from the configuration node.
     *
     * @param key The configuration key.
     * @return The string value associated to the given key, or the key itself if it does not exist.
     */
    public String getString(String key);

    /**
     * Retrieves an integer value from the configuration node.
     *
     * @param key The configuration key.
     * @param def The default value.
     * @return The integer value associated to the given key, or the default value if the key does not exist.
     */
    public int getInt(String key, int def);

    /**
     * Retrieves an integer value from the configuration node.
     *
     * @param key The configuration key.
     * @return The integer value associated to the given key, or zero if the key does not exist.
     */
    public int getInt(String key);

    /**
     * Retrieves a long value from the configuration node.
     *
     * @param key The configuration key.
     * @param def The default value.
     * @return The long value associated to the given key, or the default value if the key does not exist.
     */
    public long getLong(String key, long def);

    /**
     * Retrieves a long value from the configuration node.
     *
     * @param key The configuration key.
     * @return The long value associated to the given key, or zero if the key does not exist.
     */
    public long getLong(String key);

    /**
     * Retrieves a float value from the configuration node.
     *
     * @param key The configuration key.
     * @param def The default value.
     * @return The float value associated to the given key, or the default value if the key does not exist.
     */
    public float getFloat(String key, float def);

    /**
     * Retrieves a float value from the configuration node.
     *
     * @param key The configuration key.
     * @return The float value associated to the given key, or zero if the key does not exist.
     */
    public float getFloat(String key);

    /**
     * Retrieves a double value from the configuration node.
     *
     * @param key The configuration key.
     * @param def The default value.
     * @return The double value associated to the given key, or the default value if the key does not exist.
     */
    public double getDouble(String key, double def);

    /**
     * Retrieves a double value from the configuration node.
     *
     * @param key The configuration key.
     * @return The double value associated to the given key, or zero if the key does not exist.
     */
    public double getDouble(String key);
}

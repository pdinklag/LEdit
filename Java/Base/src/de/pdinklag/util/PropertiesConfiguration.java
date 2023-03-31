package de.pdinklag.util;

import java.util.Properties;

/**
 * Implementation of a {@link ReadableConfiguration} that is loaded from a properties file.
 */
public class PropertiesConfiguration extends Properties implements ReadableConfiguration {
    private static final long serialVersionUID = 5945693969521762061L;

    @Override
    public boolean containsKey(String key) {
        return super.containsKey(key);
    }

    @Override
    public String getString(String key, String def) {
        return containsKey(key) ? getProperty(key) : def;
    }

    @Override
    public String getString(String key) {
        return getString(key, key);
    }

    @Override
    public int getInt(String key, int def) {
        return containsKey(key) ? Integer.parseInt(getProperty(key)) : def;
    }

    @Override
    public int getInt(String key) {
        return getInt(key, 0);
    }

    @Override
    public long getLong(String key, long def) {
        return containsKey(key) ? Long.parseLong(getProperty(key)) : def;
    }

    @Override
    public long getLong(String key) {
        return getLong(key, 0);
    }

    @Override
    public float getFloat(String key, float def) {
        return containsKey(key) ? Float.parseFloat(getProperty(key)) : def;
    }

    @Override
    public float getFloat(String key) {
        return getFloat(key, 0.0f);
    }

    @Override
    public double getDouble(String key, double def) {
        return containsKey(key) ? Double.parseDouble(getProperty(key)) : def;
    }

    @Override
    public double getDouble(String key) {
        return getDouble(key, 0.0);
    }
}

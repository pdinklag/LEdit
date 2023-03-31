package de.pdinklag.util;

import java.util.LinkedList;
import java.util.ResourceBundle;

/**
 * Provides functions for localization purposes.
 */
public class Localizer {
    private static final LinkedList<ResourceBundle> bundles = new LinkedList<ResourceBundle>();

    /**
     * Registers a global resource bundle.
     *
     * @param bundle The resource bundle to register.
     */
    public static void addResourceBundle(ResourceBundle bundle) {
        bundles.add(bundle);
    }

    /**
     * Unregisters a global resource bundle.
     *
     * @param bundle The resource bundle to unregister.
     */
    public static void removeResourceBundle(ResourceBundle bundle) {
        bundles.remove(bundle);
    }

    /**
     * Localizes a string.
     *
     * @param bundle The resource bundle to use.
     * @param key    The localization key.
     * @return The localized string, or <tt>key</tt> if the key is not contained in the given bundle.
     */
    public static String localize(ResourceBundle bundle, String key) {
        if (bundle.containsKey(key))
            return bundle.getString(key);
        else
            return key;
    }

    /**
     * Localizes a string and replaces parameters.
     * <p/>
     * The string "$1" will be replaced by the first parameter, "$2" by the second, and so forth.
     *
     * @param bundle The resource bundle to use.
     * @param key    The localization key.
     * @param params The parameters.
     * @return The localized string with the parameters replaced, or <tt>key</tt> if the key is not contained
     *         in the given bundle.
     */
    public static String localize(ResourceBundle bundle, String key, String... params) {
        String str = localize(bundle, key);

        for (int i = 0; i < params.length; i++)
            str = str.replaceAll("\\$" + (i + 1), params[i]);

        return str;
    }

    /**
     * Localizes a string using the global resource bundles.
     *
     * @param key The localization key.
     * @return The localized string, or <tt>key</tt> if the key is not contained in any global resource bundle.
     * @see Localizer#addResourceBundle(java.util.ResourceBundle)
     */
    public static String localize(String key) {
        for (ResourceBundle bundle : bundles) {
            if (bundle.containsKey(key))
                return bundle.getString(key);
        }
        return key;
    }

    /**
     * Localizes a string and replaces parameters.
     * <p/>
     * The string "$1" will be replaced by the first parameter, "$2" by the second, and so forth.
     *
     * @param key    The localization key.
     * @param params The parameters.
     * @return The localized string with the parameters replaced, or <tt>key</tt>
     *         if the key is not contained in any global resource bundle.
     */
    public static String localize(String key, String... params) {
        String str = localize(key);

        for (int i = 0; i < params.length; i++)
            str = str.replaceAll("\\$" + (i + 1), params[i]);

        return str;
    }
}

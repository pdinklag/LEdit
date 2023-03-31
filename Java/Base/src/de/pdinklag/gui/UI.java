package de.pdinklag.gui;

import de.pdinklag.util.Localizer;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.lang.reflect.Method;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * GUI utility class.
 */
public class UI {
    private static final Logger logger = Logger.getLogger("ui");

    /**
     * Centralized string constant containing a colon.
     */
    public static final String COLON = ":";

    /**
     * Centralized constants for GUI gaps, such as the gaps for layout managers or insets.
     */
    public static final int GAP = 5;

    /**
     * Load an image from the classpath.
     *
     * @param resource The resource name of the image to load.
     * @return An {@link Image} or <tt>null</tt> if the resource could not be loaded.
     */
    public static Image loadImage(String resource) {
        try {
            return ImageIO.read(UI.class.getResource(resource));
        } catch (Exception ex) {
            logger.log(Level.WARNING, "Cannot load image resource: " + resource, ex);
            return null;
        }
    }

    /**
     * Load an image from the classpath and returns it as an icon.
     *
     * @param resource The resource name of the image to load.
     * @return An {@link Icon} or <tt>null</tt> if the resource could not be loaded.
     */
    public static Icon loadIcon(String resource) {
        try {
            return new ImageIcon(loadImage(resource));
        } catch (Exception ex) {
            logger.log(Level.WARNING, "Cannot load image resource: " + resource, ex);
            return null;
        }
    }

    /**
     * Scales an image down to an icon, preserving its aspect ratio.
     *
     * @param image The source image.
     * @param size  The icon size.
     * @return The produced icon.
     */
    public static Icon produceIcon(Image image, int size) {
        BufferedImage iconImage = new BufferedImage(size, size, BufferedImage.TYPE_4BYTE_ABGR);

        int iw = image.getWidth(null);
        int ih = image.getHeight(null);

        int w, h;
        if (iw > ih) {
            w = size;
            h = (int) ((float) size * (float) ih / (float) iw);
            iconImage.getGraphics().drawImage(
                    image.getScaledInstance(w, h, Image.SCALE_SMOOTH), 0, (size - h) / 2, null);
        } else {
            h = size;
            w = (int) ((float) size * (float) iw / (float) ih);
            iconImage.getGraphics().drawImage(
                    image.getScaledInstance(w, h, Image.SCALE_SMOOTH), (size - w) / 2, 0, null);
        }

        return new ImageIcon(iconImage);
    }

    /**
     * Recursively enables or disables a component and all sub-components.
     *
     * @param root    The root component. If it is a container, the method will be recursively invoked on all children.
     * @param enabled <tt>true</tt> to enable the components, <tt>false</tt> to disable.
     */
    public static void setEnabled(Component root, boolean enabled) {
        root.setEnabled(enabled);

        if (root instanceof Container) {
            for (Component c : ((Container) root).getComponents())
                setEnabled(c, enabled);
        }
    }

    /**
     * Recursively registers an {@link ActionListener} on a component and all sub-components.
     *
     * @param root     The root component.
     * @param listener The listener to register.
     */
    public static void setActionListener(Component root, ActionListener listener) {
        try {
            Method method = root.getClass().getMethod("addActionListener", ActionListener.class);
            method.invoke(root, listener);
        } catch (Exception ex) {
            //
        }

        if (root instanceof JMenu) {
            for (Component c : ((JMenu) root).getMenuComponents())
                setActionListener(c, listener);
        } else if (root instanceof Container) {
            for (Component c : ((Container) root).getComponents())
                setActionListener(c, listener);
        }
    }

    /**
     * Recursively searches a component tree for a {@link ValueField} with an invalid value.
     * <p/>
     * This can be used to quickly validate a whole form.
     *
     * @param root The root component.
     * @return The first found {@link ValueField} with an invalid value, or <tt>null</tt> if there are
     *         no value fields or all have valid values.
     */
    public static ValueField findInvalidValueField(Component root) {
        if (root instanceof ValueField && !root.isValid()) {
            return (ValueField) root;
        }

        ValueField invalid = null;
        if (root instanceof Container) {
            for (Component c : ((Container) root).getComponents()) {
                invalid = findInvalidValueField(c);
                if (invalid != null)
                    break;
            }
        }

        return invalid;
    }

    /**
     * Opens a modal dialog asking the user whether he wants to override a file.
     * <p/>
     * This method assumes that the localization keys <tt>ui.save.override.message</tt> and
     * <tt>ui.save.override.title</tt> are defined (see {@link Localizer}).
     *
     * @param owner The owner frame.
     * @param file  The file in question.
     * @return <tt>true</tt> if the user clicked "yes", <tt>false</tt> otherwise.
     */
    public static boolean showFileOverrideDialog(Frame owner, File file) {
        return (JOptionPane.showConfirmDialog(owner,
                Localizer.localize("ui.save.override.message", file.getAbsolutePath()),
                Localizer.localize("ui.save.override.title"),
                JOptionPane.YES_NO_OPTION, JOptionPane.QUESTION_MESSAGE) == JOptionPane.YES_OPTION);
    }
}

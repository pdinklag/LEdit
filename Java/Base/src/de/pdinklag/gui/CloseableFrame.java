package de.pdinklag.gui;

import javax.swing.*;
import java.awt.event.WindowEvent;
import java.awt.event.WindowListener;

/**
 * A JFrame utility sub-class that provides a <tt>close</tt> method and
 * registers itself as a {@link WindowListener}.
 */
public class CloseableFrame extends JFrame implements WindowListener {
    private static final long serialVersionUID = 4394310438118893939L;

    public CloseableFrame() {
        addWindowListener(this);
        setDefaultCloseOperation(DISPOSE_ON_CLOSE);
    }

    /**
     * Attempts to close the frame by invoking the {@link WindowEvent#WINDOW_CLOSING} event.
     */
    public void close() {
        processWindowEvent(new WindowEvent(this, WindowEvent.WINDOW_CLOSING));
    }

    @Override
    public void windowOpened(WindowEvent e) {
    }

    @Override
    public void windowClosing(WindowEvent e) {
    }

    @Override
    public void windowClosed(WindowEvent e) {
    }

    @Override
    public void windowIconified(WindowEvent e) {
    }

    @Override
    public void windowDeiconified(WindowEvent e) {
    }

    @Override
    public void windowActivated(WindowEvent e) {
    }

    @Override
    public void windowDeactivated(WindowEvent e) {
    }
}

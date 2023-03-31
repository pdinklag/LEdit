package de.pdinklag.ledit.gui;

import de.pdinklag.gui.UI;
import de.pdinklag.util.Localizer;

import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.*;

public class WaitDialog extends JDialog {
    private static final long serialVersionUID = 5837820443292912869L;

    public static void show(Frame owner, String title, String text, final Runnable action) {
        final WaitDialog wait = new WaitDialog(owner, title, text);
        new Thread(new Runnable() {
            @Override
            public void run() {
                action.run();
                wait.dispose();
            }
        }).start();
        wait.setVisible(true);
    }

    private final JLabel lblText = new JLabel();
    private final JProgressBar progress = new JProgressBar();

    public WaitDialog(Frame owner, String title, String text) {
        super(owner, title, true);

        lblText.setText(text);
        lblText.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.wait"))));

        progress.setIndeterminate(true);

        JPanel panel = new JPanel(new GridLayout(2, 1, UI.GAP, UI.GAP));
        panel.setBorder(new EmptyBorder(UI.GAP, UI.GAP, UI.GAP, UI.GAP));
        panel.add(lblText);
        panel.add(progress);

        setLayout(new BorderLayout());
        add(panel, BorderLayout.CENTER);

        setDefaultCloseOperation(DO_NOTHING_ON_CLOSE);

        setSize(400, 100);
        setLocationRelativeTo(owner);

        setCursor(Cursor.getPredefinedCursor(Cursor.WAIT_CURSOR));
    }
}

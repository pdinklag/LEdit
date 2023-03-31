package de.pdinklag.ledit.gui;

import de.pdinklag.gui.UI;
import de.pdinklag.io.FileUtils;
import de.pdinklag.util.Localizer;

import javax.swing.*;
import javax.swing.border.EmptyBorder;
import java.awt.*;
import java.io.File;
import java.io.InputStream;
import java.net.MalformedURLException;
import java.util.Locale;

public class AboutDialog extends JDialog {
    private static final long serialVersionUID = -5961038563191686183L;
    private static final String RESOURCE_PATH = "/de/pdinklag/ledit/resources";

    private static File iconFile = FileUtils.extractResource(AboutDialog.class, RESOURCE_PATH + "/icons/ledit.png");
    private static String iconURL = "";

    static {
        try {
            if (iconFile != null) iconURL = iconFile.toURI().toURL().toString();
        } catch (MalformedURLException e) {
            //
        }
    }

    private static String load(String resource) {
        try {
            InputStream in = AboutDialog.class.getResourceAsStream(resource);
            byte[] b = new byte[in.available()];
            in.read(b);
            in.close();

            return new String(b, "UTF-8");
        } catch (Exception ex) {
            return "";
        }
    }

    public AboutDialog(Frame owner) {
        super(owner, Localizer.localize("ledit.menu.help.about"), true);

        String html = load(RESOURCE_PATH + "/about-style.html");
        html += load(RESOURCE_PATH + "/about_" + Locale.getDefault().getLanguage() + ".html");

        html = html.replace("$iconURL", iconURL);

        JEditorPane editorPane = new JEditorPane("text/html", "");
        editorPane.setOpaque(false);
        editorPane.setEditable(false);
        editorPane.setText(html);

        JScrollPane panel = new JScrollPane(editorPane);
        panel.setBorder(new EmptyBorder(UI.GAP, UI.GAP, UI.GAP, UI.GAP));

        setLayout(new GridLayout());
        add(panel);

        setIconImage(UI.loadImage(Localizer.localize("icon.help.about")));
        setSize(640, 480);
        setResizable(false);
    }
}

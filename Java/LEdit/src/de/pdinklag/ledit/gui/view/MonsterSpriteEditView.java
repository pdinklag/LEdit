package de.pdinklag.ledit.gui.view;

import de.pdinklag.gui.UI;
import de.pdinklag.ledit.LEdit;
import de.pdinklag.ledit.gui.MonsterPaletteComboBox;
import de.pdinklag.ledit.gui.MonsterSpritePanel;
import de.pdinklag.ledit.gui.WaitDialog;
import de.pdinklag.ledit.gui.tree.MonsterSpriteNode;
import de.pdinklag.snes.Palette;
import de.pdinklag.snes.Tile4;
import de.pdinklag.snes.gui.PaletteBox;
import de.pdinklag.snes.gui.PaletteListener;
import de.pdinklag.snes.l2.MonsterSprite;
import de.pdinklag.util.Hex;
import de.pdinklag.util.Localizer;

import javax.imageio.ImageIO;
import javax.swing.*;
import javax.swing.border.TitledBorder;
import javax.swing.filechooser.FileFilter;
import javax.swing.filechooser.FileNameExtensionFilter;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;
import java.awt.image.BufferedImage;
import java.awt.image.IndexColorModel;
import java.io.IOException;

public class MonsterSpriteEditView extends View implements ActionListener, ItemListener, PaletteListener {
    private static final long serialVersionUID = 6513545873231676603L;

    private static final JFileChooser fileChooser = new JFileChooser();

    private static final FileFilter FORMAT_GIF = new FileNameExtensionFilter("Graphics Interchange Format (*.gif)", "gif");

    static {
        fileChooser.addChoosableFileFilter(FORMAT_GIF);
        fileChooser.setAcceptAllFileFilterUsed(false);
    }

    private final MonsterSpriteNode node;

    private int spriteId;
    private MonsterSprite sprite;
    private MonsterSprite edit;
    private Palette editPalette;

    private final JButton btExport = new JButton();
    private final MonsterSpritePanel panelMS = new MonsterSpritePanel();
    private final PaletteBox pbPalette = new PaletteBox();
    private final MonsterPaletteComboBox cbPalette = new MonsterPaletteComboBox();

    private final JButton btSavePalette = new JButton();
    private final JButton btAddPalette = new JButton();
    private final JButton btRemovePalette = new JButton();

    private boolean ignoreEvents = false;

    public MonsterSpriteEditView(MonsterSpriteNode node, final LEdit project) {
        super(node);

        this.node = node;

        this.spriteId = node.getId();
        this.sprite = node.getSprite();

        //extend toolbar
        btExport.setIcon(UI.loadIcon(Localizer.localize("icon.file.export")));
        btExport.setToolTipText(Localizer.localize("ledit.view.export"));
        btExport.addActionListener(this);

        //
        btAddPalette.setIcon(UI.loadIcon(Localizer.localize("icon.add")));
        btAddPalette.setToolTipText(Localizer.localize("msprite.palette.add"));
        btAddPalette.addActionListener(this);

        btRemovePalette.setIcon(UI.loadIcon(Localizer.localize("icon.remove")));
        btRemovePalette.setToolTipText(Localizer.localize("msprite.palette.remove"));
        btRemovePalette.addActionListener(this);

        btSavePalette.setIcon(UI.loadIcon(Localizer.localize("icon.file.save")));
        btSavePalette.setToolTipText(Localizer.localize("msprite.palette.save"));
        btSavePalette.addActionListener(this);

        pbPalette.addPaletteListener(this);

        getToolBar().addSeparator();
        getToolBar().add(btExport);

        //create a local copy of the monster sprite which will be used for editing
        edit = new MonsterSprite();

        edit.setTilesX(sprite.getTilesX());
        edit.setTilesY(sprite.getTilesY());

        for (int i = 0; i < sprite.getPaletteCount(); i++)
            edit.addPalette(new Palette(sprite.getPalette(i)));

        for (int i = 0; i < sprite.getTileCount(); i++)
            edit.addTile(new Tile4(sprite.getTile(i)));

        editPalette = new Palette(edit.getPalette(0));

        panelMS.setSprite(edit);
        panelMS.setPalette(editPalette);
        cbPalette.initialize(sprite);
        cbPalette.setSelectedItemId(0);
        pbPalette.setPalette(editPalette);

        cbPalette.addItemListener(this);

        //add controls
        JPanel panelPreview = new JPanel();
        {
            panelPreview.setBorder(new TitledBorder(Localizer.localize("msprite.preview")));
            panelPreview.setLayout(new BorderLayout(UI.GAP, UI.GAP));
            panelPreview.add(panelMS, BorderLayout.CENTER);
        }

        JPanel panelPalette = new JPanel();
        {
            JToolBar tbPaletteList = new JToolBar();
            tbPaletteList.setFloatable(false);
            tbPaletteList.add(btAddPalette);
            tbPaletteList.add(btRemovePalette);

            JToolBar tbPaletteTools = new JToolBar();
            tbPaletteTools.setFloatable(false);
            tbPaletteTools.add(btSavePalette);

            JPanel panelPaletteList = new JPanel();
            panelPaletteList.setLayout(new BorderLayout(UI.GAP, UI.GAP));
            panelPaletteList.add(cbPalette, BorderLayout.CENTER);
            panelPaletteList.add(tbPaletteList, BorderLayout.EAST);

            JPanel panelPaletteTools = new JPanel();
            panelPaletteTools.setLayout(new BorderLayout(UI.GAP, UI.GAP));
            panelPaletteTools.add(pbPalette, BorderLayout.CENTER);
            panelPaletteTools.add(tbPaletteTools, BorderLayout.EAST);

            panelPalette.setBorder(new TitledBorder(Localizer.localize("msprite.palette")));
            panelPalette.setLayout(new GridLayout(2, 1, UI.GAP, UI.GAP));
            panelPalette.add(panelPaletteList);
            panelPalette.add(panelPaletteTools);
        }

        setLayout(new BorderLayout(UI.GAP, UI.GAP));
        add(panelPalette, BorderLayout.NORTH);
        add(panelPreview, BorderLayout.CENTER);
    }

    @Override
    public ViewTabTitle getTitle() {
        ViewTabTitle title = new ViewTabTitle(Localizer.localize("msprite") + " 0x" + Hex.formatByte(spriteId));
        title.setIcon(node.getIcon());
        return title;
    }

    @Override
    public void saveData() {
        //copy palettes
        while (sprite.getPaletteCount() > 0)
            sprite.removePalette(0);

        for (int i = 0; i < edit.getPaletteCount(); i++)
            sprite.addPalette(edit.getPalette(i));

        //TODO save tiles

        sprite.clearImageCache(); //make sure it gets redrawn properly
        node.updateIcon();
    }

    @Override
    public void close() {
        edit.clearImageCache(); //clean up
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        super.actionPerformed(e);

        if (e.getSource() == btExport) {
            if (fileChooser.showSaveDialog(this) == JFileChooser.APPROVE_OPTION) {
                WaitDialog.show((Frame) SwingUtilities.getRoot(this),
                        Localizer.localize("ledit.msprite.export.title"),
                        Localizer.localize("ledit.msprite.export.text"),
                        new Runnable() {
                            @Override
                            public void run() {
                                if (fileChooser.getFileFilter() == FORMAT_GIF) {
                                    //create color model
                                    byte[] r = new byte[16];
                                    byte[] g = new byte[16];
                                    byte[] b = new byte[16];

                                    for (int i = 0; i < 16; i++) {
                                        Color color = editPalette.getColor(i);
                                        r[i] = (byte) color.getRed();
                                        g[i] = (byte) color.getGreen();
                                        b[i] = (byte) color.getBlue();
                                    }

                                    //convert image
                                    BufferedImage source = edit.getImage(editPalette);

                                    BufferedImage image = new BufferedImage(source.getWidth(), source.getHeight(),
                                            BufferedImage.TYPE_BYTE_INDEXED, new IndexColorModel(4, 16, r, g, b, 0));

                                    image.getGraphics().drawImage(source, 0, 0, null);

                                    try {
                                        ImageIO.write(image, "gif", fileChooser.getSelectedFile());
                                    } catch (IOException ex) {
                                        ex.printStackTrace(); //TODO
                                    }
                                }
                            }
                        });
            }
        } else if (e.getSource() == btSavePalette) {
            edit.setPalette(cbPalette.getSelectedItemId(), new Palette(editPalette));
        } else if (e.getSource() == btRemovePalette) {
            if (edit.getPaletteCount() > 0) {
                //TODO ask
                int i = cbPalette.getSelectedItemId();
                edit.removePalette(i);

                ignoreEvents = true;
                cbPalette.initialize(edit);
                ignoreEvents = false;

                cbPalette.setSelectedItemId(Math.min(i, edit.getPaletteCount() - 1));
            } else {
                //TODO message
            }
        } else if (e.getSource() == btAddPalette) {
            edit.addPalette(new Palette(editPalette));

            ignoreEvents = true;
            cbPalette.initialize(edit);
            ignoreEvents = false;

            cbPalette.setSelectedItemId(edit.getPaletteCount() - 1);
        }
    }

    @Override
    public void colorChanged(PaletteBox source, int color, Color old) {
        edit.clearImageCache();
        panelMS.repaint();
    }

    @Override
    public void itemStateChanged(ItemEvent e) {
        if (!ignoreEvents) {
            Palette pal = edit.getPalette(cbPalette.getSelectedItemId());

            editPalette = new Palette(pal);
            pbPalette.setPalette(editPalette);
            panelMS.setPalette(editPalette);
        }
    }
}
 
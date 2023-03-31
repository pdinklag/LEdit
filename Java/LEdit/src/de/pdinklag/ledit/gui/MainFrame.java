package de.pdinklag.ledit.gui;

import de.pdinklag.gui.CloseableFrame;
import de.pdinklag.gui.UI;
import de.pdinklag.ledit.LEdit;
import de.pdinklag.ledit.gui.tree.*;
import de.pdinklag.ledit.gui.view.View;
import de.pdinklag.ledit.gui.view.ViewListener;
import de.pdinklag.snes.SnesRom;
import de.pdinklag.util.Localizer;

import javax.swing.*;
import javax.swing.filechooser.FileFilter;
import javax.swing.tree.DefaultTreeModel;
import javax.swing.tree.TreePath;
import java.awt.*;
import java.awt.event.*;
import java.io.File;
import java.util.HashMap;
import java.util.ResourceBundle;

public class MainFrame extends CloseableFrame implements ActionListener, MouseListener, ViewListener {
    private static final long serialVersionUID = -3741499894751941547L;

    public static void main(String[] args) throws Exception {
        Localizer.addResourceBundle(ResourceBundle.getBundle("de.pdinklag.ledit.resources.local"));
        Localizer.addResourceBundle(ResourceBundle.getBundle("de.pdinklag.ledit.resources.icons"));

        UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName());

        MainFrame window = new MainFrame();
        window.setVisible(true);
    }

    private final HashMap<ViewableNode, View> views = new HashMap<ViewableNode, View>();

    private final JMenu menuFile = new JMenu(Localizer.localize("ledit.menu.file"));
    private final JMenuItem menuFileNew = new JMenuItem(Localizer.localize("ledit.menu.file.new"));
    private final JMenuItem menuFileOpen = new JMenuItem(Localizer.localize("ledit.menu.file.open"));
    private final JMenuItem menuFileSave = new JMenuItem(Localizer.localize("ledit.menu.file.save"));
    private final JMenuItem menuFileGenerate = new JMenuItem(Localizer.localize("ledit.menu.file.generate"));
    private final JMenuItem menuFileExit = new JMenuItem(Localizer.localize("ledit.menu.file.exit"));
    private final JMenu menuHelp = new JMenu(Localizer.localize("ledit.menu.help"));
    private final JMenuItem menuHelpAbout = new JMenuItem(Localizer.localize("ledit.menu.help.about"));

    private final JSplitPane splitPane = new JSplitPane(JSplitPane.HORIZONTAL_SPLIT);
    private final JTabbedPane tabs = new JTabbedPane();

    private final LEditTreeNode nodeRoot =
            new LEditTreeNode(Localizer.localize("ledit.tree.root"));

    private final LEditTreeNode nodeMonsters =
            new LEditTreeNode(Localizer.localize("ledit.tree.monsters"));

    private final LEditTreeNode nodeMonsterSprites =
            new LEditTreeNode(Localizer.localize("ledit.tree.monstersprites"));

    private final DefaultTreeModel treeModel = new DefaultTreeModel(nodeRoot);
    private final JTree tree = new JTree(treeModel);

    private LEdit project = null;

    public MainFrame() {
        setTitle(Localizer.localize("ledit.title"));
        setIconImage(UI.loadImage(Localizer.localize("icon.ledit")));

        menuFileNew.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.file.new"))));
        menuFileOpen.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.file.open"))));
        menuFileSave.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.file.save"))));
        menuFileGenerate.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.file.generate"))));
        menuFileExit.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.file.exit"))));
        menuHelpAbout.setIcon(new ImageIcon(UI.loadImage(Localizer.localize("icon.help.about"))));

        menuFile.add(menuFileNew);
        menuFile.addSeparator();
        menuFile.add(menuFileOpen);
        menuFile.addSeparator();
        menuFile.add(menuFileSave);
        menuFile.add(menuFileGenerate);
        menuFile.addSeparator();
        menuFile.add(menuFileExit);

        menuHelp.add(menuHelpAbout);

        JMenuBar menuBar = new JMenuBar();
        menuBar.add(menuFile);
        menuBar.add(menuHelp);
        setJMenuBar(menuBar);

        setLayout(new GridLayout());
        add(splitPane);

        splitPane.setLeftComponent(new JScrollPane(tree));
        splitPane.setRightComponent(tabs);

        nodeRoot.setIcon(new ImageIcon(
                UI.loadImage(Localizer.localize("icon.ledit")).getScaledInstance(16, 16, Image.SCALE_SMOOTH)));

        tree.setCellRenderer(new LEditTreeCellRenderer());
        nodeRoot.add(nodeMonsterSprites);
        nodeRoot.add(nodeMonsters);
        tree.expandRow(0);

        menuFileSave.setEnabled(false);
        menuFileGenerate.setEnabled(false);

        UI.setActionListener(menuBar, this);
        tree.addMouseListener(this);
        tabs.addMouseListener(this);

        setMinimumSize(new Dimension(640, 480));
    }

    private void setProject(LEdit project) {
        this.project = project;

        menuFileSave.setEnabled(true);
        menuFileGenerate.setEnabled(true);

        //list monster sprites
        nodeMonsterSprites.removeAllChildren();
        for (int i = 0; i < project.getRom().getMonsterSpriteCount(); i++)
            nodeMonsterSprites.add(new MonsterSpriteNode(project, i));

        //list monsters
        nodeMonsters.removeAllChildren();
        for (int i = 0; i < project.getRom().getMonsterCount(); i++)
            nodeMonsters.add(new MonsterNode(project, i));
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        Object source = e.getSource();
        if (source == menuFileNew) {
            //select a ROM file
            final JFileChooser romFileChooser = new JFileChooser(".");
            romFileChooser.setFileFilter(new FileFilter() {
                @Override
                public boolean accept(File f) {
                    return f.isDirectory() || f.getName().endsWith(SnesRom.FILE_EXT_SMC);
                }

                @Override
                public String getDescription() {
                    return Localizer.localize("ledit.project.smc");
                }
            });
            romFileChooser.setDialogTitle(Localizer.localize("ledit.project.selectrom"));

            if (romFileChooser.showOpenDialog(this) != JFileChooser.CANCEL_OPTION) {
                final JFileChooser dirChooser = new JFileChooser(romFileChooser.getSelectedFile().getParentFile());
                dirChooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
                dirChooser.setDialogTitle(Localizer.localize("ledit.project.selectdir"));

                if (dirChooser.showOpenDialog(this) != JFileChooser.CANCEL_OPTION) {
                    WaitDialog.show(this,
                            Localizer.localize("ledit.project.create.title"),
                            Localizer.localize("ledit.project.create.message"),
                            new Runnable() {
                                @Override
                                public void run() {
                                    try {
                                        setProject(LEdit.create(
                                                romFileChooser.getSelectedFile(),
                                                dirChooser.getSelectedFile()));
                                    } catch (Exception ex) {
                                        ex.printStackTrace(); //TODO
                                    }
                                }
                            });
                }
            }
        } else if (source == menuFileOpen) {
            final JFileChooser fileChooser = new JFileChooser(".");
            fileChooser.setDialogTitle(Localizer.localize("ledit.project.selectfile"));
            fileChooser.setFileFilter(new FileFilter() {
                @Override
                public boolean accept(File f) {
                    return f.isDirectory() || f.getName().endsWith(LEdit.PROJECT_FILE_EXT);
                }

                @Override
                public String getDescription() {
                    return Localizer.localize("ledit.project.l2project");
                }
            });

            if (fileChooser.showOpenDialog(this) != JFileChooser.CANCEL_OPTION) {
                WaitDialog.show(this,
                        Localizer.localize("ledit.project.loading.title"),
                        Localizer.localize("ledit.project.loading.message"),
                        new Runnable() {
                            @Override
                            public void run() {
                                try {
                                    setProject(LEdit.load(fileChooser.getSelectedFile()));
                                } catch (Exception ex) {
                                    ex.printStackTrace(); //TODO
                                }
                            }
                        });
            }
        } else if (source == menuFileSave) {
            if (project != null) {
                WaitDialog.show(this,
                        Localizer.localize("ledit.project.saving.title"),
                        Localizer.localize("ledit.project.saving.message"),
                        new Runnable() {
                            @Override
                            public void run() {
                                try {
                                    project.save();
                                } catch (Exception ex) {
                                    ex.printStackTrace(); //TODO
                                }
                            }
                        });
            }
        } else if (source == menuFileExit) {
            close();
        } else if (source == menuFileGenerate) {
            //select a ROM file
            final JFileChooser romFileChooser = new JFileChooser(".");
            romFileChooser.setFileFilter(new FileFilter() {
                @Override
                public boolean accept(File f) {
                    return f.isDirectory() || f.getName().endsWith(SnesRom.FILE_EXT_SMC);
                }

                @Override
                public String getDescription() {
                    return Localizer.localize("ledit.project.smc");
                }
            });
            romFileChooser.setDialogTitle(Localizer.localize("ledit.project.selectrom"));

            if (romFileChooser.showSaveDialog(this) != JFileChooser.CANCEL_OPTION) {
                File selectedFile = romFileChooser.getSelectedFile();

                if (!selectedFile.getName().endsWith(SnesRom.FILE_EXT_SMC))
                    selectedFile = new File(selectedFile.getParentFile(), selectedFile.getName() + SnesRom.FILE_EXT_SMC);

                final File outFile = selectedFile;

                if (!outFile.exists() || UI.showFileOverrideDialog(this, outFile)) {
                    WaitDialog.show(this,
                            Localizer.localize("ledit.project.generating.title"),
                            Localizer.localize("ledit.project.generating.message"),
                            new Runnable() {
                                @Override
                                public void run() {
                                    try {
                                        project.createRom(outFile);
                                    } catch (Exception ex) {
                                        ex.printStackTrace(); //TODO
                                    }
                                }
                            });
                }
            }
        } else if (source == menuHelpAbout) {
            new AboutDialog(this).setVisible(true);
        }
    }

    public void openView(ViewableNode node) {
        if (views.containsKey(node)) {
            tabs.setSelectedComponent(views.get(node));
        } else {
            View view = node.getView();
            if (view != null) {
                tabs.setSelectedComponent(tabs.add(view));
                tabs.setTabComponentAt(tabs.getSelectedIndex(), view.getTitle());

                view.addViewListener(this);
            }
        }
    }

    public void closeView(View view) {
        ViewableNode node = view.getNode();
        if (views.containsKey(node)) {
            tabs.remove(view);
            views.remove(node);
        }
        tabs.remove(view);
    }

    @Override
    public void windowClosing(WindowEvent e) {
        setDefaultCloseOperation(EXIT_ON_CLOSE);
    }

    @Override
    public void mouseClicked(MouseEvent e) {
        Object source = e.getSource();
        if (source == tree) {
            if (e.getClickCount() > 1) {
                //double clicked a node
                TreePath selPath = tree.getSelectionPath();
                if (selPath != null) {
                    Object sel = tree.getSelectionPath().getLastPathComponent();
                    if (sel instanceof ViewableNode)
                        openView((ViewableNode) sel);
                }
            }
        }
    }

    @Override
    public void mousePressed(MouseEvent e) {
    }

    @Override
    public void mouseReleased(MouseEvent e) {
    }

    @Override
    public void mouseEntered(MouseEvent e) {
    }

    @Override
    public void mouseExited(MouseEvent e) {
    }

    @Override
    public void viewClosed(View view) {
        closeView(view);

        SwingUtilities.invokeLater(new Runnable() {
            @Override
            public void run() {
                tree.repaint();
            }
        });
    }

    @Override
    public void viewSaved(View view) {
    }
}

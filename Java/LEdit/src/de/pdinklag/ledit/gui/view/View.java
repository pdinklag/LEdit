package de.pdinklag.ledit.gui.view;

import de.pdinklag.gui.UI;
import de.pdinklag.ledit.gui.tree.ViewableNode;
import de.pdinklag.util.Localizer;

import javax.swing.*;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.LinkedList;

public abstract class View extends JPanel implements ActionListener {
    private static final long serialVersionUID = -2138219572815398324L;

    private final LinkedList<ViewListener> listeners = new LinkedList<ViewListener>();

    private final JToolBar toolBar = new JToolBar();
    private final JToolBar toolBarClose = new JToolBar();

    private ViewableNode node;

    private final JButton btSave = new JButton();
    private final JButton btClose = new JButton();

    private final JPanel panelContent = new JPanel();

    private boolean init = false;

    public View(ViewableNode node) {
        super(new BorderLayout(UI.GAP, UI.GAP));

        this.node = node;
        
        btClose.setToolTipText(Localizer.localize("ledit.view.close"));
        btClose.setIcon(UI.loadIcon(Localizer.localize("icon.tab.close")));

        btSave.setToolTipText(Localizer.localize("ledit.view.save"));
        btSave.setIcon(UI.loadIcon(Localizer.localize("icon.file.save")));

        btClose.addActionListener(this);
        btSave.addActionListener(this);

        toolBar.setFloatable(false);
        toolBar.add(btSave);
        toolBar.add(btClose);

        toolBarClose.setFloatable(false);
        toolBarClose.add(btClose);

        JPanel panelToolBar = new JPanel(new BorderLayout());
        panelToolBar.add(toolBar, BorderLayout.CENTER);
        panelToolBar.add(toolBarClose, BorderLayout.EAST);

        add(panelToolBar, BorderLayout.NORTH);
        add(new JScrollPane(panelContent), BorderLayout.CENTER);

        init = true;
    }

    @Override
    public void setLayout(LayoutManager mgr) {
        if (init)
            panelContent.setLayout(mgr);
        else
            super.setLayout(mgr);
    }

    @Override
    protected void addImpl(Component comp, Object constraints, int index) {
        if (init)
            panelContent.add(comp, constraints);
        else
            super.addImpl(comp, constraints, index);
    }

    public JToolBar getToolBar() {
        return toolBar;
    }

    public ViewableNode getNode() {
        return node;
    }

    @Override
    public void actionPerformed(ActionEvent e) {
        Object source = e.getSource();
        if (source == btSave) {
            saveData();

            for (ViewListener l : listeners)
                l.viewSaved(this);
        } else if (source == btClose) {
            close();

            for (ViewListener l : listeners)
                l.viewClosed(this);
        }
    }

    public void addViewListener(ViewListener l) {
        if (!listeners.contains(l))
            listeners.add(l);
    }

    public void removeViewListener(ViewListener l) {
        listeners.remove(l);
    }

    public abstract ViewTabTitle getTitle();

    public abstract void saveData();

    public abstract void close();
}

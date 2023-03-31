package de.pdinklag.ledit.gui.tree;

import de.pdinklag.gui.Iconified;

import javax.swing.*;
import javax.swing.tree.DefaultMutableTreeNode;

public class LEditTreeNode extends DefaultMutableTreeNode implements Iconified {
    private static final long serialVersionUID = -2497744841343681552L;
    
    private Icon icon = null;

    public LEditTreeNode() {
    }

    public LEditTreeNode(Object userObject) {
        setUserObject(userObject);
    }

    public Icon getIcon() {
        return icon;
    }

    public void setIcon(Icon icon) {
        this.icon = icon;
    }
}

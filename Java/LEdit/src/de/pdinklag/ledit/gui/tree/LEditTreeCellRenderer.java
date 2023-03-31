package de.pdinklag.ledit.gui.tree;

import javax.swing.*;
import javax.swing.tree.DefaultTreeCellRenderer;
import java.awt.*;

public class LEditTreeCellRenderer extends DefaultTreeCellRenderer {
    private static final long serialVersionUID = -6041169621412679046L;

    public LEditTreeCellRenderer() {
        super();
    }

    @Override
    public Component getTreeCellRendererComponent(JTree tree, Object value, boolean selected, boolean expanded, boolean leaf, int row, boolean hasFocus) {
        super.getTreeCellRendererComponent(tree, value, selected, expanded, leaf, row, hasFocus);

        if (value instanceof LEditTreeNode) {
            Icon icon = ((LEditTreeNode) value).getIcon();
            if (icon != null)
                setIcon(icon);
        }

        return this;
    }
}

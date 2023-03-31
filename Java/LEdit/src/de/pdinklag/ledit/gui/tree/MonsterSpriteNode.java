package de.pdinklag.ledit.gui.tree;

import de.pdinklag.gui.UI;
import de.pdinklag.ledit.LEdit;
import de.pdinklag.ledit.gui.view.MonsterSpriteEditView;
import de.pdinklag.ledit.gui.view.View;
import de.pdinklag.snes.l2.MonsterSprite;
import de.pdinklag.util.Hex;

import java.awt.image.BufferedImage;

public class MonsterSpriteNode extends LEditTreeNode implements ViewableNode {
    private static final long serialVersionUID = 727692539704428128L;

    private final LEdit project;
    private final int id;
    private final MonsterSprite sprite;

    public MonsterSpriteNode(LEdit project, int id) {
        this.project = project;
        this.id = id;
        this.sprite = project.getRom().getMonsterSprite(id);

        updateIcon();
    }

    public void updateIcon() {
        BufferedImage image = sprite.getImage(sprite.getPalette(0));
        setIcon(UI.produceIcon(image, 16));
    }

    @Override
    public String toString() {
        return "[0x" + Hex.formatByte(id) + "]";
    }

    @Override
    public View getView() {
        return new MonsterSpriteEditView(this, project);
    }

    public int getId() {
        return id;
    }

    public MonsterSprite getSprite() {
        return sprite;
    }
}

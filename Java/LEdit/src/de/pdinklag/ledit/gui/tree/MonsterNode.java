package de.pdinklag.ledit.gui.tree;

import de.pdinklag.gui.UI;
import de.pdinklag.ledit.LEdit;
import de.pdinklag.ledit.gui.view.MonsterEditView;
import de.pdinklag.ledit.gui.view.View;
import de.pdinklag.snes.l2.Monster;
import de.pdinklag.snes.l2.MonsterSprite;
import de.pdinklag.util.Hex;

import java.awt.image.BufferedImage;

public class MonsterNode extends LEditTreeNode implements ViewableNode {
    private static final long serialVersionUID = -2975192674729058720L;

    private final LEdit project;
    private final Monster monster;
    private final int monsterId;

    public MonsterNode(LEdit project, int monsterId) {
        this.project = project;
        this.monsterId = monsterId;
        this.monster = project.getRom().getMonster(monsterId);

        updateIcon();
    }

    public void updateIcon() {
        MonsterSprite sprite = project.getRom().getMonsterSprite(monster.getMonsterSpriteId() - 1);
        BufferedImage image = sprite.getImage(sprite.getPalette(monster.getPalette()));
        setIcon(UI.produceIcon(image, 16));
    }

    @Override
    public String toString() {
        return "[0x" + Hex.formatByte(monsterId) + "] " + monster.getName().getValue().trim();
    }

    @Override
    public View getView() {
        return new MonsterEditView(this, project);
    }

    public Monster getMonster() {
        return monster;
    }

    public int getMonsterId() {
        return monsterId;
    }
}

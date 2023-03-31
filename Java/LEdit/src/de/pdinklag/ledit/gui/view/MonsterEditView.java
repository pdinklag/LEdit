package de.pdinklag.ledit.gui.view;

import de.pdinklag.gui.*;
import de.pdinklag.ledit.LEdit;
import de.pdinklag.ledit.gui.ItemComboBox;
import de.pdinklag.ledit.gui.MonsterPaletteComboBox;
import de.pdinklag.ledit.gui.MonsterSpriteComboBox;
import de.pdinklag.ledit.gui.MonsterSpritePanel;
import de.pdinklag.ledit.gui.tree.MonsterNode;
import de.pdinklag.snes.gui.PaletteBox;
import de.pdinklag.snes.l2.Lufia2Rom;
import de.pdinklag.snes.l2.Monster;
import de.pdinklag.snes.l2.MonsterSprite;
import de.pdinklag.util.Localizer;

import javax.swing.*;
import javax.swing.border.TitledBorder;
import java.awt.*;
import java.awt.event.ItemEvent;
import java.awt.event.ItemListener;

public class MonsterEditView extends View {
    private static final long serialVersionUID = -3937870834511596917L;

    private final MonsterNode node;

    private final Lufia2Rom rom;
    private Monster monster;
    private MonsterSprite sprite;

    private boolean ignoreEvents = false;

    private final LimitedTextField tfName = new LimitedTextField(13);
    private final IntField ifLevel = new RangedIntField(0, 0xFF);
    private final IntField ifUnknown = new RangedIntField(0, 0xFF);
    private final MonsterSpriteComboBox cbMonsterSprite = new MonsterSpriteComboBox();
    private final MonsterPaletteComboBox cbPalette = new MonsterPaletteComboBox();
    private final MonsterSpritePanel mspPreview = new MonsterSpritePanel();
    private final PaletteBox pbPalette = new PaletteBox();
    private final IntField ifHP = new RangedIntField(0, 0xFFFF);
    private final IntField ifMP = new RangedIntField(0, 0xFFFF);
    private final IntField ifATP = new RangedIntField(0, 0xFFFF);
    private final IntField ifDFP = new RangedIntField(0, 0xFFFF);
    private final IntField ifAGT = new RangedIntField(0, 0xFF);
    private final IntField ifINT = new RangedIntField(0, 0xFF);
    private final IntField ifGUT = new RangedIntField(0, 0xFF);
    private final IntField ifMGR = new RangedIntField(0, 0xFF);
    private final IntField ifXP = new RangedIntField(0, 0xFFFF);
    private final IntField ifGold = new RangedIntField(0, 0xFFFF);
    private final ItemComboBox cbItem = new ItemComboBox(true);
    private final IntField ifItemChance = new RangedIntField(0, 0x7F);

    public MonsterEditView(MonsterNode node, final LEdit project) {
        super(node);

        this.node = node;
        this.rom = project.getRom();

        this.monster = node.getMonster();
        this.sprite = rom.getMonsterSprite(monster.getMonsterSpriteId() - 1);

        pbPalette.setEditable(false);

        //init ROM dependant controls
        cbItem.initialize(rom);
        cbMonsterSprite.initialize(rom);

        //init listeners
        cbPalette.addItemListener(new ItemListener() {
            @Override
            public void itemStateChanged(ItemEvent e) {
                if (!ignoreEvents && e.getStateChange() == ItemEvent.SELECTED) {
                    mspPreview.setPalette(cbPalette.getSelectedItemId());
                    pbPalette.setPalette(sprite.getPalette(cbPalette.getSelectedItemId()));
                }
            }
        });

        cbMonsterSprite.addItemListener(new ItemListener() {
            @Override
            public void itemStateChanged(ItemEvent e) {
                if (!ignoreEvents && e.getStateChange() == ItemEvent.SELECTED) {
                    sprite = rom.getMonsterSprite(cbMonsterSprite.getSelectedItemId());

                    ignoreEvents = true;
                    cbPalette.initialize(sprite);
                    cbPalette.setSelectedItemId(0);
                    ignoreEvents = false;

                    mspPreview.setSprite(sprite);
                    mspPreview.setPalette(0);
                    pbPalette.setPalette(sprite.getPalette(0));
                }
            }
        });

        //add controls
        ConfigurationPanel panelStats = new ConfigurationPanel(UI.GAP, UI.GAP);
        {
            panelStats.setBorder(new TitledBorder(Localizer.localize("monster.stat")));
            panelStats.add(new JLabel(Localizer.localize("monster.stat.level") + UI.COLON), ifLevel);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.unknown") + UI.COLON), ifUnknown);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.hp") + UI.COLON), ifHP);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.mp") + UI.COLON), ifMP);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.atp") + UI.COLON), ifATP);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.dfp") + UI.COLON), ifDFP);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.agt") + UI.COLON), ifAGT);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.int") + UI.COLON), ifINT);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.gut") + UI.COLON), ifGUT);
            panelStats.add(new JLabel(Localizer.localize("monster.stat.mgr") + UI.COLON), ifMGR);
            panelStats.addBottom();
        }

        ConfigurationPanel panelReward = new ConfigurationPanel(UI.GAP, UI.GAP);
        {
            panelReward.setBorder(new TitledBorder(Localizer.localize("monster.reward")));
            panelReward.add(new JLabel(Localizer.localize("monster.reward.xp") + UI.COLON), ifXP);
            panelReward.add(new JLabel(Localizer.localize("monster.reward.gold") + UI.COLON), ifGold);
            panelReward.add(new JLabel(Localizer.localize("monster.reward.item") + UI.COLON), cbItem);
            panelReward.add(new JLabel(Localizer.localize("monster.reward.item.chance") + UI.COLON), ifItemChance);
            panelReward.addBottom();
        }

        JPanel panelPreview = new JPanel(new BorderLayout(UI.GAP, UI.GAP));
        panelPreview.setBorder(new TitledBorder(Localizer.localize("monster.display.preview")));
        panelPreview.add(pbPalette, BorderLayout.NORTH);
        panelPreview.add(mspPreview, BorderLayout.CENTER);

        ConfigurationPanel panelDisplay = new ConfigurationPanel(UI.GAP, UI.GAP);
        {
            panelDisplay.setBorder(new TitledBorder(Localizer.localize("monster.display")));
            panelDisplay.add(new JLabel(Localizer.localize("monster.display.name") + UI.COLON), tfName);
            panelDisplay.add(new JLabel(Localizer.localize("monster.display.sprite") + UI.COLON), cbMonsterSprite);
            panelDisplay.add(new JLabel(Localizer.localize("monster.display.palette") + UI.COLON), cbPalette);
            panelDisplay.addBottom(panelPreview);
        }

        JPanel panelWest = new JPanel(new BorderLayout(UI.GAP, UI.GAP));
        panelWest.add(panelDisplay, BorderLayout.NORTH);
        panelWest.add(panelPreview, BorderLayout.CENTER);

        JPanel panelEast = new JPanel(new BorderLayout(UI.GAP, UI.GAP));
        panelEast.add(panelStats, BorderLayout.CENTER);
        panelEast.add(panelReward, BorderLayout.SOUTH);

        setLayout(new BorderLayout(UI.GAP, UI.GAP));
        add(panelWest, BorderLayout.CENTER);
        add(panelEast, BorderLayout.EAST);

        ignoreEvents = true;

        tfName.setText(monster.getName().getValue().trim());

        cbPalette.initialize(sprite);
        cbPalette.setSelectedItemId(monster.getPalette());
        pbPalette.setPalette(sprite.getPalette(monster.getPalette()));
        mspPreview.setSprite(sprite);
        mspPreview.setPalette(monster.getPalette());
        cbMonsterSprite.setSelectedItemId(monster.getMonsterSpriteId() - 1);
        ifLevel.setValue(monster.getLevel());
        ifUnknown.setValue(monster.getUnknown());
        ifHP.setValue(monster.getHealthPoints());
        ifMP.setValue(monster.getMagicPoints());
        ifATP.setValue(monster.getAttackPower());
        ifDFP.setValue(monster.getDefensePower());
        ifAGT.setValue(monster.getAgility());
        ifINT.setValue(monster.getIntelligence());
        ifGUT.setValue(monster.getGuts());
        ifMGR.setValue(monster.getMagicResistance());
        ifXP.setValue(monster.getRewardXp());
        ifGold.setValue(monster.getRewardGold());
        cbItem.setSelectedItemId(monster.getRewardItemId());
        ifItemChance.setValue(monster.getRewardItemChance());

        ignoreEvents = false;
    }

    @Override
    public void saveData() {
        ValueField invalid = UI.findInvalidValueField(this);
        if (invalid == null) {
            monster.getName().setValue(tfName.getText());
            monster.setPalette(cbPalette.getSelectedItemId());
            monster.setMonsterSpriteId(cbMonsterSprite.getSelectedItemId() + 1);
            monster.setLevel(ifLevel.getValue());
            monster.setUnknown(ifUnknown.getValue());
            monster.setAttackPower(ifATP.getValue());
            monster.setDefensePower(ifDFP.getValue());
            monster.setAgility(ifAGT.getValue());
            monster.setIntelligence(ifINT.getValue());
            monster.setGuts(ifGUT.getValue());
            monster.setMagicResistance(ifMGR.getValue());
            monster.setRewardXp(ifXP.getValue());
            monster.setRewardGold(ifGold.getValue());
            monster.setRewardItemId(cbItem.getSelectedItemId());
            monster.setRewardItemChance(ifItemChance.getValue());
        } else {
            System.out.println("Error in " + invalid); //TODO
        }
    }

    @Override
    public void close() {
        node.updateIcon();
    }

    @Override
    public ViewTabTitle getTitle() {
        ViewTabTitle title = new ViewTabTitle(monster.getName().getValue().trim());
        title.setIcon(node.getIcon());
        return title;
    }
}

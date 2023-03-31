package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.snes.SnesSerializable;
import de.pdinklag.util.Hex;

import java.io.IOException;
import java.io.Serializable;
import java.util.logging.Logger;

public class Monster implements Serializable, SnesSerializable {
    private static final long serialVersionUID = -8149862207805640533L;
    private static final Logger logger = Logger.getLogger("l2.Monster");

    private final L2String name = new L2String(0x0D);
    private int level, unknown;
    private int monsterSpriteId, palette;

    private int healthPoints, magicPoints;
    private int attackPower, defensePower;
    private int agility, intelligence;
    private int guts, magicResistance;

    private int rewardXp, rewardGold;

    private int rewardItemId = -1;
    private int rewardItemChance;

    private final BattleScript attackScript = new BattleScript();
    private final BattleScript defenseScript = new BattleScript();

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        name.readSnes(in);

        level = in.readByte();
        unknown = in.readByte();
        monsterSpriteId = in.readByte();
        palette = in.readByte();

        healthPoints = in.readUnsignedShort();
        magicPoints = in.readUnsignedShort();
        attackPower = in.readUnsignedShort();
        defensePower = in.readUnsignedShort();

        agility = in.readByte();
        intelligence = in.readByte();
        guts = in.readByte();
        magicResistance = in.readByte();

        rewardXp = in.readUnsignedShort();
        rewardGold = in.readUnsignedShort();

        int x = 0x21;

        int attackScriptOffset = 0;
        int defenseScriptOffset = 0;

        //extra data
        for (int b = in.readByte(); b != 0; b = in.readByte()) {
            switch (b) {
                case 0x03:
                    //reward item
                    rewardItemId = in.readByte();
                    rewardItemChance = in.readByte();

                    if ((rewardItemChance & 0x01) != 0)
                        rewardItemId |= 0x100;

                    rewardItemChance >>= 1;
                    break;

                case 0x07:
                    //attack script offset
                    attackScriptOffset = in.readUnsignedShort();
                    break;

                case 0x08:
                    //defense script offset
                    defenseScriptOffset = in.readUnsignedShort();
                    break;

                default:
                    //unknown
                    logger.warning("Encountered unknown monster extra data 0x" + Hex.format(b, 2) +
                            " for " + name.getValue());

                    in.readUnsignedShort();
                    break;
            }

            x += 3;
        }
        x++; //zero

        //load attack script
        if (attackScriptOffset != 0) {
            for (; x < attackScriptOffset; x++)
                in.readByte(); //skip bytes

            attackScript.setOffsetBase(attackScriptOffset);
            x += attackScript.readSnes(in);
        }

        //load defense script
        if (defenseScriptOffset != 0) {
            for (; x < defenseScriptOffset; x++)
                in.readByte(); //skip bytes

            defenseScript.setOffsetBase(defenseScriptOffset);
            x += defenseScript.readSnes(in);
        }

        return x;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        name.writeSnes(out);

        out.writeByte(level);
        out.writeByte(unknown);
        out.writeByte(monsterSpriteId);
        out.writeByte(palette);

        out.writeShort(healthPoints);
        out.writeShort(magicPoints);
        out.writeShort(attackPower);
        out.writeShort(defensePower);

        out.writeByte(agility);
        out.writeByte(intelligence);
        out.writeByte(guts);
        out.writeByte(magicResistance);

        out.writeShort(rewardXp);
        out.writeShort(rewardGold);

        int x = 0x21;

        if (rewardItemId >= 0) {
            out.writeByte(0x03);
            out.writeByte(rewardItemId & 0xFF);

            int b = rewardItemChance << 1;

            if ((rewardItemId & 0x100) != 0)
                b |= 0x01;

            out.writeByte(b);
            x += 3;
        }

        int attackScriptOffset = 0;
        int defenseScriptOffset = 0;

        if (attackScript.getBytes().size() > 0) {
            attackScriptOffset = x + 4;
        }

        if (defenseScript.getBytes().size() > 0) {
            if (attackScriptOffset > 0) {
                attackScriptOffset += 3;
                defenseScriptOffset = attackScriptOffset + attackScript.getBytes().size();
            } else {
                defenseScriptOffset = x + 4;
            }
        }

        if (attackScriptOffset > 0) {
            out.writeByte(0x07);
            out.writeShort(attackScriptOffset);
            x += 3;
        }

        if (defenseScriptOffset > 0) {
            out.writeByte(0x08);
            out.writeShort(defenseScriptOffset);
            x += 3;
        }

        out.writeByte(0);
        x++;

        if (attackScriptOffset > 0)
            x += attackScript.writeSnes(out);

        if (defenseScriptOffset > 0)
            x += defenseScript.writeSnes(out);

        return x;
    }

    public L2String getName() {
        return name;
    }

    public int getLevel() {
        return level;
    }

    public void setLevel(int level) {
        this.level = level;
    }

    public int getUnknown() {
        return unknown;
    }

    public void setUnknown(int unknown) {
        this.unknown = unknown;
    }

    public int getMonsterSpriteId() {
        return monsterSpriteId;
    }

    public void setMonsterSpriteId(int monsterSpriteId) {
        this.monsterSpriteId = monsterSpriteId;
    }

    public int getPalette() {
        return palette;
    }

    public void setPalette(int palette) {
        this.palette = palette;
    }

    public int getHealthPoints() {
        return healthPoints;
    }

    public void setHealthPoints(int healthPoints) {
        this.healthPoints = healthPoints;
    }

    public int getMagicPoints() {
        return magicPoints;
    }

    public void setMagicPoints(int magicPoints) {
        this.magicPoints = magicPoints;
    }

    public int getAttackPower() {
        return attackPower;
    }

    public void setAttackPower(int attackPower) {
        this.attackPower = attackPower;
    }

    public int getDefensePower() {
        return defensePower;
    }

    public void setDefensePower(int defensePower) {
        this.defensePower = defensePower;
    }

    public int getAgility() {
        return agility;
    }

    public void setAgility(int agility) {
        this.agility = agility;
    }

    public int getIntelligence() {
        return intelligence;
    }

    public void setIntelligence(int intelligence) {
        this.intelligence = intelligence;
    }

    public int getGuts() {
        return guts;
    }

    public void setGuts(int guts) {
        this.guts = guts;
    }

    public int getMagicResistance() {
        return magicResistance;
    }

    public void setMagicResistance(int magicResistance) {
        this.magicResistance = magicResistance;
    }

    public int getRewardXp() {
        return rewardXp;
    }

    public void setRewardXp(int rewardXp) {
        this.rewardXp = rewardXp;
    }

    public int getRewardGold() {
        return rewardGold;
    }

    public void setRewardGold(int rewardGold) {
        this.rewardGold = rewardGold;
    }

    public int getRewardItemId() {
        return rewardItemId;
    }

    public void setRewardItemId(int rewardItemId) {
        this.rewardItemId = rewardItemId;
    }

    public int getRewardItemChance() {
        return rewardItemChance;
    }

    public void setRewardItemChance(int rewardItemChance) {
        this.rewardItemChance = rewardItemChance;
    }

    public BattleScript getAttackScript() {
        return attackScript;
    }

    public BattleScript getDefenseScript() {
        return defenseScript;
    }
}

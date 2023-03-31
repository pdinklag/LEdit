package de.pdinklag.snes.l2;

import de.pdinklag.io.BinaryInputStream;
import de.pdinklag.io.BinaryOutputStream;
import de.pdinklag.snes.SnesSerializable;

import java.io.IOException;
import java.io.Serializable;
import java.util.logging.Logger;

public class Item implements Serializable, SnesSerializable {
    private static final long serialVersionUID = 1186179037385092501L;
    private static final Logger logger = Logger.getLogger("l2.Item");

    public static final int FLAG_CONSUMABLE = 0x01; //is a consumable potion
    public static final int FLAG_EQUIPABLE = 0x02; //is equipable
    public static final int FLAG_BOOTS = 0x04; //is boots
    public static final int FLAG_CURSED = 0x08; //is cursed
    public static final int FLAG_FRUIT = 0x10; //is a fruit
    public static final int FLAG_UNDROPABLE = 0x20; //cannot be dropped
    public static final int FLAG_MENU_ITEM = 0x40; //can be used in inventory menu
    public static final int FLAG_BATTLE_ITEM = 0x80; //can be used in battle

    public static final int TARGETING_NONE = 0x00; //no target is chosen
    public static final int TARGETING_MANY = 0x01; //one or more targets can be chosen
    public static final int TARGETING_ONE = 0x02; //one target can be chosen
    public static final int TARGETING_ENEMIES = 0x80; //enemies are targets (instead of friends)

    public static final int EQUIPABLE_WEAPON = 0x01;
    public static final int EQUIPABLE_ARMOR = 0x02;
    public static final int EQUIPABLE_SHIELD = 0x04;
    public static final int EQUIPABLE_HELMET = 0x08;
    public static final int EQUIPABLE_RING = 0x10;
    public static final int EQUIPABLE_JEWEL = 0x20;

    public static final int CHARACTER_MAXIM = 0x01;
    public static final int CHARACTER_SELAN = 0x02;
    public static final int CHARACTER_GUY = 0x04;
    public static final int CHARACTER_ARTEA = 0x08;
    public static final int CHARACTER_TIA = 0x10;
    public static final int CHARACTER_DEKAR = 0x20;
    public static final int CHARACTER_LEXIS = 0x40;

    private static final int INTERNAL_HAS_SCRIPT_MENU = 0x0001;
    private static final int INTERNAL_HAS_SCRIPT_BATTLE = 0x0002;
    private static final int INTERNAL_HAS_SCRIPT_WEAPON = 0x0004;
    private static final int INTERNAL_HAS_SCRIPT_ARMOR = 0x0008;
    private static final int INTERNAL_HAS_BONUS_ATP = 0x0010;
    private static final int INTERNAL_HAS_BONUS_DFP = 0x0020;
    private static final int INTERNAL_HAS_BONUS_STR = 0x0040;
    private static final int INTERNAL_HAS_BONUS_AGT = 0x0080;
    private static final int INTERNAL_HAS_BONUS_INT = 0x0100;
    private static final int INTERNAL_HAS_BONUS_GUT = 0x0200;
    private static final int INTERNAL_HAS_BONUS_MGR = 0x0400;
    private static final int INTERNAL_UNKNOWN1 = 0x0800; //has no data
    private static final int INTERNAL_UNKNOWN2 = 0x1000; //has no data
    private static final int INTERNAL_HAS_BATTLE_ANIM = 0x2000;
    private static final int INTERNAL_UNKNOWN3 = 0x4000;
    private static final int INTERNAL_HAS_IP_ATTACK = 0x8000;

    private final L2String name = new L2String(0x0C);

    private int flags;
    private int unknown;
    private int targeting;
    private int icon;
    private int mapSpriteId;
    private int value;
    private int equipType;
    private int characters;
    private transient int internalFlags;

    private int battleAnimId, ipAttackId;
    private int bonusATP, bonusDFP, bonusSTR, bonusAGT, bonusINT, bonusGUT, bonusMGR;
    private int unknownExtra;

    private final BattleScript menuScript = new BattleScript();
    private final BattleScript battleScript = new BattleScript();
    private final BattleScript weaponScript = new BattleScript();
    private final BattleScript armorScript = new BattleScript();

    private int readExtra(BinaryInputStream in, int flag) throws IOException {
        if ((internalFlags & flag) != 0)
            return in.readUnsignedShort();
        else
            return 0;
    }

    @Override
    public int readSnes(BinaryInputStream in) throws IOException {
        flags = in.readByte();
        unknown = in.readByte();
        targeting = in.readByte();
        icon = in.readByte();
        mapSpriteId = in.readByte();
        value = in.readUnsignedShort();
        equipType = in.readByte();
        characters = in.readByte();
        internalFlags = in.readUnsignedShort();

        int x = 0x0B;

        int menuScriptOffset = readExtra(in, INTERNAL_HAS_SCRIPT_MENU);
        int battleScriptOffset = readExtra(in, INTERNAL_HAS_SCRIPT_BATTLE);
        int weaponScriptOffset = readExtra(in, INTERNAL_HAS_SCRIPT_WEAPON);
        int armorScriptOffset = readExtra(in, INTERNAL_HAS_SCRIPT_ARMOR);
        bonusATP = readExtra(in, INTERNAL_HAS_BONUS_ATP);
        bonusDFP = readExtra(in, INTERNAL_HAS_BONUS_DFP);
        bonusSTR = readExtra(in, INTERNAL_HAS_BONUS_STR);
        bonusAGT = readExtra(in, INTERNAL_HAS_BONUS_AGT);
        bonusINT = readExtra(in, INTERNAL_HAS_BONUS_INT);
        bonusGUT = readExtra(in, INTERNAL_HAS_BONUS_GUT);
        bonusMGR = readExtra(in, INTERNAL_HAS_BONUS_MGR);
        battleAnimId = readExtra(in, INTERNAL_HAS_BATTLE_ANIM);
        unknownExtra = readExtra(in, INTERNAL_UNKNOWN3);
        ipAttackId = readExtra(in, INTERNAL_HAS_IP_ATTACK);

        //calculate amount of bytes read by counting the amount of set flags
        for (int a = internalFlags; a > 0; a >>= 1) {
            if ((a & 1) == 1)
                x += 2;
        }

        //corrections...
        if ((internalFlags & INTERNAL_UNKNOWN1) != 0) x -= 2;
        if ((internalFlags & INTERNAL_UNKNOWN2) != 0) x -= 2;

        if (menuScriptOffset > 0) {
            for (; x < menuScriptOffset; x++)
                in.readByte(); //skip bytes

            menuScript.setOffsetBase(menuScriptOffset);
            x += menuScript.readSnes(in);
        }

        if (battleScriptOffset > 0) {
            for (; x < battleScriptOffset; x++)
                in.readByte(); //skip bytes

            battleScript.setOffsetBase(battleScriptOffset);
            x += battleScript.readSnes(in);
        }

        if (weaponScriptOffset > 0) {
            for (; x < weaponScriptOffset; x++)
                in.readByte(); //skip bytes

            weaponScript.setOffsetBase(weaponScriptOffset);
            x += weaponScript.readSnes(in);
        }

        if (armorScriptOffset > 0) {
            for (; x < armorScriptOffset; x++)
                in.readByte(); //skip bytes

            armorScript.setOffsetBase(armorScriptOffset);
            x += armorScript.readSnes(in);
        }

        return x;
    }

    @Override
    public int writeSnes(BinaryOutputStream out) throws IOException {
        return 0; //TODO
    }

    public L2String getName() {
        return name;
    }

    public BattleScript getMenuScript() {
        return menuScript;
    }

    public BattleScript getBattleScript() {
        return battleScript;
    }

    public BattleScript getArmorScript() {
        return armorScript;
    }

    public BattleScript getWeaponScript() {
        return weaponScript;
    }

    public int getFlags() {
        return flags;
    }

    public void setFlags(int flags) {
        this.flags = flags;
    }

    public int getUnknown() {
        return unknown;
    }

    public void setUnknown(int unknown) {
        this.unknown = unknown;
    }

    public int getTargeting() {
        return targeting;
    }

    public void setTargeting(int targeting) {
        this.targeting = targeting;
    }

    public int getIcon() {
        return icon;
    }

    public void setIcon(int icon) {
        this.icon = icon;
    }

    public int getMapSpriteId() {
        return mapSpriteId;
    }

    public void setMapSpriteId(int mapSpriteId) {
        this.mapSpriteId = mapSpriteId;
    }

    public int getValue() {
        return value;
    }

    public void setValue(int value) {
        this.value = value;
    }

    public int getEquipType() {
        return equipType;
    }

    public void setEquipType(int equipType) {
        this.equipType = equipType;
    }

    public int getCharacters() {
        return characters;
    }

    public void setCharacters(int characters) {
        this.characters = characters;
    }

    public int getBattleAnimId() {
        return battleAnimId;
    }

    public void setBattleAnimId(int battleAnimId) {
        this.battleAnimId = battleAnimId;
    }

    public int getIpAttackId() {
        return ipAttackId;
    }

    public void setIpAttackId(int ipAttackId) {
        this.ipAttackId = ipAttackId;
    }

    public int getBonusATP() {
        return bonusATP;
    }

    public void setBonusATP(int bonusATP) {
        this.bonusATP = bonusATP;
    }

    public int getBonusDFP() {
        return bonusDFP;
    }

    public void setBonusDFP(int bonusDFP) {
        this.bonusDFP = bonusDFP;
    }

    public int getBonusSTR() {
        return bonusSTR;
    }

    public void setBonusSTR(int bonusSTR) {
        this.bonusSTR = bonusSTR;
    }

    public int getBonusAGT() {
        return bonusAGT;
    }

    public void setBonusAGT(int bonusAGT) {
        this.bonusAGT = bonusAGT;
    }

    public int getBonusINT() {
        return bonusINT;
    }

    public void setBonusINT(int bonusINT) {
        this.bonusINT = bonusINT;
    }

    public int getBonusGUT() {
        return bonusGUT;
    }

    public void setBonusGUT(int bonusGUT) {
        this.bonusGUT = bonusGUT;
    }

    public int getBonusMGR() {
        return bonusMGR;
    }

    public void setBonusMGR(int bonusMGR) {
        this.bonusMGR = bonusMGR;
    }

    public int getUnknownExtra() {
        return unknownExtra;
    }

    public void setUnknownExtra(int unknownExtra) {
        this.unknownExtra = unknownExtra;
    }
}

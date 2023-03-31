using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class ItemEditForm : Form
    {
        MainForm parent;

        L2Item item_sel;

        bool bNoEvents;

        public ItemEditForm(MainForm parent_form, int sel_index)
        {
            InitializeComponent();

            bNoEvents = true;
            item_sel = null;
            parent = parent_form;

            //List
            for (int i = 0; i < L2Item.Count; i++)
            {
                L2Item item = L2Item.Get(i);

                lstI.Items.Add("0x" + i.ToString("X3") + " - " + item.Name.Value.TrimEnd());
            }

            cbTargetingMode.Items.Add("Es wird kein Ziel gewählt.");
            cbTargetingMode.Items.Add("Ein oder mehrere Freunde werden gewählt.");
            cbTargetingMode.Items.Add("Genau ein Freund wird gewählt.");
            cbTargetingMode.Items.Add("Ein oder mehrere Gegner werden gewählt.");
            cbTargetingMode.Items.Add("Genau ein Gegner wird gewählt.");

            bNoEvents = false;

            //Selection
            lstI.SelectedIndex = sel_index;
        }

        private void ItemEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.itemEditClosed();
        }

        private void CheckIf(CheckBox chk, bool b)
        {
            chk.CheckState = b ? CheckState.Checked : CheckState.Unchecked;
        }

        private void lstI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!bNoEvents)
            {
                item_sel = L2Item.Get(lstI.SelectedIndex);

                txtName.Text = item_sel.Name.Value.TrimEnd();

                CheckIf(chkUse01, (item_sel.Usability & 0x01) != 0);
                CheckIf(chkUse02, (item_sel.Usability & 0x02) != 0);
                CheckIf(chkUse04, (item_sel.Usability & 0x04) != 0);
                CheckIf(chkUse08, (item_sel.Usability & 0x08) != 0);
                CheckIf(chkUse10, (item_sel.Usability & 0x10) != 0);
                CheckIf(chkUse20, (item_sel.Usability & 0x20) != 0);
                CheckIf(chkUse40, (item_sel.Usability & 0x40) != 0);
                CheckIf(chkUse80, (item_sel.Usability & 0x80) != 0);

                int x = item_sel.TargetingMode & 0xF;
                if ((item_sel.TargetingMode & 0x80) != 0)
                    x += 2;

                cbTargetingMode.SelectedIndex = x;

                udCost.Value = item_sel.Cost;

                CheckIf(chkEquip01, (item_sel.Type & 0x01) != 0);
                CheckIf(chkEquip02, (item_sel.Type & 0x02) != 0);
                CheckIf(chkEquip04, (item_sel.Type & 0x04) != 0);
                CheckIf(chkEquip08, (item_sel.Type & 0x08) != 0);
                CheckIf(chkEquip10, (item_sel.Type & 0x10) != 0);
                CheckIf(chkEquip20, (item_sel.Type & 0x20) != 0);

                CheckIf(chkBy01, (item_sel.EquipableBy & 0x01) != 0);
                CheckIf(chkBy02, (item_sel.EquipableBy & 0x02) != 0);
                CheckIf(chkBy04, (item_sel.EquipableBy & 0x04) != 0);
                CheckIf(chkBy08, (item_sel.EquipableBy & 0x08) != 0);
                CheckIf(chkBy10, (item_sel.EquipableBy & 0x10) != 0);
                CheckIf(chkBy20, (item_sel.EquipableBy & 0x20) != 0);
                CheckIf(chkBy40, (item_sel.EquipableBy & 0x40) != 0);

                udANG.Value = item_sel.Bonus_ANG;
                udABW.Value = item_sel.Bonus_ABW;
                udSTR.Value = item_sel.Bonus_STR;
                udBWG.Value = item_sel.Bonus_BWG;
                udINT.Value = item_sel.Bonus_INT;
                udMUT.Value = item_sel.Bonus_MUT;
                udMGA.Value = item_sel.Bonus_MGA;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (item_sel != null)
                new ScriptEditForm(
                    item_sel.Script_Menu,
                    item_sel.Name.Value.TrimEnd() + " - Menü-Skript"
                    ).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (item_sel != null)
                new ScriptEditForm(
                    item_sel.Script_Battle,
                    item_sel.Name.Value.TrimEnd() + " - Kampf-Skript"
                    ).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (item_sel != null)
                new ScriptEditForm(
                    item_sel.Script_Weapon,
                    item_sel.Name.Value.TrimEnd() + " - Ausrüstungs-Skript"
                    ).Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (item_sel != null)
                new ScriptEditForm(
                    item_sel.Script_Armor,
                    item_sel.Name.Value.TrimEnd() + " - Rüstungs-Skript"
                    ).Show();
        }
    }
}

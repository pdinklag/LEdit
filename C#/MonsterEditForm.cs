using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class MonsterEditForm : Form
    {
        static GradientBuffer preview_gradient;

        MainForm parent;

        L2Monster Monster;
        L2MonsterSprite MonsterSprite;

        bool bMonsterChanged;

        bool bNoEvents;

        public MonsterEditForm(MainForm ParentForm, int Selection)
        {
            InitializeComponent();

            parent = ParentForm;

            //Gradient
            preview_gradient = new GradientBuffer(
                pb.Width, pb.Height,
                SystemColors.GradientActiveCaption,
                SystemColors.ActiveCaption,
                GradientBuffer.GradientDirection.Vertical);

            
            bNoEvents = true;

            /*
            cbItemList.Items.Add("Keins");
            for (int i = 0; i < L2Item.Count; i++)
            {
                L2Item item = L2Item.Get(i);
                cbItemList.Items.Add("0x" + i.ToString("X2") + " - " + item.Name.Value.TrimEnd());
            }
             */

            bNoEvents = false;

            SetTitle();
        }

        private void pbpal_Paint(object sender, PaintEventArgs e)
        {
            if (MonsterSprite != null)
            {
                for (int i = 0; i < 16; i++)
                    e.Graphics.FillRectangle(
                        new SolidBrush(MonsterSprite.Palettes[(int)udPal.Value - 1].GetColor((byte)i)),
                        new Rectangle(0, i << 3, pbpal.Width, (i + 1) << 3));
            }

            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                              new Rectangle(0, 0, pbpal.Width - 1, pbpal.Height - 1));
        }

        private void MonsterEditForm_Load(object sender, EventArgs e)
        {
            
        }

        private void MonsterEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.monsterEditClosed();
        }

        private void pb_Paint(object sender, PaintEventArgs e)
        {
            //Gradient
            preview_gradient.Draw(e.Graphics, 0, 0);

            if (MonsterSprite != null)
            {
                int x = (pb.Width >> 1) - (MonsterSprite.TilesX << 2);
                int y = (pb.Height >> 1) - (MonsterSprite.TilesY << 2);

                MonsterSprite.Draw(e.Graphics, (byte)(udPal.Value - 1), x, y);
            }


            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                              new Rectangle(0, 0, pb.Width - 1, pb.Height - 1));
        }

        private void MonsterChanged()
        {
            if (!bNoEvents)
            {
                if (!bMonsterChanged)
                {
                    bMonsterChanged = true;
                    SetTitle();
                }
            }
        }

        private void SaveMonster()
        {
            if (Monster != null)
            {
                bool bOldNoEvents = bNoEvents;
                bNoEvents = true;

                string name = txtName.Text + ((txtName.Text.Length < 0x0D) ?
                    new string(' ', 0x0D - txtName.Text.Length) : string.Empty);

                Monster.Name.Value = name;

                Monster.Level = (byte)udLevel.Value;
                Monster.Unknown1 = (byte)udUnk.Value;
                Monster.Palette = (byte)(udPal.Value - 1);
                Monster.HP = (ushort)udHP.Value;
                Monster.MP = (ushort)udMP.Value;
                Monster.ANG = (ushort)udANG.Value;
                Monster.ABW = (ushort)udABW.Value;
                Monster.BWG = (byte)udBWG.Value;
                Monster.INT = (byte)udINT.Value;
                Monster.MUT = (byte)udMUT.Value;
                Monster.MGA = (byte)udMGA.Value;
                Monster.Experience = (ushort)udEXP.Value;
                Monster.Gold = (ushort)udGold.Value;

                bool bHadItem = (Monster.GiftItem >= 0);

                Monster.GiftItem = cbItemList.SelectedIndex - 1;
                Monster.GiftProbability = (Monster.GiftItem >= 0) ? (int)(udItemProb.Value * 2) : 0;

                //correct script jumps!!!
                if (Monster.AttackScript.Bytes.Count >= 0)
                {
                    if ((Monster.GiftItem >= 0 && !bHadItem) ||
                        Monster.GiftItem < 0 && bHadItem)
                    {
                        int correct = (Monster.GiftItem < 0 && bHadItem) ? -3 : 3;

                        Monster.AttackScript.Offset += correct;

                        int i = 0;
                        while (i < Monster.AttackScript.Bytes.Count)
                        {
                            byte cmd = Monster.AttackScript.Bytes[i++];
                            int x = -1;

                            if (cmd == 0x03 || cmd == 0x04)
                                x = 0;
                            else if (cmd == 0x05)
                                x = 1;
                            else if (cmd >= 0x06 && cmd <= 0x0B)
                                x = 3;

                            if (x >= 0)
                                Monster.AttackScript.Bytes[i + x] =
                                    (byte)(Monster.AttackScript.Bytes[i + x] + correct);

                            i += L2BattleScript.GetArgumentByteCount(cmd);
                        }

                        /*
                        if (
                            MessageBox.Show(
                            "Dem Monster wurde ein Item hinzugefügt oder entnommen, was alle Sprung-Adressen im " +
                            "Attacke-Skript verschiebt. Bevor das Monster weiter bearbeitet wird, " +
                            "sollte die Monster-Datei gespeichert werden.\r\n" +
                            "Soll dies nun geschehen?",
                            "Daten sichern",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            SaveFile();
                        }
                         */
                    }
                }

                bMonsterChanged = false;
                bNoEvents = bOldNoEvents;
            }
        }

        public void OpenMonster(int id)
        {
            if (!AskSaveMonster())
                return;

            Monster = L2Monster.Array[id];
            MonsterSprite = L2MonsterSprite.Array[Monster.MonsterSpriteID - 1];

            bool bNoEvents_old = bNoEvents;
            bNoEvents = true;

            txtName.Text = Monster.Name.Value.TrimEnd();
            txtID.Text = Content.FormatID(id);

            udLevel.Value = Monster.Level;
            udUnk.Value = Monster.Unknown1;

            udPal.Maximum = MonsterSprite.Palettes.Count;
            udPal.Value = Monster.Palette + 1;

            lblNumPals.Text = "/ " + udPal.Maximum.ToString();

            udImage.Value = Monster.MonsterSpriteID;

            udHP.Value = Monster.HP;
            udMP.Value = Monster.MP;
            udANG.Value = Monster.ANG;
            udABW.Value = Monster.ABW;

            udBWG.Value = Monster.BWG;
            udINT.Value = Monster.INT;
            udMUT.Value = Monster.MUT;
            udMGA.Value = Monster.MGA;

            udEXP.Value = Monster.Experience;
            udGold.Value = Monster.Gold;

            int gift = Monster.GiftItem;
            if (gift >= 0)
            {
                //TODO: cbItemList.SelectedIndex = gift + 1;
                udItemProb.Value = (decimal)Monster.GiftProbability / 2;
                udItemProb.Enabled = true;
            }
            else
            {
                //TODO: cbItemList.SelectedIndex = 0;
                udItemProb.Value = 0;
                udItemProb.Enabled = false;
            }

            bNoEvents = bNoEvents_old;
            bMonsterChanged = false;

            SetTitle();

            pb.Refresh();
            pbpal.Refresh();
        }

        private void udPal_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
            pb.Refresh();
            pbpal.Refresh();
        }

        private void pb_DoubleClick(object sender, EventArgs e)
        {
            if (Monster != null)
                parent.GUIMSprEdit(Monster.MonsterSpriteID - 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Monster != null)
                new ScriptEditForm(
                    Monster.AttackScript,
                    Monster.Name.Value.TrimEnd() + " - Attacke-Skript"
                    ).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Monster != null)
                new ScriptEditForm(
                    Monster.DefendScript,
                    Monster.Name.Value.TrimEnd() + " - Defensiv-Skript"
                    ).Show();
        }

        private void cbItemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            udItemProb.Enabled = (cbItemList.SelectedIndex > 0);
            MonsterChanged();
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUISave();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udLevel_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udUnk_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udHP_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udMP_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udANG_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udABW_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udBWG_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udINT_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udMUT_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udMGA_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udImage_ValueChanged(object sender, EventArgs e)
        {
            //TODO
        }

        private void udEXP_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udGold_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void udItemProb_ValueChanged(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private bool AskSaveMonster()
        {
            if (Monster != null && bMonsterChanged)
            {
                DialogResult x = MessageBox.Show(
                     "Die Daten dieses Monsters wurden verändert, sollen die Änderungen gespeichert werden?",
                     "Veränderungen speichern",
                     MessageBoxButtons.YesNoCancel,
                     MessageBoxIcon.Exclamation);

                if (x == DialogResult.Cancel)
                {
                    return false;
                }
                else
                {
                    if (x == DialogResult.Yes)
                        GUISave();

                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private void MonsterEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AskSaveMonster())
                e.Cancel = true;
        }

        private void GUIOpen()
        {
            try
            {
                ContentSelectionDialog dlg = ContentSelectionDialog.Map[Content.ContentType.Monster];

                if (dlg != null && dlg.ShowDialog() != DialogResult.Cancel)
                    OpenMonster(dlg.SelectedID);
            }
            catch (Exception)
            {
            }
        }

        private void GUISave()
        {
            if (Monster == null || !bMonsterChanged)
                return;

            Cursor.Current = Cursors.WaitCursor;
            SaveMonster();

            FileInfo dest = Project.RegisterMod(Content.ContentType.Monster, Monster.ID);
            Monster.Save(dest);

            L2Monster.PrepareContentSelectionDialog();

            SetTitle();

            Cursor.Current = Cursors.Default;
        }

        private void öffnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIOpen();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GUIOpen();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            GUISave();
        }

        private void txtName_TextChanged_1(object sender, EventArgs e)
        {
            MonsterChanged();
        }

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetTitle()
        {
            Text = "Monster-Editor";

            if (Monster != null)
            {
                Text += " - " + Monster.GetName();

                if (bMonsterChanged)
                    Text += " *";
            }
        }
    }
}

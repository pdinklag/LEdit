using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace LEdit
{
    public partial class MonsterSpriteEditForm : Form
    {
        MainForm parent;

        static GradientBuffer preview_gradient;

        private L2MonsterSprite MonsterSprite;
        private bool bMonsterSpriteChanged;

        public MonsterSpriteEditForm(MainForm ParentForm,int Selection)
        {
            InitializeComponent();

            parent = ParentForm;

            //Gradient
            preview_gradient = new GradientBuffer(
                pb.Width, pb.Height,
                SystemColors.GradientActiveCaption,
                SystemColors.ActiveCaption,
                GradientBuffer.GradientDirection.Vertical);
        }

        private void SetTitle()
        {
            Text = "MonsterSprite-Editor";

            if (MonsterSprite != null)
            {
                Text += " - " + MonsterSprite.GetName();

                if (bMonsterSpriteChanged)
                    Text += " *";
            }
        }

        private void MonsterSpriteEditForm_Load(object sender, EventArgs e)
        {
            SetTitle();
        }

        private void pb_Paint(object sender, PaintEventArgs e)
        {
            //Gradient
            preview_gradient.Draw(e.Graphics, 0, 0);

            if (MonsterSprite != null)
            {
                int x = (pb.Width >> 1) - (MonsterSprite.TilesX << 2);
                int y = (pb.Height >> 1) - (MonsterSprite.TilesY << 2);

                MonsterSprite.Draw(e.Graphics, (byte)(lstPals.SelectedIndex), x, y);
            }


            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                              new Rectangle(0, 0, pb.Width - 1, pb.Height - 1));
        }

        private void pbpal_Paint(object sender, PaintEventArgs e)
        {
            if (MonsterSprite != null)
            {
                for (int i = 0; i < 16; i++)
                    e.Graphics.FillRectangle(
                        new SolidBrush(MonsterSprite.Palettes[lstPals.SelectedIndex].GetColor((byte)i)),
                        new Rectangle(0, i << 3, pbpal.Width, (i + 1) << 3));
            }

            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                              new Rectangle(0, 0, pbpal.Width - 1, pbpal.Height - 1));
        }

        private void ListPalettes()
        {
            lstPals.Items.Clear();
            if (MonsterSprite!=null)
            {
                for (int i = 0; i < MonsterSprite.Palettes.Count; i++)
                    lstPals.Items.Add("Palette 0x" + i.ToString("X2"));
            }
            lstPals.SelectedIndex = 0;
        }

        private void MonsterSpriteEditForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.MSprEditClosed();
        }

        private void lstPals_SelectedIndexChanged(object sender, EventArgs e)
        {
            pb.Refresh();
            pbpal.Refresh();
        }

        private void GUIExportBMP()
        {
            if (MonsterSprite != null)
            {
                if (savedlg.ShowDialog() != DialogResult.Cancel)
                {
                    FileInfo File = new FileInfo(savedlg.FileName);

                    Console.WriteLine(savedlg.FilterIndex.ToString());

                    if (savedlg.FilterIndex == 1)
                        MonsterSprite.ExportBMP(File, lstPals.SelectedIndex);
                    else if (savedlg.FilterIndex == 2)
                        MonsterSprite.ExportPNG(File, lstPals.SelectedIndex);
                }
            }
        }

        private void MonsterSpriteChanged()
        {
            if (!bMonsterSpriteChanged)
            {
                bMonsterSpriteChanged = true;
                SetTitle();
            }
        }

        private void GUIImportBMP()
        {
            if (MonsterSprite != null)
            {
                //opendlg.FileName = "MonsterSprite-0x" +
                //    lstMSpr.SelectedIndex.ToString("X2") + ".bmp";

                if (opendlg.ShowDialog() != DialogResult.Cancel)
                {
                    if (!MonsterSprite.ImportBMP(new FileInfo(opendlg.FileName)))
                        MessageBox.Show(
                            "\"" + opendlg.FileName + "\" konnte nicht geladen werden!", 
                            "Fehler", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Error);

                    MonsterSpriteChanged();

                    pb.Refresh();
                    pbpal.Refresh();
                }
            }
        }

        private void GUIImportPal()
        {
            if (MonsterSprite != null)
            {
                //opendlg.FileName = "MonsterSprite-0x" +
                //    lstMSpr.SelectedIndex.ToString("X2") + ".bmp";

                if (opendlg.ShowDialog() != DialogResult.Cancel)
                {
                    SNESPalette pal = BMP4bppIO.ImportPalette(new FileInfo(opendlg.FileName));

                    MonsterSprite.SetPalette(lstPals.SelectedIndex,
                        pal);

                    MonsterSpriteChanged();

                    pb.Refresh();
                    pbpal.Refresh();
                }
            }
        }

        private void exportBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIExportBMP();
        }

        private void importPalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIImportPal();
        }

        private void aus16FarbenBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIImportPal();
        }

        private void bMPexportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIExportBMP();
        }

        private void importBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIImportBMP();
        }

        private void bMPImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIImportBMP();
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUISave();
        }

        private void GUISave()
        {
            if (MonsterSprite == null || !bMonsterSpriteChanged)
                return;

            Cursor.Current = Cursors.WaitCursor;

            FileInfo dest = Project.RegisterMod(Content.ContentType.MonsterSprite, MonsterSprite.ID);
            MonsterSprite.Save(dest);

            L2MonsterSprite.Array[MonsterSprite.ID] = MonsterSprite;
            L2MonsterSprite.PrepareContentSelectionDialog();
            L2Monster.PrepareContentSelectionDialog();

            bMonsterSpriteChanged = false;
            SetTitle();

            Cursor.Current = Cursors.Default;
        }

        private void GUIOpen()
        {
            try
            {
                ContentSelectionDialog dlg = ContentSelectionDialog.Map[Content.ContentType.MonsterSprite];

                if (dlg != null && dlg.ShowDialog() != DialogResult.Cancel)
                {
                    MonsterSprite = new L2MonsterSprite();
                    MonsterSprite.ID = dlg.SelectedID;
                    MonsterSprite.Load(Project.RequestSource(
                        Content.ContentType.MonsterSprite,
                        MonsterSprite.ID));

                    //MonsterSprite = L2MonsterSprite.Array[dlg.SelectedID];
                    ListPalettes();

                    bMonsterSpriteChanged = false;
                    SetTitle();

                    pb.Refresh();
                    pbpal.Refresh();
                }
            }
            catch (Exception)
            {
            }
        }

        void GUIRemovePalette()
        {
            if (MonsterSprite != null && lstPals.SelectedIndex >= 0)
            {
                if (lstPals.Items.Count > 1)
                {
                    if (MessageBox.Show(
                        "Soll diese Palette wirklich entfernt werden?",
                        "Palette entfernen",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MonsterSprite.RemovePalette(lstPals.SelectedIndex);

                        ListPalettes();
                        MonsterSpriteChanged();
                    }
                }
                else
                {
                    MessageBox.Show(
                        "Eine Palette muss mindestens vorhanden sein!",
                        "Nicht möglich",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

            }
        }
        
        private void lstPals_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                GUIRemovePalette();
        }

        void GUIAddPalette()
        {
            if (MonsterSprite != null && lstPals.SelectedIndex >= 0)
            {
                MonsterSprite.AddPalette(new SNESPalette(MonsterSprite.Palettes[lstPals.SelectedIndex]));

                ListPalettes();
                lstPals.SelectedIndex = MonsterSprite.Palettes.Count - 1;

                MonsterSpriteChanged();
            }
        }

        private void lstPals_DoubleClick(object sender, EventArgs e)
        {
            GUIAddPalette();
        }

        private void wiederherstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIAddPalette();
        }

        private void entfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIRemovePalette();
        }

        private void paletteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void öffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIOpen();
        }

        private void schließenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

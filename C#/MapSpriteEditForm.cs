using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LEdit
{
	public partial class MapSpriteEditForm : Form
	{   
        static GradientBuffer preview_gradient;

        MainForm parent;
		
		bool bNoEvents;
		SNESPalette[] pal;
		
		int sel;
		L2MapSprite ms_sel;

        public MapSpriteEditForm(MainForm ParentForm, int Selection)
        {
            bNoEvents = true;

            InitializeComponent();

            parent = ParentForm;

            //Gradient
            preview_gradient = new GradientBuffer(
                pb.Width, pb.Height,
                SystemColors.GradientActiveCaption,
                SystemColors.ActiveCaption,
                GradientBuffer.GradientDirection.Vertical);

            //Palettes
            pal = L2MapSprite.GetPalettes();

            //List
            lstMS.Items.Clear();
            for (int i = 0; i < 0xFB; i++)
                lstMS.Items.Add("0x" + i.ToString("X2") + " - " + L2ROM.GetMapSpriteName(i));

            sel = -1;
            ms_sel = null;

            exportBMPToolStripMenuItem.Enabled = false;
            importBMPToolStripMenuItem.Enabled = false;
            importPalToolStripMenuItem.Enabled = false;

            bNoEvents = false;

            //Select
            lstMS.SelectedIndex = Selection;
        }
		
		void MapSpriteEditFormClosed(object sender, FormClosedEventArgs e)
		{
            //Notify main window
			parent.monsterspriteEditClosed();
		}

        void LstMSSelectedIndexChanged(object sender, EventArgs e)
        {
            bNoEvents = true;

            exportBMPToolStripMenuItem.Enabled = true;
            importBMPToolStripMenuItem.Enabled = true;
            importPalToolStripMenuItem.Enabled = true;

            sel = lstMS.SelectedIndex;
            ms_sel = L2MapSprite.Get(sel);

            if (ms_sel != null)
            {
                txtName.Text = L2ROM.GetMapSpriteName(sel);

                byte flags = ms_sel.Flags;

                flag01.CheckState = ((flags & 0x01) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag02.CheckState = ((flags & 0x02) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag04.CheckState = ((flags & 0x04) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag08.CheckState = ((flags & 0x08) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag10.CheckState = ((flags & 0x10) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag20.CheckState = ((flags & 0x20) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag40.CheckState = ((flags & 0x40) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;
                flag80.CheckState = ((flags & 0x80) != 0) ?
                                    CheckState.Checked :
                                    CheckState.Unchecked;

                udPal.Value = ms_sel.Palette;

                txtAddr.Text = ms_sel.TileAddress.ToString();
                txtROM.Text = "0x" + ms_sel.TileAddress.ToPCAddress().ToString("X6");

                pbpal.Refresh();
                pb.Refresh();
            }

            bNoEvents = false;
        }
		
		void PbPaint(object sender, PaintEventArgs e)
		{
            //Gradient
            preview_gradient.Draw(e.Graphics, 0, 0);

            if (sel >= 0)
            {
                //MapSprite
                ms_sel.Draw(e.Graphics, 1, 1, 2, (byte)udPal.Value);
            }

            //Control outline
            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                            new Rectangle(0, 0, pb.Width - 1, pb.Height - 1));
		}
		
		void PbpalPaint(object sender, PaintEventArgs e)
		{
            if (sel >= 0)
            {
                for (int i = 0; i < 16; i++)
                    e.Graphics.FillRectangle(
                        new SolidBrush(pal[(int)udPal.Value].GetColor((byte)i)),
                        new Rectangle(i << 3, 0, (i + 1) << 3, pbpal.Height));
            }

            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                              new Rectangle(0, 0, pbpal.Width - 1, pbpal.Height - 1));
		}
		
		void UdPalValueChanged(object sender, EventArgs e)
		{
			if(!bNoEvents)
			{
                pbpal.Refresh();
                pb.Refresh();
			}
		}

        void GUI_ExportTilemap()
        {
            if (sel >= 0)
            {
                savedlg.FileName =
                    "MapSprite_0x" + sel.ToString("X2") +
                    "_0x" + ms_sel.TileAddress.ToPCAddress().ToString("X6") +
                    ((L2ROM.GetMapSpriteName(sel).Length > 0) ? "_" + L2ROM.GetMapSpriteName(sel) : String.Empty) +
                    ".bmp";

                if (savedlg.ShowDialog() == DialogResult.OK)
                {
                    FileInfo File = new FileInfo(savedlg.FileName);

                    if (File.Exists)
                        File.Delete();

                    ms_sel.ExportBMP(File, (byte)udPal.Value);
                }
            }
        }
		
		void GUI_ImportTilemap()
		{
			if(sel>=0)
			{
				opendlg.FileName=String.Empty;
				
				if(opendlg.ShowDialog()==DialogResult.OK)
					ms_sel.ImportBMP(new FileInfo(opendlg.FileName));

                pb.Refresh();
			}
		}
		
		void GUI_ImportPalette()
		{
            if (sel >= 0)
            {
                opendlg.FileName = String.Empty;

                if (opendlg.ShowDialog() == DialogResult.OK)
                {
                    pal[(int)udPal.Value] = BMP4bppIO.ImportPalette(new FileInfo(opendlg.FileName));
                }

                pbpal.Refresh();
                pb.Refresh();
            }
		}
		
		void ExportBMPToolStripMenuItemClick(object sender, EventArgs e)
		{
			GUI_ExportTilemap();
		}
		
		void ImportBMPToolStripMenuItemClick(object sender, EventArgs e)
		{
			GUI_ImportTilemap();
		}
		
		void ImportPalToolStripMenuItemClick(object sender, EventArgs e)
		{
			GUI_ImportPalette();
		}
		
		void BMPexportierenToolStripMenuItemClick(object sender, EventArgs e)
		{
			GUI_ExportTilemap();
		}
		
		void BMPImportierenToolStripMenuItemClick(object sender, EventArgs e)
		{
			GUI_ImportTilemap();
		}

        private void MapSpriteEditForm_Load(object sender, EventArgs e)
        {

        }

        private void aus16FarbenBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUI_ImportPalette();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void iToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ms_sel != null)
            {
            if (
                MessageBox.Show(
                "Sicher?",
                "Tilemap in ROM einfügen",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                FileStream fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);

                //TileMap
                bw.Seek((int)ms_sel.TileAddress.ToPCAddress(), SeekOrigin.Begin);
                ms_sel.Tileset.ToStream(bw);

                bw.Close();
                fs.Close();

                MessageBox.Show(
                    "Tilemap erfolgreich ins Spiel eingefügt!",
                    "Erfolg",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            }
        }
	}
}

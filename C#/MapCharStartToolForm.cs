using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class MapCharStartToolForm : Form
    {
        MapEditForm parent;
        L2MapHeader.CharacterStart edit;

        public L2MapHeader.CharacterStart CharacterStart { get { return edit; } }

        public MapCharStartToolForm(MapEditForm parent_window, L2MapHeader.CharacterStart edit_arg)
        {
            InitializeComponent();

            parent = parent_window;
            edit = edit_arg;

            numDestX.Value = edit.x1;
            numDestY.Value = edit.y1;
            numX1.Value = edit.move_area_x1;
            numY1.Value = edit.move_area_y1;
            numX2.Value = edit.move_area_x2;
            numY2.Value = edit.move_area_y2;
            numericUpDown9.Value = edit.unknown;

            this.Text = "Charakter-Bereich 0x" + edit.id.ToString("X2");
        }

        private void Apply()
        {
            edit.x1 = (byte)numDestX.Value;
            edit.y1 = (byte)numDestY.Value;

            edit.move_area_x1 = (byte)numX1.Value;
            edit.move_area_y1 = (byte)numY1.Value;
            edit.move_area_x2 = (byte)numX2.Value;
            edit.move_area_y2 = (byte)numY2.Value;

            edit.unknown = (byte)numericUpDown9.Value;

            parent.RefreshView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void MapCharStartToolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.CharStartEditClosed(this);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int x1, y1, x2, y2;
            parent.GetSelection(out x1, out y1, out x2, out y2);

            numX1.Value = x1;
            numY1.Value = y1;
            numX2.Value = x2;
            numY2.Value = y2;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            numX1.Value = 255;
            numY1.Value = 255;
            numX2.Value = 255;
            numY2.Value = 255;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int x, y;
            parent.GetSelection(out x, out y);

            numDestX.Value = x;
            numDestY.Value = y;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            numDestX.Value = 255;
            numDestY.Value = 255;
        }
    }
}

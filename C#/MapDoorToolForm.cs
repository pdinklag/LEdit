using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class MapDoorToolForm : Form
    {
        MapEditForm parent;
        L2MapHeader.Door door;

        public L2MapHeader.Door Door { get { return door; } }

        public MapDoorToolForm(MapEditForm parent_window, L2MapHeader.Door edit_door)
        {
            InitializeComponent();

            parent = parent_window;
            door = edit_door;

            numX1.Value = door.x1;
            numX2.Value = door.x2;
            numY1.Value = door.y1;
            numY2.Value = door.y2;

            numericUpDown4.Value = door.area1_x1;
            numericUpDown3.Value = door.area1_y1;
            numericUpDown2.Value = door.area1_x2;
            numericUpDown1.Value = door.area1_y2;

            numericUpDown8.Value = door.area2_x1;
            numericUpDown7.Value = door.area2_y1;
            numericUpDown6.Value = door.area2_x2;
            numericUpDown5.Value = door.area2_y2;

            numericUpDown9.Value = door.unknown1;
            numericUpDown10.Value = door.unknown2;

            this.Text = "Tür 0x" + door.id.ToString("X2");
        }

        private void MapDoorToolForm_Load(object sender, EventArgs e)
        {

        }

        private void Apply()
        {
            door.x1 = (byte)numX1.Value;
            door.x2 = (byte)numX2.Value;
            door.y1 = (byte)numY1.Value;
            door.y2 = (byte)numY2.Value;

            door.area1_x1 = (byte)numericUpDown4.Value;
            door.area1_y1 = (byte)numericUpDown3.Value;
            door.area1_x2 = (byte)numericUpDown2.Value;
            door.area1_y2 = (byte)numericUpDown1.Value;

            door.area2_x1 = (byte)numericUpDown8.Value;
            door.area2_y1 = (byte)numericUpDown7.Value;
            door.area2_x2 = (byte)numericUpDown6.Value;
            door.area2_y2 = (byte)numericUpDown5.Value;

            door.unknown1 = (byte)numericUpDown9.Value;
            door.unknown2 = (byte)numericUpDown10.Value;

            parent.RefreshView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void MapDoorToolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.DoorEditClosed(this);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int x1, y1, x2, y2;
            parent.GetSelection(out x1, out y1, out x2, out y2);

            numX1.Value = x1;
            numY1.Value = y1;
            numX2.Value = x2;
            numY2.Value = y2;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            numX1.Value = 255;
            numY1.Value = 255;
            numX2.Value = 255;
            numY2.Value = 255;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            int x1, y1, x2, y2;
            parent.GetSelection(out x1, out y1, out x2, out y2);

            numericUpDown4.Value = x1;
            numericUpDown3.Value = y1;
            numericUpDown2.Value = x2;
            numericUpDown1.Value = y2;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            numericUpDown4.Value = 255;
            numericUpDown3.Value = 255;
            numericUpDown2.Value = 255;
            numericUpDown1.Value = 255;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            int x1, y1, x2, y2;
            parent.GetSelection(out x1, out y1, out x2, out y2);

            numericUpDown8.Value = x1;
            numericUpDown7.Value = y1;
            numericUpDown6.Value = x2;
            numericUpDown5.Value = y2;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            numericUpDown8.Value = 255;
            numericUpDown7.Value = 255;
            numericUpDown6.Value = 255;
            numericUpDown5.Value = 255;
        }
    }
}

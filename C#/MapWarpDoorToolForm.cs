using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class MapWarpDoorToolForm : Form
    {
        MapEditForm parent;
        L2MapHeader.WarpDoor door;

        public L2MapHeader.WarpDoor WarpDoor { get { return door; } }

        public MapWarpDoorToolForm(MapEditForm parent_window, L2MapHeader.WarpDoor warp_door)
        {
            InitializeComponent();

            parent = parent_window;
            door = warp_door;

            cbDestMap.Items.Clear();
            cbDestMap.Items.Add("0xFF - (diese)");
            for (int i = 0; i < L2MapHeader.Count; i++)
                cbDestMap.Items.Add("0x" + i.ToString("X2") + " - " + L2MapHeader.Get(i).Name.Value);

            numX1.Value = door.x1;
            numX2.Value = door.x2;
            numY1.Value = door.y1;
            numY2.Value = door.y2;
            numericUpDown1.Value = door.unknown;

            numDestX.Value = door.dest_x;
            numDestY.Value = door.dest_y;

            if (door.dest_map == 255)
                cbDestMap.SelectedIndex = 0;
            else
                cbDestMap.SelectedIndex = door.dest_map + 1;

            this.Text = "Warp-Tür 0x" + door.id.ToString("X2");
        }

        private void MapWarpDoorToolForm_Load(object sender, EventArgs e)
        {

        }

        private void Apply()
        {
            door.x1 = (byte)numX1.Value;
            door.y1 = (byte)numY1.Value;
            door.x2 = (byte)numX2.Value;
            door.y2 = (byte)numY2.Value;
            door.unknown = (byte)numericUpDown1.Value;

            door.dest_x = (byte)numDestX.Value;
            door.dest_y = (byte)numDestY.Value;

            if (cbDestMap.SelectedIndex == 0)
                door.dest_map = 255;
            else
                door.dest_map = (byte)(cbDestMap.SelectedIndex - 1);

            parent.RefreshView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void MapWarpDoorToolForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.WarpDoorEditClosed(this);
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
    }
}

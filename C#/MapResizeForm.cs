using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class MapResizeForm : Form
    {
        MapEditForm mapedit;
        bool bCancel;

        public MapResizeForm(MapEditForm parent, int w, int h)
        {
            InitializeComponent();

            mapedit = parent;
            udWidth.Value = w;
            udHeight.Value = h;

            bCancel = true;
        }

        private void MapResizeForm_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bCancel = false;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bCancel = true;
            Close();
        }

        private void MapResizeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bCancel)
                mapedit.ResizeFormClosed(-1, -1);
            else
                mapedit.ResizeFormClosed((int)udWidth.Value, (int)udHeight.Value);
        }
    }
}

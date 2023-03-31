using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class ContentSelectionDialog : Form
    {
        #region STATIC

        public static Dictionary<Content.ContentType, ContentSelectionDialog> Map =
            new Dictionary<Content.ContentType, ContentSelectionDialog>();

        public static void StaticUnload()
        {
            foreach (KeyValuePair<Content.ContentType, ContentSelectionDialog> x in Map)
                x.Value.Dispose();

            Map.Clear();
        }

        #endregion

        public int SelectedID = -1;

        int w, h;

        public ContentSelectionDialog(string Title)
        {
            InitializeComponent();

            Text = Title;

            w = Width;
            h = Height;
        }

        public void AddItem(Content item)
        {
            ListViewItem listItem = new ListViewItem(item.GetName());
            listItem.Tag = item.ID;

            listItem.ToolTipText =
                "ID: " + Content.FormatID(item.ID) + Environment.NewLine +
                "Name: " + item.GetName();

            Bitmap icon = item.GetIcon();
            if (icon != null)
            {
                listItem.ImageIndex = iconList.Images.Count;
                iconList.Images.Add(icon);
            }

            listView.Items.Add(listItem);
        }

        private void ContentSelectionDialog_Load(object sender, EventArgs e)
        {
            listView.SelectedItems.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                SelectedID = (int)listView.SelectedItems[0].Tag;
            }
            else
            {
                SelectedID = -1;
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView_DoubleClick(object sender, EventArgs e)
        {
            if(listView.SelectedItems.Count > 0)
                button2.PerformClick();
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && listView.SelectedItems.Count > 0)
                button2.PerformClick();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class ProgressForm : Form
    {
        private Form parent;

        public ProgressForm(Form parent, string title)
        {
            InitializeComponent();

            this.parent = parent;
            Text = title;
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            if (parent != null)
                parent.Enabled = false;
        }

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (parent != null)
                parent.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        public void SetText(string text)
        {
            label1.Text = text;
            Refresh();
        }

        public void SetMaxProgress(int max)
        {
            progressBar1.Maximum = max;
        }

        public void Reset()
        {
            progressBar1.Value = 0;
        }
        
        public void Progress(int amount)
        {
            progressBar1.Value += amount;
        }
    }
}

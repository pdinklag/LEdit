using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{
    public partial class FileManagerForm : Form
    {
        MainForm main;

        ushort file;

        public FileManagerForm(MainForm mainform)
        {
            InitializeComponent();

            main = mainform;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Dies kann eine Weile dauern, alle bereits extrahierten Dateien werden übreschrieben.\r\nSicher?",
                "Alle extrahieren",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                this.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                for (ushort i = 0; i < (ushort)L2FileManager.File_Count; i++)
                    L2FileManager.ExtractFile(i, true);

                Cursor.Current = Cursors.Default;
                this.Enabled = true;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string FileName = L2FileManager.GetFileName(file);

            if (File.Exists(FileName))
            {
                if (MessageBox.Show(
                    "Die ausgewählte Datei wurde bereits extrahiert.\r\n" +
                    "Soll sie überschrieben werden?",
                    "Datei extrahieren",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    return;
                }
            }

            L2FileManager.ExtractFile(file, true);
            numericUpDown1_ValueChanged(sender, e);

            MessageBox.Show("Datei extrahiert!", "Erfolg",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            file = (ushort)numericUpDown1.Value;

            string FileName = L2FileManager.GetFileName(file);
            textBox1.Text = FileName;

            if (File.Exists(FileName))
                textBox1.ForeColor = Color.Green;
            else
                textBox1.ForeColor = Color.DarkRed;
        }

        private void FileManagerForm_Load(object sender, EventArgs e)
        {
            numericUpDown1_ValueChanged(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FileName = L2FileManager.GetFileName(file);

            if (!File.Exists(FileName))
            {
                MessageBox.Show(
                    "Die ausgewählte Datei wurde noch nicht extrahiert!",
                    "Datei in ROM einfügen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            L2FileManager.InsertFile(file);

            MessageBox.Show("Datei in ROM eingefügt!", "Erfolg",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void FileManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            main.fileManagerClosed();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string FileName = L2FileManager.GetFileName(file);

            try
            {
                File.Delete(FileName);
            }
            catch (System.Exception)
            {
            }

            L2FileManager.RemoveInsertedFile(file);

            MessageBox.Show("Original wiederhergestellt!", "Erfolg",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            numericUpDown1_ValueChanged(sender, e);
        }
    }
}

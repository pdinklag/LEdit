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
    public partial class NewProjectDialog : Form
    {
        public string ROMFile, ProjectName, ProjectFolder;

        public NewProjectDialog()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            ROMFile = txtROMFile.Text;
            ProjectName = txtName.Text;
            ProjectFolder = txtFolder.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectROMDialog.ShowDialog() != DialogResult.Cancel)
            {
                txtROMFile.Text = selectROMDialog.FileName;

                FileInfo nfo = new FileInfo(selectROMDialog.FileName);
                ProjectName = nfo.Name.Substring(0, nfo.Name.Length - nfo.Extension.Length);

                txtName.Text = ProjectName;
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            ProjectName = txtName.Text;

            if (ProjectName.Length > 0)
            {
                DirectoryInfo dir = new DirectoryInfo(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                    Path.DirectorySeparatorChar +
                    "Meine LEdit-Projekte" +
                    Path.DirectorySeparatorChar +
                    ProjectName);

                txtFolder.Text = dir.FullName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectFolderDialog.ShowDialog() != DialogResult.Cancel)
            {
                txtFolder.Text = selectFolderDialog.SelectedPath;
            }
        }
    }
}

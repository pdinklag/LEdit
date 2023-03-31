using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace LEdit
{
	public partial class MainForm : Form
	{
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

        private bool ForceQuit = false;

        private FileManagerForm fileManager;
        private ItemEditForm itemEdit;
        private MapSpriteEditForm mapspriteEdit;
        private MonsterEditForm monsterEdit;
        private MapEditForm mapEdit;
        private MonsterSpriteEditForm monsterSpriteEdit;

        private int w, h;
        private bool bBalloonTipShown;

        public MainForm()
        {
            InitializeComponent();

            fileManagerClosed();
            itemEditClosed();
            monsterspriteEditClosed();
            mapEditClosed();
            monsterEditClosed();
            MSprEditClosed();

            //ROMLoaded = L2ROM.LoadROM("Lufia 2.smc");
            w = Width;
            h = Height;
        }

        public void fileManagerClosed()
        {
            fileManager = null;
        }

        public void itemEditClosed()
        {
            itemEdit = null;
        }

        public void mapEditClosed()
        {
            mapEdit = null;
        }

        public void monsterEditClosed()
        {
            monsterEdit = null;
        }

		public void monsterspriteEditClosed()
		{
            mapspriteEdit = null;
		}

        public void MSprEditClosed()
        {
            monsterSpriteEdit = null;
        }

        public void GUIFileManager()
        {
            if (fileManager == null)
            {
                fileManager = new FileManagerForm(this);
                fileManager.Show();
            }
            else
                fileManager.BringToFront();
        }

        public void GUIItemEdit(int sel)
        {
            if (itemEdit == null)
            {
                itemEdit = new ItemEditForm(this, sel);
                itemEdit.Show();
            }
            else
                itemEdit.BringToFront();
        }

        public void GUIMSEdit(int sel)
        {
            if (mapspriteEdit == null)
            {
                mapspriteEdit = new MapSpriteEditForm(this, sel);
                mapspriteEdit.Show();
            }
            else
                mapspriteEdit.BringToFront();
        }

        private void EnableMenus(bool enable)
        {
            editorenToolStripMenuItem.Enabled = enable;
            kompilierenToolStripMenuItem.Enabled = enable;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            EnableMenus(false);

            tabs.TabPages.Remove(tabProjectInfo);

            //Retrieve recent project
            RegistryKey RegKey = Registry.CurrentUser.CreateSubKey("Software\\LEdit");

            FileInfo RecentProjectFile = null;
            string RecentProjectName = string.Empty; ;

            try
            {
                RecentProjectFile = new FileInfo(RegKey.GetValue("RecentProjectFile").ToString());
                RecentProjectName = RegKey.GetValue("RecentProjectName").ToString();
            }
            catch (Exception)
            {
            }

            if (RecentProjectFile != null && RecentProjectFile.Exists)
            {
                txtRecentProjectName.Text = RecentProjectName;
                btOpenRecent.Tag = RecentProjectFile;
            }
            else
            {
                tabs.TabPages.Remove(tabStart);
            }
        }

        public void ListMods()
        {
            lstMods.Items.Clear();

            if (!Project.Initialized)
                return;

            foreach (Project.Mod x in Project.Mods)
            {
                ListViewItem item = new ListViewItem(Content.FormatID(x.ID));
                item.SubItems.Add(Content.Retrieve(x.Type, x.ID).GetName());
                item.SubItems.Add(x.File.FullName);

                switch(x.Type)
                {
                    case Content.ContentType.Monster:
                        item.Group = lstMods.Groups[0];
                        break;

                    case Content.ContentType.MonsterSprite:
                        item.Group = lstMods.Groups[1];
                        break;
                }

                item.Tag = x;
                lstMods.Items.Add(item);
            }
        }

        private void ProjectOpened()
        {
            EnableMenus(Project.Initialized);
            Text = "LEdit";

            if (Project.Initialized)
            {
                Text += " - " + Project.Name;

                tabs.TabPages.Remove(tabStart);
                tabs.TabPages.Add(tabProjectInfo);

                tabs.SelectedTab = tabProjectInfo;

                RegistryKey RegKey = Registry.CurrentUser.CreateSubKey("Software\\LEdit");
                RegKey.SetValue("RecentProjectName", Project.Name);
                RegKey.SetValue("RecentProjectFile", Project.GetInfoFile().FullName);
            }
            else
            {
                tabs.TabPages.Clear();
                tabs.TabPages.Add(tabStart);
            }

            tray.Text = Text;
            ListMods();
        }

        public void GUIMapEdit()
        {
            if (mapEdit == null)
            {
                mapEdit = new MapEditForm(this);
                mapEdit.Show();
            }
            else
                mapEdit.BringToFront();
        }

        public void GUIMEdit(int sel)
        {
            if (monsterEdit == null)
            {
                monsterEdit = new MonsterEditForm(this,sel);
                monsterEdit.Show();
            }
            else
                monsterEdit.BringToFront();
        }

        public void GUIMSprEdit(int sel)
        {
            if (monsterSpriteEdit == null)
            {
                monsterSpriteEdit = new MonsterSpriteEditForm(this, sel);
                monsterSpriteEdit.Show();
            }
            else
            {
                monsterSpriteEdit.BringToFront();
                //monsterSpriteEdit.Select(sel);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ForceQuit)
            {
                tray.Visible = true;

                if (!bBalloonTipShown)
                {
                    tray.ShowBalloonTip(
                        0,
                        "LEdit wurde ins System Tray minimiert.",
                        "Über dieses Icon können das Hauptfenster und die einzelnen" + Environment.NewLine +
                        "Editoren weiterhin erreicht werden.",
                        ToolTipIcon.Info);

                    bBalloonTipShown = true;
                }

                Hide();
                e.Cancel = true;
            }
        }

        private void tray_DoubleClick(object sender, EventArgs e)
        {
            Show();
            tray.Visible = false;
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Show();
            tray.Visible = false;
        }

        public void End()
        {
            if (Project.Initialized)
                Project.Close();

            Close();
        }

        public void GUIEnd()
        {
            ForceQuit = true;
            End();
        }

        private void endLEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIEnd();
        }

        private void monsterEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIMEdit(0);
        }

        private void mapSpriteEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIMSEdit(0);
        }

        private void mapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIMapEdit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIEnd();
        }

        private void mapEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GUIMapEdit();
        }

        private void mapSpriteEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GUIMSEdit(0);
        }

        private void monsterEditorToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GUIMEdit(0);
        }

        private void monsterSpriteEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIMSprEdit(0);
        }

        private void mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIMSprEdit(0);
        }

        private void itemEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIItemEdit(0);
        }

        private void dateiManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GUIFileManager();
        }

        private void lEditToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void überLEditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = new AboutLEdit().ShowDialog(this);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void neuesProjektToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project.NewProject();
            ProjectOpened();
        }

        private void projektÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() != DialogResult.Cancel)
            {
                Project.Open(new FileInfo(openProjectDialog.FileName));
                ProjectOpened();
            }
        }

        private void kompilierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Project.Compile();
        }

        private void btOpenRecent_Click(object sender, EventArgs e)
        {
            Project.Open(((FileInfo)btOpenRecent.Tag));
            ProjectOpened();
        }

    }
}

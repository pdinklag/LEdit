/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Patrick
 * Datum: 17.08.2007
 * Zeit: 23:08
 *
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
namespace LEdit
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Monster", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("MonsterSprites", System.Windows.Forms.HorizontalAlignment.Left);
            this.tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.trayContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.dateiManagerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.itemEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mapEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapSpriteEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monsterEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monsterSpriteEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.endLEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuesProjektToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projektÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.kompilierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dateiManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.itemEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mapSpriteEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.monsterEditorToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.überLEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.grpStart = new System.Windows.Forms.GroupBox();
            this.btOpenRecent = new System.Windows.Forms.Button();
            this.txtRecentProjectName = new System.Windows.Forms.TextBox();
            this.grpAbout = new System.Windows.Forms.GroupBox();
            this.tabProjectInfo = new System.Windows.Forms.TabPage();
            this.lstMods = new System.Windows.Forms.ListView();
            this.colID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.trayContext.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.grpStart.SuspendLayout();
            this.tabProjectInfo.SuspendLayout();
            this.SuspendLayout();
            //
            // tray
            //
            this.tray.ContextMenuStrip = this.trayContext;
            this.tray.Icon = ((System.Drawing.Icon)(resources.GetObject("tray.Icon")));
            this.tray.Text = "LEdit";
            this.tray.DoubleClick += new System.EventHandler(this.tray_DoubleClick);
            //
            // trayContext
            //
            this.trayContext.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.trayContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.toolStripMenuItem1,
            this.dateiManagerToolStripMenuItem1,
            this.itemEditorToolStripMenuItem1,
            this.mapEditorToolStripMenuItem,
            this.mapSpriteEditorToolStripMenuItem,
            this.monsterEditorToolStripMenuItem,
            this.monsterSpriteEditorToolStripMenuItem,
            this.toolStripMenuItem2,
            this.endLEditToolStripMenuItem});
            this.trayContext.Name = "trayContext";
            this.trayContext.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.trayContext.Size = new System.Drawing.Size(259, 256);
            //
            // restoreToolStripMenuItem
            //
            this.restoreToolStripMenuItem.Image = global::LEdit.Properties.Resources.L16;
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.restoreToolStripMenuItem.Text = "Hauptfenster";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            //
            // toolStripMenuItem1
            //
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(255, 6);
            //
            // dateiManagerToolStripMenuItem1
            //
            this.dateiManagerToolStripMenuItem1.Image = global::LEdit.Properties.Resources.binary;
            this.dateiManagerToolStripMenuItem1.Name = "dateiManagerToolStripMenuItem1";
            this.dateiManagerToolStripMenuItem1.Size = new System.Drawing.Size(258, 30);
            this.dateiManagerToolStripMenuItem1.Text = "Datei-Manager";
            this.dateiManagerToolStripMenuItem1.Visible = false;
            //
            // itemEditorToolStripMenuItem1
            //
            this.itemEditorToolStripMenuItem1.Image = global::LEdit.Properties.Resources.item;
            this.itemEditorToolStripMenuItem1.Name = "itemEditorToolStripMenuItem1";
            this.itemEditorToolStripMenuItem1.Size = new System.Drawing.Size(258, 30);
            this.itemEditorToolStripMenuItem1.Text = "Item-Editor";
            this.itemEditorToolStripMenuItem1.Visible = false;
            //
            // mapEditorToolStripMenuItem
            //
            this.mapEditorToolStripMenuItem.Image = global::LEdit.Properties.Resources.bush;
            this.mapEditorToolStripMenuItem.Name = "mapEditorToolStripMenuItem";
            this.mapEditorToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.mapEditorToolStripMenuItem.Text = "Map-Editor";
            this.mapEditorToolStripMenuItem.Visible = false;
            this.mapEditorToolStripMenuItem.Click += new System.EventHandler(this.mapEditorToolStripMenuItem_Click);
            //
            // mapSpriteEditorToolStripMenuItem
            //
            this.mapSpriteEditorToolStripMenuItem.Image = global::LEdit.Properties.Resources.maxim;
            this.mapSpriteEditorToolStripMenuItem.Name = "mapSpriteEditorToolStripMenuItem";
            this.mapSpriteEditorToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.mapSpriteEditorToolStripMenuItem.Text = "MapSprite-Editor";
            this.mapSpriteEditorToolStripMenuItem.Visible = false;
            this.mapSpriteEditorToolStripMenuItem.Click += new System.EventHandler(this.mapSpriteEditorToolStripMenuItem_Click);
            //
            // monsterEditorToolStripMenuItem
            //
            this.monsterEditorToolStripMenuItem.Image = global::LEdit.Properties.Resources.blob;
            this.monsterEditorToolStripMenuItem.Name = "monsterEditorToolStripMenuItem";
            this.monsterEditorToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.monsterEditorToolStripMenuItem.Text = "Monster-Editor";
            this.monsterEditorToolStripMenuItem.Click += new System.EventHandler(this.monsterEditorToolStripMenuItem_Click);
            //
            // monsterSpriteEditorToolStripMenuItem
            //
            this.monsterSpriteEditorToolStripMenuItem.Image = global::LEdit.Properties.Resources.motte;
            this.monsterSpriteEditorToolStripMenuItem.Name = "monsterSpriteEditorToolStripMenuItem";
            this.monsterSpriteEditorToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.monsterSpriteEditorToolStripMenuItem.Text = "MonsterSprite-Editor";
            this.monsterSpriteEditorToolStripMenuItem.Click += new System.EventHandler(this.monsterSpriteEditorToolStripMenuItem_Click);
            //
            // toolStripMenuItem2
            //
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(255, 6);
            //
            // endLEditToolStripMenuItem
            //
            this.endLEditToolStripMenuItem.Image = global::LEdit.Properties.Resources.clear_16;
            this.endLEditToolStripMenuItem.Name = "endLEditToolStripMenuItem";
            this.endLEditToolStripMenuItem.Size = new System.Drawing.Size(258, 30);
            this.endLEditToolStripMenuItem.Text = "Beenden";
            this.endLEditToolStripMenuItem.Click += new System.EventHandler(this.endLEditToolStripMenuItem_Click);
            //
            // menuStrip1
            //
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editorenToolStripMenuItem,
            this.hilfeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(718, 35);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            //
            // fileToolStripMenuItem
            //
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuesProjektToolStripMenuItem,
            this.projektÖffnenToolStripMenuItem,
            this.toolStripMenuItem4,
            this.kompilierenToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.fileToolStripMenuItem.Text = "&Datei";
            //
            // neuesProjektToolStripMenuItem
            //
            this.neuesProjektToolStripMenuItem.Image = global::LEdit.Properties.Resources.new_16;
            this.neuesProjektToolStripMenuItem.Name = "neuesProjektToolStripMenuItem";
            this.neuesProjektToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.neuesProjektToolStripMenuItem.Text = "Neues Projekt...";
            this.neuesProjektToolStripMenuItem.Click += new System.EventHandler(this.neuesProjektToolStripMenuItem_Click);
            //
            // projektÖffnenToolStripMenuItem
            //
            this.projektÖffnenToolStripMenuItem.Image = global::LEdit.Properties.Resources.open_fnt_16;
            this.projektÖffnenToolStripMenuItem.Name = "projektÖffnenToolStripMenuItem";
            this.projektÖffnenToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.projektÖffnenToolStripMenuItem.Text = "Projekt öffnen...";
            this.projektÖffnenToolStripMenuItem.Click += new System.EventHandler(this.projektÖffnenToolStripMenuItem_Click);
            //
            // toolStripMenuItem4
            //
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(218, 6);
            //
            // kompilierenToolStripMenuItem
            //
            this.kompilierenToolStripMenuItem.Image = global::LEdit.Properties.Resources.batch_import_16;
            this.kompilierenToolStripMenuItem.Name = "kompilierenToolStripMenuItem";
            this.kompilierenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F7;
            this.kompilierenToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.kompilierenToolStripMenuItem.Text = "Kompilieren";
            this.kompilierenToolStripMenuItem.Click += new System.EventHandler(this.kompilierenToolStripMenuItem_Click);
            //
            // toolStripMenuItem3
            //
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(218, 6);
            //
            // exitToolStripMenuItem
            //
            this.exitToolStripMenuItem.Image = global::LEdit.Properties.Resources.clear_16;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(221, 30);
            this.exitToolStripMenuItem.Text = "&Beenden";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            //
            // editorenToolStripMenuItem
            //
            this.editorenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiManagerToolStripMenuItem,
            this.itemEditorToolStripMenuItem,
            this.mapEditorToolStripMenuItem1,
            this.mapSpriteEditorToolStripMenuItem1,
            this.monsterEditorToolStripMenuItem1,
            this.mToolStripMenuItem});
            this.editorenToolStripMenuItem.Name = "editorenToolStripMenuItem";
            this.editorenToolStripMenuItem.Size = new System.Drawing.Size(90, 29);
            this.editorenToolStripMenuItem.Text = "&Editoren";
            //
            // dateiManagerToolStripMenuItem
            //
            this.dateiManagerToolStripMenuItem.Image = global::LEdit.Properties.Resources.binary;
            this.dateiManagerToolStripMenuItem.Name = "dateiManagerToolStripMenuItem";
            this.dateiManagerToolStripMenuItem.Size = new System.Drawing.Size(262, 30);
            this.dateiManagerToolStripMenuItem.Text = "&Datei-Manager";
            this.dateiManagerToolStripMenuItem.Click += new System.EventHandler(this.dateiManagerToolStripMenuItem_Click);
            //
            // itemEditorToolStripMenuItem
            //
            this.itemEditorToolStripMenuItem.Image = global::LEdit.Properties.Resources.item;
            this.itemEditorToolStripMenuItem.Name = "itemEditorToolStripMenuItem";
            this.itemEditorToolStripMenuItem.Size = new System.Drawing.Size(262, 30);
            this.itemEditorToolStripMenuItem.Text = "Item-Editor";
            this.itemEditorToolStripMenuItem.Click += new System.EventHandler(this.itemEditorToolStripMenuItem_Click);
            //
            // mapEditorToolStripMenuItem1
            //
            this.mapEditorToolStripMenuItem1.Image = global::LEdit.Properties.Resources.bush;
            this.mapEditorToolStripMenuItem1.Name = "mapEditorToolStripMenuItem1";
            this.mapEditorToolStripMenuItem1.Size = new System.Drawing.Size(262, 30);
            this.mapEditorToolStripMenuItem1.Text = "Map-Editor";
            this.mapEditorToolStripMenuItem1.Click += new System.EventHandler(this.mapEditorToolStripMenuItem1_Click);
            //
            // mapSpriteEditorToolStripMenuItem1
            //
            this.mapSpriteEditorToolStripMenuItem1.Image = global::LEdit.Properties.Resources.maxim;
            this.mapSpriteEditorToolStripMenuItem1.Name = "mapSpriteEditorToolStripMenuItem1";
            this.mapSpriteEditorToolStripMenuItem1.Size = new System.Drawing.Size(262, 30);
            this.mapSpriteEditorToolStripMenuItem1.Text = "MapSprite-Editor";
            this.mapSpriteEditorToolStripMenuItem1.Click += new System.EventHandler(this.mapSpriteEditorToolStripMenuItem1_Click);
            //
            // monsterEditorToolStripMenuItem1
            //
            this.monsterEditorToolStripMenuItem1.Image = global::LEdit.Properties.Resources.blob;
            this.monsterEditorToolStripMenuItem1.Name = "monsterEditorToolStripMenuItem1";
            this.monsterEditorToolStripMenuItem1.Size = new System.Drawing.Size(262, 30);
            this.monsterEditorToolStripMenuItem1.Text = "Monster-Editor";
            this.monsterEditorToolStripMenuItem1.Click += new System.EventHandler(this.monsterEditorToolStripMenuItem1_Click);
            //
            // mToolStripMenuItem
            //
            this.mToolStripMenuItem.Image = global::LEdit.Properties.Resources.motte;
            this.mToolStripMenuItem.Name = "mToolStripMenuItem";
            this.mToolStripMenuItem.Size = new System.Drawing.Size(262, 30);
            this.mToolStripMenuItem.Text = "MonsterSprite-Editor";
            this.mToolStripMenuItem.Click += new System.EventHandler(this.mToolStripMenuItem_Click);
            //
            // hilfeToolStripMenuItem
            //
            this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.überLEditToolStripMenuItem});
            this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            this.hilfeToolStripMenuItem.Size = new System.Drawing.Size(60, 29);
            this.hilfeToolStripMenuItem.Text = "&Hilfe";
            //
            // überLEditToolStripMenuItem
            //
            this.überLEditToolStripMenuItem.Image = global::LEdit.Properties.Resources.about_16;
            this.überLEditToolStripMenuItem.Name = "überLEditToolStripMenuItem";
            this.überLEditToolStripMenuItem.Size = new System.Drawing.Size(189, 30);
            this.überLEditToolStripMenuItem.Text = "Über LEdit...";
            this.überLEditToolStripMenuItem.Click += new System.EventHandler(this.überLEditToolStripMenuItem_Click);
            //
            // openProjectDialog
            //
            this.openProjectDialog.Filter = "LEdit-Projekte|*.l2project";
            this.openProjectDialog.Title = "Lufia-ROM auswählen";
            //
            // tabs
            //
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabs.Controls.Add(this.tabStart);
            this.tabs.Controls.Add(this.tabProjectInfo);
            this.tabs.Location = new System.Drawing.Point(18, 42);
            this.tabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(682, 345);
            this.tabs.TabIndex = 7;
            //
            // tabStart
            //
            this.tabStart.Controls.Add(this.grpStart);
            this.tabStart.Controls.Add(this.grpAbout);
            this.tabStart.Location = new System.Drawing.Point(4, 32);
            this.tabStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabStart.Name = "tabStart";
            this.tabStart.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabStart.Size = new System.Drawing.Size(674, 309);
            this.tabStart.TabIndex = 0;
            this.tabStart.Text = "Start";
            this.tabStart.UseVisualStyleBackColor = true;
            //
            // grpStart
            //
            this.grpStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpStart.Controls.Add(this.btOpenRecent);
            this.grpStart.Controls.Add(this.txtRecentProjectName);
            this.grpStart.Location = new System.Drawing.Point(0, 220);
            this.grpStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpStart.Name = "grpStart";
            this.grpStart.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpStart.Size = new System.Drawing.Size(670, 80);
            this.grpStart.TabIndex = 6;
            this.grpStart.TabStop = false;
            this.grpStart.Text = "Zuletzt geöffnetes Projekt";
            //
            // btOpenRecent
            //
            this.btOpenRecent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btOpenRecent.Image = global::LEdit.Properties.Resources.open_fnt_16;
            this.btOpenRecent.Location = new System.Drawing.Point(495, 29);
            this.btOpenRecent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btOpenRecent.Name = "btOpenRecent";
            this.btOpenRecent.Size = new System.Drawing.Size(166, 42);
            this.btOpenRecent.TabIndex = 7;
            this.btOpenRecent.Text = "Öffnen";
            this.btOpenRecent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btOpenRecent.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOpenRecent.UseVisualStyleBackColor = true;
            this.btOpenRecent.Click += new System.EventHandler(this.btOpenRecent_Click);
            //
            // txtRecentProjectName
            //
            this.txtRecentProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRecentProjectName.Location = new System.Drawing.Point(9, 35);
            this.txtRecentProjectName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRecentProjectName.Name = "txtRecentProjectName";
            this.txtRecentProjectName.ReadOnly = true;
            this.txtRecentProjectName.Size = new System.Drawing.Size(475, 26);
            this.txtRecentProjectName.TabIndex = 6;
            //
            // grpAbout
            //
            this.grpAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAbout.Cursor = System.Windows.Forms.Cursors.Default;
            this.grpAbout.Location = new System.Drawing.Point(0, 0);
            this.grpAbout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpAbout.Name = "grpAbout";
            this.grpAbout.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpAbout.Size = new System.Drawing.Size(670, 211);
            this.grpAbout.TabIndex = 5;
            this.grpAbout.TabStop = false;
            this.grpAbout.Text = "LEdit";
            //
            // tabProjectInfo
            //
            this.tabProjectInfo.Controls.Add(this.lstMods);
            this.tabProjectInfo.Location = new System.Drawing.Point(4, 32);
            this.tabProjectInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabProjectInfo.Name = "tabProjectInfo";
            this.tabProjectInfo.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabProjectInfo.Size = new System.Drawing.Size(674, 309);
            this.tabProjectInfo.TabIndex = 1;
            this.tabProjectInfo.Text = "Modifikationen";
            this.tabProjectInfo.UseVisualStyleBackColor = true;
            //
            // lstMods
            //
            this.lstMods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstMods.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colID,
            this.colName,
            this.colFile});
            this.lstMods.Cursor = System.Windows.Forms.Cursors.Default;
            this.lstMods.FullRowSelect = true;
            listViewGroup1.Header = "Monster";
            listViewGroup1.Name = "lgrpMonsters";
            listViewGroup2.Header = "MonsterSprites";
            listViewGroup2.Name = "lgrpMonsterSprites";
            this.lstMods.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.lstMods.Location = new System.Drawing.Point(0, 0);
            this.lstMods.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstMods.MultiSelect = false;
            this.lstMods.Name = "lstMods";
            this.lstMods.Size = new System.Drawing.Size(668, 298);
            this.lstMods.TabIndex = 1;
            this.lstMods.UseCompatibleStateImageBehavior = false;
            this.lstMods.View = System.Windows.Forms.View.Details;
            //
            // colID
            //
            this.colID.Text = "ID";
            this.colID.Width = 68;
            //
            // colName
            //
            this.colName.Text = "Name";
            this.colName.Width = 122;
            //
            // colFile
            //
            this.colFile.Text = "Datei";
            this.colFile.Width = 251;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 405);
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "LEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.trayContext.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.grpStart.ResumeLayout(false);
            this.grpStart.PerformLayout();
            this.tabProjectInfo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.NotifyIcon tray;
        private System.Windows.Forms.ContextMenuStrip trayContext;
        private System.Windows.Forms.ToolStripMenuItem endLEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mapEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapSpriteEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monsterEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapEditorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mapSpriteEditorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem monsterEditorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monsterSpriteEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem itemEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateiManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem überLEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dateiManagerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem itemEditorToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem neuesProjektToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.OpenFileDialog openProjectDialog;
        private System.Windows.Forms.ToolStripMenuItem projektÖffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem kompilierenToolStripMenuItem;
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.GroupBox grpStart;
        private System.Windows.Forms.Button btOpenRecent;
        private System.Windows.Forms.TextBox txtRecentProjectName;
        private System.Windows.Forms.GroupBox grpAbout;
        private System.Windows.Forms.TabPage tabProjectInfo;
        private System.Windows.Forms.ListView lstMods;
        private System.Windows.Forms.ColumnHeader colID;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colFile;
	}
}

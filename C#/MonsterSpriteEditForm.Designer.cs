namespace LEdit
{
    partial class MonsterSpriteEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonsterSpriteEditForm));
            this.contextMenuTilemap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuPal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tilemapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstPals = new System.Windows.Forms.ListBox();
            this.opendlg = new System.Windows.Forms.OpenFileDialog();
            this.savedlg = new System.Windows.Forms.SaveFileDialog();
            this.öffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speichernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schließenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMPexportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMPImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hinzufügenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.entfernenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aus16FarbenBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pb = new System.Windows.Forms.PictureBox();
            this.pbpal = new System.Windows.Forms.PictureBox();
            this.exportBMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importPalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuTilemap.SuspendLayout();
            this.contextMenuPal.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbpal)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuTilemap
            // 
            this.contextMenuTilemap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportBMPToolStripMenuItem,
            this.importBMPToolStripMenuItem});
            this.contextMenuTilemap.Name = "contextMenuTilemap";
            this.contextMenuTilemap.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuTilemap.Size = new System.Drawing.Size(143, 48);
            // 
            // contextMenuPal
            // 
            this.contextMenuPal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importPalToolStripMenuItem});
            this.contextMenuPal.Name = "contextMenuPal";
            this.contextMenuPal.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuPal.Size = new System.Drawing.Size(185, 26);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.tilemapToolStripMenuItem,
            this.paletteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(396, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.öffnenToolStripMenuItem,
            this.speichernToolStripMenuItem,
            this.toolStripMenuItem1,
            this.schließenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.dateiToolStripMenuItem.Text = "&Datei";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // tilemapToolStripMenuItem
            // 
            this.tilemapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bMPexportierenToolStripMenuItem,
            this.bMPImportierenToolStripMenuItem});
            this.tilemapToolStripMenuItem.Name = "tilemapToolStripMenuItem";
            this.tilemapToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.tilemapToolStripMenuItem.Text = "&Tilemap";
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hinzufügenToolStripMenuItem,
            this.entfernenToolStripMenuItem,
            this.toolStripMenuItem2,
            this.aus16FarbenBitmapToolStripMenuItem});
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.paletteToolStripMenuItem.Text = "&Palette";
            this.paletteToolStripMenuItem.Click += new System.EventHandler(this.paletteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(183, 6);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pb);
            this.groupBox3.Controls.Add(this.pbpal);
            this.groupBox3.Location = new System.Drawing.Point(12, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(169, 156);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Ansicht";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstPals);
            this.groupBox2.Location = new System.Drawing.Point(187, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 156);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Paletten";
            // 
            // lstPals
            // 
            this.lstPals.FormattingEnabled = true;
            this.lstPals.IntegralHeight = false;
            this.lstPals.Location = new System.Drawing.Point(6, 19);
            this.lstPals.Name = "lstPals";
            this.lstPals.ScrollAlwaysVisible = true;
            this.lstPals.Size = new System.Drawing.Size(188, 130);
            this.lstPals.TabIndex = 0;
            this.lstPals.SelectedIndexChanged += new System.EventHandler(this.lstPals_SelectedIndexChanged);
            this.lstPals.DoubleClick += new System.EventHandler(this.lstPals_DoubleClick);
            this.lstPals.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstPals_KeyDown);
            // 
            // opendlg
            // 
            this.opendlg.DefaultExt = "bmp";
            this.opendlg.Filter = "Windows 16-Farben-Bitmap (*.bmp)|*.bmp";
            // 
            // savedlg
            // 
            this.savedlg.DefaultExt = "bmp";
            this.savedlg.Filter = "Windows 16-Farben-Bitmap (*.bmp)|*.bmp|Portable Network Graphics (*.png)|*.png";
            // 
            // öffnenToolStripMenuItem
            // 
            this.öffnenToolStripMenuItem.Image = global::LEdit.Properties.Resources.open_fnt_16;
            this.öffnenToolStripMenuItem.Name = "öffnenToolStripMenuItem";
            this.öffnenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.öffnenToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.öffnenToolStripMenuItem.Text = "&Öffnen...";
            this.öffnenToolStripMenuItem.Click += new System.EventHandler(this.öffnenToolStripMenuItem_Click);
            // 
            // speichernToolStripMenuItem
            // 
            this.speichernToolStripMenuItem.Image = global::LEdit.Properties.Resources.save_fnt_16;
            this.speichernToolStripMenuItem.Name = "speichernToolStripMenuItem";
            this.speichernToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.speichernToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.speichernToolStripMenuItem.Text = "&Speichern";
            this.speichernToolStripMenuItem.Click += new System.EventHandler(this.speichernToolStripMenuItem_Click);
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Image = global::LEdit.Properties.Resources.clear_16;
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.schließenToolStripMenuItem.Text = "Schließen";
            this.schließenToolStripMenuItem.Click += new System.EventHandler(this.schließenToolStripMenuItem_Click);
            // 
            // bMPexportierenToolStripMenuItem
            // 
            this.bMPexportierenToolStripMenuItem.Image = global::LEdit.Properties.Resources.redo_16;
            this.bMPexportierenToolStripMenuItem.Name = "bMPexportierenToolStripMenuItem";
            this.bMPexportierenToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.bMPexportierenToolStripMenuItem.Text = "&Exportieren...";
            this.bMPexportierenToolStripMenuItem.Click += new System.EventHandler(this.bMPexportierenToolStripMenuItem_Click);
            // 
            // bMPImportierenToolStripMenuItem
            // 
            this.bMPImportierenToolStripMenuItem.Image = global::LEdit.Properties.Resources.undo_16;
            this.bMPImportierenToolStripMenuItem.Name = "bMPImportierenToolStripMenuItem";
            this.bMPImportierenToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.bMPImportierenToolStripMenuItem.Text = "&Importieren...";
            this.bMPImportierenToolStripMenuItem.Click += new System.EventHandler(this.bMPImportierenToolStripMenuItem_Click);
            // 
            // hinzufügenToolStripMenuItem
            // 
            this.hinzufügenToolStripMenuItem.Image = global::LEdit.Properties.Resources.new_16;
            this.hinzufügenToolStripMenuItem.Name = "hinzufügenToolStripMenuItem";
            this.hinzufügenToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.hinzufügenToolStripMenuItem.Text = "&Hinzufügen";
            this.hinzufügenToolStripMenuItem.Click += new System.EventHandler(this.hinzufügenToolStripMenuItem_Click);
            // 
            // entfernenToolStripMenuItem
            // 
            this.entfernenToolStripMenuItem.Image = global::LEdit.Properties.Resources.clear_16;
            this.entfernenToolStripMenuItem.Name = "entfernenToolStripMenuItem";
            this.entfernenToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.entfernenToolStripMenuItem.Text = "&Entfernen";
            this.entfernenToolStripMenuItem.Click += new System.EventHandler(this.entfernenToolStripMenuItem_Click);
            // 
            // aus16FarbenBitmapToolStripMenuItem
            // 
            this.aus16FarbenBitmapToolStripMenuItem.Image = global::LEdit.Properties.Resources.undo_16;
            this.aus16FarbenBitmapToolStripMenuItem.Name = "aus16FarbenBitmapToolStripMenuItem";
            this.aus16FarbenBitmapToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.aus16FarbenBitmapToolStripMenuItem.Text = "Aus BMP &Importieren...";
            this.aus16FarbenBitmapToolStripMenuItem.Click += new System.EventHandler(this.aus16FarbenBitmapToolStripMenuItem_Click);
            // 
            // pb
            // 
            this.pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb.ContextMenuStrip = this.contextMenuTilemap;
            this.pb.Location = new System.Drawing.Point(6, 19);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(130, 130);
            this.pb.TabIndex = 36;
            this.pb.TabStop = false;
            this.pb.Paint += new System.Windows.Forms.PaintEventHandler(this.pb_Paint);
            // 
            // pbpal
            // 
            this.pbpal.ContextMenuStrip = this.contextMenuPal;
            this.pbpal.Location = new System.Drawing.Point(143, 19);
            this.pbpal.Name = "pbpal";
            this.pbpal.Size = new System.Drawing.Size(20, 130);
            this.pbpal.TabIndex = 7;
            this.pbpal.TabStop = false;
            this.pbpal.Paint += new System.Windows.Forms.PaintEventHandler(this.pbpal_Paint);
            // 
            // exportBMPToolStripMenuItem
            // 
            this.exportBMPToolStripMenuItem.Image = global::LEdit.Properties.Resources.redo_16;
            this.exportBMPToolStripMenuItem.Name = "exportBMPToolStripMenuItem";
            this.exportBMPToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exportBMPToolStripMenuItem.Text = "Exportieren...";
            this.exportBMPToolStripMenuItem.Click += new System.EventHandler(this.exportBMPToolStripMenuItem_Click);
            // 
            // importBMPToolStripMenuItem
            // 
            this.importBMPToolStripMenuItem.Image = global::LEdit.Properties.Resources.undo_16;
            this.importBMPToolStripMenuItem.Name = "importBMPToolStripMenuItem";
            this.importBMPToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.importBMPToolStripMenuItem.Text = "Importieren...";
            this.importBMPToolStripMenuItem.Click += new System.EventHandler(this.importBMPToolStripMenuItem_Click);
            // 
            // importPalToolStripMenuItem
            // 
            this.importPalToolStripMenuItem.Image = global::LEdit.Properties.Resources.undo_16;
            this.importPalToolStripMenuItem.Name = "importPalToolStripMenuItem";
            this.importPalToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.importPalToolStripMenuItem.Text = "Aus BMP importieren...";
            this.importPalToolStripMenuItem.Click += new System.EventHandler(this.importPalToolStripMenuItem_Click);
            // 
            // MonsterSpriteEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 192);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MonsterSpriteEditForm";
            this.Load += new System.EventHandler(this.MonsterSpriteEditForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MonsterSpriteEditForm_FormClosed);
            this.contextMenuTilemap.ResumeLayout(false);
            this.contextMenuPal.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbpal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuTilemap;
        private System.Windows.Forms.ToolStripMenuItem exportBMPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importBMPToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuPal;
        private System.Windows.Forms.ToolStripMenuItem importPalToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tilemapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bMPexportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bMPImportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aus16FarbenBitmapToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pb;
        private System.Windows.Forms.PictureBox pbpal;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstPals;
        private System.Windows.Forms.OpenFileDialog opendlg;
        private System.Windows.Forms.SaveFileDialog savedlg;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speichernToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hinzufügenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem entfernenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem öffnenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem schließenToolStripMenuItem;
    }
}
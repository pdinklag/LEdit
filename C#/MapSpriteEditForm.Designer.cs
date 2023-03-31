/*
 * Erstellt mit SharpDevelop.
 * Benutzer: Patrick
 * Datum: 18.08.2007
 * Zeit: 07:26
 * 
 * Sie können diese Vorlage unter Extras > Optionen > Codeerstellung > Standardheader ändern.
 */
namespace LEdit
{
	partial class MapSpriteEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSpriteEditForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstMS = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pb = new System.Windows.Forms.PictureBox();
            this.contextMenuTilemap = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportBMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importBMPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.flag80 = new System.Windows.Forms.CheckBox();
            this.flag40 = new System.Windows.Forms.CheckBox();
            this.flag20 = new System.Windows.Forms.CheckBox();
            this.flag10 = new System.Windows.Forms.CheckBox();
            this.flag08 = new System.Windows.Forms.CheckBox();
            this.flag04 = new System.Windows.Forms.CheckBox();
            this.flag02 = new System.Windows.Forms.CheckBox();
            this.flag01 = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtROM = new System.Windows.Forms.TextBox();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pbpal = new System.Windows.Forms.PictureBox();
            this.contextMenuPal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importPalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.udPal = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.savedlg = new System.Windows.Forms.SaveFileDialog();
            this.opendlg = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tilemapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMPexportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bMPImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aus16FarbenBitmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.iToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
            this.contextMenuTilemap.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbpal)).BeginInit();
            this.contextMenuPal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udPal)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstMS);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 300);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MapSprites";
            // 
            // lstMS
            // 
            this.lstMS.BackColor = System.Drawing.SystemColors.Window;
            this.lstMS.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstMS.FormattingEnabled = true;
            this.lstMS.IntegralHeight = false;
            this.lstMS.Location = new System.Drawing.Point(6, 19);
            this.lstMS.Name = "lstMS";
            this.lstMS.ScrollAlwaysVisible = true;
            this.lstMS.Size = new System.Drawing.Size(190, 275);
            this.lstMS.TabIndex = 2;
            this.lstMS.SelectedIndexChanged += new System.EventHandler(this.LstMSSelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pb);
            this.groupBox2.Location = new System.Drawing.Point(220, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(398, 91);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ansicht";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // pb
            // 
            this.pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pb.ContextMenuStrip = this.contextMenuTilemap;
            this.pb.Location = new System.Drawing.Point(6, 19);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(386, 66);
            this.pb.TabIndex = 4;
            this.pb.TabStop = false;
            this.pb.Paint += new System.Windows.Forms.PaintEventHandler(this.PbPaint);
            // 
            // contextMenuTilemap
            // 
            this.contextMenuTilemap.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportBMPToolStripMenuItem,
            this.importBMPToolStripMenuItem});
            this.contextMenuTilemap.Name = "contextMenuTilemap";
            this.contextMenuTilemap.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuTilemap.Size = new System.Drawing.Size(166, 48);
            // 
            // exportBMPToolStripMenuItem
            // 
            this.exportBMPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportBMPToolStripMenuItem.Image")));
            this.exportBMPToolStripMenuItem.Name = "exportBMPToolStripMenuItem";
            this.exportBMPToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exportBMPToolStripMenuItem.Text = "BMP exportieren...";
            this.exportBMPToolStripMenuItem.Click += new System.EventHandler(this.ExportBMPToolStripMenuItemClick);
            // 
            // importBMPToolStripMenuItem
            // 
            this.importBMPToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importBMPToolStripMenuItem.Image")));
            this.importBMPToolStripMenuItem.Name = "importBMPToolStripMenuItem";
            this.importBMPToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.importBMPToolStripMenuItem.Text = "BMP importieren...";
            this.importBMPToolStripMenuItem.Click += new System.EventHandler(this.ImportBMPToolStripMenuItemClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.flag80);
            this.groupBox3.Controls.Add(this.flag40);
            this.groupBox3.Controls.Add(this.flag20);
            this.groupBox3.Controls.Add(this.flag10);
            this.groupBox3.Controls.Add(this.flag08);
            this.groupBox3.Controls.Add(this.flag04);
            this.groupBox3.Controls.Add(this.flag02);
            this.groupBox3.Controls.Add(this.flag01);
            this.groupBox3.Location = new System.Drawing.Point(477, 27);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(141, 203);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Flags";
            // 
            // flag80
            // 
            this.flag80.AutoSize = true;
            this.flag80.Enabled = false;
            this.flag80.Location = new System.Drawing.Point(6, 180);
            this.flag80.Name = "flag80";
            this.flag80.Size = new System.Drawing.Size(111, 17);
            this.flag80.TabIndex = 7;
            this.flag80.Text = "0x80 - Unbekannt";
            this.flag80.UseVisualStyleBackColor = true;
            // 
            // flag40
            // 
            this.flag40.AutoSize = true;
            this.flag40.Enabled = false;
            this.flag40.Location = new System.Drawing.Point(6, 157);
            this.flag40.Name = "flag40";
            this.flag40.Size = new System.Drawing.Size(111, 17);
            this.flag40.TabIndex = 6;
            this.flag40.Text = "0x40 - Unbekannt";
            this.flag40.UseVisualStyleBackColor = true;
            // 
            // flag20
            // 
            this.flag20.AutoSize = true;
            this.flag20.Enabled = false;
            this.flag20.Location = new System.Drawing.Point(6, 134);
            this.flag20.Name = "flag20";
            this.flag20.Size = new System.Drawing.Size(111, 17);
            this.flag20.TabIndex = 5;
            this.flag20.Text = "0x20 - Unbekannt";
            this.flag20.UseVisualStyleBackColor = true;
            // 
            // flag10
            // 
            this.flag10.AutoSize = true;
            this.flag10.Enabled = false;
            this.flag10.Location = new System.Drawing.Point(6, 111);
            this.flag10.Name = "flag10";
            this.flag10.Size = new System.Drawing.Size(131, 17);
            this.flag10.TabIndex = 4;
            this.flag10.Text = "0x10 - Erweiterte Anim";
            this.flag10.UseVisualStyleBackColor = true;
            // 
            // flag08
            // 
            this.flag08.AutoSize = true;
            this.flag08.Enabled = false;
            this.flag08.Location = new System.Drawing.Point(6, 88);
            this.flag08.Name = "flag08";
            this.flag08.Size = new System.Drawing.Size(104, 17);
            this.flag08.TabIndex = 3;
            this.flag08.Text = "0x08 - Animation";
            this.flag08.UseVisualStyleBackColor = true;
            // 
            // flag04
            // 
            this.flag04.AutoSize = true;
            this.flag04.Enabled = false;
            this.flag04.Location = new System.Drawing.Point(6, 65);
            this.flag04.Name = "flag04";
            this.flag04.Size = new System.Drawing.Size(111, 17);
            this.flag04.TabIndex = 2;
            this.flag04.Text = "0x04 - Unbekannt";
            this.flag04.UseVisualStyleBackColor = true;
            // 
            // flag02
            // 
            this.flag02.AutoSize = true;
            this.flag02.Enabled = false;
            this.flag02.Location = new System.Drawing.Point(6, 42);
            this.flag02.Name = "flag02";
            this.flag02.Size = new System.Drawing.Size(119, 17);
            this.flag02.TabIndex = 1;
            this.flag02.Text = "0x02 - Horizontal x2";
            this.flag02.UseVisualStyleBackColor = true;
            // 
            // flag01
            // 
            this.flag01.AutoSize = true;
            this.flag01.Enabled = false;
            this.flag01.Location = new System.Drawing.Point(6, 19);
            this.flag01.Name = "flag01";
            this.flag01.Size = new System.Drawing.Size(107, 17);
            this.flag01.TabIndex = 0;
            this.flag01.Text = "0x01 - Vertikal x2";
            this.flag01.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.txtROM);
            this.groupBox4.Controls.Add(this.txtAddr);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.txtName);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.pbpal);
            this.groupBox4.Controls.Add(this.udPal);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Location = new System.Drawing.Point(220, 27);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(251, 203);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Bearbeiten";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "ROM-PC-Addresse:";
            // 
            // txtROM
            // 
            this.txtROM.Location = new System.Drawing.Point(115, 91);
            this.txtROM.Name = "txtROM";
            this.txtROM.ReadOnly = true;
            this.txtROM.Size = new System.Drawing.Size(130, 20);
            this.txtROM.TabIndex = 7;
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(115, 68);
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.ReadOnly = true;
            this.txtAddr.Size = new System.Drawing.Size(130, 20);
            this.txtAddr.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tilemap-Addresse:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(55, 19);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(190, 20);
            this.txtName.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            // 
            // pbpal
            // 
            this.pbpal.ContextMenuStrip = this.contextMenuPal;
            this.pbpal.Location = new System.Drawing.Point(115, 42);
            this.pbpal.Name = "pbpal";
            this.pbpal.Size = new System.Drawing.Size(130, 20);
            this.pbpal.TabIndex = 2;
            this.pbpal.TabStop = false;
            this.pbpal.Paint += new System.Windows.Forms.PaintEventHandler(this.PbpalPaint);
            // 
            // contextMenuPal
            // 
            this.contextMenuPal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importPalToolStripMenuItem});
            this.contextMenuPal.Name = "contextMenuPal";
            this.contextMenuPal.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuPal.Size = new System.Drawing.Size(251, 26);
            // 
            // importPalToolStripMenuItem
            // 
            this.importPalToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("importPalToolStripMenuItem.Image")));
            this.importPalToolStripMenuItem.Name = "importPalToolStripMenuItem";
            this.importPalToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.importPalToolStripMenuItem.Text = "Aus 16-Farben-Bitmap importieren...";
            this.importPalToolStripMenuItem.Click += new System.EventHandler(this.ImportPalToolStripMenuItemClick);
            // 
            // udPal
            // 
            this.udPal.Location = new System.Drawing.Point(55, 42);
            this.udPal.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.udPal.Name = "udPal";
            this.udPal.Size = new System.Drawing.Size(54, 20);
            this.udPal.TabIndex = 1;
            this.udPal.ValueChanged += new System.EventHandler(this.UdPalValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Palette:";
            // 
            // savedlg
            // 
            this.savedlg.DefaultExt = "bmp";
            this.savedlg.Filter = "Windows 16-Farben-Bitmap (*.bmp)|*.bmp";
            // 
            // opendlg
            // 
            this.opendlg.DefaultExt = "bmp";
            this.opendlg.Filter = "Windows 16-Farben-Bitmap (*.bmp)|*.bmp";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tilemapToolStripMenuItem,
            this.paletteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(630, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tilemapToolStripMenuItem
            // 
            this.tilemapToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bMPexportierenToolStripMenuItem,
            this.bMPImportierenToolStripMenuItem,
            this.toolStripMenuItem1,
            this.iToolStripMenuItem});
            this.tilemapToolStripMenuItem.Name = "tilemapToolStripMenuItem";
            this.tilemapToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.tilemapToolStripMenuItem.Text = "&Tilemap";
            // 
            // bMPexportierenToolStripMenuItem
            // 
            this.bMPexportierenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("bMPexportierenToolStripMenuItem.Image")));
            this.bMPexportierenToolStripMenuItem.Name = "bMPexportierenToolStripMenuItem";
            this.bMPexportierenToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bMPexportierenToolStripMenuItem.Text = "BMP &Exportieren...";
            this.bMPexportierenToolStripMenuItem.Click += new System.EventHandler(this.BMPexportierenToolStripMenuItemClick);
            // 
            // bMPImportierenToolStripMenuItem
            // 
            this.bMPImportierenToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("bMPImportierenToolStripMenuItem.Image")));
            this.bMPImportierenToolStripMenuItem.Name = "bMPImportierenToolStripMenuItem";
            this.bMPImportierenToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bMPImportierenToolStripMenuItem.Text = "BMP &Importieren...";
            this.bMPImportierenToolStripMenuItem.Click += new System.EventHandler(this.BMPImportierenToolStripMenuItemClick);
            // 
            // paletteToolStripMenuItem
            // 
            this.paletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aus16FarbenBitmapToolStripMenuItem});
            this.paletteToolStripMenuItem.Name = "paletteToolStripMenuItem";
            this.paletteToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.paletteToolStripMenuItem.Text = "&Palette";
            // 
            // aus16FarbenBitmapToolStripMenuItem
            // 
            this.aus16FarbenBitmapToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aus16FarbenBitmapToolStripMenuItem.Image")));
            this.aus16FarbenBitmapToolStripMenuItem.Name = "aus16FarbenBitmapToolStripMenuItem";
            this.aus16FarbenBitmapToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.aus16FarbenBitmapToolStripMenuItem.Text = "Aus 16-Farben-Bitmap &Importieren...";
            this.aus16FarbenBitmapToolStripMenuItem.Click += new System.EventHandler(this.aus16FarbenBitmapToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // iToolStripMenuItem
            // 
            this.iToolStripMenuItem.Image = global::LEdit.Properties.Resources.redo_16;
            this.iToolStripMenuItem.Name = "iToolStripMenuItem";
            this.iToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.iToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.iToolStripMenuItem.Text = "&In ROM einfügen...";
            this.iToolStripMenuItem.Click += new System.EventHandler(this.iToolStripMenuItem_Click);
            // 
            // MapSpriteEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 339);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MapSpriteEditForm";
            this.Text = "MapSprite-Editor";
            this.Load += new System.EventHandler(this.MapSpriteEditForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapSpriteEditFormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
            this.contextMenuTilemap.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbpal)).EndInit();
            this.contextMenuPal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.udPal)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripMenuItem aus16FarbenBitmapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem paletteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bMPImportierenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem bMPexportierenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem tilemapToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem importPalToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuPal;
		private System.Windows.Forms.OpenFileDialog opendlg;
		private System.Windows.Forms.ToolStripMenuItem importBMPToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportBMPToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuTilemap;
		private System.Windows.Forms.SaveFileDialog savedlg;
		private System.Windows.Forms.TextBox txtROM;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtAddr;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown udPal;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pbpal;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.CheckBox flag01;
		private System.Windows.Forms.CheckBox flag02;
		private System.Windows.Forms.CheckBox flag04;
		private System.Windows.Forms.CheckBox flag08;
		private System.Windows.Forms.CheckBox flag10;
		private System.Windows.Forms.CheckBox flag20;
		private System.Windows.Forms.CheckBox flag40;
		private System.Windows.Forms.CheckBox flag80;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pb;
		private System.Windows.Forms.ListBox lstMS;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem iToolStripMenuItem;
	}
}

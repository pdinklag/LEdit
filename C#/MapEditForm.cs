using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LEdit
{   
    public partial class MapEditForm : Form
    {
        static int MaxUndo = 20;
        static int UndoClearTreshold = 40;

        MainForm parent;

        Collection<MapCharStartToolForm> charstartedit = new Collection<MapCharStartToolForm>();
        Collection<MapDoorToolForm> dooredit = new Collection<MapDoorToolForm>();
        Collection<MapWarpDoorToolForm> warpdooredit = new Collection<MapWarpDoorToolForm>();

        MapResizeForm resizeform;

        GradientBuffer gradient_blocks;

        enum Tool
        {
            Select, Paint, FillArea,
            CollisionTop, CollisionLeft, Rooms, Obstacles, RoomTransitions
        };

        enum View
        {
            Doors, WarpDoors, CharacterStarts, CutsceneTriggers, Grid
        };

        Tool tool = Tool.Select;

        FormWindowState wnd_state;
        int wnd_w, wnd_h;

        bool bWorldMap;
        L2WorldMap wmap;

        bool bDraw;
        int sx, sy;
        int sby;
        L2Map map;

        int selx, sely;
        int selw, selh;

        L2Map.L2MapTile[,,] clipboard;
        int cpw, cph;

        int selblock;
        int editlayer;

        bool bDisableRightMouseDrag;

        L2Map.DrawInfo drawinfo = new L2Map.DrawInfo();

        enum UndoAction
        {
            None, Paint, Paste
        };

        struct UndoStruct
        {
            public UndoAction action;
            public int layer, x, y, w, h;

            public short block;

            public L2Map.L2MapTile[, ,] clipboard;
        }
        Stack undo = new Stack(MaxUndo);

        public MapEditForm(MainForm ParentForm)
        {
            InitializeComponent();

            drawinfo.layer1 = true;
            drawinfo.layer2 = true;
            drawinfo.collision = false;
            drawinfo.roomborders = false;
            drawinfo.obstacles = false;
            drawinfo.doors = false;
            drawinfo.warpdoors = false;
            drawinfo.grid = false;

            cbMaps.Items.Clear();

            for (int i = 2; i < L2MapHeader.Count; i++)
                cbMaps.Items.Add("0x" + i.ToString("X2") + " - " + L2MapHeader.Get(i).Name.Value);

            cbMaps.SelectedIndex = 0;

            sx = 0; sy = 0;
            parent = ParentForm;
            bDraw = false;

            selx = -1; sely = -1;

            gradient_blocks = new GradientBuffer(
                pbb.Width, Screen.PrimaryScreen.Bounds.Height,
                SystemColors.GradientActiveCaption,
                SystemColors.ActiveCaption,
                GradientBuffer.GradientDirection.Vertical);

            wnd_w = Width;
            wnd_h = Height;
            wnd_state = WindowState;
        }

        public void GetSelection(out int x1, out int y1)
        {
            x1 = selx;
            y1 = sely;
        }

        public void GetSelection(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = selx;
            y1 = sely;

            x2 = selx + selw;
            y2 = sely + selh;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            undo.Clear();

            for (int i = 0; i < warpdooredit.Count; i++)
                warpdooredit[i].Close();
            warpdooredit.Clear();

            for (int i = 0; i < dooredit.Count; i++)
                dooredit[i].Close();
            dooredit.Clear();

            for (int i = 0; i < charstartedit.Count; i++)
                charstartedit[i].Close();
            charstartedit.Clear();

            bWorldMap = false;
            wmap = null;

            map = new L2Map(cbMaps.SelectedIndex + 2);
            map.PreDrawBlocks();

            //Window stuff
            pb_InitScrollBar();
            hsMap.Value = 0;
            vsMap.Value = 0;

            pbb_InitScrollBar();
            vsB.Value = 0;

            toolStripStatusLabel2.Text = map.Header.Name.Value;
            bsetinfo.Text = "B 0x" + map.Header.BlockSetFile.ToString("X3");
            setinfo.Text = "T 0x" + map.Header.TileSetFile.ToString("X3");
            sizeInfo.Text = map.Width + "x" + map.Height;

            bDraw = true;
            pb.Refresh();
            pbb.Refresh();
        }

        private void pb_InitScrollBar()
        {
            if (map != null)
            {
                int max_x = pb.Width / 16;

                if (max_x > map.Width)
                    hsMap.Enabled = false;
                else
                {
                    hsMap.Enabled = true;
                    hsMap.Maximum = map.Width - max_x;
                }

                int max_y = pb.Height / 16;

                if (max_y > map.Height)
                    vsMap.Enabled = false;
                else
                {
                    vsMap.Enabled = true;
                    vsMap.Maximum = map.Height - max_y;
                }
            }
            else if (wmap != null)
            {
                int max_x = pb.Width / 32;

                if (max_x > wmap.Width)
                    hsMap.Enabled = false;
                else
                {
                    hsMap.Enabled = true;
                    hsMap.Maximum = wmap.Width - max_x;
                }

                int max_y = pb.Height / 32;

                if (max_y > wmap.Height)
                    vsMap.Enabled = false;
                else
                {
                    vsMap.Enabled = true;
                    vsMap.Maximum = wmap.Height - max_y;
                }
            }
        }

        private void pbb_InitScrollBar()
        {
            if (bDraw && map != null)
            {
                int blocks_y = map.BlockSet.Blocks.Count / 12 + ((map.BlockSet.Blocks.Count % 12 == 0) ? 0 : 1);
                int max_blocks_y = pbb.Height >> 4;

                if (max_blocks_y >= blocks_y)
                    vsB.Enabled = false;
                else
                {
                    vsB.Enabled = true;
                    vsB.Maximum = blocks_y - max_blocks_y;
                }
            }
            else if (bDraw && bWorldMap && wmap != null)
            {
                int blocks_y = L2WorldMapBigBlock.Count / 6 + ((L2WorldMapBigBlock.Count % 6 == 0) ? 0 : 1);
                int max_blocks_y = pbb.Height >> 5;

                if (max_blocks_y >= blocks_y)
                    vsB.Enabled = false;
                else
                {
                    vsB.Enabled = true;
                    vsB.Maximum = blocks_y - max_blocks_y;
                }
            }
        }

        private void TestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.mapEditClosed();
        }

        private void Copy()
        {
            cpw = selw;
            cph = selh;

            clipboard = new L2Map.L2MapTile[2, cpw, cph];

            for (int y = 0; y < cph; y++)
            {
                for (int x = 0; x < cpw; x++)
                {
                    clipboard[0, x, y] = map.GetTile(0, selx + x, sely + y);
                    clipboard[1, x, y] = map.GetTile(1, selx + x, sely + y);
                }
            }

            einfügenToolStripMenuItem.Enabled = true;
            einfügenToolStripMenuItem1.Enabled = true;
        }

        private void Paste()
        {
            UndoStruct u = new UndoStruct();
            u.action = UndoAction.Paste;
            u.x = selx;
            u.y = sely;
            u.w = cpw;
            u.h = cph;

            u.clipboard = new L2Map.L2MapTile[2, cpw, cph];

            for (int y = 0; y < cph; y++)
            {
                for (int x = 0; x < cpw; x++)
                {
                    u.clipboard[0, x, y] = map.GetTile(0, selx + x, sely + y);
                    u.clipboard[1, x, y] = map.GetTile(1, selx + x, sely + y);

                    map.SetTile(0, selx + x, sely + y, clipboard[0, x, y]);
                    map.SetTile(1, selx + x, sely + y, clipboard[1, x, y]);
                }
            }

            AddUndo(u);

            pb.Refresh();
        }

        private void pbb_Paint(object sender, PaintEventArgs e)
        {
            //Gradient
            gradient_blocks.Draw(e.Graphics,0,0);

            if (bDraw && map != null)
            {
                L2MapBlockSet bset = map.BlockSet;

                int bx = 0;
                int by = 0;

                Pen grid_pen = new Pen(Color.FromArgb(0x80, Color.White));

                int start = vsB.Value * 12;
                int end = start + (12 * ((pbb.Height >> 4) + 1));

                if (end > bset.Blocks.Count)
                    end = bset.Blocks.Count;

                for (int i = start; i < end; i++)
                {
                    int x = bx << 4;
                    int y = by << 4; //(by - (int)vsB.Value) << 4;

                    bset.Blocks[i].Draw(
                        e.Graphics,
                        map.TileSet,
                        map.Palettes,
                        x + 1, y + 1);

                    e.Graphics.DrawLine(
                        grid_pen, x, y, x + 15, y);

                    e.Graphics.DrawLine(
                        grid_pen, x, y, x, y + 15);

                    if (i == selblock)
                    {
                        Color col = Color.FromArgb(0x40, Color.White);

                        e.Graphics.FillRectangle(
                            new SolidBrush(col),
                            new Rectangle(
                                x, y,
                                16, 16));

                        e.Graphics.DrawRectangle(
                            new Pen(Color.White),
                            new Rectangle(
                                x, y,
                                16, 16));
                    }

                    if (++bx >= 12)
                    {
                        bx = 0;
                        ++by;
                    }
                }
            }
            else if (bDraw && bWorldMap && wmap != null)
            {
                int bx = 0;
                int by = 0;

                Pen grid_pen = new Pen(Color.FromArgb(0x80, Color.White));

                int start = vsB.Value * 6;
                int end = start + (6 * ((pbb.Height >> 5) + 1));

                if (end > L2WorldMapBigBlock.Count)
                    end = L2WorldMapBigBlock.Count;

                for (int i = start; i < end; i++)
                {
                    //Console.WriteLine("0x" + i.ToString("X4"));
                    int x = bx << 5;
                    int y = by << 5;//(by - (int)vsB.Value) << 5;

                    L2WorldMapBigBlock.Get(i).Draw(
                        e.Graphics,
                        wmap.TileSet,
                        wmap.Palettes,
                        x + 1, y + 1);

                    e.Graphics.DrawLine(
                        grid_pen, x, y, x + 31, y);

                    e.Graphics.DrawLine(
                        grid_pen, x, y, x, y + 31);

                    if (++bx >= 6)
                    {
                        bx = 0;
                        ++by;
                    }
                }
            }

            //Control outline
            e.Graphics.DrawRectangle(new Pen(SystemColors.ControlDark),
                new Rectangle(0, 0, pbb.Width - 1, pbb.Height - 1));
        }

        private void pb_Paint(object sender, PaintEventArgs e)
        {
            if (bDraw && map != null)
            {
                //Map
                //map.Draw(e.Graphics, 0, 0, sx, sy, 24, 16, 1, drawinfo);
                map.Draw(e.Graphics, 0, 0, sx, sy, (pb.Width >> 4) + 1, (pb.Height >> 4) + 1, 1, drawinfo);

                //Selection
                int fsx = (selx - sx) << 4;
                int fsy = (sely - sy) << 4;

                if (tool == Tool.Select &&
                    (fsx + (selw << 4)) >= 0 && fsx < pb.Width &&
                    (fsy + (selh << 4)) >= 0 && fsy < pb.Height)
                {
                    Color col = Color.FromArgb(0x40, Color.Aqua);

                    e.Graphics.FillRectangle(
                        new SolidBrush(col),
                        new Rectangle(
                            fsx, fsy,
                            selw << 4,
                            selh << 4));

                    e.Graphics.DrawRectangle(
                        new Pen(Color.Aqua),
                        new Rectangle(
                            fsx, fsy,
                            selw << 4,
                            selh << 4));
                }

                //Characters
                for (int i = 0; i < map.Header.Characters.Count; i++)
                {
                    L2MapHeader.Character c = map.Header.Characters[i];
                    L2MapSprite ms = L2MapSprite.Get(c.map_sprite);
                    L2MapHeader.CharacterStart p = c.start;

                    if (ms != null && p != null)
                    {
                        int x = (p.x1 - sx) << 4;
                        int y = (p.y1 - sy) << 4;

                        //vertical x2 flag
                        if ((ms.Flags & 0x1) != 0) y -= 16;

                        if (
                            x >= -16 && x < pb.Width &&
                            y >= -16 && y < pb.Height)
                            ms.DrawFrame(e.Graphics, 0, x, y);
                    }
                }
            }
            else if (bDraw && bWorldMap && wmap != null)
            {
                //WorldMap
                wmap.Draw(e.Graphics, 0, 0, sx, sy, (pb.Width >> 4) + 1, (pb.Height >> 4) + 1, 1, drawinfo);

                //Selection
                int fsx = (selx - sx - sx) << 4;
                int fsy = (sely - sy - sy) << 4;

                if (tool == Tool.Select &&
                    fsx >= 0 && fsx < pb.Width &&
                    fsy >= 0 && fsy < pb.Height)
                {
                    Color col = Color.FromArgb(0x40, Color.Aqua);

                    e.Graphics.FillRectangle(
                        new SolidBrush(col),
                        new Rectangle(
                            fsx, fsy,
                            16, 16));

                    e.Graphics.DrawRectangle(
                        new Pen(Color.Aqua),
                        new Rectangle(
                            fsx, fsy,
                            16, 16));
                }
            }
        }

        private bool pb_IsSelectedAt(int x, int y)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            return (x >= selx && 
                x < selx + selw &&
                y >= sely && 
                y < sely + selh);
        }

        private void pb_SelectBlockAt(int x, int y, bool drag)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            if (bWorldMap)
            {
                x += sx;
                y += sy;
            }

            pb_SelectBlock(x, y, drag);
        }

        private void pb_SelectBlock(int x, int y, bool drag)
        {
            int w = bWorldMap ? (wmap.Width << 1) : map.Width;
            int h = bWorldMap ? (wmap.Height << 1) : map.Height;

            if (x >= 0 && y >= 0 && x < w && y < h)
            {
                if (drag && !bWorldMap)
                {
                    if (x > selx)
                        selw = x + 1 - selx;
                    else
                    {
                        selw += selx - x;
                        selx = x;
                    }

                    if (y > sely)
                        selh = y + 1 - sely;
                    else
                    {
                        selh += sely - y;
                        sely = y;
                    }
                }
                else
                {
                    selx = x;
                    sely = y;

                    selw = 1;
                    selh = 1;
                }

                pb.Refresh();
            }
            else
            {
                //Do nothing
            }
        }

        private void pb_SetCollisionAt(int x, int y, bool left, bool collision)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            if(left)
                map.SetCollisionLeft(x, y, collision);
            else
                map.SetCollisionTop(x, y, collision);

            pb.Refresh();
        }

        private void pb_SetRoomBorderAt(int x, int y, bool border)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            map.SetRoomBorder(x, y, border);

            pb.Refresh();
        }

        private void pb_SetRoomTransitionAt(int x, int y, bool door)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            map.SetDoor(x, y, door);

            pb.Refresh();
        }

        private void pb_SetObstacleAt(int x, int y, bool obstacle)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            map.SetObstacle(x, y, obstacle);

            pb.Refresh();
        }

        private void AddUndo(UndoStruct u)
        {
            undo.Push(u);
            rückgängigToolStripMenuItem.Enabled = true;

            if (undo.Count >= UndoClearTreshold)
            {
                object[] old = undo.ToArray();

                undo.Clear();
                for (int i = MaxUndo - 1; i >= 0; i--)
                    undo.Push(old[i]);
            }
        }

        private void pb_PaintBlockAt(int x, int y)
        {
            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            UndoStruct u;
            bool can_add_undo = true;

            if (undo.Count > 0)
            {
                u = (UndoStruct)undo.Peek();
                can_add_undo = (u.x != x || u.y != y || u.layer != editlayer);
            }

            if (can_add_undo)
            {
                u = new UndoStruct();
                u.action = UndoAction.Paint;
                u.layer = editlayer;
                u.x = x;
                u.y = y;
                u.block = map.GetBlock(editlayer, x, y);
                AddUndo(u);
            }

            map.SetBlock(editlayer, x, y, (short)selblock);

            pb.Refresh();
        }

        private void pb_GrabBlockAt(int x, int y)
        {
            if (bWorldMap)
                return;

            x = (x >> 4) + sx;
            y = (y >> 4) + sy;

            if (x < map.Width && y < map.Height)
            {
                selblock = map.GetBlock(editlayer, x, y);

                pbb.Refresh();
            }
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (bDraw && map != null && e.Button != MouseButtons.None)
            {
                if (tool == Tool.Paint)
                {
                    //if (e.Button == MouseButtons.Left) //What to do with this?
                    //    pb_SelectBlockAt(e.X, e.Y);
                    if (e.Button == MouseButtons.Right) //Paint
                        pb_PaintBlockAt(e.X, e.Y);
                    else if (e.Button == MouseButtons.Middle) //Grab block for painting
                        pb_GrabBlockAt(e.X, e.Y);
                }
                else if (tool == Tool.CollisionTop)
                {
                    if (e.Button == MouseButtons.Left)
                        pb_SetCollisionAt(e.X, e.Y, false, true);
                    else if (e.Button == MouseButtons.Right)
                        pb_SetCollisionAt(e.X, e.Y, false, false);
                }
                else if (tool == Tool.CollisionLeft)
                {
                    if (e.Button == MouseButtons.Left)
                        pb_SetCollisionAt(e.X, e.Y, true, true);
                    else if (e.Button == MouseButtons.Right)
                        pb_SetCollisionAt(e.X, e.Y, true, false);
                }
                else if (tool == Tool.Rooms)
                {
                    if (e.Button == MouseButtons.Left)
                        pb_SetRoomBorderAt(e.X, e.Y, true);
                    else if (e.Button == MouseButtons.Right)
                        pb_SetRoomBorderAt(e.X, e.Y, false);
                }
                else if (tool == Tool.Obstacles)
                {
                    if (e.Button == MouseButtons.Left)
                        pb_SetObstacleAt(e.X, e.Y, true);
                    else if (e.Button == MouseButtons.Right)
                        pb_SetObstacleAt(e.X, e.Y, false);
                }
                else if (tool == Tool.RoomTransitions)
                {
                    if (e.Button == MouseButtons.Left)
                        pb_SetRoomTransitionAt(e.X, e.Y, true);
                    else if (e.Button == MouseButtons.Right)
                        pb_SetRoomTransitionAt(e.X, e.Y, false);
                }
                else if(tool == Tool.Select)
                {
                    if (e.Button == MouseButtons.Middle)
                        pb_GrabBlockAt(e.X, e.Y);
                    else if(e.Button == MouseButtons.Left)
                        pb_SelectBlockAt(e.X, e.Y, false);
                    else if (e.Button == MouseButtons.Right)
                    {
                        if (!pb_IsSelectedAt(e.X, e.Y))
                        {
                            bDisableRightMouseDrag = false;
                            pb_SelectBlockAt(e.X, e.Y, false);
                        }
                        else
                            bDisableRightMouseDrag = true;
                    }
                }
            }
            else if (bDraw && bWorldMap && wmap != null && e.Button != MouseButtons.None)
            {
                if(tool == Tool.Select)
                {
                    //if (e.Button == MouseButtons.Middle)
                    //    pb_GrabBlockAt(e.X, e.Y);
                    //else
                    pb_SelectBlockAt(e.X, e.Y, false);
                }
            }
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            if (bDraw && map != null)
            {
                if (tool == Tool.Select && e.Button == MouseButtons.Right)
                    tilecontext.Show(pb, e.X, e.Y);
            }
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            if (bDraw && (map != null || (bWorldMap && wmap != null)))
            {
                bool bDoMouseDown = (e.Button != MouseButtons.None); 

                int x = (e.X >> 4) + sx;
                int y = (e.Y >> 4) + sy;

                if (bWorldMap)
                {
                    x += sx;
                    y += sy;
                }

                int w = bWorldMap ? (wmap.Width << 1) : map.Width;
                int h = bWorldMap ? (wmap.Height << 1) : map.Height;

                if (x >= 0 && y >= 0 && x < w && y < h)
                {
                    toolStripStatusLabel1.Text = "(" + x + "," + y + ")";

                    if (!bWorldMap)
                    {
                        layer1info.Text = "L1: 0x" + map.Layers[0, x, y].block.ToString("X3");
                        layer2info.Text = "L2: 0x" + map.Layers[1, x, y].block.ToString("X3");
                    }
                    else
                    {
                        layer1info.Text = "0x" + wmap.Tiles[x >> 1, y >> 1].block.ToString("X3");
                        layer2info.Text = string.Empty;
                    }

                    if (e.Button != MouseButtons.None && tool == Tool.Select)
                    {
                        bDoMouseDown = false;

                        if (e.Button == MouseButtons.Middle)
                            pb_GrabBlockAt(e.X, e.Y);
                        else if(e.Button == MouseButtons.Left)
                            pb_SelectBlockAt(e.X, e.Y, true);
                        else if (e.Button == MouseButtons.Right)
                        {
                            if(!bDisableRightMouseDrag)
                                pb_SelectBlockAt(e.X, e.Y, true);
                        }
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = string.Empty;
                    layer1info.Text = string.Empty;
                    layer2info.Text = string.Empty;
                }

                if(bDoMouseDown)
                    pb_MouseDown(sender, e);
            }
        }

        private void hsMap_ValueChanged(object sender, EventArgs e)
        {
            sx = hsMap.Value;
            pb.Refresh();
        }

        private void pb_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            layer1info.Text = string.Empty;
            layer2info.Text = string.Empty;
        }

        private void vsMap_ValueChanged(object sender, EventArgs e)
        {
            sy = vsMap.Value;
            pb.Refresh();
        }

        private void vsB_ValueChanged(object sender, EventArgs e)
        {
            sby = vsB.Value;
            pbb.Refresh();
        }

        private void pbb_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X >> 4;
            int y = (e.Y >> 4) + sby;

            selblock = y * 12 + x;

            pbb.Refresh();
        }


        private void pbb_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None)
                pbb_MouseDown(sender, e);
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            map.Save();
        }

        private void SaveToROM()
        {
            if (map != null)
            {
                if (
                    MessageBox.Show(
                    "Sicher?",
                    "Map in ROM einfügen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    map.SaveToROM();

                    MessageBox.Show(
                        "Map erfolgreich ins Spiel eingefügt!",
                        "Erfolg",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void inROMEinfügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveToROM();
        }

        private void Undo()
        {
            if (undo.Count > 0)
            {
                UndoStruct u = (UndoStruct)undo.Pop();
                if (u.action == UndoAction.Paint)
                {
                    map.SetBlock(u.layer, u.x, u.y, u.block);
                    pb.Refresh();
                }
                else if (u.action == UndoAction.Paste)
                {
                    for (int y = 0; y < u.h; y++)
                    {
                        for (int x = 0; x < u.w; x++)
                        {
                            map.SetTile(0, u.x + x, u.y + y, u.clipboard[0, x, y]);
                            map.SetTile(1, u.x + x, u.y + y, u.clipboard[1, x, y]);
                        }
                    }

                    pb.Refresh();
                }
            }

            if (undo.Count == 0)
                rückgängigToolStripMenuItem.Enabled = false;
        }

        private void rückgängigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void SetEditLayer(int layer)
        {
            editlayer = layer;

            toolStripButton2.CheckState = (editlayer == 0) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton3.CheckState = (editlayer == 1) ? CheckState.Checked : CheckState.Unchecked;
        }

        private void ToggleView(View view)
        {
            switch (view)
            {
                case View.Doors:
                    drawinfo.doors = !drawinfo.doors;
                    break;

                case View.WarpDoors:
                    drawinfo.warpdoors = !drawinfo.warpdoors;
                    break;

                case View.CharacterStarts:
                    drawinfo.characterstarts = !drawinfo.characterstarts;
                    break;

                case View.CutsceneTriggers:
                    drawinfo.cutscenetriggers = !drawinfo.cutscenetriggers;
                    break;

                case View.Grid:
                    drawinfo.grid = !drawinfo.grid;
                    break;
            }

            toolStripButton11.CheckState = drawinfo.doors ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton14.CheckState = drawinfo.warpdoors ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton16.CheckState = drawinfo.characterstarts ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton17.CheckState = drawinfo.cutscenetriggers ? CheckState.Checked : CheckState.Unchecked;

            toolStripButton6.CheckState = drawinfo.grid ? CheckState.Checked : CheckState.Unchecked;
            gitternetzlinienToolStripMenuItem.CheckState = toolStripButton6.CheckState;

            pb.Refresh();
        }

        private void SetTool(Tool new_tool)
        {
            tool = new_tool;

            toolStripButton1.CheckState = (tool == Tool.Select) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton7.CheckState = (tool == Tool.Paint) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton10.CheckState = (tool == Tool.FillArea) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton4.CheckState = (tool == Tool.CollisionTop) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton5.CheckState = (tool == Tool.CollisionLeft) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton12.CheckState = (tool == Tool.Rooms) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton13.CheckState = (tool == Tool.Obstacles) ? CheckState.Checked : CheckState.Unchecked;
            toolStripButton15.CheckState = (tool == Tool.RoomTransitions) ? CheckState.Checked : CheckState.Unchecked;

            toolStripButton2.Enabled =
                (tool == Tool.Select || tool == Tool.Paint || tool == Tool.FillArea);

            toolStripButton3.Enabled = toolStripButton2.Enabled;

            drawinfo.collision = (tool == Tool.CollisionTop || tool == Tool.CollisionLeft);
            drawinfo.roomborders = (tool == Tool.Rooms);
            drawinfo.obstacles = (tool == Tool.Obstacles);
            drawinfo.doorways = (tool == Tool.RoomTransitions);

            pb.Refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Select);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SetEditLayer(0);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SetEditLayer(1);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SetTool(Tool.CollisionTop);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            SetTool(Tool.CollisionLeft);
        }

        private void auswählenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Select);
        }

        private void zeichnenLayer1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetEditLayer(0);
        }

        private void zeichnenLayer2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetEditLayer(1);
        }

        private void horizontaleKollisionenBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.CollisionTop);
        }

        private void vertikaleKollisioneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.CollisionLeft);
        }

        private void MapEditForm_Resize(object sender, EventArgs e)
        {
            if (wnd_state != FormWindowState.Minimized &&
                WindowState != FormWindowState.Minimized)
            {
                int x = Width - wnd_w;
                int y = Height - wnd_h;

                pbb.Height += y;

                grpBlocks.Left += x;
                grpBlocks.Height += y;
                vsB.Height += y;

                grpMap.Width += x;
                grpMap.Height += y;

                vsMap.Left += x;
                vsMap.Height += y;

                hsMap.Top += y;
                hsMap.Width += x;

                pb.Width += x;
                pb.Height += y;

                wnd_w = Width;
                wnd_h = Height;

                pb_InitScrollBar();
                pbb_InitScrollBar();

                pb.Refresh();
                pbb.Refresh();
            }
            wnd_state = WindowState;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ToggleView(View.Grid);
        }

        private void gitternetzlinienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleView(View.Grid);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Paint);
        }

        private void zeichnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Paint);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            map.Save();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            SaveToROM();
        }

        private void pbb_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            SetTool(Tool.FillArea);
        }

        private void flächeFüllenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.FillArea);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            undo.Clear();

            map = null;
            bWorldMap = true;
            bDraw = true;

            wmap = L2ROM.GetWorld();

            pb_InitScrollBar();
            pbb_InitScrollBar();

            pb.Refresh();
            pbb.Refresh();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Rooms);
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Obstacles);
        }

        private void raumgrenzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Rooms);
        }

        private void hindernisseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.Obstacles);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void einfügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void kopierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void einfügenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void größeÄndernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map != null)
            {
                resizeform = new MapResizeForm(this, map.Width, map.Height);
                this.Enabled = false;

                resizeform.Show();
            }
        }

        public void ResizeFormClosed(int w, int h)
        {
            this.Enabled = true;

            if (w > 0 && h > 0)
            {
                map.Resize(w, h);

                pb_InitScrollBar();
                pb.Refresh();
            }
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            ToggleView(View.Doors);
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            ToggleView(View.WarpDoors);
        }

        private void raumübergängeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetTool(Tool.RoomTransitions);
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            SetTool(Tool.RoomTransitions);
        }

        private void CharStartCreate()
        {
            if (map != null)
            {
                if (CharStartGetSelected() != null)
                {
                    MessageBox.Show("Hier existiert bereits ein Charakter-Startpunkt!",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                byte id = 1;
                for (int i = 0; i < map.Header.CharacterStarts.Count; i++)
                {
                    if (map.Header.CharacterStarts[i].id == id)
                        id++;
                }

                L2MapHeader.CharacterStart charstart = new L2MapHeader.CharacterStart();

                charstart.id = id;
                charstart.x1 = (byte)selx;
                charstart.y1 = (byte)sely;

                charstart.move_area_x1 = (byte)selx;
                charstart.move_area_y1 = (byte)sely;
                charstart.move_area_x2 = (byte)(selx + 1);
                charstart.move_area_y2 = (byte)(sely + 1);

                charstart.unknown = 0;

                map.Header.CharacterStarts.Add(charstart);

                pb.Refresh();

                CharStartEditOpen(charstart);
            }
        }

        public void CharStartEditOpen(L2MapHeader.CharacterStart charstart)
        {
            for (int i = 0; i < charstartedit.Count; i++)
            {
                if (charstartedit[i].CharacterStart == charstart)
                {
                    charstartedit[i].BringToFront();
                    return;
                }
            }

            MapCharStartToolForm frm = new MapCharStartToolForm(this, charstart);
            charstartedit.Add(frm);
            frm.Show();
        }

        public void CharStartEditClosed(MapCharStartToolForm frm)
        {
            charstartedit.Remove(frm);
        }

        private L2MapHeader.CharacterStart CharStartGetSelected()
        {
            if (map != null)
            {
                for (int i = 0; i < map.Header.CharacterStarts.Count; i++)
                {
                    L2MapHeader.CharacterStart charstart = map.Header.CharacterStarts[i];

                    if (selx == charstart.x1 && sely == charstart.y1)
                        return charstart;
                }
            }
            return null;
        }

        private void CharStartEdit()
        {
            L2MapHeader.CharacterStart charstart = CharStartGetSelected();

            if (charstart != null)
                CharStartEditOpen(charstart);
        }

        private void CharStartRemove()
        {
            L2MapHeader.CharacterStart charstart = CharStartGetSelected();

            if (charstart != null)
            {
                if (MessageBox.Show("Sicher?", "Charakter-Bereich entfernen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    map.Header.CharacterStarts.Remove(charstart);
                    pb.Refresh();
                }
            }
        }

        
        private L2MapHeader.Door DoorGetSelected()
        {
            if (map != null)
            {
                for (int i = 0; i < map.Header.Doors.Count; i++)
                {
                    L2MapHeader.Door door = map.Header.Doors[i];

                    if (selx + selw >= door.x1 && selx < door.x2 &&
                        sely + selh >= door.y1 && sely < door.y2)
                    {
                        return door;
                    }
                }
            }
            return null;
        }

        private void DoorEdit()
        {
            L2MapHeader.Door door = DoorGetSelected();

            if (door != null)
                DoorEditOpen(door);
        }

        private void DoorRemove()
        {
            L2MapHeader.Door door = DoorGetSelected();

            if (door != null)
            {
                if (MessageBox.Show("Sicher?", "Tür entfernen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    map.Header.Doors.Remove(door);
                    pb.Refresh();
                }
            }
        }

        private void DoorCreate()
        {
            if (map != null)
            {
                if (DoorGetSelected() != null)
                {
                    MessageBox.Show("Innerhalb dieser Fläche existiert bereits eine Tür!",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                byte id = 1;
                for (int i = 0; i < map.Header.Doors.Count; i++)
                {
                    if (map.Header.Doors[i].id == id)
                        id++;
                }

                L2MapHeader.Door door = new L2MapHeader.Door();

                door.id = id;
                door.x1 = (byte)selx;
                door.y1 = (byte)sely;
                door.x2 = (byte)(selx + selw);
                door.y2 = (byte)(sely + selh);

                door.area1_x1 = (byte)selx;
                door.area1_y1 = (byte)(sely + selh);
                door.area1_x2 = (byte)(selx + selw);
                door.area1_y2 = (byte)(sely + selh + 1);

                door.area2_x1 = 255;
                door.area2_y1 = 255;
                door.area2_x2 = 255;
                door.area2_y2 = 255;

                door.unknown1 = 0;
                door.unknown2 = 255;

                map.Header.Doors.Add(door);

                pb.Refresh();

                DoorEditOpen(door);
            }
        }

        public void DoorEditOpen(L2MapHeader.Door door)
        {
            for (int i = 0; i < dooredit.Count; i++)
            {
                if (dooredit[i].Door == door)
                {
                    dooredit[i].BringToFront();
                    return;
                }
            }

            MapDoorToolForm frm = new MapDoorToolForm(this, door);
            dooredit.Add(frm);
            frm.Show();
        }

        public void DoorEditClosed(MapDoorToolForm frm)
        {
            dooredit.Remove(frm);
        }

        private L2MapHeader.WarpDoor WarpDoorGetSelected()
        {
            if (map != null)
            {
                for (int i = 0; i < map.Header.WarpDoors.Count; i++)
                {
                    L2MapHeader.WarpDoor door = map.Header.WarpDoors[i];

                    if (selx + selw >= door.x1 && selx < door.x2 &&
                        sely + selh >= door.y1 && sely < door.y2)
                    {
                        return door;
                    }
                }
            }
            return null;
        }

        private void WarpDoorEdit()
        {
            L2MapHeader.WarpDoor door = WarpDoorGetSelected();

            if (door != null)
                WarpDoorEditOpen(door);
        }

        private void WarpDoorRemove()
        {
            L2MapHeader.WarpDoor door = WarpDoorGetSelected();

            if (door != null)
            {
                if (MessageBox.Show("Sicher?", "Warp-Tür entfernen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    map.Header.WarpDoors.Remove(door);
                    pb.Refresh();
                }
            }
        }

        private void WarpDoorCreate()
        {
            if (map != null)
            {
                if (WarpDoorGetSelected() != null)
                {
                    MessageBox.Show("Innerhalb dieser Fläche existiert bereits eine Warp-Tür!",
                        "Fehler",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                byte id = 1;
                for (int i = 0; i < map.Header.WarpDoors.Count; i++)
                {
                    if (map.Header.WarpDoors[i].id == id)
                        id++;
                }

                L2MapHeader.WarpDoor door = new L2MapHeader.WarpDoor();

                door.id = id;
                door.x1 = (byte)selx;
                door.y1 = (byte)sely;
                door.x2 = (byte)(selx + selw);
                door.y2 = (byte)(sely + selh);

                door.unknown = 0;

                door.dest_x = 0;
                door.dest_y = 0;
                door.dest_map = 255;

                map.Header.WarpDoors.Add(door);

                pb.Refresh();

                WarpDoorEditOpen(door);
            }
        }

        public void RefreshView()
        {
            pb.Refresh();
        }

        public void WarpDoorEditOpen(L2MapHeader.WarpDoor door)
        {
            for (int i = 0; i < warpdooredit.Count; i++)
            {
                if (warpdooredit[i].WarpDoor == door)
                {
                    warpdooredit[i].BringToFront();
                    return;
                }
            }

            MapWarpDoorToolForm frm = new MapWarpDoorToolForm(this, door);
            warpdooredit.Add(frm);
            frm.Show();
        }

        public void WarpDoorEditClosed(MapWarpDoorToolForm frm)
        {
            warpdooredit.Remove(frm);
        }

        private void türErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void warpTürErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void hiererstellenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoorCreate();
        }

        private void bearbeitenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoorEdit();
        }

        private void hiererstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WarpDoorCreate();
        }

        private void bearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WarpDoorEdit();
        }

        private void entfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WarpDoorRemove();
        }

        private void entfernenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DoorRemove();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            ToggleView(View.CharacterStarts);
        }

        private void erstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharStartCreate();
        }

        private void entToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharStartRemove();
        }

        private void bearbeitenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            CharStartEdit();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MakeRoom()
        {
            UndoStruct u = new UndoStruct();
            u.action = UndoAction.Paste;
            u.x = 0;
            u.y = 0;
            u.w = map.Width;
            u.h = map.Height;

            u.clipboard = new L2Map.L2MapTile[2, map.Width, map.Height];

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    u.clipboard[0, x, y] = map.GetTile(0, x, y);
                    u.clipboard[1, x, y] = map.GetTile(1, x, y);
                }
            }

            AddUndo(u);

            //map.BlockSet.MakeWallsAround(map, selx, sely);
            map.BlockSet.MakeRoom(map, selx, sely, selw, selh);
            pb.Refresh();
        }

        private void ConnectWalls()
        {
            UndoStruct u = new UndoStruct();
            u.action = UndoAction.Paste;
            u.x = 0;
            u.y = 0;
            u.w = map.Width;
            u.h = map.Height;

            u.clipboard = new L2Map.L2MapTile[2, map.Width, map.Height];

            for (int y = 0; y < map.Height; y++)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    u.clipboard[0, x, y] = map.GetTile(0, x, y);
                    u.clipboard[1, x, y] = map.GetTile(1, x, y);
                }
            }

            AddUndo(u);

            map.BlockSet.ConnectWalls(map, selx, sely);
            pb.Refresh();
        }

        private void raumErstellenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MakeRoom();
        }

        private void wändeVerbindenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectWalls();
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            ToggleView(View.CutsceneTriggers);
        }
    }
}

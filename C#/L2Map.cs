using System;
using System.Drawing;
using System.IO;

namespace LEdit
{
    public class L2Map
    {
        static Font id_font = new Font(FontFamily.GenericMonospace, 9.0f, FontStyle.Bold);

        public struct DrawInfo
        {
            public bool layer1;
            public bool layer2;

            public bool collision;
            public bool roomborders;
            public bool obstacles;
            public bool doorways;

            public bool doors;
            public bool warpdoors;
            public bool characterstarts;
            public bool cutscenetriggers;

            public bool grid;
        }

        public struct L2MapTile
        {
            public short block;
            public bool collision_top;
            public bool collision_left;
            public bool door;
            public bool roomborder;
            public bool higher_level;
            public bool obstacle;
        };

        SNESPalette[] pals = new SNESPalette[6];
        SNESTileset set;
        L2MapBlockSet bset;
        L2MapHeader header;

        byte width, height;
        L2MapTile[,,] layers;

        bool saved;

        public L2MapBlockSet BlockSet { get { return bset; } }
        public SNESTileset TileSet { get { return set; } }
        public SNESPalette[] Palettes { get { return pals; } }

        public byte Width { get { return width; } }
        public byte Height { get { return height; } }
        public L2MapTile[,,] Layers { get { return layers; } }
        public L2MapHeader Header { get { return header; } }

        public L2Map(int n) { FromID(n); }

        public void FromID(int n)
        {
            if (n > 1 && n < L2MapHeader.Count)
            {
                header = L2MapHeader.Get(n);

                //Load PaletteSet
                FileStream fs = new FileStream(L2ROM.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                fs.Seek(header.PalSetAddress, SeekOrigin.Begin);
                for (int i = 0; i < 6; i++)
                    pals[i] = new SNESPalette(br);

                br.Close();
                fs.Close();

                //Load BlockSet
                string FileName = L2FileManager.ExtractFile(header.BlockSetFile);

                bset = new L2MapBlockSet(FileName);
                bset.InitInfo(header.BlockSetFile);

                //Load TileSet
                FileName = L2FileManager.ExtractFile(header.TileSetFile);

                set = new SNESTileset(new FileInfo(FileName));

                //Load map file
                FileName = L2FileManager.ExtractFile(header.MapFile);

                ReadFile(FileName);
            }
        }

        public void Save()
        {
            if(header != null)
            {
                //Save map file
                string FileName = L2FileManager.GetFileName(header.MapFile);

                WriteFile(FileName);
            }
            saved = true;
        }

        public void SaveToROM()
        {
            if (header != null)
            {
                if (!saved) Save();

                L2FileManager.InsertFile(header.MapFile, string.Empty);

                header.SaveToROM();
            }
        }

        private void ReadFile(String FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte validate = br.ReadByte();
            if (validate == 0x02)    //Map sig
            {
                br.ReadByte();      //Unknown
                br.ReadUInt16();    //Unknown
                br.ReadUInt16();    //Unknown

                for (int l = 0; l < 2; l++)
                {
                    if (l == 0)
                    {
                        br.ReadUInt16();    //Unknown
                        width = br.ReadByte();
                        height = br.ReadByte();

                        layers = new L2MapTile[2, width, height];
                    }
                    else
                    {
                        br.ReadUInt16();    //Unknown
                        br.ReadByte();      //width
                        br.ReadByte();      //height
                    }

                    for (int y = 0; y < (int)height; y++)
                    {
                        for (int x = 0; x < (int)width; x++)
                        {
                            layers[l, x, y] = new L2MapTile();
                            layers[l, x, y].collision_top = false;
                            layers[l, x, y].collision_left = false;

                            layers[l, x, y].block = (short)br.ReadByte();
                            byte flags = br.ReadByte();

                            if ((flags & 0x01) != 0) layers[l, x, y].block |= 0x100;
                            if ((flags & 0x02) != 0) layers[l, x, y].block |= 0x200;
                            if ((flags & 0x04) != 0) layers[l, x, y].collision_top = true;
                            if ((flags & 0x08) != 0) layers[l, x, y].collision_left = true;
                            if ((flags & 0x10) != 0) layers[l, x, y].roomborder = true;
                            if ((flags & 0x20) != 0) layers[l, x, y].door = true;
                            if ((flags & 0x40) != 0) layers[l, x, y].higher_level = true;
                            if ((flags & 0x80) != 0) layers[l, x, y].obstacle = true;
                        }
                    }
                }
            }

            br.Close();
            fs.Close();

            saved = true;
        }

        private void WriteFile(String FileName)
        {
            FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write((byte)0x02);

            bw.Write((byte)0x00); //Unknown
            bw.Write((ushort)0x00); //Unknown
            bw.Write((ushort)0x00); //Unknown

            for (int l = 0; l < 2; l++)
            {
                if (l == 0)
                {
                    bw.Write((ushort)0x00); //Unknown
                    bw.Write((byte)width);
                    bw.Write((byte)height);
                }
                else
                {
                    bw.Write((ushort)0x00); //Unknown
                    bw.Write((byte)width);
                    bw.Write((byte)height);
                }

                for (int y = 0; y < (int)height; y++)
                {
                    for (int x = 0; x < (int)width; x++)
                    {
                        //layers[l, x, y].block = (short)br.ReadByte();
                        bw.Write((byte)(layers[l, x, y].block & 0xFF));

                        byte flags = 0; // br.ReadByte();

                        if ((layers[l, x, y].block & 0x100) != 0)
                            flags |= 0x01;
                        if ((layers[l, x, y].block & 0x200) != 0)
                            flags |= 0x02;
                        if (layers[l, x, y].collision_top)
                            flags |= 0x04;
                        if (layers[l, x, y].collision_left)
                            flags |= 0x08;
                        if (layers[l, x, y].roomborder)
                            flags |= 0x10;
                        if (layers[l, x, y].door)
                            flags |= 0x20;
                        if (layers[l, x, y].higher_level)
                            flags |= 0x40;
                        if (layers[l, x, y].obstacle)
                            flags |= 0x80;

                        bw.Write(flags);
                    }
                }
            }

            bw.Close();
            fs.Close();
        }

        public void Resize(int w, int h)
        {
            int oldw = width;
            int oldh = height;

            width = (byte)w;
            height = (byte)h;

            L2MapTile[, ,] newlayers = new L2MapTile[2, width, height];

            for (int l = 0; l < 2; l++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        if (x < oldw && y < oldh)
                            newlayers[l, x, y] = layers[l, x, y];
                        else
                        {
                            newlayers[l, x, y].block = 0;
                        }
                    }
                }
            }

            layers = null;
            layers = newlayers;
        }

        public L2MapTile GetTile(int layer, int x, int y)
        {
            if (layer >= 0 && layer < 2 &&
                x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[layer, x, y];
            else
                return new L2MapTile();
        }

        public void SetTile(int layer, int x, int y, L2MapTile tile)
        {
            if (layer >= 0 && layer < 2 &&
                x >= 0 && x < width &&
                y >= 0 && y < height)
                layers[layer, x, y] = tile;
        }

        public short GetBlock(int layer, int x, int y)
        {
            if (layer >= 0 && layer < 2 &&
                x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[layer, x, y].block;
            else
                return 0;
        }

        public void SetBlock(int layer, int x, int y, short block)
        {
            if (layer >= 0 && layer < 2 &&
                x >= 0 && x < width &&
                y >= 0 && y < height &&
                block >= 0 && block < bset.Blocks.Count)
            {
                layers[layer, x, y].block = block;
                bset.Blocks[block].PreDraw(set, pals);

                saved = false;
            }
        }

        public bool IsRoomBorder(int x, int y)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[1, x, y].roomborder;

            return false;
        }

        public void SetRoomBorder(int x, int y, bool value)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                layers[1, x, y].roomborder = value;
                saved = false;
            }
        }

        public bool IsObstacle(int x, int y)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[1, x, y].obstacle;

            return false;
        }

        public void SetObstacle(int x, int y, bool value)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                layers[1, x, y].obstacle = value;
                saved = false;
            }
        }

        public bool IsDoor(int x, int y)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[1, x, y].door;

            return false;
        }

        public void SetDoor(int x, int y, bool value)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                layers[1, x, y].door = value;
                saved = false;
            }
        }

        public bool IsCollisionTop(int x, int y)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[1, x, y].collision_top;

            return false;
        }

        public void SetCollisionTop(int x, int y, bool value)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                layers[1, x, y].collision_top = value;
                saved = false;
            }
        }

        public bool IsCollisionLeft(int x, int y)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
                return layers[1, x, y].collision_left;

            return false;
        }

        public void SetCollisionLeft(int x, int y, bool value)
        {
            if (x >= 0 && x < width &&
                y >= 0 && y < height)
            {
                layers[1, x, y].collision_left = value;
                saved = false;
            }
        }

        public void PreDrawBlocks()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (layers[0, x, y].block != 0)
                        bset.Blocks[layers[0, x, y].block].PreDraw(set, pals);

                    if (layers[1, x, y].block != 0)
                        bset.Blocks[layers[1, x, y].block].PreDraw(set, pals);
                }
            }
        }

        public void Draw(Graphics dst, int dx, int dy, int mx, int my, int mw, int mh, int zoom, DrawInfo draw)
        {
            if (zoom < 1) zoom = 1;
            if (mw == 0) mw = (int)width;
            if (mh == 0) mh = (int)height;

            Pen grid_pen = new Pen(Color.FromArgb(0x40, Color.White));

            //Layers
            for (int y = my; (y < my + mh) && (y < height); y++)
            {
                for (int x = mx; (x < mx + mw) && (x < width); x++)
                {
                    int final_x = dx + zoom * ((x - mx) << 4);
                    int final_y = dy + zoom * ((y - my) << 4);

                    if (draw.layer1)
                    {
                        if (layers[0, x, y].block != 0)
                            bset.Blocks[layers[0, x, y].block].Draw(
                            dst, set, pals,
                            final_x,
                            final_y,
                            zoom);
                    }

                    if (draw.layer2)
                    {
                        if (layers[1, x, y].block != 0)
                            bset.Blocks[layers[1, x, y].block].Draw(
                            dst, set, pals,
                            final_x,
                            final_y,
                            zoom);
                    }

                    if (draw.grid)
                    {
                        dst.DrawLine(grid_pen,
                            final_x, 
                            final_y,
                            final_x + (zoom << 4) - 1,
                            final_y);

                        dst.DrawLine(grid_pen,
                            final_x,
                            final_y,
                            final_x,
                            final_y + (zoom << 4) - 1);
                    }

                    if (draw.collision)
                    {
                        if (layers[1, x, y].collision_top)
                            dst.DrawLine(
                                new Pen(Color.Red),
                                final_x,
                                final_y,
                                final_x + (zoom << 4),
                                final_y);

                        if (layers[1, x, y].collision_left)
                            dst.DrawLine(
                                new Pen(Color.Red),
                                final_x,
                                final_y,
                                final_x,
                                final_y + (zoom << 4));
                    }

                    if (draw.doorways && layers[1, x, y].door)
                    {
                        dst.DrawLine(
                            new Pen(Color.LightGreen),
                            final_x,
                            final_y,
                            final_x + (zoom << 4),
                            final_y + (zoom << 4));

                        dst.DrawLine(
                            new Pen(Color.LightGreen),
                            final_x + (zoom << 4),
                            final_y,
                            final_x,
                            final_y + (zoom << 4));
                    }

                    if (draw.roomborders && layers[1, x, y].roomborder)
                    {
                        dst.DrawLine(
                            new Pen(Color.Blue),
                            final_x,
                            final_y,
                            final_x + (zoom << 4),
                            final_y + (zoom << 4));

                        dst.DrawLine(
                            new Pen(Color.Blue),
                            final_x + (zoom << 4),
                            final_y,
                            final_x,
                            final_y + (zoom << 4));
                    }

                    if (draw.obstacles && layers[1, x, y].obstacle)
                    {
                        dst.DrawLine(
                            new Pen(Color.Yellow),
                            final_x + (zoom << 3),
                            final_y,
                            final_x + (zoom << 3),
                            final_y + (zoom << 4));

                        dst.DrawLine(
                            new Pen(Color.Yellow),
                            final_x,
                            final_y + (zoom << 3),
                            final_x + (zoom << 4),
                            final_y + (zoom << 3));
                    }
                }
            }

            //if (icons)
            //{


            //Doors
            if (draw.doors)
            {
                for (int i = 0; i < header.Doors.Count; i++)
                {
                    DrawArea(dst, header.Doors[i], Color.Red, dx, dy, mx, my, mw, mh, zoom);

                    DrawArea(dst,
                        header.Doors[i].area1_x1,
                        header.Doors[i].area1_y1,
                        header.Doors[i].area1_x2,
                        header.Doors[i].area1_y2,
                        Color.Yellow, dx, dy, mx, my, mw, mh, zoom);

                    DrawArea(dst,
                        header.Doors[i].area2_x1,
                        header.Doors[i].area2_y1,
                        header.Doors[i].area2_x2,
                        header.Doors[i].area2_y2,
                        Color.LightGreen, dx, dy, mx, my, mw, mh, zoom);

                    DrawID(dst, header.Doors[i], Color.Red, dx, dy, mx, my, mw, mh, zoom);
                }
            }

            //WarpDoors
            if (draw.warpdoors)
            {
                for (int i = 0; i < header.WarpDoors.Count; i++)
                {
                    DrawArea(dst, header.WarpDoors[i], Color.BlueViolet, dx, dy, mx, my, mw, mh, zoom);
                    DrawID(dst, header.WarpDoors[i], Color.BlueViolet, dx, dy, mx, my, mw, mh, zoom);
                }
            }

            //CharacterStarts
            if (draw.characterstarts)
            {
                for (int i = 0; i < header.CharacterStarts.Count; i++)
                {
                    DrawArea(dst,
                        header.CharacterStarts[i].move_area_x1,
                        header.CharacterStarts[i].move_area_y1,
                        header.CharacterStarts[i].move_area_x2,
                        header.CharacterStarts[i].move_area_y2,
                        Color.Orange, dx, dy, mx, my, mw, mh, zoom);

                    DrawArea(dst, header.CharacterStarts[i], Color.Yellow, dx, dy, mx, my, mw, mh, zoom);
                }

                for (int i = 0; i < header.CharacterStarts.Count; i++)
                    DrawID(dst, header.CharacterStarts[i], Color.Yellow, dx, dy, mx, my, mw, mh, zoom);
            }

            //CutsceneTriggers
            if (draw.cutscenetriggers)
            {
                for (int i = 0; i < header.CutsceneTriggers.Count; i++)
                {
                    DrawArea(dst, header.CutsceneTriggers[i], Color.SkyBlue, dx, dy, mx, my, mw, mh, zoom);
                    DrawID(dst, header.CutsceneTriggers[i], Color.SkyBlue, dx, dy, mx, my, mw, mh, zoom);
                }
            }
        }

        private void DrawID(Graphics dst, L2MapHeader.Spot spot, Color color, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            if (spot.x1 == 255 || spot.y1 == 255)
                return;

            DrawArea(
                dst,
                (byte)(spot.x1 + 1), spot.y1,
                (byte)(spot.x1 + 3), (byte)(spot.y1 + 1),
                Color.Black,
                dx, dy,
                mx, my, mw, mh,
                zoom);

            dst.DrawString(
                "0x" + spot.id.ToString("X2"),
                id_font,
                new SolidBrush(color),
                dx + zoom * ((spot.x1 - mx + 1) << 4),
                dy + zoom * ((spot.y1 - my) << 4));
        }

        private void DrawArea(Graphics dst, L2MapHeader.Area area, Color basecolor, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            DrawArea(
                dst,
                area.x1,
                area.y1,
                area.x2,
                area.y2,
                basecolor,
                dx,
                dy,
                mx,
                my,
                mw,
                mh,
                zoom);
        }

        private void DrawArea(Graphics dst, L2MapHeader.Spot spot, Color basecolor, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            DrawArea(
                dst,
                spot.x1,
                spot.y1,
                (byte)(spot.x1 + 1),
                (byte)(spot.y1 + 1),
                basecolor,
                dx,
                dy,
                mx,
                my,
                mw,
                mh,
                zoom);
        }

        private void DrawArea(Graphics dst, byte x1, byte y1, byte x2, byte y2, Color basecolor, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            if (x1 < mx + mw && x2 > mx &&
                y1 < my + mh && y2 > my)
            {
                int final_x = dx + zoom * ((x1 - mx) << 4);
                int final_y = dy + zoom * ((y1 - my) << 4);
                int final_w = (x2 - x1) * (zoom << 4);
                int final_h = (y2 - y1) * (zoom << 4);

                Color alpha = Color.FromArgb(0x80, basecolor);

                dst.FillRectangle(
                    new SolidBrush(alpha),
                    final_x, final_y, final_w, final_h);

                dst.DrawRectangle(
                    new Pen(basecolor),
                    final_x, final_y, final_w, final_h);
            }
        }

        private void DrawIconArea(L2MapHeader.Area area, Bitmap icon, Graphics dst, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            for (int y = area.y1; y < area.y2; y++)
            {
                for (int x = area.x1; x < area.x2; x++)
                {
                    if (x >= mx && y >= my &&
                        x < mx + mw && y < my + mh)
                    {
                        int final_x = dx + zoom * ((x - mx) << 4);
                        int final_y = dy + zoom * ((y - my) << 4);

                        dst.DrawImage(icon, final_x, final_y, 16, 16);
                    }
                }
            }
        }

        private void DrawIconSpot(L2MapHeader.Spot spot, Bitmap icon, Graphics dst, int dx, int dy, int mx, int my, int mw, int mh, int zoom)
        {
            int x = spot.x1;
            int y = spot.y1;

            if (x >= mx && y >= my &&
                x < mx + mw && y < my + mh)
            {
                int final_x = dx + zoom * ((x - mx) << 4);
                int final_y = dy + zoom * ((y - my) << 4);

                dst.DrawImage(icon, final_x, final_y, 16, 16);
            }
        }
    }
}

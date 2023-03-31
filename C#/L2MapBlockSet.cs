using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace LEdit
{
    public class L2MapBlockSet
    {
        string name;
        public string Name { get { return name; } }

        Collection<L2MapBlock> blocks = new Collection<L2MapBlock>();
        public Collection<L2MapBlock> Blocks { get { return blocks; } }

        public L2MapBlockSet() { }
        public L2MapBlockSet(string FileName) { FromFile(FileName); }

        public void FromFile(string FileName)
        {
            blocks.Clear();
            if (File.Exists(FileName))
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);

                ushort validate = br.ReadUInt16();
                if (validate == 0x434D)    //"MC"
                {
                    short num_blocks = br.ReadInt16();  //Amount of blocks
                    short file_size = br.ReadInt16();   //Original Filesize
                    br.ReadUInt16();    //unused and always zero?

                    char[] nam = br.ReadChars(8);
                    name = new string(nam);

                    L2MapBlock current_block;
                    for (int i = 0; i < num_blocks; i++)
                    {
                        current_block = new L2MapBlock(br);
                        blocks.Add(current_block);
                    }
                }

                br.Close();
                fs.Close();
            }
        }

        //BLOCK INFORMATION
        struct Coords
        {
            public int x, y;

            public Coords(int xx, int yy)
            {
                x = xx;
                y = yy;
            }
        }

        struct BlockInfo
        {
            public bool initialized;
            public int wall_height;

            public short clear;
            public short tile_base, tile_base_shad_w, tile_base_shad_s;
            public short[] tile_wall_n, tile_wall_shad_n;
            public short tile_wall_s, tile_wall_w, tile_wall_e;
            public short tile_wall_nw_i, tile_wall_ne_i, tile_wall_sw_i, tile_wall_se_i;
            public short[] tile_wall_sw_o, tile_wall_se_o, tile_wall_shad_se_o;
            public short tile_wall_nw_o, tile_wall_ne_o;
        };

        BlockInfo info;

        public void InitInfo(int FileNumber)
        {
            if (FileNumber == 0x135)
            {
                info.initialized = true;
                info.wall_height = 3;

                info.clear = 0x5A;
                info.tile_base = 0x04;
                info.tile_base_shad_w = 0x0B;
                info.tile_base_shad_s = 0x21;

                info.tile_wall_n = new short[info.wall_height];
                info.tile_wall_n[0] = 0x09;
                info.tile_wall_n[1] = 0x03;
                info.tile_wall_n[2] = 0x0E;

                info.tile_wall_shad_n = new short[info.wall_height];
                info.tile_wall_shad_n[0] = 0x09;
                info.tile_wall_shad_n[1] = 0x2B;
                info.tile_wall_shad_n[2] = 0x2E;

                info.tile_wall_s = 0x12;
                info.tile_wall_w = 0x1E;
                info.tile_wall_e = 0x16;

                info.tile_wall_nw_i = 0x26;
                info.tile_wall_ne_i = 0x27;
                info.tile_wall_sw_i = 0x31;
                info.tile_wall_se_i = 0x32;

                info.tile_wall_ne_o = 0x33;
                info.tile_wall_nw_o = 0x34;

                info.tile_wall_se_o = new short[info.wall_height];
                info.tile_wall_se_o[0] = 0x28;
                info.tile_wall_se_o[1] = 0x2C;
                info.tile_wall_se_o[2] = 0x2F;

                info.tile_wall_shad_se_o = new short[info.wall_height];
                info.tile_wall_shad_se_o[0] = 0x28;
                info.tile_wall_shad_se_o[1] = 0x5B;
                info.tile_wall_shad_se_o[2] = 0x5C;

                info.tile_wall_sw_o = new short[info.wall_height];
                info.tile_wall_sw_o[0] = 0x29;
                info.tile_wall_sw_o[1] = 0x2D;
                info.tile_wall_sw_o[2] = 0x30;
            }
            else if (FileNumber == 0x13D)
            {
                info.initialized = true;
                info.wall_height = 4;

                info.clear = 0x02;
                info.tile_base = 0x31;
                info.tile_base_shad_w = 0x35;
                info.tile_base_shad_s = 0x40;

                info.tile_wall_n = new short[info.wall_height];
                info.tile_wall_n[0] = 0x04;
                info.tile_wall_n[1] = 0x0A;
                info.tile_wall_n[2] = 0x13;
                info.tile_wall_n[3] = 0x20;

                info.tile_wall_shad_n = new short[info.wall_height];
                info.tile_wall_shad_n[0] = 0x04;
                info.tile_wall_shad_n[1] = 0x0E;
                info.tile_wall_shad_n[2] = 0x1C;
                info.tile_wall_shad_n[3] = 0x23;

                info.tile_wall_s = 0x4B;
                info.tile_wall_w = 0x0D;
                info.tile_wall_e = 0x10;

                info.tile_wall_nw_i = 0x03;
                info.tile_wall_ne_i = 0x05;
                info.tile_wall_sw_i = 0x45;
                info.tile_wall_se_i = 0x4A;

                info.tile_wall_ne_o = 0x46;
                info.tile_wall_nw_o = 0x49;

                info.tile_wall_se_o = new short[info.wall_height];
                info.tile_wall_se_o[0] = 0x07;
                info.tile_wall_se_o[1] = 0x55; //0x0A
                info.tile_wall_se_o[2] = 0x58; //0x13
                info.tile_wall_se_o[2] = 0x60; //0x20

                info.tile_wall_shad_se_o = new short[info.wall_height];
                info.tile_wall_shad_se_o[0] = 0x07;
                info.tile_wall_shad_se_o[1] = 0xBF; //0x0E
                info.tile_wall_shad_se_o[2] = 0xC1; //0x1C
                info.tile_wall_shad_se_o[3] = 0xC4; //0x23

                info.tile_wall_sw_o = new short[info.wall_height];
                info.tile_wall_sw_o[0] = 0x0C;
                info.tile_wall_sw_o[1] = 0x55; //0x0A
                info.tile_wall_sw_o[2] = 0x58; //0x13
                info.tile_wall_sw_o[3] = 0x60; //0x20
            }
        }

        private bool IsNothing(short b)
        {
            if (
                b == info.clear ||
                b == 0x00)
                return true;
            else
                return false;
        }

        private bool IsWall(short b)
        {
            if (
                b == info.tile_wall_e ||
                b == info.tile_wall_s ||
                b == info.tile_wall_w ||
                b == info.tile_wall_sw_i ||
                b == info.tile_wall_se_i ||
                b == info.tile_wall_ne_i ||
                b == info.tile_wall_nw_i ||
                b == info.tile_wall_nw_o ||
                b == info.tile_wall_ne_o
                )
                return true;

            for (int i = 0; i < info.wall_height; i++)
            {
                if (
                    b == info.tile_wall_n[i] ||
                    b == info.tile_wall_shad_n[i] ||
                    b == info.tile_wall_sw_o[i] ||
                    b == info.tile_wall_se_o[i] ||
                    b == info.tile_wall_shad_se_o[i]
                    )
                    return true;
            }

            return false;
        }

        private bool IsWallOrNothing(short b)
        {
            if (IsNothing(b))
                return true;
            else
                return IsWall(b);
        }

        public void MakeRoom(L2Map map, int mx, int my, int mw, int mh)
        {
            if (!info.initialized)
                return;

            for (int y = my; y < my + mh; y++)
            {
                for (int x = mx; x < mx + mw; x++)
                {
                    map.SetBlock(0, x, y, info.tile_base);
                    map.SetBlock(1, x, y, 0x00);
                    map.SetRoomBorder(x, y, false);
                    map.SetObstacle(x, y, false);
                    map.SetCollisionTop(x, y, false);
                    map.SetCollisionLeft(x, y, false);

                    if (y == my)
                    {
                        bool nwall = true;
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            if (!IsWallOrNothing(map.GetBlock(0, x, y - 1 - q)))
                            {
                                nwall = false;
                                break;
                            }
                        }

                        if (nwall)
                        {
                            for (int q = 0; q < info.wall_height; q++)
                            {
                                map.SetBlock(0, x, y - 1 - q, info.tile_wall_n[info.wall_height - 1 - q]);
                                map.SetBlock(1, x, y - 1 - q, 0x00);

                                map.SetObstacle(x, y - 1 - q, false);
                                map.SetRoomBorder(x, y - 1 - q, false);
                                map.SetCollisionLeft(x, y - 1 - q, false);
                                map.SetCollisionTop(x, y - 1 - q, false);
                            }
                        }
                    }
                    else if (y + 1 == my + mh)
                    {
                        if (IsWallOrNothing(map.GetBlock(0, x, y + 1)))
                        {
                            map.SetBlock(0, x, y + 1, info.tile_wall_s);
                            map.SetBlock(1, x, y + 1, 0x00);

                            map.SetObstacle(x, y + 1, false);
                            map.SetRoomBorder(x, y + 1, false);
                            map.SetCollisionLeft(x, y + 1, false);
                            map.SetCollisionTop(x, y + 1, false);
                        }
                    }

                    if (x == mx)
                    {
                        if (IsWallOrNothing(map.GetBlock(0, x - 1, y)))
                        {
                            map.SetBlock(0, x - 1, y, info.tile_wall_w);
                            map.SetBlock(1, x - 1, y, 0x00);

                            map.SetObstacle(x - 1, y, false);
                            map.SetRoomBorder(x - 1, y, false);
                            map.SetCollisionLeft(x - 1, y, false);
                            map.SetCollisionTop(x - 1, y, false);
                        }
                    }
                    else if (x + 1 == mx + mw)
                    {
                        if (IsWallOrNothing(map.GetBlock(0, x + 1, y)))
                        {
                            map.SetBlock(0, x + 1, y, info.tile_wall_e);
                            map.SetBlock(1, x + 1, y, 0x00);

                            map.SetObstacle(x + 1, y, false);
                            map.SetRoomBorder(x + 1, y, false);
                            map.SetCollisionLeft(x + 1, y, false);
                            map.SetCollisionTop(x + 1, y, false);
                        }
                    }
                }
            }
        }

        public void ConnectWalls(L2Map map, int wx, int wy)
        {
            int x = wx;
            int y = wy;

            int lastx = x;
            int lasty = y; //to prevent infinite loops

            short b;

            while (true)
            {
                b = map.GetBlock(0, x, y);

                if (b == info.tile_wall_n[0] ||
                    b == info.tile_wall_sw_o[0])
                {
                    #region North
                    //North tile
                    map.SetRoomBorder(x, y, true);
                    map.SetCollisionTop(x, y + info.wall_height, true);
                    for (int q = 1; q < info.wall_height; q++)
                        map.SetObstacle(x, y + q, true);

                    //Check tile to the right
                    b = map.GetBlock(0, x + 1, y);
                    if (b == info.tile_wall_n[0])
                    {
                        x++;
                        goto Next;
                    }

                    if (b == info.tile_wall_se_o[0])
                    {
                        x++;
                        y--;
                        goto Next;
                    }

                    //Check tile below right
                    if (map.GetBlock(0, x + 1, y + info.wall_height) == info.tile_wall_e)
                    {
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            map.SetRoomBorder(x + 1, y + q, true);
                            map.SetObstacle(x + 1, y + q, q > 0);
                            map.SetBlock(0, x + 1, y + q,
                                (q > 0) ?
                                info.tile_wall_e :
                                info.tile_wall_ne_i);
                        }
                        x++;
                        y += info.wall_height;
                        goto Next;
                    }

                    //Check tile above
                    if (map.GetBlock(0, x, y - 1) == info.tile_wall_w)
                    {
                        map.SetObstacle(x, y, true);
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            map.SetCollisionLeft(x + 1, y + q, true);
                            map.SetBlock(0, x, y + q,
                                info.tile_wall_se_o[q]);
                        }
                        y--;
                        goto Next;
                    }

                    //check tiles below right (in wall_height range)
                    int d = 0;
                    for (int q = 0; q < info.wall_height; q++)
                    {
                        if (map.GetBlock(0, x + 1, y + 1 + q) == info.tile_wall_n[0])
                        {
                            d = q + 1;
                            break;
                        }
                    }

                    if (d != 0)
                    {
                        for (int q = 0; q < d; q++)
                        {
                            map.SetRoomBorder(x + 1, y + q, true);
                            map.SetBlock(0, x + 1, y + q,
                                (q == 0) ?
                                info.tile_wall_ne_i :
                                info.tile_wall_e
                                );
                        }
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            bool coll = !IsWallOrNothing(map.GetBlock(0, x, y + d + q));
                            map.SetCollisionLeft(x + 1, y + d + q, coll);
                            map.SetRoomBorder(x + 1, y + d + q, (q == 0));
                            map.SetObstacle(x + 1, y + d + q, coll);
                            map.SetBlock(0, x + 1, y + d + q, info.tile_wall_sw_o[q]);
                        }

                        x++;
                        y += d;
                        goto Next;
                    }

                    //check tiles above right (in wall_height range)
                    for (int q = 0; q < info.wall_height; q++)
                    {
                        if (map.GetBlock(0, x + 1, y - 1 - q) == info.tile_wall_n[0])
                        {
                            d = q + 1;
                            break;
                        }
                    }

                    if (d != 0)
                    {
                        map.SetCollisionTop(x, y + info.wall_height, true);
                        map.SetRoomBorder(x, y, true);
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            bool coll = !IsWallOrNothing(map.GetBlock(0, x + 1, y + q));
                            map.SetBlock(0, x, y + q, info.tile_wall_se_o[q]);
                            map.SetCollisionLeft(x + 1, y + q, coll);
                            map.SetObstacle(x, y + q, q > 0);
                        }
                        for (int q = 0; q < d; q++)
                        {
                            map.SetRoomBorder(x, y - 1 - q, true);
                            map.SetBlock(0, x, y - 1 - q,
                                (q + 1 == d) ?
                                info.tile_wall_nw_i :
                                info.tile_wall_w
                                );
                        }

                        x += 1;
                        y -= d;
                        goto Next;
                    }
                    #endregion
                }
                else if (b == info.tile_wall_s ||
                    b == info.tile_wall_ne_o)
                {
                    #region South
                    map.SetRoomBorder(x, y, true);
                    map.SetObstacle(x, y, true);
                    map.SetCollisionTop(x, y, true);

                    //check tile left
                    b = map.GetBlock(0, x - 1, y);
                    if (b == info.tile_wall_s)
                    {
                        x--;
                        goto Next;
                    }

                    if (b == info.tile_wall_e)
                    {
                        map.SetBlock(0, x - 1, y, info.tile_wall_nw_o);
                        map.SetCollisionTop(x - 1, y, true);

                        x--;
                        goto Next;
                    }

                    if (b == info.tile_wall_nw_o)
                    {
                        x--;
                        goto Next;
                    }

                    //check tile below
                    if (map.GetBlock(0, x, y + 1) == info.tile_wall_e)
                    {
                        map.SetCollisionLeft(x, y, true);
                        map.SetBlock(0, x, y, info.tile_wall_nw_o);

                        y++;
                        goto Next;
                    }

                    //check tile above left
                    if (map.GetBlock(0, x - 1, y - 1) == info.tile_wall_w)
                    {
                        map.SetBlock(0, x - 1, y, info.tile_wall_sw_i);
                        map.SetRoomBorder(x - 1, y, true);
                        map.SetObstacle(x - 1, y, true);

                        x--;
                        y--;
                        goto Next;
                    }

                    if (map.GetBlock(0, x - 1, y - 1) == info.tile_wall_s)
                    {
                        map.SetBlock(0, x - 1, y, info.tile_wall_sw_i);
                        map.SetRoomBorder(x - 1, y, true);
                        map.SetObstacle(x - 1, y, true);

                        map.SetBlock(0, x - 1, y - 1, info.tile_wall_ne_o);
                        map.SetCollisionLeft(x, y - 1, true);

                        x--;
                        y--;
                        goto Next;
                    }

                    //check tile below left
                    if (map.GetBlock(0, x - 1, y + 1) == info.tile_wall_s)
                    {
                        map.SetBlock(0, x, y, info.tile_wall_nw_o);
                        map.SetCollisionLeft(x, y, true);

                        map.SetBlock(0, x, y + 1, info.tile_wall_se_i);
                        map.SetRoomBorder(x, y + 1, true);
                        map.SetObstacle(x, y + 1, true);

                        x--;
                        y++;
                        goto Next;
                    }
                    #endregion
                }
                else if (b == info.tile_wall_w)
                {
                    #region West
                    map.SetCollisionLeft(x + 1, y, true);
                    map.SetRoomBorder(x, y, true);
                    map.SetObstacle(x, y, true);

                    //check tile above
                    b = map.GetBlock(0, x, y - 1);
                    if (b == info.tile_wall_w)
                    {
                        y--;
                        goto Next;
                    }

                    if (b == info.tile_wall_s)
                    {
                        map.SetBlock(0, x, y - 1, info.tile_wall_ne_o);
                        map.SetCollisionLeft(x + 1, y - 1, true);
                        y--;
                        goto Next;
                    }

                    if (b == info.tile_wall_ne_o)
                    {
                        y--;
                        goto Next;
                    }

                    if (b == info.tile_wall_nw_i)
                    {
                        y--;
                        x++;
                        goto Next;
                    }

                    //check tile left
                    b = map.GetBlock(0, x - 1, y);
                    if (b == info.tile_wall_s ||
                        b == info.tile_wall_sw_i)
                    {
                        map.SetBlock(0, x, y, info.tile_wall_ne_o);
                        map.SetRoomBorder(x, y, true);
                        map.SetObstacle(x, y, true);
                        map.SetCollisionLeft(x + 1, y, true);
                        map.SetCollisionTop(x, y, true);
                        x--;

                        if (b == info.tile_wall_sw_i)
                            y--;

                        goto Next;
                    }

                    //check tile above right
                    if (map.GetBlock(0, x + 1, y - info.wall_height) == info.tile_wall_n[0] ||
                       map.GetBlock(0, x + 1, y - info.wall_height) == info.tile_wall_sw_o[0])
                    {
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            map.SetBlock(0, x, y - 1 - q,
                                (q + 1 < info.wall_height) ?
                                info.tile_wall_w :
                                info.tile_wall_nw_i);
                            map.SetRoomBorder(x, y - 1 - q, true);
                            map.SetObstacle(x, y - 1 - q, (q + 1 < info.wall_height));
                        }

                        x++;
                        y -= info.wall_height;
                        goto Next;
                    }
                    #endregion
                }
                else if (b == info.tile_wall_e ||
                    b == info.tile_wall_nw_o)
                {
                    #region East
                    map.SetCollisionLeft(x, y, true);
                    map.SetRoomBorder(x, y, true);
                    map.SetObstacle(x, y, true);

                    //check tile below
                    b = map.GetBlock(0, x, y + 1);
                    if (map.GetBlock(0, x, y + 1) == info.tile_wall_e)
                    {
                        y++;
                        goto Next;
                    }

                    if (b == info.tile_wall_n[0])
                    {
                        map.SetRoomBorder(x, y + 1, true);
                        for (int q = 0; q < info.wall_height; q++)
                        {
                            map.SetBlock(0, x, y + 1 + q, info.tile_wall_sw_o[q]);
                            map.SetObstacle(x, y + 1 + q, true);
                            map.SetCollisionLeft(x, y + 1 + q, true);
                        }

                        y++;
                        goto Next;
                    }

                    //check tile below left
                    if (map.GetBlock(0, x - 1, y + 1) == info.tile_wall_s)
                    {
                        map.SetBlock(0, x, y + 1, info.tile_wall_se_i);
                        map.SetRoomBorder(x, y + 1, true);
                        map.SetObstacle(x, y + 1, true);

                        x--;
                        y++;
                        goto Next;
                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine(x.ToString() + ", " + y.ToString());
                    break;
                }

            Next:

                if (((x == wx && y == wy) ||
                    (x == lastx && y == lasty)))
                {
                    Console.WriteLine(x.ToString() + ", " + y.ToString());
                    break;
                }

                lastx = x;
                lasty = y;
            }
        }

        #region OLD
        public void MakeWallsAround(L2Map map, int mx, int my)
        {
            MakeWallsAround(map, mx, my, true);
        }

        static Collection<Coords> processed = new Collection<Coords>();
        static int wx, wy;
        static bool wset;

        private static void Wall(int x, int y)
        {
            if (!wset)
            {
                wx = x;
                wy = y;

                wset = true;
            }
        }

        private void MakeWallsAround(L2Map map, int mx, int my, bool start)
        {
            int x, y;

            if (start)
            {
                processed.Clear();
                wset= false;
            }

            if (info.initialized)
            {
                Coords mm = new Coords(mx, my);
                if (!processed.Contains(mm))
                {
                    processed.Add(mm);

                    map.SetCollisionLeft(mx, my, false);
                    map.SetCollisionTop(mx, my, false);
                    map.SetRoomBorder(mx, my, false);
                    map.SetObstacle(mx, my, false);

                    //FLOOD FILL algorithm: do this for all non-wall tiles around (3x3)
                    for (y = my - 1; y <= my + 1; y++)
                    {
                        for (x = mx - 1; x <= mx + 1; x++)
                        {
                            if (!(x == mx && y == my) &&
                                !processed.Contains(new Coords(x, y)) &&
                                !IsWallOrNothing(map.GetBlock(0, x, y)))
                                MakeWallsAround(map, x, y, false);
                        }
                    }
                }
            }

            if (start)
            {
                for (int i = 0; i < processed.Count; i++)
                {
                    bool nwall = false;

                    x = processed[i].x;
                    y = processed[i].y;

                    //NORTH
                    nwall = true;
                    for (int q = 1; q <= info.wall_height; q++)
                    {
                        if (!IsNothing(map.GetBlock(0, x, y - q)))
                        {
                            nwall = false;
                            break;
                        }
                    }

                    if (nwall)
                    {
                        Wall(x, y - info.wall_height);
                        for (int q = 1; q <= info.wall_height; q++)
                        {
                            map.SetBlock(0, x, y - q, info.tile_wall_n[info.wall_height - q]);
                            map.SetBlock(1, x, y - q, 0x00);
                        }
                    }
                }

                for (int i = 0; i < processed.Count; i++)
                {
                    x = processed[i].x;
                    y = processed[i].y;

                    //SOUTH
                    if (IsNothing(map.GetBlock(0, x, y + 1)))
                    {
                        map.SetBlock(0, x, y + 1, info.tile_wall_s);
                        map.SetBlock(1, x, y + 1, 0x00);

                        Wall(x, y + 1);
                    }
                }

                for (int i = 0; i < processed.Count; i++)
                {
                    x = processed[i].x;
                    y = processed[i].y;

                    //WEST
                    if (IsNothing(map.GetBlock(0, x - 1, y)))
                    {
                        map.SetBlock(0, x - 1, y, info.tile_wall_w);
                        map.SetBlock(1, x - 1, y, 0x00);

                        Wall(x - 1, y);
                    }
                    //EAST
                    else if (IsNothing(map.GetBlock(0, x + 1, y)))
                    {
                        map.SetBlock(0, x + 1, y, info.tile_wall_e);
                        map.SetBlock(1, x + 1, y, 0x00);

                        Wall(x + 1, y);
                    }
                }

                /*
                 * FINALLY, "walk the wall"
                 * Set collisions, room borders and make corners
                 */
                if (wset)
                {
                    x = wx;
                    y = wy;

                    int lastx = x;
                    int lasty = y; //to prevent infinite loops

                    short b;

                    while (true)
                    {
                        b = map.GetBlock(0, x, y);

                        if (b == info.tile_wall_n[0] ||
                            b == info.tile_wall_sw_o[0])
                        {
                            #region North
                            //North tile
                            map.SetRoomBorder(x, y, true);
                            map.SetCollisionTop(x, y + info.wall_height, true);
                            for (int q = 1; q < info.wall_height; q++)
                                map.SetObstacle(x, y + q, true);

                            //Check tile to the right
                            if (map.GetBlock(0, x + 1, y) == info.tile_wall_n[0])
                            {
                                x++;
                            }
                            //Check tile below right
                            else if (map.GetBlock(0, x + 1, y + info.wall_height) == info.tile_wall_e)
                            {
                                for (int q = 0; q < info.wall_height; q++)
                                {
                                    map.SetRoomBorder(x + 1, y + q, true);
                                    map.SetObstacle(x + 1, y + q, q > 0);
                                    map.SetBlock(0, x + 1, y + q,
                                        (q > 0) ?
                                        info.tile_wall_e :
                                        info.tile_wall_ne_i);
                                }
                                x++;
                                y += info.wall_height;
                            }
                            //Check tile above
                            else if (map.GetBlock(0, x, y - 1) == info.tile_wall_w)
                            {
                                map.SetObstacle(x, y, true);
                                for (int q = 0; q < info.wall_height; q++)
                                {
                                    map.SetCollisionLeft(x + 1, y + q, true);
                                    map.SetBlock(0, x, y + q,
                                        info.tile_wall_se_o[q]);
                                }
                                y--;
                            }
                            else
                            {
                                //check tiles below right (in wall_height range)
                                int d = 0;
                                for (int q = 0; q < info.wall_height; q++)
                                {
                                    if (map.GetBlock(0, x + 1, y + 1 + q) == info.tile_wall_n[0])
                                    {
                                        d = q + 1;
                                        break;
                                    }
                                }

                                if (d != 0)
                                {
                                    for (int q = 0; q < d; q++)
                                    {
                                        map.SetRoomBorder(x + 1, y + q, true);
                                        map.SetBlock(0, x + 1, y + q,
                                            (q == 0) ?
                                            info.tile_wall_ne_i :
                                            info.tile_wall_e
                                            );
                                    }
                                    for (int q = 0; q < info.wall_height; q++)
                                    {
                                        bool coll = !IsWallOrNothing(map.GetBlock(0, x, y + d + q));
                                        map.SetCollisionLeft(x + 1, y + d + q, coll);
                                        map.SetRoomBorder(x + 1, y + d + q, (q == 0));
                                        map.SetObstacle(x + 1, y + d + q, coll);
                                        map.SetBlock(0, x + 1, y + d + q, info.tile_wall_sw_o[q]);
                                    }

                                    x++;
                                    y += d;
                                }
                                else
                                {
                                    //check tiles above right (in wall_height range)
                                    for (int q = 0; q < info.wall_height; q++)
                                    {
                                        if (map.GetBlock(0, x + 1, y - 1 - q) == info.tile_wall_n[0])
                                        {
                                            d = q + 1;
                                            break;
                                        }
                                    }

                                    if (d != 0)
                                    {
                                        map.SetCollisionTop(x, y + info.wall_height, true);
                                        map.SetRoomBorder(x, y, true);
                                        for (int q = 0; q < info.wall_height; q++)
                                        {
                                            bool coll = !IsWallOrNothing(map.GetBlock(0, x + 1, y + q));
                                            map.SetBlock(0, x, y + q, info.tile_wall_se_o[q]);
                                            map.SetCollisionLeft(x + 1, y + q, coll);
                                            map.SetObstacle(x, y + q, q > 0);
                                        }
                                        for (int q = 0; q < d ; q++)
                                        {
                                            map.SetRoomBorder(x , y - 1 - q, true);
                                            map.SetBlock(0, x, y - 1 - q,
                                                (q + 1 == d) ?
                                                info.tile_wall_nw_i :
                                                info.tile_wall_w
                                                );
                                        }

                                        x += 1;
                                        y -= d;
                                    }
                                }
                            }
                            #endregion
                        }
                        else if (b == info.tile_wall_s ||
                            b == info.tile_wall_ne_o)
                        {
                            #region South
                            map.SetRoomBorder(x, y, true);
                            map.SetObstacle(x, y, true);
                            map.SetCollisionTop(x, y, true);
                            
                            //check tile left
                            if (map.GetBlock(0, x - 1, y) == info.tile_wall_s)
                            {
                                x--;
                            }
                            //check tile below
                            else if (map.GetBlock(0, x, y + 1) == info.tile_wall_e)
                            {
                                map.SetCollisionLeft(x, y, true);
                                map.SetBlock(0, x, y, info.tile_wall_nw_o);

                                y++;
                            }
                            //check tile above left
                            else if (map.GetBlock(0, x - 1, y - 1) == info.tile_wall_w)
                            {
                                map.SetBlock(0, x - 1, y, info.tile_wall_sw_i);
                                map.SetRoomBorder(x - 1, y, true);
                                map.SetObstacle(x - 1, y, true);

                                x--;
                                y--;
                            }
                            else if (map.GetBlock(0, x - 1, y - 1) == info.tile_wall_s)
                            {
                                map.SetBlock(0, x - 1, y, info.tile_wall_sw_i);
                                map.SetRoomBorder(x - 1, y, true);
                                map.SetObstacle(x - 1, y, true);

                                map.SetBlock(0, x - 1, y - 1, info.tile_wall_ne_o);
                                map.SetCollisionLeft(x, y - 1, true);

                                x--;
                                y--;
                            }
                            //check tile below left
                            else if (map.GetBlock(0, x - 1, y + 1) == info.tile_wall_s)
                            {
                                map.SetBlock(0, x, y, info.tile_wall_nw_o);
                                map.SetCollisionLeft(x, y, true);

                                map.SetBlock(0, x, y + 1, info.tile_wall_se_i);
                                map.SetRoomBorder(x, y + 1, true);
                                map.SetObstacle(x, y + 1, true);

                                x--;
                                y++;
                            }
                            #endregion
                        }
                        else if (b == info.tile_wall_w)
                        {
                            #region West
                            map.SetCollisionLeft(x + 1, y, true);
                            map.SetRoomBorder(x, y, true);
                            map.SetObstacle(x, y, true);

                            //check tile above
                            if (map.GetBlock(0, x, y - 1) == info.tile_wall_w)
                            {
                                y--;
                            }
                            else if (map.GetBlock(0, x, y - 1) == info.tile_wall_s)
                            {
                                map.SetBlock(0, x, y - 1, info.tile_wall_ne_o);
                                map.SetCollisionLeft(x + 1, y - 1, true);
                                y--;
                            }
                            //check tile above right
                            else if (map.GetBlock(0, x + 1, y - info.wall_height) == info.tile_wall_n[0] ||
                               map.GetBlock(0, x + 1, y - info.wall_height) == info.tile_wall_sw_o[0])
                            {
                                for (int q = 0; q < info.wall_height; q++)
                                {
                                    map.SetBlock(0, x, y - 1 - q,
                                        (q + 1 < info.wall_height) ?
                                        info.tile_wall_w :
                                        info.tile_wall_nw_i);
                                    map.SetRoomBorder(x, y - 1 - q, true);
                                    map.SetObstacle(x, y - 1 - q, (q + 1 < info.wall_height));
                                }

                                x++;
                                y -= info.wall_height;
                            }
                            #endregion
                        }
                        else if (b == info.tile_wall_e ||
                            b == info.tile_wall_nw_o)
                        {
                            #region East
                            map.SetCollisionLeft(x, y, true);
                            map.SetRoomBorder(x, y, true);
                            map.SetObstacle(x, y, true);

                            //check tile below
                            if (map.GetBlock(0, x, y + 1) == info.tile_wall_e)
                            {
                                y++;
                            }
                            else if (map.GetBlock(0, x, y + 1) == info.tile_wall_n[0])
                            {
                                map.SetRoomBorder(x, y + 1, true);
                                for (int q = 0; q < info.wall_height; q++)
                                {
                                    map.SetBlock(0, x, y + 1 + q, info.tile_wall_sw_o[q]);
                                    map.SetObstacle(x, y + 1 + q, true);
                                    map.SetCollisionLeft(x, y + 1 + q, true);
                                }

                                y++;
                            }
                            //check tile below left
                            else if (map.GetBlock(0, x - 1, y + 1) == info.tile_wall_s)
                            {
                                map.SetBlock(0, x, y + 1, info.tile_wall_se_i);
                                map.SetRoomBorder(x, y + 1, true);
                                map.SetObstacle(x, y + 1, true);

                                x--;
                                y++;
                            }


                            #endregion
                        }
                        else
                        {
                            Console.WriteLine(x.ToString() + ", " + y.ToString());
                            break;
                        }

                        if (((x == wx && y == wy) ||
                            (x == lastx && y == lasty)))
                        {
                            Console.WriteLine(x.ToString() + ", " + y.ToString());
                            break;
                        }

                        lastx = x;
                        lasty = y;
                    }
                }
            }
        }
        #endregion
    }
}

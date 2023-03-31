using System.Drawing;

namespace LEdit
{
    public class GradientBuffer
    {
        Bitmap bmp = null;
        bool drawn = false;

        public enum GradientDirection
        {
            Horizontal, Vertical,
        }

        int width, height;
        Color color1, color2;
        GradientDirection dir;

        ~GradientBuffer()
        {
            if (bmp != null) bmp.Dispose();
        }

        public GradientBuffer() { bmp = null; drawn = false; }

        public GradientBuffer(int Width, int Height, Color Color1, Color Color2, GradientDirection Direction)
        {
            Reset(Width, Height, Color1, Color2, Direction);
        }

        public void Reset(int Width, int Height, Color Color1, Color Color2, GradientDirection Direction)
        {
            if (bmp != null) bmp.Dispose();
            bmp = null;
            drawn = false;

            width = Width;
            height = Height;
            color1 = Color1;
            color2 = Color2;
            dir = Direction;

            PreDraw();
        }

        public void Draw(Graphics g, int dx, int dy)
        {
            PreDraw();

            g.DrawImage(bmp, dx, dy);
        }

        private void PreDraw()
        {
            if (!drawn)
            {
                bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);

                byte r1 = color1.R;
                byte g1 = color1.G;
                byte b1 = color1.B;
                byte r2 = color2.R;
                byte g2 = color2.G;
                byte b2 = color2.B;

                double ir = (double)(r2 - r1) / (double)height;
                double ig = (double)(g2 - g1) / (double)height;
                double ib = (double)(b2 - b1) / (double)height;

                double cr = (double)r1;
                double cg = (double)g1;
                double cb = (double)b1;

                if (dir == GradientDirection.Vertical)
                {
                    for (int y = 0; y < height; y++)
                    {
                        g.DrawLine(new Pen(Color.FromArgb((int)cr, (int)cg, (int)cb)),
                                   0, y, width, y);

                        cr += ir;
                        cg += ig;
                        cb += ib;
                    }
                }
                else if (dir == GradientDirection.Horizontal)
                {
                    for (int x = 0; x < width; x++)
                    {
                        g.DrawLine(new Pen(Color.FromArgb((int)cr, (int)cg, (int)cb)),
                                   x, 0, x, height);

                        cr += ir;
                        cg += ig;
                        cb += ib;
                    }
                }

                drawn = true;
            }
        }
    }
}

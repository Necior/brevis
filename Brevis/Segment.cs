using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Segment
    {
        internal int x1;
        internal int y1;
        internal int x2;
        internal int y2;
        public Segment(int x1, int y1, int x2, int y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public Segment(Vertex2D a, Vertex2D b)
        {
            /*
             * TODO: what about double <-> int conversion?
             */
            this.x1 = (int)a.X;
            this.y1 = (int)a.Y;
            this.x2 = (int)b.X;
            this.y2 = (int)b.Y;
        }

        public void Draw(IHasSetPixel canvas)
        {
            this.DrawBresenham(canvas);
        }

        private void DrawBresenham(IHasSetPixel canvas)
        {
            int x0 = this.x1;
            int x1 = this.x2;
            int y0 = this.y1;
            int y1 = this.y2;
            int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2; /* error value e_xy */

            while (true)
            {
                /*
                 * TODO: currently, color is white. Make it customizable.
                 */
                canvas.SetPixel(y0, x0, 16777215);
                if (x0 == x1 && y0 == y1)
                    break;
                e2 = 2 * err;
                if (e2 >= dy)
                {
                    err += dy;
                    x0 += sx;
                }
                if (e2 <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }
    }
}

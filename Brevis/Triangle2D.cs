using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle2D : IDrawable
    {
        private readonly Vertex2D _a;
        private readonly Vertex2D _b;
        private readonly Vertex2D _c;
        private readonly int _color;

        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c, int color)
        {
            this._a = a;
            this._b = b;
            this._c = c;
            this._color = color;
        }

        public void Draw(IHasSetPixel canvas, int color)
        {
            this.DrawWireframe(canvas, color);
        }

        public void Draw(IHasSetPixel canvas)
        {
            this.Draw(canvas, this._color);
        }

        private void DrawWireframe(IHasSetPixel canvas, int color)
        {
            new Segment(this._a, this._b).Draw(canvas, color);
            new Segment(this._b, this._c).Draw(canvas, color);
            new Segment(this._c, this._a).Draw(canvas, color);
        }
    }
}

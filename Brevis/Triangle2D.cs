using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle2D
    {
        private readonly Vertex2D _a;
        private readonly Vertex2D _b;
        private readonly Vertex2D _c;

        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }

        public void Draw(IHasSetPixel canvas)
        {
            this.DrawWireframe(canvas);
        }

        private void DrawWireframe(IHasSetPixel canvas)
        {
            new Segment(this._a, this._b).Draw(canvas);
            new Segment(this._b, this._c).Draw(canvas);
            new Segment(this._c, this._a).Draw(canvas);
        }
    }
}

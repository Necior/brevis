using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle3D
    {
        private readonly Vertex3D _a;
        private readonly Vertex3D _b;
        private readonly Vertex3D _c;
        private readonly int _color;
        public Triangle3D(Vertex3D a, Vertex3D b, Vertex3D c, int color)
        {
            this._a = a;
            this._b = b;
            this._c = c;
            this._color = color;
        }

        public Triangle2D OrthogonalProjection()
        {
            return new Triangle2D(this._a.OrthogonalProjection(), this._b.OrthogonalProjection(), this._c.OrthogonalProjection(), this._color);
        }

        public Triangle2D PerspectiveProjection(Matrix projectionMatrix)
        {
            return new Triangle2D(
                this._a.PerspectiveProjection(projectionMatrix),
                this._b.PerspectiveProjection(projectionMatrix),
                this._c.PerspectiveProjection(projectionMatrix),
                this._color
            );
        }
    }
}

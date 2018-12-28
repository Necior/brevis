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
        public Triangle3D(Vertex3D a, Vertex3D b, Vertex3D c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }

        public Triangle2D OrthogonalProjection()
        {
            return new Triangle2D(this._a.OrthogonalProjection(), this._b.OrthogonalProjection(), this._c.OrthogonalProjection());
        }
    }
}

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
    }
}

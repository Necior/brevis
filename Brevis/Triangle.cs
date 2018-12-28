using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    internal class Triangle
    {
        private readonly Vertex3D _a;
        private readonly Vertex3D _b;
        private readonly Vertex3D _c;
        internal Triangle(Vertex3D a, Vertex3D b, Vertex3D c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }
    }
}

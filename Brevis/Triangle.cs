using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    internal class Triangle
    {
        private readonly Vertex _a;
        private readonly Vertex _b;
        private readonly Vertex _c;
        internal Triangle(Vertex a, Vertex b, Vertex c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }
    }
}

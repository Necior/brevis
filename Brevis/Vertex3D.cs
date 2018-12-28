using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    internal class Vertex3D
    {
        internal readonly double _x;
        internal readonly double _y;
        internal readonly double _z;
        internal Vertex3D(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }
    }
}

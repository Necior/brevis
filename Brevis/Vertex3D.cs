using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Vertex3D
    {
        private readonly double _x;
        private readonly double _y;
        private readonly double _z;
        public Vertex3D(double x, double y, double z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }
    }
}

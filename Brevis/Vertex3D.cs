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

        public Vertex2D OrthogonalProjection()
        {
            return new Vertex2D(this._x, this._y);
        }

        public Vertex2D PerspectiveProjection(Matrix projectionMatrix)
        {
            var point = new Matrix(4, 1);
            point.SetValue(0, 0, this._x);
            point.SetValue(1, 0, this._y);
            point.SetValue(2, 0, this._z);
            point.SetValue(3, 0, 1);
            return Vertex2D.FromMatrix(Matrix.Multiply(projectionMatrix, point));
        }
    }
}

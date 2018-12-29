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

        public Vertex2D PerspectiveProjection()
        {
            /*
             * TODO: remove hardcoded perspective matrix.
             */
            var ppm = Matrix.PerspectiveProjectionMatrix(1, 100);

            var viewMatrix = new Matrix(4, 4);
            viewMatrix.SetValue(0, 0, -0.447);
            viewMatrix.SetValue(0, 1, 0.894);
            viewMatrix.SetValue(0, 2, 0);
            viewMatrix.SetValue(0, 3, -0.447);

            viewMatrix.SetValue(1, 0, -0.458);
            viewMatrix.SetValue(1, 1, -0.229);
            viewMatrix.SetValue(1, 2, 0.859);
            viewMatrix.SetValue(1, 3, -0.315);

            viewMatrix.SetValue(2, 0, 0.768);
            viewMatrix.SetValue(2, 1, 0.384);
            viewMatrix.SetValue(2, 2, 0.512);
            viewMatrix.SetValue(2, 3, -4.353);

            viewMatrix.SetValue(3, 0, 0);
            viewMatrix.SetValue(3, 1, 0);
            viewMatrix.SetValue(3, 2, 0);
            viewMatrix.SetValue(3, 3, 1);
            
            var point = new Matrix(4, 1);
            point.SetValue(0, 0, this._x);
            point.SetValue(1, 0, this._y);
            point.SetValue(2, 0, this._z);
            point.SetValue(3, 0, 1);
            return Vertex2D.FromMatrix(Matrix.Multiply(Matrix.Multiply(ppm, viewMatrix), point));
        }
    }
}

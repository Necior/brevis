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
        public readonly double x;
        public readonly double y;
        public readonly double z;
        public Vector3D normal;
        public Vertex3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vertex2D OrthogonalProjection()
        {
            return new Vertex2D(this.x, this.y);
        }

        public Vertex2D PerspectiveProjection(Matrix projectionMatrix)
        {
            var point = new Matrix(4, 1);
            point.SetValue(0, 0, this.x);
            point.SetValue(1, 0, this.y);
            point.SetValue(2, 0, this.z);
            point.SetValue(3, 0, 1);
            return Vertex2D.FromMatrix(Matrix.Multiply(projectionMatrix, point), normal);
        }

        public static Vector3D operator -(Vertex3D a, Vertex3D b)
        {
            return new Vector3D(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public Vertex3D Add(Vector3D v)
        {
            return new Vertex3D(x + v.x, y + v.y, z + v.z);
        }

        public override bool Equals(Object obj)
        {
            return (obj is Vertex3D) && ((Vertex3D) obj).x == x && ((Vertex3D) obj).y == y && ((Vertex3D) obj).z == z;
        }

        public void SetNormal(Vector3D normal)
        {
            this.normal = normal;
        }

        public static Vertex3D Average(Vertex3D a, Vertex3D b, Vertex3D c, double w1, double w2, double w3)
        {
            return new Vertex3D(
                a.x * w1 + b.x * w2 + c.x * w3,
                a.y * w1 + b.y * w2 + c.y * w3,
                a.z * w1 + b.z * w2 + c.z * w3
            );
        }
    }
}

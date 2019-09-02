using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Vector3D
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;
        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3D Normalize()
        {
            double norm = Math.Sqrt(x * x + y * y + z * z);
            return new Vector3D(x/norm, y/norm, z/norm);
        }

        public Vector3D Add(Vector3D other)
        {
            return new Vector3D(x + other.x, y + other.y, z + other.z);
        }

        public Vector3D Mul(double v)
        {
            return new Vector3D(x*v, y*v, z*v);
        }

        public Matrix ToMatrix()
        {
            Matrix m = new Matrix(3, 1);
            m.SetValue(0, 0, x);
            m.SetValue(1, 0, y);
            m.SetValue(2, 0, z);
            return m;
        }

        public static Vector3D FromMatrix(Matrix m)
        {
            return new Vector3D(m.GetValue(0, 0), m.GetValue(1, 0), m.GetValue(2, 0));
        }

        static public Vector3D PerpendicularVector(Vector3D a, Vector3D b)
        {
            /*
             * We will find direction by using perpendicular vector equation:
             *     Let a = [ax, ay, az], b = [bx, by, bz].
             *     Then c = a x b = (ay*bz - by*az)i + (ax*bz - bx*az)j + (ax*by - bx*ay)k
             */
            return new Vector3D(a.y * b.z - b.y * a.z, a.x * b.z - b.x * a.z, a.x * b.y - b.x * a.y);
        }
    }
}

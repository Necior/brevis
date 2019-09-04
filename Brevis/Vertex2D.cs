using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Vertex2D
    {
        private readonly int x_;
        private readonly int y_;
        private readonly double z_; /* Might be 0 if unset... */
        public Vector3D normal;

        public Vertex2D(int x, int y, double z = 0)
        {
            this.x_ = x;
            this.y_ = y;
            this.z_ = z;
        }

        public Vertex2D(double x, double y, double z = 0)
        {
            this.x_ = (int) Math.Round(x);
            this.y_ = (int) Math.Round(y);
            this.z_ = z;
        }

        public int X => x_;
        public int Y => y_;
        public double Z => z_;

        public void SetNormal(Vector3D normal)
        {
            this.normal = normal;
        }

        public static Vertex2D FromMatrix(Matrix m, Vector3D normal)
        {
            /*
             * TODO: remove hardcoded mapping: [-1, 1] -> [0, 255].
             */
            var normalizationFactor = m.GetValue(3, 0);
            var result = new Vertex2D(
                (m.GetValue(0, 0)/normalizationFactor + 1) * 127.5,
                (m.GetValue(1, 0)/normalizationFactor + 1) * 127.5,
                (m.GetValue(2, 0)/normalizationFactor + 1) * 127.5
                );
            result.SetNormal(normal);
            return result;
        }
    }
}

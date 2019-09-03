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

        public Vertex2D(int x, int y)
        {
            this.x_ = x;
            this.y_ = y;
        }

        public Vertex2D(double x, double y)
        {
            this.x_ = (int) Math.Round(x);
            this.y_ = (int) Math.Round(y);
        }

        public int X => x_;
        public int Y => y_;

        public static Vertex2D FromMatrix(Matrix m)
        {
            /*
             * TODO: remove hardcoded mapping: [-1, 1] -> [0, 255].
             */
            var normalizationFactor = m.GetValue(3, 0);
            return new Vertex2D((m.GetValue(0, 0)/normalizationFactor + 1) * 127.5, (m.GetValue(1, 0)/normalizationFactor + 1) * 127.5);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public interface IHasSetPixel
    {
        void SetPixel(int row, int column, int color);
        void SetPixel(int row, int column, double z, int color);
        void SetPixel(int row, int column, double z, int color, bool transparent);
    }
}

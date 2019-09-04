using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public interface IDrawable
    {
        void Draw(IHasSetPixel canvas, int color);
    }

    public interface IDrawableAdvanced
    {
        void Draw(IHasSetPixel canvas, int color, VisualParams vp);
    }
}

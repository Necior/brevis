﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    internal interface IDrawable
    {
        void SetPixel(int row, int column, int color);
    }
}

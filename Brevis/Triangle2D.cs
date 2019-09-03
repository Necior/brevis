﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle2D : IDrawable
    {
        private readonly Vertex2D _a;
        private readonly Vertex2D _b;
        private readonly Vertex2D _c;
        private readonly int _color;

        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c, int color)
        {
            this._a = a;
            this._b = b;
            this._c = c;
            this._color = color;
        }

        public void Draw(IHasSetPixel canvas, int color)
        {
            this.DrawMesh(canvas, color);
        }

        public void Draw(IHasSetPixel canvas)
        {
            this.Draw(canvas, this._color);
        }

        private void DrawMesh(IHasSetPixel canvas, int color)
        {
            DrawWireframe(canvas, color);

            var p1 = new Segment(_a, _b);
            var p2 = new Segment(_b, _c);
            var p3 = new Segment(_c, _a);

            var minY = Math.Min(Math.Min(_a.Y, _b.Y), _c.Y);
            var maxY = Math.Max(Math.Max(_a.Y, _b.Y), _c.Y);
            var minX = Math.Min(Math.Min(_a.X, _b.X), _c.X);
            var maxX = Math.Max(Math.Max(_a.X, _b.X), _c.X);
            /*
             * TODO: don't hardcode canvas size.
             */
            for (int x = Math.Max(0, minX); x <= Math.Min(256, maxX); x++)
            {
                for (int y = Math.Max(0, minY); y <= Math.Min(256, maxY); y++)
                {
                    if (Inside(x, y))
                    {
                        canvas.SetPixel(y, x, color);
                    }
                }
            }
        }

        private bool Inside(int x, int y)
        {
            /*
             * Highly inspired by https://stackoverflow.com/a/2049593.
             */
            Vertex2D pt = new Vertex2D(x, y);
            var d1 = (pt.X - _b.X) * (_a.Y - _b.Y) - (_a.X - _b.X) * (pt.Y - _b.Y);
            var d2 = (pt.X - _c.X) * (_b.Y - _c.Y) - (_b.X - _c.X) * (pt.Y - _c.Y);
            var d3 = (pt.X - _a.X) * (_c.Y - _a.Y) - (_c.X - _a.X) * (pt.Y - _a.Y);

            return !(((d1 < 0) || (d2 < 0) || (d3 < 0)) && ((d1 > 0) || (d2 > 0) || (d3 > 0)));
        }

        private void DrawWireframe(IHasSetPixel canvas, int color)
        {
            new Segment(this._a, this._b).Draw(canvas, color);
            new Segment(this._b, this._c).Draw(canvas, color);
            new Segment(this._c, this._a).Draw(canvas, color);
        }
    }
}

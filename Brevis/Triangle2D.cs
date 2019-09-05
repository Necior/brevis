using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle2D : IDrawableAdvanced
    {
        private readonly Vertex2D _a;
        private readonly Vertex2D _b;
        private readonly Vertex2D _c;
        private readonly int _color;
        private readonly Triangle3D _original;

        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c, Triangle3D original, int color)
        {
            this._a = a;
            this._b = b;
            this._c = c;
            this._color = color;
            this._original = original;
        }

        public void Draw(IHasSetPixel canvas, VisualParams vp)
        {
            this.DrawMesh(canvas, vp);
        }

        private void DrawMesh(IHasSetPixel canvas, VisualParams vp)
        {
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
            for (int x = Math.Max(0, minX); x <= Math.Min(255, maxX); x++)
            {
                for (int y = Math.Max(0, minY); y <= Math.Min(255, maxY); y++)
                {
                    if (Inside(x, y))
                    {
                        /*
                         * First things first: Z-buffer :-)
                         */
                        /*
                         * https://en.wikipedia.org/wiki/Barycentric_coordinate_system
                         */
                        /*
                         * TODO: maybe extract constants out of the loop... If compiler won't ;-)
                         */
                        double x1 = _a.X;
                        double x2 = _b.X;
                        double x3 = _c.X;
                        double y1 = _a.Y;
                        double y2 = _b.Y;
                        double y3 = _c.Y;
                        double detT = (y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3);
                        if(detT == 0)
                            continue;
                        double lambda1 = ((y2 - y3)*(x - x3) + (x3 - x2)*(y - y3))/detT;
                        double lambda2 = ((y3 - y1)*(x - x3) + (x1 - x3)*(y - y3))/detT;
                        double lambda3 = 1 - lambda1 - lambda2;
                        var z = lambda1 * _a.Z + lambda2 * _b.Z + lambda3 * _c.Z;

                        /*
                         * https://en.wikipedia.org/wiki/Phong_reflection_model
                         *
                         * I interpolate current pixel's 3D position and its normal vector using barycentric coordinates.
                         * Especially interpolating normals make render so smooooooth :-)
                         */
                        var myPos = Vertex3D.Average(_original.a, _original.b, _original.c, lambda1, lambda2, lambda3);
                        var finalColor = GetColor(vp, myPos);
                        if (vp.phong)
                        {
                            var L = (vp.lightPos - myPos).Normalize();
                            var N = _a.normal.Mul(lambda1).Add(_b.normal.Mul(lambda2).Add(_c.normal.Mul(lambda3)));
                            var R = (N.Mul(2 * Vector3D.DotProduct(L, N)) - L).Normalize();
                            var V = (vp.camPos - myPos).Normalize();

                            double ambient = vp.k_a * vp.i_aR;
                            double diffuse = vp.k_d * Math.Max(0, Vector3D.DotProduct(L, N)) * vp.i_dR;
                            double specular = vp.k_s * Math.Pow(Math.Max(0, Vector3D.DotProduct(R, V)), vp.specularAlpha) * vp.i_sR;
                            var red = Math.Min(255, Math.Max(0, (int)((ambient + diffuse + specular))));

                            ambient = vp.k_a * vp.i_aG;
                            diffuse = vp.k_d * Math.Max(0, Vector3D.DotProduct(L, N)) * vp.i_dG;
                            specular = vp.k_s * Math.Pow(Math.Max(0, Vector3D.DotProduct(R, V)), vp.specularAlpha) * vp.i_sG;
                            var green = Math.Min(255, Math.Max(0, (int)((ambient + diffuse + specular))));

                            ambient = vp.k_a * vp.i_aB;
                            diffuse = vp.k_d * Math.Max(0, Vector3D.DotProduct(L, N)) * vp.i_dB;
                            specular = vp.k_s * Math.Pow(Math.Max(0, Vector3D.DotProduct(R, V)), vp.specularAlpha) * vp.i_sB;
                            var blue = Math.Min(255, Math.Max(0, (int)((ambient + diffuse + specular))));

                            finalColor = MixColors(finalColor, red, green, blue);
                        }

                        if (vp.fog)
                        {
                            var distance = (vp.camPos - myPos).Norm();
                            if (distance > 10)
                                distance = 10;
                            /* Yay, another magic constant to make teapot look good with a fog. */
                            finalColor = MixColors(finalColor, vp.fogColor, distance/10);
                        }

                        canvas.SetPixel(y, x, z, finalColor);
                    }
                }
            }
        }

        private int GetColor(VisualParams vp, Vertex3D v)
        {
            if (vp.colorMode == "TEXTURE")
                return UseTexture(v, vp.pixels);
            if (vp.colorMode == "RANDOM")
                return _color;
            if (vp.colorMode == "COLOR")
                return vp.defaultColor;
            return Const.Color.white;
        }

        private int MixColors(int c, int r, int g, int b)
        {
            return (((((c & 0xff0000) >> 16) + r) / 2) << 16) + 
                   (((((c & 0x00ff00) >> 8) + g) / 2) << 8) +
                   (((c & 0x0000ff) + b) / 2);
        }

        private int MixColors(int c1, int c2, double alpha)
        {
            return (MixColor((c1 & 0xff0000) >> 16, (c2 & 0xff0000) >> 16, alpha) << 16) +
                   (MixColor((c1 & 0x00ff00) >> 8, (c2 & 0x00ff00) >> 8, alpha) << 8) +
                   MixColor(c1 & 0x0000ff, c2 & 0x0000ff, alpha);
        }

        private int MixColor(int a, int b, double alpha)
        {
            return Math.Max(0, Math.Min(255, (int)(a * (1 - alpha) + b * alpha)));
        }

        private int UseTexture(Vertex3D v, PixelColor[,] pixels)
        {
            /*
             * Some magic constants... They make "snow" texture look good enough.
             */
            var x = (int)Math.Min(Math.Max(0, (int)(pixels.GetLength(0)/2 + v.x*32)), pixels.GetLength(0)-1);
            var z = (int)Math.Min(Math.Max(0, (int)(pixels.GetLength(1)/2 + v.z*32)), pixels.GetLength(1)-1);
            var r = pixels[x, z];

            return (r.Red << 16) + (r.Green << 8) + r.Blue;
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

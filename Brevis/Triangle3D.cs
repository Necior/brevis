using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Triangle3D
    {
        public readonly Vertex3D a;
        public readonly Vertex3D b;
        public readonly Vertex3D c;
        private readonly int _color;
        public readonly Vector3D normal;
        private bool transparent;
        public Triangle3D(Vertex3D a, Vertex3D b, Vertex3D c, int color)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this._color = color;
            this.normal = CalculateNormal();
        }

        public void SetTransparent(bool t)
        {
            this.transparent = t;
        }

        private Vector3D CalculateNormal()
        {
            /* Normal of a triangle is cross product of its two sides. */
            return Vector3D.CrossProduct(b - a, c - a).Normalize();
        }

        public Triangle2D OrthogonalProjection()
        {
            return new Triangle2D(this.a.OrthogonalProjection(), this.b.OrthogonalProjection(), this.c.OrthogonalProjection(), this, this._color);
        }

        public Triangle2D PerspectiveProjection(Matrix projectionMatrix)
        {
            return new Triangle2D(
                this.a.PerspectiveProjection(projectionMatrix),
                this.b.PerspectiveProjection(projectionMatrix),
                this.c.PerspectiveProjection(projectionMatrix),
                this,
                this._color,
                this.transparent
            );
        }

        public bool Contains(Vertex3D v)
        {
            return a.Equals(v) || b.Equals(v) || c.Equals(v);
        }
    }
}

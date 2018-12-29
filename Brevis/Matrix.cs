using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Matrix
    {
        private readonly double[,] _matrix;
        public Matrix(int rows, int columns)
        {
            this._matrix = new double[rows,columns];
        }

        public void SetValue(int row, int column, double value)
        {
            this._matrix[row, column] = value;
        }

        public double GetValue(int row, int column)
        {
            return this._matrix[row, column];
        }

        public int Rows => this._matrix.GetLength(0);
        public int Columns => this._matrix.GetLength(1);

        public static Matrix Multiply(Matrix a, Matrix b)
        {
            if(a.Columns != b.Rows)
                throw new ArgumentException("Invalid matrices dimensions");
            var result = new Matrix(a.Rows, b.Columns);
            for (var r = 0; r < result.Rows; r++)
            {
                for (var c = 0; c < result.Columns; c++)
                {
                    var value = 0.0;
                    for (var i = 0; i < a.Columns; i++)
                        value += a.GetValue(r, i) * b.GetValue(i, c);
                    result.SetValue(r, c, value);
                }
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Matrix;
            if (item == null || item.Rows != this.Rows || item.Columns != this.Columns)
                return false;
            for (var r = 0; r < this.Rows; r++)
                for (var c = 0; c < this.Columns; c++)
                    if (this.GetValue(r, c) != item.GetValue(r, c))
                        return false;
            return true;
        }

        public static Matrix PerspectiveProjectionMatrix(double near, double far, double fov=45, double a=1)
        {
            var e = 1.0 / Math.Tan(Math.PI * fov / 360);
            var result = new Matrix(4, 4);
            result.SetValue(0, 0, e);
            result.SetValue(1, 1, e/a);
            result.SetValue(2, 2, (far+near)/(near-far));
            result.SetValue(2, 3, (2 * far * near)/(near - far));
            result.SetValue(3, 2, -1.0);
            return result;
        }

        public static Matrix ViewMatrix(
            double cameraPositionX, double cameraPositionY, double cameraPositionZ,
            double cameraTargetX, double cameraTargetY, double cameraTargetZ,
            double upVersorX, double upVersorY, double upVersorZ
        )
        {
            /*
             * TODO: this is messy implementation. Clean it up.
             */
            var viewMatrix = new Matrix(4, 4);

            var b13 = cameraPositionX - cameraTargetX;
            var c13 = cameraPositionY - cameraTargetY;
            var d13 = cameraPositionZ - cameraTargetZ;
            // normalize
            var k = Math.Sqrt(Math.Pow(b13, 2) + Math.Pow(c13, 2) + Math.Pow(d13, 2));
            var b14 = b13 / k;
            var c14 = c13 / k;
            var d14 = d13 / k;
            // xAxis = UpVector * zAxis
            var b17 = upVersorY * d14 - upVersorZ * c14;
            var c17 = -1.0 * upVersorX * d14 + upVersorZ * b14;
            var d17 = upVersorX * c14 - upVersorY * b14;
            // normalize
            var l = Math.Sqrt(Math.Pow(b17, 2) + Math.Pow(c17, 2) + Math.Pow(d17, 2));
            var b18 = b17 / l;
            var c18 = c17 / l;
            var d18 = d17 / l;
            // yAxis = zAxis * xAxis
            var b21 = c14 * d18 - d14 * c18;
            var c21 = -1.0 * b14 * d18 + d14 * b18;
            var d21 = b14 * c18 - c14 * b18;

            viewMatrix.SetValue(0, 0, b18);
            viewMatrix.SetValue(0, 1, b21);
            viewMatrix.SetValue(0, 2, b14);
            viewMatrix.SetValue(0, 3, cameraPositionX);

            viewMatrix.SetValue(1, 0, c18);
            viewMatrix.SetValue(1, 1, c21);
            viewMatrix.SetValue(1, 2, c14);
            viewMatrix.SetValue(1, 3, cameraPositionY);

            viewMatrix.SetValue(2, 0, d18);
            viewMatrix.SetValue(2, 1, d21);
            viewMatrix.SetValue(2, 2, d14);
            viewMatrix.SetValue(2, 3, cameraPositionZ);

            viewMatrix.SetValue(3, 0, 0);
            viewMatrix.SetValue(3, 1, 0);
            viewMatrix.SetValue(3, 2, 0);
            viewMatrix.SetValue(3, 3, 1);
            return viewMatrix.Inverse();
        }

        private Matrix Inverse()
        {
            if(this.Rows != 4 || this.Columns != 4)
                throw new ArgumentException("Currently implemented only for 4x4 matrices");
            var m = new Matrix4x4(
                (float)this.GetValue(0, 0), (float)this.GetValue(0, 1), (float)this.GetValue(0, 2), (float)this.GetValue(0, 3),
                (float)this.GetValue(1, 0), (float)this.GetValue(1, 1), (float)this.GetValue(1, 2), (float)this.GetValue(1, 3),
                (float)this.GetValue(2, 0), (float)this.GetValue(2, 1), (float)this.GetValue(2, 2), (float)this.GetValue(2, 3),
                (float)this.GetValue(3, 0), (float)this.GetValue(3, 1), (float)this.GetValue(3, 2), (float)this.GetValue(3, 3)
            );
            Matrix4x4.Invert(m, out var result4x4);
            var result = new Matrix(4, 4);
            result.SetValue(0, 0, result4x4.M11);
            result.SetValue(0, 1, result4x4.M12);
            result.SetValue(0, 2, result4x4.M13);
            result.SetValue(0, 3, result4x4.M14);
            result.SetValue(1, 0, result4x4.M21);
            result.SetValue(1, 1, result4x4.M22);
            result.SetValue(1, 2, result4x4.M23);
            result.SetValue(1, 3, result4x4.M24);
            result.SetValue(2, 0, result4x4.M31);
            result.SetValue(2, 1, result4x4.M32);
            result.SetValue(2, 2, result4x4.M33);
            result.SetValue(2, 3, result4x4.M34);
            result.SetValue(3, 0, result4x4.M41);
            result.SetValue(3, 1, result4x4.M42);
            result.SetValue(3, 2, result4x4.M43);
            result.SetValue(3, 3, result4x4.M44);
            return result;
        }
    }
}

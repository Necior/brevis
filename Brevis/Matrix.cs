using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Matrix ViewMatrix()
        {
            var viewMatrix = new Matrix(4, 4);
            viewMatrix.SetValue(0, 0, -0.447);
            viewMatrix.SetValue(0, 1, 0.894);
            viewMatrix.SetValue(0, 2, 0);
            viewMatrix.SetValue(0, 3, -0.447);

            viewMatrix.SetValue(1, 0, -0.458);
            viewMatrix.SetValue(1, 1, -0.229);
            viewMatrix.SetValue(1, 2, 0.859);
            viewMatrix.SetValue(1, 3, -0.315);

            viewMatrix.SetValue(2, 0, 0.768);
            viewMatrix.SetValue(2, 1, 0.384);
            viewMatrix.SetValue(2, 2, 0.512);
            viewMatrix.SetValue(2, 3, -4.353);

            viewMatrix.SetValue(3, 0, 0);
            viewMatrix.SetValue(3, 1, 0);
            viewMatrix.SetValue(3, 2, 0);
            viewMatrix.SetValue(3, 3, 1);
            return viewMatrix;
        }
    }
}

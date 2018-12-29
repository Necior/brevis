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
    }
}

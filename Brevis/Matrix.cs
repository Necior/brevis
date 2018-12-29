using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brevis
{
    public class Matrix
    {
        public readonly double[,] _matrix;
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
    }
}

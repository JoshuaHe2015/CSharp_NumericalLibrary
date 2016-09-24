using System;
using NumericalLibrary.Extensions;
namespace NumericalLibrary.LinearAlgebra.Matrices
{
    public static class SpecialMatrices
    {
        public static Matrix Zero(int row, int col)
        {
            return new Matrix(row, col);
        }
        public static Matrix Eye(int row, int col)
        {
            var M = new Matrix(row, col);
            int n = Math.Min(M.RowCount, M.ColumnCount);
            for (int i = 0; i < n; i++)
                M[i, i] = 1;
            return M;
        }
        public static Matrix Ones(int row, int col)
        {
            var M = new Matrix(row, col);
            M.Values.MapInPlace(i => 1.0);
            return M;
        }
        public static Matrix Rand(int row, int col)
        {
            var rnd = new Random();
            var M = new Matrix(row, col);
            M.Values.MapInPlace(rnd, (x, y) => rnd.NextDouble());
            return M;
        }
        public static Matrix Zero(int size)
        {
            return Zero(size, size);
        }
        public static Matrix Eye(int size)
        {
            return Eye(size, size);
        }
        public static Matrix Ones(int size)
        {
            return Ones(size, size);
        }
        public static Matrix Rand(int size)
        {
            return Rand(size, size);
        }
    }
}

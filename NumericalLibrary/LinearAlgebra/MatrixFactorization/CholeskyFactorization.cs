using System;
using NumericalLibrary.LinearAlgebra.Matrices;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class CholeskyFactorization
    {
        public double Determinant { get; private set; }
        public Matrix Factor { get; private set; }
        public CholeskyFactorization(Matrix Mat, bool CreateNewInstance = true)
        {
            Solve(CreateNewInstance ? +Mat : Mat);
        }
        private void Solve(Matrix Mat)
        {
            var mat = Mat.Values;
            if (mat[0] <= 0.0)
                throw new ArgumentOutOfRangeException("Cholesky factorization unsolved");
            mat[0] = Math.Sqrt(mat[0]);
            double d = mat[0];
            int nRows = Mat.RowCount;
            int nCols = Mat.ColumnCount;
            for (int i = 1; i < nCols; i++)
                Mat[i, 0] /= mat[0];
            for (int j = 1; j < nCols; j++)
            {
                int u = j * nCols + j;
                for (int k = 0; k < j; k++)
                    mat[u] -= CMath.Square(Mat[j, k]);
                if (mat[u] <= 0.0)
                    throw new ArgumentOutOfRangeException("Cholesky factorization unsolved");
                mat[u] = Math.Sqrt(mat[u]);
                d *= mat[u];
                for (int i = j + 1; i < nCols; i++)
                {
                    int v = i * nCols + j;
                    for (int k = 0; k < j; k++)
                        mat[v] -= Mat[i, k] * Mat[j, k];
                    mat[v] /= mat[u];
                }
            }
            Determinant = d * d;
            for (int i = 0; i < nCols - 1; i++)
                for (int j = i + 1; j < nCols; j++)
                    Mat[i, j] = 0.0;
            Factor = Mat;
        }
    }
}

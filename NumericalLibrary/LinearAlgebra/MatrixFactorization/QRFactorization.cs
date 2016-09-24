using System;
using NumericalLibrary.LinearAlgebra.Matrices;
using Utility = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class QRFactorization
    {
        const double eps = 1e-09;
        public Matrix Q { get; private set; }
        public Matrix R { get; private set; }
        public QRFactorization(Matrix Mat, bool CreateNewInstance = true)
        {
            Solve(CreateNewInstance ? +Mat : Mat);
        }
        private void Solve(Matrix mat)
        {
            int nRows = mat.RowCount;
            int nCols = mat.ColumnCount;
            if (nRows < nCols)
                throw new ArgumentOutOfRangeException("To make QR factorization,the rows count of matrix must greater than or equal columns count of it");
            Q = SpecialMatrices.Eye(nRows, nRows);
            R = mat;
            int n = nCols;
            if (nRows == nCols) n--;
            double d, w;
            int p, u, v;
            double alpha, t;
            double[] q = Q.Values;
            double[] r = R.Values;
            for (int k = 0; k < n; k++)
            {
                d = 0.0;
                u = k * nCols + k;
                for (int i = k; i < nRows; i++)
                {
                    v = i * nCols + k;
                    w = Math.Abs(r[v]);
                    if (w > d) d = w;
                }

                alpha = 0.0;
                for (int i = k; i < nRows; i++)
                {
                    v = i * nCols + k;
                    t = r[v] / d;
                    alpha += t * t;
                }

                if (r[u] > 0.0) d = -d;

                alpha = d * Math.Sqrt(alpha);
                if (Math.Abs(alpha) < eps)
                    throw new Exception("QR Factorization unsolved");

                d = Math.Sqrt(2.0 * alpha * (alpha - r[u]));
                if (d > eps)
                {
                    r[u] = (r[u] - alpha) / d;
                    for (int i = k + 1; i < nRows; i++)
                        R[i, k] /= d;

                    for (int j = 0; j < nRows; j++)
                    {
                        t = 0.0;
                        for (int m = k; m < nRows; m++)
                            t += R[m, k] * q[m * nRows + j];

                        for (int i = k; i < nRows; i++)
                        {
                            p = i * nRows + j;
                            q[p] -= 2.0 * t * R[i, k];
                        }
                    }

                    for (int j = k + 1; j < nCols; j++)
                    {
                        t = 0.0;
                        for (int m = k; m < nRows; m++)
                            t += R[m, k] * R[m, j];

                        for (int i = k; i < nRows; i++)
                            R[i, j] -= 2.0 * t * R[i, k];
                    }

                    r[u] = alpha;
                    for (int i = k + 1; i < nRows; i++)
                        R[i, k] = 0.0;
                }

            }

            for (int i = 0; i < nRows - 1; i++)
                for (int j = i + 1; j < nRows; j++)
                {
                    p = i * nRows + j;
                    u = j * nRows + i;
                    Utility.Swap(q, u, p);
                }
        }

    }
}

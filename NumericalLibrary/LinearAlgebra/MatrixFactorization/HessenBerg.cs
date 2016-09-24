using System;
using NumericalLibrary.LinearAlgebra.Matrices;
using U = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class HessenBerg
    {
        public Matrix HessenBergMatrix { get; private set; }
        public HessenBerg(Matrix Mat, bool CreateNewInstance = true)
        {
            HessenBergMatrix = Solve(CreateNewInstance ? +Mat : Mat);
        }
        private Matrix Solve(Matrix Mat)
        {
            int nRows = Mat.RowCount;
            int nCols = Mat.ColumnCount;
            if (nRows != nCols) throw new ArgumentOutOfRangeException("HessenBerg matrix must be derived from a square matrix");
            Matrix H = Mat;
            for (int k = 1; k < nCols - 1; k++)
            {
                double max = 0.0;
                int i_max = 0;
                for (int j = k; j < nCols; j++)
                {
                    double t = H[j, k - 1];
                    if (Math.Abs(t) > Math.Abs(max))
                    {
                        max = t;
                        i_max = j;
                    }
                }

                if (max != 0.0)
                {
                    if (i_max != k)
                    {
                        for (int j = k - 1; j < nCols; j++)
                        {
                            int u = i_max * nCols + j;
                            int v = k * nCols + j;
                            U.Swap(H.Values, u, v);
                        }
                        U.SwapColumn(H.Values, i_max, k, nRows, nCols);
                    }

                    for (i_max = k + 1; i_max < nCols; i_max++)
                    {
                        double t = H[i_max, k - 1] / max;
                        H[i_max, k - 1] = 0.0;
                        for (int j = k; j < nCols; j++)
                            H[i_max, j] -= t * H[k, j];

                        for (int j = 0; j < nCols; j++)
                            H[j, k] += t * H[j, i_max];
                    }
                }
            }
            return H;
        }
    }
}

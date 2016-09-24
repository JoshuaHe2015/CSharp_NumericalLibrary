using System;
using System.Linq;
using NumericalLibrary.LinearAlgebra.Matrices;
using Utility = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class LUFactorization
    {
        public Matrix L { get; private set; }
        public Matrix U { get; private set; }
        public Matrix P { get; private set; }

        internal int SwapTimes { get; set; }
        public LUFactorization(Matrix Mat, bool CreateNewInstance = true)
        {
            Solve(CreateNewInstance ? +Mat : Mat);
        }
        //private void Solve(Matrix mat)
        //{
        //    int nRows = mat.RowCount;
        //    int nCols = mat.ColumnCount;
        //    if (nRows != nCols)
        //        throw new ArgumentOutOfRangeException("LU Decomposition only support square matrix");
        //    L = new Matrix(nCols, nCols);
        //    U = mat;
        //    var pnRows = Enumerable.Range(0, nCols).ToArray();
        //    int w = 0, v;
        //    int SwapTimes = 1;
        //    for (int k = 0; k < nCols - 1; k++)
        //    {
        //        double p = 0.0;
        //        for (int i = k; i < nRows; i++)  //pivot
        //        {
        //            double d = Math.Abs(U[i, k]);
        //            if (d > p)
        //            {
        //                p = d;
        //                w = i;
        //            }
        //        }

        //        if (p == 0)
        //            throw new Exception("LU Decomposition unsolved");

        //        Utility.Swap(pnRows, k, w);

        //        for (int i = 0; i < k; i++)
        //            Utility.Swap(L.Values, k * nCols + i, w * nCols + i);

        //        if (k != w) SwapTimes *= -1;//Swap row will change the value of determinant
        //        Utility.SwapRow(U.Values, k, w, nCols);
        //        for (int i = k + 1; i < nRows; i++)
        //        {
        //            v = i * nCols + k;
        //            L.Values[v] = U.Values[v] / U[k, k];
        //            for (int j = k; j < nCols; j++)
        //            {
        //                v = i * nCols + j;
        //                U.Values[v] -= L[i, k] * U[k, j];
        //            }
        //        }
        //    }
        //}
        internal double Det()
        {
            double det = SwapTimes;
            int n = U.ColumnCount;
            for (int i = 0; i < n; i++)
                det *= U[i, i];
            return det;
        }

        private void Solve(Matrix mat)
        {
            int nRows = mat.RowCount;
            int nCols = mat.ColumnCount;
            if (nRows != nCols)
                throw new ArgumentOutOfRangeException("LU Decomposition only support square matrix");
            int n = nRows;
             P = SpecialMatrices.Eye(n);
            SwapTimes = 1;
            for (int i = 0; i < n; i++) //pivot
            {
                double max = mat[i, i];
                int max_row = i;
                for (int j = i; j < n; j++)
                {
                    if (mat[j, i] > max)
                    {
                        max = mat[j, i];
                        max_row = j;
                    }
                }

                if (i != max_row)
                {
                    Utility.SwapRow(P.Values, i, max_row, nCols);
                    SwapTimes *= (-1);
                }
            }

            L = new Matrix(n);
            U = new Matrix(n);
            Matrix A = P * mat;
            for (int j = 0; j < n; j++)
            {
                L[j, j] = 1.0;
                for (int i = 0; i <= j; i++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < i; k++)
                        sum += U[k, j] * L[i, k];
                    U[i, j] = A[i, j] - sum;
                }

                for (int i = j; i < n; i++)
                {
                    double sum = 0.0;
                    for (int k = 0; k < j; k++)
                        sum += U[k, j] * L[i, k];
                    L[i, j] = (A[i, j] - sum) / U[j, j];
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NumericalLibrary.LinearAlgebra.Matrices
{
    internal static class MatrixUtility
    {
        internal static void Swap<T>(T[] x, int i, int j)
        {
            T z = x[i];
            x[i] = x[j];
            x[j] = z;
        }
        internal static void SwapRow<T>(T[] x, int i, int j, int nCols)
        {
            int u = i * nCols, v = j * nCols;
            for (int m = 0; m < nCols; m++)
            {
                Swap(x, u, v);
                u++;
                v++;
            }
        }
        internal static void SwapColumn<T>(T[] x, int i, int j, int nRows, int nCols)
        {
            int u = i, v = j;
            for (int m = 0; m < nRows; m++)
            {
                Swap(x, u, v);
                u += nCols;
                v += nCols;
            }
        }
    }
}

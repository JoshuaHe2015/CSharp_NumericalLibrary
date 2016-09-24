using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class SVD
    {
        const double eps = 1e-09;
        public SVD() { }
        public void SplitUV(Matrix mat)
        {
            double[] fg = new double[2];
            double[] cs = new double[2];
            int nRows = mat.RowCount;
            int nCols = mat.ColumnCount;
            Matrix mtxU = new Matrix(nRows), mtxV = new Matrix(nCols);
            int ka = Math.Max(nRows, nCols) + 1;
            double[] s = new double[ka];
            double[] e = new double[ka];
            double[] w = new double[ka];

            //iteration 60
            int it = 60;
            int k = nCols;

            if (nRows - 1 < nCols)
                k = nRows - 1;
            int l = nRows;
            if (nCols - 2 < nRows)
                l = nRows - 2;
            if (l < 0) l = 0;

            int ll = k;
            if (l > k)
                ll = l;
            if (ll >= 1)
            {
                for (int kk = 1; kk <= ll; kk++)
                {
                    if (kk <= k)
                    {
                        double d = 0.0;
                        for (int i = kk; i <= nRows; i++)
                        {
                            int ix = (i - 1) * nCols + kk - 1;
                            d += mat.Values[ix] * mat.Values[ix];
                        }

                        s[kk - 1] = Math.Sqrt(d);
                        if (s[kk - 1] != 0.0)
                        {
                            int ix = (kk - 1) * nCols + kk - 1;
                            if (mat.Values[ix] != 0.0)
                            {
                                s[kk - 1] = Math.Sqrt(s[kk - 1]);
                                if (mat.Values[ix] < 0.0)
                                    s[kk - 1] = -s[kk - 1];
                            }

                            for (int i = kk; i <= nRows; i++)
                            {
                                int iy = (i - 1) * nCols + kk - 1;
                                mat.Values[iy] /= s[kk - 1];
                            }

                            mat.Values[ix] = 1.0 + mat.Values[ix];
                        }
                        s[kk - 1] = -s[kk - 1];
                    }

                    if (nCols >= kk + 1)
                    {
                        for (int j = kk + 1; j <= nCols; j++)
                        {
                            if ((kk <= k) && (s[kk - 1] != 0.0))
                            {
                                double d = 0.0;
                                for (int i = kk; i <= nRows; i++)
                                {
                                    int ix = (i - 1) * nCols + kk - 1;
                                    int iy = (i - 1) * nCols + j - 1;
                                    d += mat.Values[ix] * mat.Values[iy];
                                }

                                d = -d / mat.Values[(kk - 1) * nCols + kk - 1];

                                for (int i = kk; i <= nRows; i++)
                                {
                                    int ix = (i - 1) * nCols + j - 1;
                                    int iy = (i - 1) * nCols + kk - 1;
                                    mat.Values[ix] = mat.Values[ix] + d * mat.Values[iy];
                                }

                            }

                            e[j - 1] = mat.Values[(kk - 1) * nCols + j - 1];
                        }
                    }

                    if (kk <= k)
                    {
                        for (int i = kk; i <= nRows; i++)
                        {
                            int ix = (i - 1) * nRows + kk - 1;
                            int iy = (i - 1) * nCols + kk - 1;
                            mtxU.Values[ix] = mat.Values[iy];
                        }
                    }






                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
using U = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
using NumericalLibrary.Extensions;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public class EVD
    {
        public double[] RealPart { get; private set; }
        public double[] ImaginaryPart { get; private set; }
        public Complex[] ComplexResult
        {
            get
            {
                return RealPart.Map2(ImaginaryPart, (x, y) => new Complex(x, y));
            }
        }
        public EVD(Matrix Mat, int MaxTimes = 100, double Precision = 0.0001, bool CreateNewInstance = true)
        {
            var matHB = new HessenBerg(Mat, CreateNewInstance).HessenBergMatrix;
            Eigen(matHB, MaxTimes, Precision);
        }
        private void Eigen(Matrix matHB, int MaxTimes, double Precision)
        {
            int nRows = matHB.RowCount;
            int nCols = matHB.ColumnCount;
            var Real = new double[nCols];
            var Imaginary = new double[nCols];
            var IterationTimes = 0;
            int m = nCols;
            double p = 0.0, q = 0.0, r = 0.0, xy = 0.0;
            double[] mat = matHB.Values;
            while (m != 0)
            {
                int t = m - 1;
                while ((t > 0) &&
                    (Math.Abs(matHB[t, t - 1])
                    > Precision * (Math.Abs(matHB[t - 1, t - 1]) + Math.Abs(matHB[t, t]))))
                    t--;

                int ii = (m - 1) * nCols + (m - 1);
                int jj = (m - 1) * nCols + (m - 2);
                int kk = (m - 2) * nCols + (m - 1);
                int tt = (m - 2) * nCols + (m - 2);

                if (t == m - 1)
                {
                    Real[m - 1] = matHB[m - 1, m - 1];
                    Imaginary[m - 1] = 0.0;
                    m--;
                    IterationTimes = 0;
                }
                else if (t == m - 2)
                {
                    double b = -(mat[ii] + mat[tt]);
                    double c = mat[ii] * mat[tt] - mat[jj] * mat[kk];
                    double w = b * b - 4.0 * c;
                    double y = Math.Sqrt(Math.Abs(w));

                    if (w > 0.0)
                    {
                        xy = 1.0;
                        if (b < 0.0)
                            xy = -1.0;
                        Real[m - 1] = (-b - xy * y) / 2.0;
                        Real[m - 2] = c / Real[m - 1];
                        Imaginary[m - 1] = 0.0;
                        Imaginary[m - 2] = 0.0;
                    }
                    else
                    {
                        Real[m - 1] = -b / 2.0;
                        Real[m - 2] = Real[m - 1];
                        Imaginary[m - 1] = y / 2.0;
                        Imaginary[m - 2] = -Imaginary[m - 1];
                    }

                    m -= 2;
                    IterationTimes = 0;
                }
                else
                {
                    if (IterationTimes > MaxTimes)
                        throw new ArithmeticException(string.Format("Still Unsolved after {0} iteration", IterationTimes));

                    IterationTimes++;

                    for (int j = t + 2; j < m; j++)
                        matHB[j, j - 2] = 0.0;
                    for (int j = t + 3; j < m; j++)
                        matHB[j, j - 3] = 0.0;

                    for (int k = t; k < m - 1; k++)
                    {
                        if (k != t)
                        {
                            p = matHB[k, k - 1];
                            q = matHB[k + 1, k - 1];
                            r = 0.0;
                            if (k != m - 2)
                                r = matHB[k + 2, k - 1];
                        }
                        else
                        {
                            double x = mat[ii] + mat[tt];
                            double y = mat[tt] * mat[ii] - mat[kk] * mat[jj];

                            ii = t * nCols + t;
                            jj = t * nCols + t + 1;
                            kk = (t + 1) * nCols + t;
                            tt = (t + 1) * nCols + t + 1;

                            p = mat[ii] * (mat[ii] - x)
                                + mat[jj] * mat[kk] + y;
                            q = mat[kk] * (mat[ii] + mat[tt] - x);
                            r = mat[kk] * matHB[t + 2, t + 1];
                        }

                        if ((Math.Abs(p) + Math.Abs(q) + Math.Abs(r)) != 0.0)
                        {
                            xy = 1.0;
                            if (p < 0.0)
                                xy = -1.0;
                            double s = xy * Math.Sqrt(p * p + q * q + r * r);
                            if (k != t)
                                matHB[k, k - 1] = -s;

                            double e = -q / s;
                            double f = -r / s;
                            double x = -p / s;
                            double y = -x - f * r / (p + s);
                            double g = e * r / (p + s);
                            double z = -x - e * q / (p + s);

                            for (int j = k; j < m; j++)
                            {
                                ii = k * nCols + j;
                                jj = (k + 1) * nCols + j;
                                p = x * mat[ii] + e * mat[jj];
                                q = e * mat[ii] + y * mat[jj];
                                r = f * mat[ii] + g * mat[jj];

                                if (k != m - 2)
                                {
                                    kk = (k + 2) * nCols + j;
                                    p += f * mat[kk];
                                    q += g * mat[kk];
                                    r += z * mat[kk];
                                    mat[kk] = r;
                                }

                                mat[jj] = q;
                                mat[ii] = p;
                            }

                            int u = k + 3;
                            if (u >= m - 1)
                                u = m - 1;

                            for (int i = t; i <= u; i++)
                            {
                                ii = i * nCols + k;
                                jj = i * nCols + k + 1;
                                p = x * mat[ii] + e * mat[jj];
                                q = e * mat[ii] + y * mat[jj];
                                r = f * mat[ii] + g * mat[jj];

                                if (k != m - 2)
                                {
                                    kk = i * nCols + k + 2;
                                    p += f * mat[kk];
                                    q += g * mat[kk];
                                    r += z * mat[kk];
                                    mat[kk] = r;
                                }

                                mat[jj] = q;
                                mat[ii] = p;
                            }
                        }
                    }
                }
            }
            RealPart = Real;
            ImaginaryPart = Imaginary;
        }
    }
}

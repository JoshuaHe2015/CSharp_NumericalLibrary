using System;
using System.Text;
using System.Collections;
using U = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
using System.Collections.Generic;
using NumericalLibrary.Extensions;
using NumericalLibrary.LinearAlgebra.MatrixFactorization;
namespace NumericalLibrary.LinearAlgebra.Matrices
{
    public sealed class Matrix : IEnumerable<double>
    {
        readonly double[] values;
        public double[] Values
        {
            get { return values; }
        }

        readonly int nRows;
        public int RowCount { get { return nRows; } }

        readonly int nCols;
        public int ColumnCount { get { return nCols; } }

        public double this[int Row, int Column]
        {
            get { return values[Row * nCols + Column]; }
            set { values[Row * nCols + Column] = value; }
        }

        public Matrix(int rowCount, int columnCount, double[] values)
        {
            if (rowCount * columnCount != values.Length) throw new ArgumentOutOfRangeException("matrix values length error");
            this.nRows = rowCount;
            this.nCols = columnCount;
            this.values = values;
        }
        public Matrix(int rowCount, int columnCount)
        {
            this.nRows = rowCount;
            this.nCols = columnCount;
            this.values = new double[rowCount * columnCount];
        }
        public Matrix(int size)
        {
            this.nRows = size;
            this.nCols = size;
            this.values = new double[size * size];
        }
        public static Matrix Create(int rowCount, int columnCount, double[] values)
        {
            return new Matrix(rowCount, columnCount, values);
        }
        public static Matrix OfArray(double[,] array)
        {
            var result = new Matrix(array.GetLength(0), array.GetLength(1));
            Buffer.BlockCopy(array, 0, result.values, 0, sizeof(double) * array.Length);
            return result;
        }
        public Matrix Fill(params double[] values)
        {
            if (values.Length > this.values.Length)
                throw new ArgumentOutOfRangeException();
            else
            {
                Array.Copy(values, this.values, values.Length);
                return this;
            }
        }
        #region Unary Operations
        public static Matrix operator +(Matrix A)
        {
            return new Matrix(A.nRows, A.nCols, A.values.Copy());
        }
        public static Matrix operator -(Matrix A)
        {
            return new Matrix(A.nRows, A.nCols, A.values.Map(i => -i));
        }
        #endregion

        #region Binary Operations
        public static Matrix operator +(Matrix A, Matrix B)
        {
            return A.Add(B);
        }
        public static Matrix operator -(Matrix A, Matrix B)
        {
            return A.Subtract(B);
        }
        public static Matrix operator *(Matrix A, Matrix B)
        {
            return A.Multiply(B);
        }
        public static Matrix operator /(Matrix A, Matrix B)
        {
            return A.Divide(B);
        }
        #endregion

        #region Scalar Operations
        public static Matrix operator +(double Scalar, Matrix M)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                (x, scalar) => (scalar + x)));
        }
        public static Matrix operator +(Matrix M, double Scalar)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                  (x, scalar) => (x + scalar)));
        }
        public static Matrix operator -(double Scalar, Matrix M)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                (x, scalar) => (scalar - x)));
        }
        public static Matrix operator -(Matrix M, double Scalar)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                  (x, scalar) => (x - scalar)));
        }
        public static Matrix operator *(double Scalar, Matrix M)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                (x, scalar) => (scalar * x)));
        }
        public static Matrix operator *(Matrix M, double Scalar)
        {
            return new Matrix(M.nRows, M.nCols, M.values.Map(Scalar,
                  (x, scalar) => (x * scalar)));
        }
        public static Matrix operator /(double Scalar, Matrix M)
        {
            return Scalar * M.Inverse();
        }
        public static Matrix operator /(Matrix M, double Scalar)
        {
            return M * (1.0 / Scalar);
        }
        #endregion

        public Matrix Add(Matrix Other)
        {
            if (nRows == Other.nRows && nCols == Other.nCols)
            {
                var result = new Matrix(nRows, nCols);
                for (int i = values.Length - 1; i >= 0; i--)
                    result.values[i] = values[i] + Other.values[i];
                return result;
            }
            else throw new ArgumentOutOfRangeException(
               string.Format("[Matrix Add Error] (Left: row count = {0} column count = {1}) (Right: row count = {2} column count = {3})"
               , nRows, nCols, Other.nRows, Other.nCols));
        }
        public Matrix Subtract(Matrix Other)
        {
            if (nRows == Other.nRows && nCols == Other.nCols)
            {
                var result = new Matrix(nRows, nCols);
                for (int i = values.Length - 1; i >= 0; i--)
                    result.values[i] = values[i] - Other.values[i];
                return result;
            }
            else throw new ArgumentOutOfRangeException(
             string.Format("[Matrix Subtract Error] (Left: row count = {0} column count = {1}) (Right: row count = {2} column count = {3})"
             , nRows, nCols, Other.nRows, Other.nCols));
        }
        public Matrix Multiply(Matrix Other)
        {
            if (nCols != Other.nRows)
                throw new ArgumentOutOfRangeException(
                    string.Format("[Matrix Multiply Error] (Left: row count = {0} column count = {1}) (Right: row count = {2} column count = {3})"
                    , nRows, nCols, Other.nRows, Other.nCols));
            //The columns' count of the first matrix must agree with the rows' count of the second matrix when performing matrix multiplication
            /*
            [A][B][C]   *   [G][H]      [A * G + B * I + C * K][A * H + B * J + C * L]
            [D][E][F]       [I][J]  =   [D * G + E * I + F * K][D * H + E * J + F * L]
                            [K][L]
            */
            var ans = new double[nRows * Other.nCols];
            int inA = 0, inB = 0, knB;
            double inAk;
            for (int i = 0; i < nRows; i++)
            {
                knB = 0;/*int knB = k * nB;*/
                for (int k = 0; k < nCols; k++)
                {
                    inAk = values[inA + k];
                    for (int j = 0; j < Other.nCols; j++)
                        ans[inB + j] += inAk * Other.values[knB + j];
                    knB += Other.nCols;
                }
                inA += nCols;
                inB += Other.nCols;
            }
            return new Matrix(nRows, Other.nCols, ans);
        }
        public Matrix Divide(Matrix Other)
        {
            return Multiply(Other.Inverse());
        }
        public Matrix Inverse()
        {
            if (nRows != nCols) throw new ArgumentOutOfRangeException("Only square matrix has inverse matrix");
            var pnRow = new int[nCols];
            var pnCol = new int[nCols];
            double d = 0.0, p = 0.0;
            int k, u, v;
            var result = values.Copy();

            for (k = 0; k < nCols; k++)
            {
                d = 0.0;
                for (int i = k; i < nCols; i++)
                    for (int j = k; j < nCols; j++)
                    {
                        u = i * nCols + j;
                        p = Math.Abs(result[u]);
                        if (p > d)//pivot
                        {
                            d = p;
                            pnRow[k] = i;
                            pnCol[k] = j;
                        }
                    }

                if (d == 0.0) throw new Exception("Inverse matrix unsolved");

                if (pnRow[k] != k) U.SwapRow(result, k, pnRow[k], nCols);
                if (pnCol[k] != k) U.SwapColumn(result, k, pnCol[k], nRows, nCols);

                v = k * nCols + k;
                result[v] = 1.0 / result[v];
                for (int j = 0; j < nCols; j++)
                    if (j != k)
                    {
                        u = k * nCols + j;
                        result[u] *= result[v];
                    }

                for (int i = 0; i < nCols; i++)
                    if (i != k)
                        for (int j = 0; j < nCols; j++)
                            if (j != k)
                            {
                                u = i * nCols + j;
                                result[u] -= result[i * nCols + k] * result[k * nCols + j];
                            }

                for (int i = 0; i < nCols; i++)
                    if (i != k)
                    {
                        u = i * nCols + k;
                        result[u] *= -result[v];
                    }
            }
            //restore order
            for (k = nCols - 1; k >= 0; k--)
            {
                if (pnCol[k] != k) U.SwapRow(result, k, pnCol[k], nCols);
                if (pnRow[k] != k) U.SwapColumn(result, k, pnRow[k], nRows, nCols);
            }
            return new Matrix(nRows, nCols, result);
        }
        public Matrix DotMultiply(Matrix Other)
        {
            if (nRows == Other.nRows && nCols == Other.nCols)
            {
                var result = new Matrix(nRows, nCols);
                for (int i = values.Length - 1; i >= 0; i--)
                    result.values[i] = values[i] * Other.values[i];
                return result;
            }
            else throw new ArgumentOutOfRangeException(
               string.Format("[Matrix Dot Multiply Error] (Left: row count = {0} column count = {1}) (Right: row count = {2} column count = {3})"
               , nRows, nCols, Other.nRows, Other.nCols));
        }
        public Matrix DotDivide(Matrix Other)
        {
            if (nRows == Other.nRows && nCols == Other.nCols)
            {
                var result = new Matrix(nRows, nCols);
                for (int i = values.Length - 1; i >= 0; i--)
                    result.values[i] = values[i] / Other.values[i];
                return result;
            }
            else throw new ArgumentOutOfRangeException(
               string.Format("[Matrix Dot Divide Error] (Left: row count = {0} column count = {1}) (Right: row count = {2} column count = {3})"
               , nRows, nCols, Other.nRows, Other.nCols));
        }
        public override string ToString()
        {
            var s = new StringBuilder();
            int col = nCols - 1;
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < col; j++)
                    s.Append(this[i, j].ToString()).Append("  ");
                s.AppendLine(this[i, col].ToString());
            }
            return s.ToString();
        }
        public IEnumerator<double> GetEnumerator()
        {
            foreach (var item in values)
                yield return item;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in values)
                yield return item;
        }
        public Matrix Transpose()
        {
            var result = new Matrix(nRows, nCols);
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    result[j, i] = this[i, j];
            return result;
        }
        public double Det()
        {
            return new LUFactorization(this).Det();
        }
    }
}

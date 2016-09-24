using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
using System.IO;
using NumericalLibrary.Extensions;

namespace NumericalLibrary.LinearAlgebra.MatrixIO
{
    public static class LoadMatrix
    {
        public static Matrix FromTxtFile(string path, params char[] separators)
        {
            var list = new List<double>();
            int nRows = 0, nCols = 0;
            using (var sr = new StreamReader(path))
            {
                var line = sr.ReadLine();
                list.AddRange(line.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse));
                nRows++;
                nCols = list.Count;
                while (sr.Peek() != -1)
                {
                    line = sr.ReadLine();
                    var arr = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length != nCols)
                        throw new ArgumentOutOfRangeException("Columns Count Error");
                    list.AddRange(arr.Select(double.Parse));
                    nRows++;
                }
            }
            return new Matrix(nRows, nCols, list.Copy2Array());
        }
        public static Matrix FromMatrixMarket(string path)
        {
            using (var sr = new StreamReader(path, Encoding.ASCII))
            {
                SkipHeaders(sr);
                var arr = sr.ReadLine().Split(' ', '\t');
                if (arr.Length == 2)
                {
                    int[] size = arr.Map(int.Parse);
                    var Mat = new Matrix(size[0], size[1]);
                    int i = 0;
                    var mat = Mat.Values;
                    while (sr.Peek() != -1)
                    {
                        mat[i] = double.Parse(sr.ReadLine());
                        i++;
                    }
                    return Mat;
                }
                else throw new NotSupportedException();
            }
        }
        private static void SkipHeaders(StreamReader sr)
        {
            while (sr.Peek() != -1)
            {
                if (sr.Peek() == '%')
                    sr.ReadLine();
                else
                    break;
            }
        }
    }
}

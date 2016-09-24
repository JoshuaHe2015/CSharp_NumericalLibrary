using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
using U = NumericalLibrary.LinearAlgebra.Matrices.MatrixUtility;
using System.IO;
namespace NumericalLibrary.LinearAlgebra.MatrixIO
{
    public static class WriteMatrix
    {
        public static void WriteTxtFile(this Matrix matrix, string path, char separator)
        {
            int nRows = matrix.RowCount;
            int nCols = matrix.ColumnCount - 1;
            using (var sw = new StreamWriter(path))
            {
                for (int i = 0; i < nRows; i++)
                {
                    for (int j = 0; j < nCols; j++)
                    {
                        sw.Write(matrix[i, j].ToString());
                        sw.Write(separator);
                    }
                    sw.WriteLine(matrix[i, nCols].ToString());
                }
            }
        }
        public static void WriteMatrixMarket(this Matrix matrix, string path)
        {
            using (var sw = new StreamWriter(path, false, Encoding.ASCII))
            {
                sw.WriteLine("%%MarixMarket matrix array real general");
                sw.WriteLine(string.Format("% {0}x{1} dense matrix", matrix.RowCount, matrix.ColumnCount));
                sw.Write(matrix.RowCount);
                sw.Write(" ");
                sw.WriteLine(matrix.ColumnCount);
                foreach (var item in matrix)
                    sw.WriteLine(item);
            }
        }
    }
}

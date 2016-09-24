using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
using NumericalLibrary.LinearAlgebra.MatrixFactorization;
using NumericalLibrary.LinearAlgebra.MatrixIO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NumericalLibrary.LinearAlgebra.Vectors;
using A = NumericalLibrary.Extensions.ArrayExtensions;
namespace NumericalLibrary
{
    class Program
    {

        static void Main(string[] args)
        {
            //var m = SpecialMatrices.Rand(10);
            //m.WriteMatrixMarket("D:\\test_mat.MM");
            Console.WriteLine(LoadMatrix.FromMatrixMarket("D:\\test_mat.MM"));
            //var m = LoadMatrix.FromMatrixMarket("D:\\mat.MM");
            //Console.WriteLine(m);
            //var v1 = Vector.Create(A.Range(0.0, 100));
            //Console.WriteLine(v1);
            //A.Range(0, 100);
            Console.ReadKey();
        }
    }
}

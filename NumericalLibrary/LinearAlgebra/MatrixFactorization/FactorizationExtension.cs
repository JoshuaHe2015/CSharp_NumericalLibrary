using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.LinearAlgebra.Matrices;
namespace NumericalLibrary.LinearAlgebra.MatrixFactorization
{
    public static class FactorizationExtension
    {
        public static LUFactorization LU(this Matrix M, bool CreateNewInstance = true)
        {
            return new LUFactorization(M, CreateNewInstance);
        }
        public static QRFactorization QR(this Matrix M, bool CreateNewInstance = true)
        {
            return new QRFactorization(M, CreateNewInstance);
        }
        public static EVD EVD(this Matrix M, int MaxTimes = 100, double Precision = 0.0001, bool CreateNewInstance = true)
        {
            return new EVD(M, MaxTimes, Precision, CreateNewInstance);
        }
        public static CholeskyFactorization Cholesky(this Matrix M,bool CreateNewInstance = true)
        {
            return new CholeskyFactorization(M, CreateNewInstance);
        }
    }
}

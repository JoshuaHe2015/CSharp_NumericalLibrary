using System;
using System.Runtime.InteropServices;
namespace NumericalLibrary
{
    public static class CMath
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double frexp(double val, out int eptr);
        public static bool Approx(this double x, double y, double eps = 1e-09)
        {
            return Math.Abs(x - y) < eps;
        }
        public static double Square(double x)
        {
            return x * x;
        }
        public static double Cube(double x)
        {
            return x * x * x;
        }
        public static double Frexp(double value,out int exponent)
        {
            return frexp(value, out exponent);
        }
        public static double Cbrt(double x)
        {
            const double OneThird = 1.0 / 3.0;
            var y = Math.Pow(Math.Abs(x), OneThird);
            return x < 0 ? -y : y;
        }
        public static bool IsEven(int x)
        {
            return (x & 1) == 0;
        }
        public static bool IsOdd(int x)
        {
            return (x & 1) == 1;
        }
        public static double Log2(double x)
        {
            const double Log2E = 1.4426950408889634073599246810018921374266459541530d;
            return Math.Log(x) * Log2E;
        }
        public static double Hypot(double x,double y)
        {
            return Math.Sqrt(x * x + y * y);
        }
    }
}

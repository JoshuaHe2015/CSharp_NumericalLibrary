using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalLibrary.Calculus
{
    public class RootFinding
    {
        Func<double, double> f;
        double Eps;
        public double Precision { get { return Eps; } set { Eps = value; } }
        int nMaxIt;
        public int MaxIterations { get { return nMaxIt; } set { nMaxIt = value; } }
        const double eps = 1e-09;
        public RootFinding(Func<double, double> func, double precision = eps, int nMaxIterations = 200)
        {
            f = func;
            Eps = precision;
            nMaxIt = nMaxIterations;
        }
        public double Bisection(double Lower, double Upper)
        {
            //Lower，Upper为区间左右端点 [Lower,Upper]
            if (f(Lower) * f(Upper) < 0.0)
            {
                int n = 0;
                while (Math.Abs(Lower - Upper) > Eps)
                {
                    double Mid = (Lower + Upper) / 2.0;
                    int Mid_Sign = Math.Sign(f(Mid));//求中点
                    if (Mid_Sign == 0) return Mid;

                    if (f(Lower) * Mid_Sign < 0.0) Upper = Mid;
                    if (f(Upper) * Mid_Sign < 0.0) Lower = Mid;
                    n++;
                    CannotSolve(n);
                }
                return Lower;
            }
            else throw new ArgumentException("给定区间内不存在零点");
        }
        public double NewtonRaphson(Func<double, double> df, double Guess)
        {
            double x = Guess;
            int n = 0;
            while (Math.Abs(f(x)) > Eps)
            {
                x = x - f(x) / df(x);
                n++;
                CannotSolve(n);
            }
            return x;
        }
        public double NewtonDownHill(Func<double, double> df, double Guess)
        {
            double x = Guess;
            int n = 0;
            double f0;
            while (Math.Abs(f0 = f(x)) > Eps)
            {
                double lambda = 1.0;
                double x1 = x - lambda * f(x) / df(x);
                while (Math.Abs(f(x1)) >= Math.Abs(f0))
                {
                    lambda /= 2.0;
                    x1 = x - lambda * f(x) / df(x);
                }
                x = x - lambda * f(x) / df(x);
                n++;
                CannotSolve(n);
            }
            return x;
        }
        public double Secant(double Guess1, double Guess2)
        {
            double x0 = Guess1;
            double x1 = Guess2;
            int n = 0;
            double f0, f1;
            while (Math.Abs(f1 = f(x1)) > Eps)
            {
                f0 = f(x0);
                x1 = x0 - f0 * (x0 - x1) / (f0 - f1);
                n++;
                CannotSolve(n);
            }
            return x1;
        }
        private void CannotSolve(int n)
        {
            if (n >= nMaxIt)
                throw new Exception(string.Format("迭代了{0}次，但还是没有解出来", n));
        }
    }
}

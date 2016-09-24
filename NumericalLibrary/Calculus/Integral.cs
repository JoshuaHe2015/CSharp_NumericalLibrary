using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalLibrary.Calculus
{
    public class Integral
    {
        private readonly Func<double, double> func;
        private readonly double a;
        private readonly double b;
        public double LowerBound { get { return a; } }
        public double UpperBound { get { return b; } }
        const double eps = 1e-9;
        public Integral(double lowerbound, double upperbound,
            Func<double, double> f)
        {
            a = lowerbound;
            b = upperbound;
            func = f;
        }
        /// <summary>
        /// 变步长梯形法求定积分
        /// </summary>
        /// <returns></returns>
        public double Trapezia()
        {
            double fa = func(a);
            double fb = func(b);
            int n = 1;
            double h = b - a;
            double t1 = h * (fa + fb) / 2.0;
            double error = eps + 1.0;
            double t = 0.0;
            while (error >= eps)
            {
                double sum = 0.0;
                for (int k = 0; k < n; k++)
                {
                    double x = a + (k + 0.5) * h;
                    sum += func(x);
                }
                t = (t1 + h * sum) / 2.0;
                error = Math.Abs(t1 - t);
                t1 = t;
                n <<= 1;
                h /= 2.0;
            }
            return t;
        }
        /// <summary>
        /// 变步长辛普森法求定积分
        /// </summary>
        /// <returns></returns>
        public double Simpson()
        {
            int n = 1;
            double h = b - a;
            double t1 = h * (func(a) + func(b)) / 2.0;
            double s1 = t1;
            double error = eps + 1.0;
            double s2 = 0.0;
            while (error >= eps)
            {
                double sum = 0.0;
                for (int k = 0; k < n; k++)
                {
                    double x = a + (k + 0.5) * h;
                    sum += func(x);
                }
                double t2 = (t1 + h * sum) / 2.0;
                s2 = (4.0 * t2 - t1) / 3.0;
                error = Math.Abs(s2 - s1);
                t1 = t2;
                s1 = s2;
                n <<= 1;
                h /= 2.0;
            }
            return s2;
        }
        /// <summary>
        /// 龙贝格法求定积分
        /// </summary>
        /// <returns></returns>
        public double Romberg()
        {
            double q = 0.0;
            double[] y = new double[10];
            double h = b - a;
            y[0] = h * (func(a) + func(b)) / 2.0;
            int m = 1;
            int n = 1;
            double error = eps + 1.0;
            while ((error >= eps) && (m <= 9))
            {
                double sum = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double x = a + (i + 0.5) * h;
                    sum += func(x);
                }
                sum = (y[0] + h * sum) / 2.0;
                double s = 1.0;
                for (int k = 0; k < m; k++)
                {
                    s *= 4.0;
                    q = (s * sum - y[k]) / (s - 1.0);
                    y[k] = sum;
                    sum = q;
                }
                error = Math.Abs(q - y[m - 1]);
                y[m] = q;
                m++;
                n <<= 1;
                h /= 2.0;
            }
            return q;
        }
    }
}

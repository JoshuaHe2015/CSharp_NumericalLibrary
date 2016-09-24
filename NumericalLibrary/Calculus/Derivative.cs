using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalLibrary.Calculus
{
    public class Derivative
    {
        Func<double, double> f;
        public Derivative(Func<double, double> function)
        {
            f = function;
        }
        public double FirstForward(double x, double h)
        {
            return (f(x + h) - f(x)) / h;
        }
        public double FirstBackward(double x, double h)
        {
            return (f(x) - f(x - h)) / h;
        }
        public double FirstCentral(double x, double h)
        {
            return (f(x + h) - f(x - h)) / (2.0 * h);
        }
        public double SecondForward(double x, double h)
        {
            return (f(x + 2.0 * h) - 2.0 * f(x + h) + f(x)) / (h * h);
        }
        public double SecondBackward(double x, double h)
        {
            return (f(x) - 2.0 * f(x - h) + f(x - 2.0 * h)) / (h * h);
        }
        public double SecondCentral(double x, double h)
        {
            return (f(x + h) - 2.0 * f(x) + f(x - h)) / (h * h);
        }
        public double ThirdForward(double x, double h)
        {
            return (f(x + 3.0 * h) - 3.0 * f(x + 2.0 * h) + 3.0 * f(x + h) - f(x)) / (h * h * h);
        }
        public double ThirdBackward(double x, double h)
        {
            return (f(x) - 3.0 * f(x - h) + 3.0 * f(x - 2.0 * h) - f(x - 3.0 * h)) / (h * h * h);
        }
        public double ThirdCentral(double x, double h)
        {
            return (f(x + 2.0 * h) - 2.0 * f(x + h) + 2.0 * f(x - h) - f(x - 2.0 * h)) / (2.0 * h * h * h);
        }
    }
}

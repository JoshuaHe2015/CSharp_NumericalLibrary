using System;

namespace NumericalLibrary
{
    public struct Complex : IEquatable<Complex>, IFormattable
    {
        private double real;

        private double imaginary;

        public double Real
        {
            get
            {
                return real;
            }
        }

        public double Imaginary
        {
            get
            {
                return imaginary;
            }
        }
        const double eps = 1e-9;
        const double Radian90 = Math.PI / 2.0;
        public Complex(double real, double imaginary)
        {
            this.real = real;
            this.imaginary = imaginary;
        }
        public Complex(Tuple<double, double> tuple)
        {
            real = tuple.Item1;
            imaginary = tuple.Item2;
        }
        public Complex Conjugate()
        {
            return new Complex(real, -imaginary);
        }
        public double Magnitude
        {
            get
            {
                return Abs(this);
            }
        }
        public double Phase
        {
            get
            {
                return Math.Atan2(imaginary, real);
            }
        }
        public static implicit operator Complex(double x) { return new Complex(x, 0.0); }
        public static Complex operator +(Complex z)
        {
            return new Complex(-z.real, -z.imaginary);
        }
        public static Complex operator -(Complex z)
        {
            return new Complex(-z.real, -z.imaginary);
        }
        public static Complex operator +(Complex z1, Complex z2)
        {
            return z1.Add(z2);
        }
        public static Complex operator -(Complex z1, Complex z2)
        {
            return z1.Subtract(z2);
        }
        public static Complex operator *(Complex z1, Complex z2)
        {
            return z1.Multiply(z2);
        }
        public static Complex operator /(Complex z1, Complex z2)
        {
            return z1.Divide(z2);
        }
        public static bool operator ==(Complex z1, Complex z2)
        {
            return z1.Equals(z2);
        }
        public static bool operator !=(Complex z1, Complex z2)
        {
            return !z1.Equals(z2);
        }
        public Complex Add(Complex other)
        {
            return new Complex(real + other.real, imaginary + other.imaginary);
        }
        public Complex Subtract(Complex other)
        {
            return new Complex(real - other.real, imaginary - other.imaginary);
        }
        public Complex Multiply(Complex other)
        {
            double x = real * other.real - imaginary * other.imaginary;
            double y = real * other.imaginary + imaginary * other.real;
            return new Complex(x, y);
        }
        public Complex Divide(Complex other)
        {
            double e, f, x, y;
            if (Math.Abs(other.real) >= Math.Abs(other.imaginary))
            {
                e = other.imaginary / other.real;
                f = other.real + e * other.real;
                x = (real + imaginary * e) / f;
                y = (imaginary - real * e) / f;
            }
            else
            {
                e = other.real / other.imaginary;
                f = other.imaginary + e * other.real;
                x = (real * e + imaginary) / f;
                y = (imaginary * e - real) / f;
            }
            return new Complex(x, y);
        }
        public static double Abs(Complex value)
        {
            double x = Math.Abs(value.real);
            double y = Math.Abs(value.imaginary);
            if (value.real == 0.0) return y;
            if (value.imaginary == 0.0) return x;
            if (x > y)
                return x * Math.Sqrt(1.0 + CMath.Square(y / x));
            return y * Math.Sqrt(1.0 + CMath.Square(x / y));
        }
        public override string ToString()
        {
            if (real != 0.0)
            {
                if (imaginary > 0.0)
                    return string.Concat(real.ToString(), " + ", imaginary.ToString(), "i");
                else if (imaginary < 0.0)
                    return string.Concat(real.ToString(), " - ", Math.Abs(imaginary).ToString(), "i");
                else return real.ToString();
            }
            else return string.Concat(imaginary.ToString(), "i");
        }
        public static Complex Pow(Complex value, double Exponent)
        {
            double real = value.real;
            double imaginary = value.imaginary;
            double r, t;
            if ((real == 0.0) && (imaginary == 0.0)) return new Complex(0.0, 0.0);
            if (real == 0.0)//幂运算公式中的三角函数运算
            {
                if (imaginary > 0) t = Radian90;
                else t = -Radian90;
            }
            else
            {
                if (real > 0)
                    t = Math.Atan2(imaginary, real);
                else
                {
                    if (imaginary >= 0)
                        t = Math.Atan2(imaginary, real) + Math.PI;
                    else
                        t = Math.Atan2(imaginary, real) - Math.PI;
                }
            }
            r = Math.Exp(Exponent * Math.Log(Math.Sqrt(real * real + imaginary * imaginary)));
            double u = Exponent * t;
            return new Complex(r * Math.Cos(u), r * Math.Sin(u));
        }
        public static Complex Pow(Complex value, Complex Exponent, int n = 0)
        {
            double real = value.real;
            double imaginary = value.imaginary;
            double r, s, u, v;
            if (real == 0.0)
            {
                if (imaginary == 0.0) return new Complex(0.0, 0.0);
                s = Radian90 * (Math.Abs(imaginary) / imaginary + 4.0 * n);
            }
            else
            {
                s = 2.0 * Math.PI * n + Math.Atan2(imaginary, real);
                if (real < 0)
                {
                    if (imaginary > 0) s += Math.PI;
                    else s -= Math.PI;
                }
            }
            //求幂运算公式
            r = 0.5 * Math.Log(real * real + imaginary * imaginary);
            v = Exponent.real * r + Exponent.imaginary * s;
            u = Math.Exp(Exponent.real * r - Exponent.imaginary * s);
            return new Complex(u * Math.Cos(v), u * Math.Sin(v));
        }
        public static Complex Log(Complex value)
        {
            double real = value.real;
            double imaginary = value.imaginary;
            double p = Math.Log(Math.Sqrt(real * real + imaginary * imaginary));
            return new Complex(p, Math.Atan2(imaginary, real));
        }
        private static readonly double[] c = new double[]
             {
                1.13031820798497,
                0.04433684984866,
                0.00054292631191,
                0.00000319843646,
                0.00000001103607,
                0.00000000002498
             };
        public static Complex Sin(Complex value)
        {
            double x, y;
            value.ForSinAndCos(out x, out y);
            x *= Math.Sin(value.real);
            y *= Math.Cos(value.real);
            return new Complex(x, y);
        }
        public static Complex Cos(Complex value)
        {
            double x, y;
            value.ForSinAndCos(out x, out y);
            x *= Math.Cos(value.real);
            y *= -Math.Sin(value.real);
            return new Complex(x, y);
        }
        private void ForSinAndCos(out double x, out double y)
        {
            double y1 = Math.Exp(imaginary);
            x = 0.5 * (y1 + 1.0 / y1);
            double br = 0.0;
            if (Math.Abs(imaginary) >= 1.0)
                y = 0.5 * (y1 - 1.0 / y1);
            else
            {
                double b1 = 0.0;
                double b2 = 0.0;
                y1 = 2.0 * (2.0 * CMath.Square(imaginary) - 1.0);
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    br = y1 * b1 - b2 - c[i];
                    if (i != 0)
                    {
                        b2 = b1;
                        b1 = br;
                    }
                }
                y = imaginary * (br - b1);
            }
        }
        public static Complex Tan(Complex value)
        {
            return Sin(value).Divide(Cos(value));
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (real != 0.0)
            {
                if (imaginary > 0.0)
                    return string.Concat(
                        real.ToString(format, formatProvider), " + ", imaginary.ToString(format, formatProvider), "i");
                else if (imaginary < 0.0)
                    return string.Concat(
                        real.ToString(format, formatProvider), " - ", Math.Abs(imaginary).ToString(format, formatProvider), "i");
                else return real.ToString(format, formatProvider);
            }
            else return string.Concat(imaginary.ToString(format, formatProvider), "i");
        }
        public static Complex FromPolarCoordinates(double magnitude, double phase)
        {
            return new Complex(magnitude * Math.Cos(phase), magnitude * Math.Sin(phase));
        }
        public static Complex Sqrt(Complex value)
        {
            return FromPolarCoordinates(Math.Sqrt(value.Magnitude), value.Phase / 2.0);
        }
        public static Complex Reciprocal(Complex value)
        {
            if (value.real == 0.0 && value.imaginary == 0.0)
                return new Complex(0.0, 0.0);
            return new Complex(1.0, 0.0) / value;
        }
        public bool Equals(Complex other)
        {
            return real == other.real && imaginary == other.imaginary;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return Equals((Complex)obj);
        }
        // override object.GetHashCode
        public override int GetHashCode()
        {
            return real.GetHashCode() ^ imaginary.GetHashCode();
        }
    }
}

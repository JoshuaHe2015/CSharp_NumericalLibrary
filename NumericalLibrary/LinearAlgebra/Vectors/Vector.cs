using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumericalLibrary.Extensions;
namespace NumericalLibrary.LinearAlgebra.Vectors
{
    public sealed class Vector : IEnumerable<double>
    {
        readonly double[] values;
        public double[] Values
        {
            get { return values; }
        }
        public Vector(double[] values)
        {
            this.values = values;
        }
        public Vector(int length)
        {
            values = new double[length];
        }
        public static Vector Create(params double[] values)
        {
            return new Vector(values);
        }
        public static Vector operator +(Vector V)
        {
            return new Vector(V.values.Copy());
        }
        public static Vector operator -(Vector V)
        {
            return new Vector(V.values.Map(i => -i));
        }
        public static Vector operator +(Vector V1, Vector V2)
        {
            return V1.Add(V2);
        }
        public static Vector operator -(Vector V1, Vector V2)
        {
            return V1.Subtract(V2);
        }
        public static double operator *(Vector V1, Vector V2)
        {
            return V1.Multiply(V2);
        }


        public static Vector operator +(double Scalar, Vector V)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => scalar + v));
        }
        public static Vector operator +(Vector V, double Scalar)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => v + scalar));
        }
        public static Vector operator -(double Scalar, Vector V)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => scalar - v));
        }
        public static Vector operator -(Vector V, double Scalar)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => v - scalar));
        }
        public static Vector operator *(double Scalar, Vector V)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => scalar * v));
        }
        public static Vector operator *(Vector V, double Scalar)
        {
            return new Vector(V.values.Map(Scalar, (v, scalar) => v * scalar));
        }
        public static Vector operator /(Vector V, double Scalar)
        {
            return V * (1.0 / Scalar);
        }
        public Vector Add(Vector Other)
        {
            int n = values.Length;
            if (n != Other.values.Length) throw new ArgumentOutOfRangeException("[Add] Two vectors' length must be equal");
            var result = new Vector(n);
            for (int i = 0; i < n; i++)
                result.values[i] = values[i] + Other.values[i];
            return result;
        }
        public Vector Subtract(Vector Other)
        {
            int n = values.Length;
            if (n != Other.values.Length) throw new ArgumentOutOfRangeException("[Subtract] Two vectors' length must be equal");
            var result = new Vector(n);
            for (int i = 0; i < n; i++)
                result.values[i] = values[i] - Other.values[i];
            return result;
        }
        public double Multiply(Vector Other)
        {
            int n = values.Length;
            if (n != Other.values.Length) throw new ArgumentOutOfRangeException("[Multiply] Two vectors' length must be equal");
            double product = 0.0;
            for (int i = 0; i < n; i++)
                product += values[i] * Other.values[i];
            return product;
        }
        public Vector DotMultiply(Vector Other)
        {
            int n = values.Length;
            if (n != Other.values.Length) throw new ArgumentOutOfRangeException("[Dot Multiply] Two vectors' length must be equal");
            var result = new Vector(n);
            for (int i = 0; i < n; i++)
                result.values[i] = values[i] * Other.values[i];
            return result;
        }
        public Vector DotDivide(Vector Other)
        {
            int n = values.Length;
            if (n != Other.values.Length) throw new ArgumentOutOfRangeException("[Dot Divide] Two vectors' length must be equal");
            var result = new Vector(n);
            for (int i = 0; i < n; i++)
                result.values[i] = values[i] / Other.values[i];
            return result;
        }
        public int Length
        {
            get
            {
                return values.Length;
            }
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
        public override string ToString()
        {
            return string.Join("  ", values);
        }
    }
}

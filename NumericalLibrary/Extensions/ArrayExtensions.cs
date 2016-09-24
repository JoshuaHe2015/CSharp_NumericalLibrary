using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalLibrary.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Copy<T>(this T[] array)
        {
            var newarray = new T[array.Length];
            Array.Copy(array, newarray, array.Length);
            return newarray;
        }
        public static R[] Map<T,R>(this T[] array, Func<T, R> f)
        {
            int n = array.Length;
            var newarray = new R[array.Length];
            for (int i = 0; i < n; i++)
                newarray[i] = f(array[i]);
            return newarray;
        }
        public static T[] Map<T, R>(this T[] array, R parameter, Func<T, R, T> f)
        {
            int n = array.Length;
            var newarray = new T[array.Length];
            for (int i = 0; i < n; i++)
                newarray[i] = f(array[i], parameter);
            return newarray;
        }
        public static V[] Map2<T, R, V>(this T[] array, R[] other, Func<T, R, V> f)
        {
            int n = array.Length;
            var newarray = new V[array.Length];
            for (int i = 0; i < n; i++)
                newarray[i] = f(array[i], other[i]);
            return newarray;
        }
        public static T[] MapInPlace<T>(this T[] array, Func<T, T> f)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
                array[i] = f(array[i]);
            return array;
        }
        public static T[] MapInPlace<T, R>(this T[] array, R parameter, Func<T, R, T> f)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
                array[i] = f(array[i], parameter);
            return array;
        }
        public static T[] Copy2Array<T>(this ICollection<T> collection)
        {
            var array = new T[collection.Count];
            collection.CopyTo(array, 0);
            return array;
        }
        public static int[] Range(int Start, int Count)
        {
            var array = new int[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = Start;
                Start++;
            }
            return array;
        }
        public static double[] Range(double Start, int Count)
        {
            var array = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                array[i] = Start;
                Start++;
            }
            return array;
        }
    }
}

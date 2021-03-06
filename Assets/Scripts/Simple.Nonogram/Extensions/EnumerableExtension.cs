using Simple.Nonogram.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Nonogram.Extension
{
    public static class EnumerableExtension
    {
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            IEnumerator<T>[] enumerators = source.Select(e => e.GetEnumerator()).ToArray();

            try
            {
                while (enumerators.All(e => e.MoveNext()))
                    yield return enumerators.Select(e => e.Current).ToArray();
            }
            finally
            {
                Array.ForEach(enumerators, e => e.Dispose());
            }
        }

        public static List<List<T>> ToLists<T>(this IEnumerable<IEnumerable<T>> source)
        {
            return source.Select(e => e.ToList()).ToList();
        }

        public static T[,] ToArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            int width = source.Count();
            int height = (int)Number.Zero;
            T[,] array;

            foreach (IEnumerable<T> column in source)
                if (column.Count() > height)
                    height = column.Count();

            array = new T[width, height];

            for (int i = (int)Number.Zero; i < source.Count(); i++)
            {
                for (int j = (int)Number.Zero; j < source.ElementAt(i).Count(); j++)
                    array[i, j] = source.ElementAt(i).ElementAt(j);

                if (source.ElementAt(i).Count() < height)
                    for (int k = source.ElementAt(i).Count(); k < height; k++)
                        array[i, k] = default;
            }

            return array;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Nonogram.Extension
{
    public static class EnumerableExtension
    {
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var enumerators = source.Select(e => e.GetEnumerator()).ToArray();

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

        public static T[,] ToArray<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var width = source.Count();
            var height = 0;

            foreach (IEnumerable<T> column in source)
                if (column.Count() > height)
                    height = column.Count();

            var array = new T[width, height];

            for (int i = 0; i < source.Count(); i++)
            {
                var rowCount = source.ElementAt(i).Count();

                for (int j = rowCount; j < height - rowCount; j++)
                    array[i, j] = default;

                for (int j = 0; j < rowCount; j++)
                    array[i, height - rowCount + j] = source.ElementAt(i).ElementAt(j);
            }

            return array;
        }
    }
}

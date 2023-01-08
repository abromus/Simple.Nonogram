using System.Collections.Generic;
using Simple.Nonogram.Core;
using Simple.Nonogram.Game;

namespace Simple.Nonogram.Extension
{
    public static class ArrayExtension
    {
        public static IEnumerable<IEnumerable<Cell>> ToIEnumerable(Cell[,] source)
        {
            var result = new List<List<Cell>>();

            for (int i = 0; i < source.GetLength((int)Dimension.Width); i++)
            {
                var row = new List<Cell>();

                for (int j = 0; j < source.GetLength((int)Dimension.Height); j++)
                    row.Add(source[i, j]);

                result.Add(row);
            }

            return result;
        }
    }
}

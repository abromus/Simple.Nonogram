using System.Collections.Generic;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public static class ArrayExtension
    {
        public static T[,] Transpose<T>(T[,] array)
        {
            int width = array.GetLength((int)Dimension.Width);
            int height = array.GetLength((int)Dimension.Height);
            T[,] transposedArray = new T[height, width];

            for (int i = (int)Number.Zero; i < height; i++)
                for (int j = (int)Number.Zero; j < width; j++)
                    transposedArray[i, j] = array[j, i];

            return transposedArray;
        }

        public static bool TryFindCell<T>(T[,] cells, Vector3 position, out Vector2Int coordinate) where T : MonoBehaviour, Components.ICell
        {
            bool isFind = false;
            coordinate = new Vector2Int((int)Number.MinusOne, (int)Number.MinusOne);

            for (int i = (int)Number.Zero; i < cells.GetLength((int)Dimension.Width); i++)
                for (int j = (int)Number.Zero; j < cells.GetLength((int)Dimension.Height); j++)
                    if (cells[i, j].transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;

                        return isFind;
                    }

            return isFind;
        }

        public static IEnumerable<IEnumerable<Cell>> ToIEnumerable(Cell[,] source)
        {
            List<List<Cell>> result = new List<List<Cell>>();
            List<Cell> row;

            for (int i = (int)Number.Zero; i < source.GetLength((int)Dimension.Width); i++)
            {
                row = new List<Cell>();

                for (int j = (int)Number.Zero; j < source.GetLength((int)Dimension.Height); j++)
                    row.Add(source[i, j]);

                result.Add(row);
            }

            return result;
        }

        public static void FillArray(MonoBehaviour[,] array, MonoBehaviour prefab, Vector3 startPosition, Vector2 direction, Transform parent, SizeF spriteSize)
        {
            int directionX = (int)(direction.x < (int)Number.Zero ? Direction.Negative : Direction.Positive);
            int directionY = (int)(direction.y < (int)Number.Zero ? Direction.Negative : Direction.Positive);

            for (int i = (int)Number.Zero; i < array.GetLength((int)Dimension.Width); i++)
                for (int j = (int)Number.Zero; j < array.GetLength((int)Dimension.Height); j++)
                {
                    array[i, j] = Object.Instantiate(prefab,
                                              new Vector3(startPosition.x + directionX * j * spriteSize.Width,
                                                          startPosition.y + directionY * i * spriteSize.Height,
                                                          startPosition.z),
                                              Quaternion.identity);

                    array[i, j].transform.parent = parent;
                }
        }
    }
}

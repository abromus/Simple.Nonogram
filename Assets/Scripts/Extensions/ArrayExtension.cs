using System.Collections.Generic;
using Simple.Nonogram.Core;
using UnityEngine;

namespace Simple.Nonogram.Extensions
{
    public static class ArrayExtension
    {
        public static T[,] Transpose<T>(T[,] array)
        {
            var width = array.GetLength((int)Dimension.Width);
            var height = array.GetLength((int)Dimension.Height);
            var transposedArray = new T[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    transposedArray[i, j] = array[j, i];

            return transposedArray;
        }

        public static bool TryFindCell<T>(T[,] cells, Vector3 position, out Vector2Int coordinate) where T : MonoBehaviour, Components.ICell
        {
            var isFind = false;
            coordinate = new Vector2Int(-1, -1);

            for (int i = 0; i < cells.GetLength((int)Dimension.Width); i++)
                for (int j = 0; j < cells.GetLength((int)Dimension.Height); j++)
                    if (cells[i, j].transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;

                        break;
                    }

            return isFind;
        }

        public static IEnumerable<IEnumerable<Cell>> ToIEnumerable(Cell[,] source)
        {
            var result = new List<List<Cell>>();
            List<Cell> row;

            for (int i = 0; i < source.GetLength((int)Dimension.Width); i++)
            {
                row = new List<Cell>();

                for (int j = 0; j < source.GetLength((int)Dimension.Height); j++)
                    row.Add(source[i, j]);

                result.Add(row);
            }

            return result;
        }

        public static void FillArray(MonoBehaviour[,] array, MonoBehaviour prefab, Vector3 startPosition, Vector2 direction, Transform parent, SizeF spriteSize)
        {
            var directionX = (int)(direction.x < 0 ? Direction.Negative : Direction.Positive);
            var directionY = (int)(direction.y < 0 ? Direction.Negative : Direction.Positive);

            for (int i = 0; i < array.GetLength((int)Dimension.Width); i++)
                for (int j = 0; j < array.GetLength((int)Dimension.Height); j++)
                {
                    array[i, j] = Object.Instantiate(
                        prefab,
                        new Vector3(startPosition.x + directionX * j * spriteSize.Width,
                            startPosition.y + directionY * i * spriteSize.Height,
                            startPosition.z),
                        Quaternion.identity,
                        parent);
                }
        }
    }
}

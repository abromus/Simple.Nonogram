using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public class Cell
    {
        public CellState State { get; private set; }

        public Cell(CellState state)
        {
            State = state;
        }

        public Cell() : this(CellState.Empty) { }

        public void SetState(CellState state)
        {
            if (Enum.IsDefined(typeof(CellState), state) && State != state)
                State = state;
            else
                Debug.LogError($"{DateTime.Now}. Error CellType! {state} not found in {typeof(CellState)}");
        }

        public static IEnumerable<Cell> ToIEnumerable(Cell[] source)
        {
            return source.ToList();
        }

        public static IEnumerable<IEnumerable<Cell>> ToIEnumerable(Cell[,] source)
        {
            List<List<Cell>> result = new List<List<Cell>>();
            List<Cell> row;

            for (int i = 0; i < source.GetLength(0); i++)
            {
                row = new List<Cell>();

                for (int j = 0; j < source.GetLength(1); j++)
                {
                    row.Add(source[i, j]);
                }

                result.Add(row);
            }

            return result;
        }
    }
}

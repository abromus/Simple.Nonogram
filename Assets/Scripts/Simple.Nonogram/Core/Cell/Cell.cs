using System;
using System.Collections.Generic;
using System.Linq;

namespace Simple.Nonogram.Core
{
    public class Cell
    {
        private CellState _state;

        public CellState State => _state;

        public Cell(CellState state)
        {
            _state = state;
        }

        public Cell() : this(CellState.Blank) { }

        public void SetState(CellState state)
        {
            string message = $"InvalidEnumArgumentException: (CellState = {state}). {state} not found in {typeof(CellState)}";

            if (Enum.IsDefined(typeof(CellState), state) && State != state)
                _state = state;
            else
                DebugExtension.LogError(message);
        }

        public static IEnumerable<IEnumerable<Cell>> ToIEnumerable(Cell[,] source)
        {
            List<List<Cell>> result = new List<List<Cell>>();
            List<Cell> row;

            for (int i = 0; i < source.GetLength(0); i++)
            {
                row = new List<Cell>();

                for (int j = 0; j < source.GetLength(1); j++)
                    row.Add(source[i, j]);

                result.Add(row);
            }

            return result;
        }
    }
}

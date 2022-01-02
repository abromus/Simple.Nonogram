using Simple.Nonogram.Extension;
using System.Collections.Generic;

namespace Simple.Nonogram.Core
{
    public class NumberBoard
    {
        private int[,] _top;
        private int[,] _left;

        private readonly Cell[,] _cells;

        public int[,] Top => _top;
        public int[,] Left => _left;

        public NumberBoard(AnswerBoard board)
        {
            _cells = board.Cells;

            GenerateCells();
        }

        private void GenerateCells()
        {
            _top = ArrayExtension.Transpose(CalculateCells(true));

            _left = CalculateCells(false);
        }

        private int[,] CalculateCells(bool isTop = true)
        {
            List<List<Cell>> grid = isTop ? ArrayExtension.ToIEnumerable(_cells).Transpose().ToLists() : ArrayExtension.ToIEnumerable(_cells).ToLists();
            List<List<int>> cells = new List<List<int>>();
            int countMarked = (int)Number.Zero;
            int rowCount;

            for (int i = (int)Number.Zero; i < grid.Count; i++)
            {
                List<int> row = new List<int>();

                for (int j = (int)Number.Zero; j < grid[i].Count; j++)
                {
                    rowCount = grid[i].Count - (int)Number.One;

                    if (grid[i][j].State == CellState.Marked && j == rowCount
                        || countMarked > (int)Number.Zero && grid[i][j].State != CellState.Marked)
                    {
                        if (grid[i][j].State == CellState.Marked && j == rowCount)
                            countMarked++;

                        row.Add(countMarked);
                        countMarked = (int)Number.Zero;
                    }
                    else if (grid[i][j].State == CellState.Marked)
                    {
                        countMarked++;
                    }
                }

                countMarked = (int)Number.Zero;

                row.Reverse();
                cells.Add(row);
            }

            return cells.ToArray<int>();
        }
    }
}

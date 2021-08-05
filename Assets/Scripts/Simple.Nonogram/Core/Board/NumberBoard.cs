using System.Collections.Generic;

namespace Simple.Nonogram.Core
{
    public class NumberBoard
    {
        private int[,] _topNumberCells;
        private int[,] _leftNumberCells;

        private readonly Cell[,] _cells;

        public int[,] TopNumberCells => _topNumberCells;
        public int[,] LeftNumberCells => _leftNumberCells;

        public NumberBoard(AnswerBoard board)
        {
            _cells = board.Cells;

            GenerateNumberCells();
        }

        private void GenerateNumberCells()
        {
            _topNumberCells = ArrayExtension.Transpose(CalculateNumberCells(true));

            _leftNumberCells = CalculateNumberCells(false);
        }

        private int[,] CalculateNumberCells(bool isTop = true)
        {
            List<List<Cell>> grid = isTop ? Cell.ToIEnumerable(_cells).Transpose().ToLists() : Cell.ToIEnumerable(_cells).ToLists();
            List<List<int>> cells = new List<List<int>>();
            int countMarked = 0;
            int rowCount;

            for (int i = 0; i < grid.Count; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < grid[i].Count; j++)
                {
                    rowCount = grid[i].Count - 1;

                    if (grid[i][j].State == CellState.Marked && j == rowCount || countMarked > Constants.ZeroCount && grid[i][j].State != CellState.Marked)
                    {
                        if (grid[i][j].State == CellState.Marked && j == rowCount)
                            countMarked++;

                        row.Add(countMarked);
                        countMarked = Constants.ZeroCount;
                    }
                    else if (grid[i][j].State == CellState.Marked)
                    {
                        countMarked++;
                    }
                }

                countMarked = Constants.ZeroCount;

                row.Reverse();
                cells.Add(row);
            }

            return cells.ToArray<int>();
        }
    }
}

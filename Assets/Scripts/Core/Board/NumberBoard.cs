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
            var grid = isTop ? ArrayExtension.ToIEnumerable(_cells).Transpose().ToLists() : ArrayExtension.ToIEnumerable(_cells).ToLists();
            var cells = new List<List<int>>();
            int countMarked = 0;

            for (int i = 0; i < grid.Count; i++)
            {
                var row = new List<int>();

                for (int j = 0; j < grid[i].Count; j++)
                {
                    var rowCount = grid[i].Count - 1;

                    if (grid[i][j].State == CellState.Marked && j == rowCount || countMarked > 0 && grid[i][j].State != CellState.Marked)
                    {
                        if (grid[i][j].State == CellState.Marked && j == rowCount)
                            countMarked++;

                        row.Add(countMarked);
                        countMarked = 0;
                    }
                    else if (grid[i][j].State == CellState.Marked)
                    {
                        countMarked++;
                    }
                }

                countMarked = 0;

                row.Reverse();
                cells.Add(row);
            }

            return cells.ToArray<int>();
        }
    }
}

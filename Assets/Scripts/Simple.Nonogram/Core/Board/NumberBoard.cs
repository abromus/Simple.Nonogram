using System.Collections.Generic;

namespace Simple.Nonogram.Core
{
    class NumberBoard
    {
        private int[,] _topNumberCells;
        private int[,] _leftNumberCells;

        private readonly Cell[,] _cells;

        public int[,] TopNumberCells => _topNumberCells;
        public int[,] LeftNumberCells => _leftNumberCells;

        public NumberBoard(Board board)
        {
            _cells = board.Cells;
            GenerateNumberCells();
        }

        private void GenerateNumberCells()
        {
            _topNumberCells = CalculateNumberCells(true);

            _leftNumberCells = Transpose(CalculateNumberCells(false));
        }

        private int[,] Transpose(int[,] array)
        {
            int _widthDimension = 0;
            int _heightDimension = 1;
            int width = array.GetLength(_widthDimension);
            int height = array.GetLength(_heightDimension);
            int[,] transposedArray = new int[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    transposedArray[i, j] = array[j, i];

            return transposedArray;
        }

        private int[,] CalculateNumberCells(bool isTop = true)
        {
            List<List<Cell>> grid = isTop ? Cell.ToIEnumerable(_cells).Transpose().ToLists() : Cell.ToIEnumerable(_cells).ToLists();
            List<List<int>> cells = new List<List<int>>();
            int countMarked = 0;
            int zero = 0;
            int rowCount;

            for (int i = 0; i < grid.Count; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < grid[i].Count; j++)
                {
                    rowCount = grid[i].Count - 1;

                    if (grid[i][j].State == CellState.Marked && j == rowCount || countMarked > zero && grid[i][j].State != CellState.Marked)
                    {
                        if (grid[i][j].State == CellState.Marked && j == rowCount)
                            countMarked++;

                        row.Add(countMarked);
                        countMarked = zero;
                    }
                    else if (grid[i][j].State == CellState.Marked)
                    {
                        countMarked++;
                    }
                }

                countMarked = zero;

                row.Reverse();
                cells.Add(row);
            }

            return cells.ToArray<int>();
        }
    }
}

using System.Collections.Generic;

namespace Simple.Nonogram.Core
{
    class NumberBoard
    {
        private readonly Cell[,] _grid;

        public int[,] TopNumberCells { get; private set; }
        public int[,] LeftNumberCells { get; private set; }

        public NumberBoard(Board board)
        {
            _grid = board.Grid;
            GenerateNumberCells();
        }

        private void GenerateNumberCells()
        {
            TopNumberCells = CalculateNumberCells(true);
            LeftNumberCells = CalculateNumberCells(false);

            LeftNumberCells = Transpose(LeftNumberCells);
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
            List<List<Cell>> grid = isTop ? Cell.ToIEnumerable(_grid).Transpose().ToLists() : Cell.ToIEnumerable(_grid).ToLists();
            List<List<int>> cells = new List<List<int>>();
            int countMarked = 0;

            for (int i = 0; i < grid.Count; i++)
            {
                List<int> row = new List<int>();

                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j].State == CellState.Marked && j == grid[i].Count - 1 || countMarked > 0 && grid[i][j].State != CellState.Marked)
                    {
                        if (grid[i][j].State == CellState.Marked && j == grid[i].Count - 1)
                        {
                            countMarked++;
                        }

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

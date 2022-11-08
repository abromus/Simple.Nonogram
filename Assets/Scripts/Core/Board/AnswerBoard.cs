using System;
using System.Collections.Generic;
using Simple.Nonogram.Extension;

namespace Simple.Nonogram.Core
{
    public class AnswerBoard : Board
    {
        public AnswerBoard(string pathToFile)
        {
            Initialize(pathToFile);
        }

        private void Initialize(string path)
        {
            Cells = ParseNonogram(LoadNonogram(path));
        }

        private List<string> LoadNonogram(string path)
        {
            var firstLineIndex = 0;

            var lines = new List<string>();

            try
            {
                lines = NonogramFile.LoadFile(path);

                if (lines?.Count > 0)
                {
                    Width = lines[firstLineIndex].Length;
                    Height = lines.Count;
                }
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"AnswerBoard.LoadNonogram: {exception.Message}.");
            }

            return lines;
        }

        private Cell[,] ParseNonogram(List<string> nonogramFile)
        {
            var mark = '1';

            Cell[,] cells = null;

            if (nonogramFile?.Count > 0)
            {
                cells = new Cell[Height, Width];

                for (int i = 0; i < Height; i++)
                    for (int j = 0; j < Width; j++)
                        cells[i, j] = new Cell(nonogramFile[i][j] == mark ? CellState.Marked : CellState.Blank);
            }
            else
            {
                DebugExtension.LogError($"Nonogram {nonogramFile} is null ({nonogramFile == null}) or " +
                    $"Count <= {0} ({nonogramFile?.Count <= 0}).");
            }

            return cells;
        }
    }
}

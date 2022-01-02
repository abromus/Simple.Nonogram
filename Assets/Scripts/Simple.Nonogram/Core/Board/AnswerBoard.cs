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
            const int firstLineIndex = (int)Number.Zero;

            List<string> lines = new List<string>();

            try
            {
                lines = NonogramFile.LoadFile(path);

                if (lines?.Count > (int)Number.Zero)
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
            const char mark = '1';

            Cell[,] cells = null;

            if (nonogramFile?.Count > (int)Number.Zero)
            {
                cells = new Cell[Height, Width];

                for (int i = (int)Number.Zero; i < Height; i++)
                    for (int j = (int)Number.Zero; j < Width; j++)
                        cells[i, j] = new Cell(nonogramFile[i][j] == mark ? CellState.Marked : CellState.Blank);
            }
            else
            {
                DebugExtension.LogError($"Nonogram {nonogramFile} is null ({nonogramFile == null}) or " +
                    $"Count <= {(int)Number.Zero} ({nonogramFile?.Count <= (int)Number.Zero}).");
            }

            return cells;
        }
    }
}

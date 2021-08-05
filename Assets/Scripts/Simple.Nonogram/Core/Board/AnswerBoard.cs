using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public class AnswerBoard : Board
    {
        public AnswerBoard(string pathToFile)
        {
            InitializeBoard(Application.dataPath + pathToFile);
        }

        private void InitializeBoard(string pathToFile)
        {
            Cells = ParseNonogram(LoadNonogram(pathToFile));
        }

        private List<string> LoadNonogram(string path)
        {
            List<string> lines = new List<string>();
            int firstLineIndex = 0;

            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    do
                    {
                        lines.Add(stream.ReadLine());
                    } while (stream.EndOfStream == false);
                }

                if (lines.Count > Constants.ZeroCount)
                {
                    Width = lines[firstLineIndex].Length;
                    Height = lines.Count;
                }
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"LoadNonogram: {exception.Message}.");
            }

            return lines;
        }

        private Cell[,] ParseNonogram(List<string> nonogramFile)
        {
            Cell[,] cells = null;
            CellState state;

            if (nonogramFile != null && nonogramFile.Count > Constants.ZeroCount)
            {
                cells = new Cell[Height, Width];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        state = nonogramFile[i][j] == Constants.Mark ? CellState.Marked : CellState.Blank;

                        cells[i, j] = new Cell(state);
                    }
                }
            }
            else
            {
                DebugExtension.LogError($"Nonogram {nonogramFile} is null ({nonogramFile == null}) or " +
                    $"Count <= {Constants.ZeroCount} ({nonogramFile.Count <= Constants.ZeroCount}).");
            }

            return cells;
        }
    }
}

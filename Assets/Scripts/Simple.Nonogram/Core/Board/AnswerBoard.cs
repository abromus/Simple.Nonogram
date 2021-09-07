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
            Initialize(Application.dataPath + pathToFile);
        }

        private void Initialize(string pathToFile)
        {
            Cells = ParseNonogram(LoadNonogram(pathToFile));
        }

        private List<string> LoadNonogram(string path)
        {
            List<string> lines = new List<string>();
            int firstLineIndex = (int)Number.Zero;

            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    do
                    {
                        lines.Add(stream.ReadLine());
                    } while (stream.EndOfStream == false);
                }

                if (lines.Count > (int)Number.Zero)
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
            char mark = '1';

            if (nonogramFile != null && nonogramFile.Count > (int)Number.Zero)
            {
                cells = new Cell[Height, Width];

                for (int i = (int)Number.Zero; i < Height; i++)
                {
                    for (int j = (int)Number.Zero; j < Width; j++)
                    {
                        state = nonogramFile[i][j] == mark ? CellState.Marked : CellState.Blank;

                        cells[i, j] = new Cell(state);
                    }
                }
            }
            else
            {
                DebugExtension.LogError($"Nonogram {nonogramFile} is null ({nonogramFile == null}) or " +
                    $"Count <= {(int)Number.Zero} ({nonogramFile.Count <= (int)Number.Zero}).");
            }

            return cells;
        }
    }
}

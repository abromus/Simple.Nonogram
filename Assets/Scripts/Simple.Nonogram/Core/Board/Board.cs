using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public class Board
    {
        private Cell[,] _cells;
        private int _width;
        private int _height;

        public Cell[,] Cells => _cells;
        public int Width => _width;
        public int Height => _height;

        public Board(string pathToFile)
        {
            //pathToFile = Application.dataPath + @"\Trash\Nonograms\1.txt";            //Äëÿ Windows
            //path = "jar:file://" + Application.dataPath + "!/assets/";                //Äëÿ Android  
            InitializeBoard(Application.dataPath + pathToFile);
        }

        private void InitializeBoard(string pathToFile)
        {
            _cells = ParseNonogram(LoadNonogram(pathToFile));
        }

        private List<string> LoadNonogram(string path)
        {
            List<string> lines = new List<string>();

            try
            {
                using (StreamReader stream = new StreamReader(path))
                {
                    do
                    {
                        lines.Add(stream.ReadLine());
                    } while (stream.EndOfStream == false);
                }

                if (lines.Count > 0)
                {
                    _width = lines[0].Length;
                    _height = lines.Count;
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

            if (nonogramFile != null && nonogramFile.Count > 0)
            {
                cells = new Cell[Height, Width];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        state = nonogramFile[i][j] == mark ? CellState.Marked : CellState.Blank;

                        cells[i, j] = new Cell(state);
                    }
                }
            }
            else
            {
                DebugExtension.LogError($"Nonogram {nonogramFile} is null or Count <= 0.");
            }

            return cells;
        }
    }
}

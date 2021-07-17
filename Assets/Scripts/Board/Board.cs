using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Simple.Nonogram
{
    public class Board
    {
        public Cell[,] Grid { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Board(string pathToFile)
        {
            //pathToFile = Application.dataPath + @"\Trash\Nonograms\1.txt";              //Äëÿ Windows
            //path = "jar:file://" + Application.dataPath + "!/assets/";                //Äëÿ Android  
            InitializeBoard(Application.dataPath + pathToFile);
        }

        private void InitializeBoard(string pathToFile)
        {
            Grid = ParseNonogram(LoadNonogram(pathToFile));
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
                    Width = lines[0].Length;
                    Height = lines.Count;
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"{DateTime.Now}. {exception.Message}.");
            }

            return lines;
        }

        private Cell[,] ParseNonogram(List<string> nonogramFile)
        {
            Cell[,] board;
            CellState state;
            char mark = '1';

            if (nonogramFile != null && nonogramFile.Count > 0)
            {
                board = new Cell[Height, Width];

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        state = nonogramFile[i][j] == mark ? CellState.Marked : CellState.Blank;

                        board[i, j] = new Cell(state);
                    }
                }
            }
            else
            {
                Debug.LogError($"{DateTime.Now}. Nonogram {nonogramFile} is null or Count <= 0.");
                return null;
            }

            return board;
        }
    }
}

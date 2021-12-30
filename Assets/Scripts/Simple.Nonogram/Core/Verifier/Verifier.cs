using System;
using UnityEngine;

namespace Simple.Nonogram.Core
{
    public class Verifier
    {
        private readonly bool[,] _mistakeBoard;
        private readonly AnswerBoard _answerBoard;
        private readonly UserBoard _userBoard;

        public bool[,] MistakeBoard => _mistakeBoard;

        public event Action<Vector2Int> MarkMistake;
        public event Action<Vector2Int> RemoveMistake;

        public Verifier(AnswerBoard answerBoard, UserBoard userBoard)
        {
            _mistakeBoard = new bool[answerBoard.Height, answerBoard.Width];
            InitializeMistakeBoard();

            _answerBoard = answerBoard;
            _userBoard = userBoard;
        }

        public void CheckCell(Vector2Int coordinate)
        {
            if (_userBoard.Cells[coordinate.x, coordinate.y].State == CellState.Marked &&
                _userBoard.Cells[coordinate.x, coordinate.y].State != _answerBoard.Cells[coordinate.x, coordinate.y].State)
            {
                _mistakeBoard[coordinate.x, coordinate.y] = true;
                MarkMistake?.Invoke(coordinate);
            }
            else if (_mistakeBoard[coordinate.x, coordinate.y])
            {
                _mistakeBoard[coordinate.x, coordinate.y] = false;
                RemoveMistake?.Invoke(coordinate);
            }
        }

        private void InitializeMistakeBoard()
        {
            for (int i = (int)Number.Zero; i < _mistakeBoard.GetLength((int)Dimension.Width); i++)
                for (int j = (int)Number.Zero; j < _mistakeBoard.GetLength((int)Dimension.Height); j++)
                    _mistakeBoard[i, j] = false;
        }
    }
}

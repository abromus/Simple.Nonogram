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

        public event Action<Vector2Int> MistakeMarked;
        public event Action<Vector2Int> MistakeRemoved;

        public Verifier(AnswerBoard answerBoard, UserBoard userBoard)
        {
            _mistakeBoard = new bool[answerBoard.Height, answerBoard.Width];
            InitializeMistakeBoard();

            _answerBoard = answerBoard;
            _userBoard = userBoard;
        }

        public void CheckCell(Vector2Int coordinate)
        {
            if (IsCorrectlyMarkedCell(ref coordinate))
                MarkMistake(coordinate);
            else if (_mistakeBoard[coordinate.x, coordinate.y])
                RemoveMistake(coordinate);
        }

        private void InitializeMistakeBoard()
        {
            for (int i = 0; i < _mistakeBoard.GetLength((int)Dimension.Width); i++)
                for (int j = 0; j < _mistakeBoard.GetLength((int)Dimension.Height); j++)
                    _mistakeBoard[i, j] = false;
        }

        private bool IsCorrectlyMarkedCell(ref Vector2Int coordinate)
        {
            return _userBoard.Cells[coordinate.x, coordinate.y].State == CellState.Marked
                && _userBoard.Cells[coordinate.x, coordinate.y].State != _answerBoard.Cells[coordinate.x, coordinate.y].State;
        }

        private void MarkMistake(Vector2Int coordinate)
        {
            _mistakeBoard[coordinate.x, coordinate.y] = true;
            MistakeMarked?.Invoke(coordinate);
        }

        private void RemoveMistake(Vector2Int coordinate)
        {
            _mistakeBoard[coordinate.x, coordinate.y] = false;
            MistakeRemoved?.Invoke(coordinate);
        }
    }
}

using System;

using Simple.Nonogram.Components;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public class Verifier
    {
        private BoardView _boardView;
        private AnswerBoard _answerBoard;
        private UserBoard _userBoard;
        private bool[,] _mistakeBoard;

        public event Action<Vector3> MarkMistake;
        public event Action<Vector3> RemoveMistake;

        public Verifier(BoardView boardView)
        {
            _boardView = boardView;
            _answerBoard = _boardView.AnswerBoard;
            _userBoard = _boardView.UserBoard;

            InitializeMistakeBoard();

            boardView.BoardClicked += OnClicked;
        }

        private void InitializeMistakeBoard()
        {
            _mistakeBoard = new bool[_boardView.AnswerBoard.Height, _boardView.AnswerBoard.Width];

            for (int i = 0; i < _mistakeBoard.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < _mistakeBoard.GetLength(Constants.HeightDimension); j++)
                    _mistakeBoard[i, j] = false;
        }

        private void CheckCell(Vector3 position)
        {
            int x = Mathf.Abs((int)(position.x / _boardView.SpriteSize.Width));
            int y = Mathf.Abs((int)(position.y / _boardView.SpriteSize.Height));

            if (_userBoard.Cells[y, x].State == CellState.Marked && _userBoard.Cells[y, x].State != _answerBoard.Cells[y, x].State)
            {
                _mistakeBoard[y, x] = true;
                MarkMistake?.Invoke(position);
            }
            else if (_mistakeBoard[y, x] == true)
            {
                _mistakeBoard[y, x] = false;
                RemoveMistake?.Invoke(position);
            }
        }

        private void OnClicked(Vector3 position)
        {
            CheckCell(position);
        }
    }
}

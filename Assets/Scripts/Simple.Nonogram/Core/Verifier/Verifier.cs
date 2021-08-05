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
        private int _width;
        private int _height;

        public Verifier(BoardView boardView)
        {
            _boardView = boardView;
            _answerBoard = _boardView.AnswerBoard;
            _userBoard = _boardView.UserBoard;

            _width = _answerBoard.Width;
            _height = _answerBoard.Height;

            boardView.Clicked += OnClicked;
            //boardView.Emptied += OnEmptied;
        }

        private void Verify(Vector3 position)
        {
            int x = Mathf.Abs((int)(position.x / _boardView.SpriteSize.Width));
            int y = Mathf.Abs((int)(position.y / _boardView.SpriteSize.Height));

            if (_userBoard.Cells[y, x].State != _answerBoard.Cells[y, x].State)
            {
                Debug.LogWarning($"userBoard = {_userBoard.Cells[y, x].State} != answerBoard = {_answerBoard.Cells[y, x].State}");
                return;
            }

            Debug.Log($"Good! ({y}, {x})");
        }

        private void OnClicked(Vector3 position)
        {
            Verify(position);
        }

        private void OnEmptied(Vector3 position)
        {
            Verify(position);
        }
    }
}

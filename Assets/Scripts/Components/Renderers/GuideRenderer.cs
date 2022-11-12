using Simple.Nonogram.Core;
using Simple.Nonogram.Extensions;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class GuideRenderer
    {
        private readonly Board _board;
        private readonly bool[,] _mistakeBoard;
        private readonly Color _defaultCellColor;
        private readonly Color _defaultTopCellColor;
        private readonly Color _defaultLeftCellColor;
        private readonly Color _hoverCellColor = new Color(0.52f, 0.45f, 0.45f);
        private readonly Color _hoverTopCellColor = new Color(0.33f, 0.3f, 0.3f);
        private readonly Color _hoverLeftCellColor = new Color(0.33f, 0.3f, 0.3f);

        public GuideRenderer(Board board, Verifier verifier)
        {
            _board = board;

            _defaultCellColor = _board.UserBoardView[0, 0].GetComponent<SpriteRenderer>().color;
            _defaultTopCellColor = _board.Top[0, 0].GetComponent<SpriteRenderer>().color;
            _defaultLeftCellColor = _board.Left[0, 0].GetComponent<SpriteRenderer>().color;

            _mistakeBoard = verifier.MistakeBoard;
        }

        public void DrawGuides(Vector3 position, bool isHover = false)
        {
            var cellColor = isHover ? _hoverCellColor : _defaultCellColor;
            var topNumberCellColor = isHover ? _hoverTopCellColor : _defaultTopCellColor;
            var leftNumberCellColor = isHover ? _hoverLeftCellColor : _defaultLeftCellColor;

            if (ArrayExtension.TryFindCell(_board.UserBoardView, position, out Vector2Int coordinate))
            {
                for (int i = 0; i < _board.UserBoardView.GetLength((int)Dimension.Width); i++)
                    _board.UserBoardView[i, coordinate.y].GetComponent<SpriteRenderer>().color = _mistakeBoard[i, coordinate.y] ? Color.red : cellColor;

                for (int j = 0; j < _board.UserBoardView.GetLength((int)Dimension.Height); j++)
                    _board.UserBoardView[coordinate.x, j].GetComponent<SpriteRenderer>().color = _mistakeBoard[coordinate.x, j] ? Color.red : cellColor;

                for (int i = 0; i < _board.Top.GetLength((int)Dimension.Width); i++)
                    _board.Top[i, coordinate.y].GetComponent<SpriteRenderer>().color = topNumberCellColor;

                for (int j = 0; j < _board.Left.GetLength((int)Dimension.Height); j++)
                    _board.Left[coordinate.x, j].GetComponent<SpriteRenderer>().color = leftNumberCellColor;
            }
        }
    }
}

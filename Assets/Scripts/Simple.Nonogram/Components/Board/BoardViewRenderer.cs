using Simple.Nonogram.Core;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(BoardView))]
    public class BoardViewRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;
        [SerializeField] private Sprite _empty;

        private BoardView _boardView;
        private Color _defaultCellColor;
        private Color _defaultTopNumberCellColor;
        private Color _defaultLeftNumberCellColor;
        private Color _hoverCellColor = new Color(0.52f, 0.45f, 0.45f);
        private Color _hoverTopNumberCellColor = new Color(0.33f, 0.3f, 0.3f);
        private Color _hoverLeftNumberCellColor = new Color(0.33f, 0.3f, 0.3f);

        private readonly int _widthDimension = 0;
        private readonly int _heightDimension = 1;

        private void Start()
        {
            int x = 0;
            int y = 0;

            _boardView = GetComponent<BoardView>();

            _boardView.Clicked += OnClicked;
            _boardView.Emptied += OnEmptied;
            _boardView.HoveredBegin += OnHoveredBegin;
            _boardView.HoveredEnd += OnHoveredEnd;

            _defaultCellColor = _boardView.UserCellsView[x, y].GetComponent<SpriteRenderer>().color;
            _defaultTopNumberCellColor = _boardView.TopNumberCells[x, y].GetComponent<SpriteRenderer>().color;
            _defaultLeftNumberCellColor = _boardView.LeftNumberCells[x, y].GetComponent<SpriteRenderer>().color;
        }

        private void OnClicked(Vector3 position)
        {
            MarkCell(position, CellState.Marked, _marked);
        }

        private void OnEmptied(Vector3 position)
        {
            MarkCell(position, CellState.Empty, _empty);
        }

        private void OnHoveredBegin(Vector3 position)
        {
            DrawGuides(position, _hoverCellColor, _hoverLeftNumberCellColor, _hoverTopNumberCellColor);
        }

        private void OnHoveredEnd(Vector3 position)
        {
            DrawGuides(position, _defaultCellColor, _defaultLeftNumberCellColor, _defaultTopNumberCellColor);
        }

        private bool TryFindCell(Vector3 position, out Vector2Int coordinate)
        {
            bool isFind = false;
            coordinate = new Vector2Int(-1, -1);

            for (int i = 0; i < _boardView.UserCellsView.GetLength(_widthDimension); i++)
                for (int j = 0; j < _boardView.UserCellsView.GetLength(_heightDimension); j++)
                    if (_boardView.UserCellsView[i, j].transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;
                    }

            return isFind;
        }

        private void MarkCell(Vector3 position, CellState filledState, Sprite filledStateView)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                CellState state = _boardView.UserCells[coordinate.x, coordinate.y].State == CellState.Blank ? filledState : CellState.Blank;
                Sprite stateView = _boardView.UserCells[coordinate.x, coordinate.y].State == CellState.Blank ? filledStateView : _blank;

                _boardView.UserCells[coordinate.x, coordinate.y].SetState(state);
                _boardView.UserCellsView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().sprite = stateView;
            }
        }

        private void DrawGuides(Vector3 position, Color cellColor, Color leftNumberCellColor, Color topNumberCellColor)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                for (int i = 0; i < _boardView.UserCellsView.GetLength(_widthDimension); i++)
                    _boardView.UserCellsView[i, coordinate.y].GetComponent<SpriteRenderer>().color = cellColor;

                for (int j = 0; j < _boardView.UserCellsView.GetLength(_heightDimension); j++)
                    _boardView.UserCellsView[coordinate.x, j].GetComponent<SpriteRenderer>().color = cellColor;

                for (int i = 0; i < _boardView.LeftNumberCells.GetLength(_widthDimension); i++)
                    _boardView.LeftNumberCells[i, coordinate.y].GetComponent<SpriteRenderer>().color = leftNumberCellColor;

                for (int j = 0; j < _boardView.TopNumberCells.GetLength(_heightDimension); j++)
                    _boardView.TopNumberCells[coordinate.x, j].GetComponent<SpriteRenderer>().color = topNumberCellColor;
            }
        }
    }
}

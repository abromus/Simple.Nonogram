using Simple.Nonogram.Core;

using UnityEngine;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(BoardView))]
    public class BoardViewRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;
        [SerializeField] private Sprite _empty;

        private BoardView _boardView;
        private Color _defaultColor = new Color(1f, 1f, 1f);
        private Color _hoverColor = new Color(0.52f, 0.45f, 0.45f);

        private readonly int _widthDimension = 0;
        private readonly int _heightDimension = 1;

        private void Start()
        {
            _boardView = GetComponent<BoardView>();

            _boardView.Clicked += OnClicked;
            _boardView.Emptied += OnEmptied;
            _boardView.HoveredBegin += OnHoveredBegin;
            _boardView.HoveredEnd += OnHoveredEnd;
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
            DrawGuides(position, _hoverColor);
        }

        private void OnHoveredEnd(Vector3 position)
        {
            DrawGuides(position, _defaultColor);
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

        private void DrawGuides(Vector3 position, Color color)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                for (int i = 0; i < _boardView.UserCellsView.GetLength(_widthDimension); i++)
                    _boardView.UserCellsView[i, coordinate.y].GetComponent<SpriteRenderer>().color = color;

                for (int j = 0; j < _boardView.UserCellsView.GetLength(_heightDimension); j++)
                    _boardView.UserCellsView[coordinate.x, j].GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}

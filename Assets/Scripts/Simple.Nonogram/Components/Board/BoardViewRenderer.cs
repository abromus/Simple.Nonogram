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
            MarkCell(position, _marked);
        }

        private void OnEmptied(Vector3 position)
        {
            MarkCell(position, _empty);
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

            for (int i = 0; i < _boardView.UserCellsView.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < _boardView.UserCellsView.GetLength(Constants.HeightDimension); j++)
                    if (_boardView.UserCellsView[i, j].transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;
                    }

            return isFind;
        }

        private void MarkCell(Vector3 position, Sprite filledStateView)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                SpriteRenderer spriteRenderer = _boardView.UserCellsView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>();

                spriteRenderer.sprite = spriteRenderer.sprite != filledStateView ? filledStateView : _blank;
            }
        }

        private void DrawGuides(Vector3 position, Color cellColor, Color leftNumberCellColor, Color topNumberCellColor)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                for (int i = 0; i < _boardView.UserCellsView.GetLength(Constants.WidthDimension); i++)
                    _boardView.UserCellsView[i, coordinate.y].GetComponent<SpriteRenderer>().color = cellColor;

                for (int j = 0; j < _boardView.UserCellsView.GetLength(Constants.HeightDimension); j++)
                    _boardView.UserCellsView[coordinate.x, j].GetComponent<SpriteRenderer>().color = cellColor;

                for (int i = 0; i < _boardView.TopNumberCells.GetLength(Constants.WidthDimension); i++)
                    _boardView.TopNumberCells[i, coordinate.y].GetComponent<SpriteRenderer>().color = topNumberCellColor;

                for (int j = 0; j < _boardView.LeftNumberCells.GetLength(Constants.HeightDimension); j++)
                    _boardView.LeftNumberCells[coordinate.x, j].GetComponent<SpriteRenderer>().color = leftNumberCellColor;
            }
        }
    }
}

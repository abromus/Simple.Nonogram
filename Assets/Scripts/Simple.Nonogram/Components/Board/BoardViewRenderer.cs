using Simple.Nonogram.Core;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(BoardView))]
    [RequireComponent(typeof(Verifier))]
    public class BoardViewRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;
        [SerializeField] private Sprite _empty;

        private BoardView _boardView;
        private Verifier _verifier;
        private bool[,] _mistakeBoard;
        private Color _defaultCellColor;
        private Color _defaultTopNumberCellColor;
        private Color _defaultLeftNumberCellColor;
        private Color _hoverCellColor = new Color(0.52f, 0.45f, 0.45f);
        private Color _hoverTopNumberCellColor = new Color(0.33f, 0.3f, 0.3f);
        private Color _hoverLeftNumberCellColor = new Color(0.33f, 0.3f, 0.3f);

        private void Start()
        {
            InitializeBoardView();
            InitializeVerifier();
            InitializeMistakeBoard();
            InitializeColors();
        }

        private void InitializeBoardView()
        {
            _boardView = GetComponent<BoardView>();

            _boardView.Clicked += OnClicked;
            _boardView.Emptied += OnEmptied;
            _boardView.HoveredBegin += OnHoveredBegin;
            _boardView.HoveredEnd += OnHoveredEnd;
        }

        private void InitializeVerifier()
        {
            _verifier = GetComponent<Verifier>();

            _verifier.MarkMistake += OnMarkMistake;
            _verifier.RemoveMistake += OnRemoveMistake;
        }

        private void InitializeMistakeBoard()
        {
            _mistakeBoard = new bool[_boardView.AnswerBoard.Height, _boardView.AnswerBoard.Width];

            for (int i = 0; i < _mistakeBoard.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < _mistakeBoard.GetLength(Constants.HeightDimension); j++)
                    _mistakeBoard[i, j] = false;
        }

        private void InitializeColors()
        {
            int x = 0;
            int y = 0;

            _defaultCellColor = _boardView.UserCellsView[x, y].GetComponent<SpriteRenderer>().color;
            _defaultTopNumberCellColor = _boardView.TopNumberCells[x, y].GetComponent<SpriteRenderer>().color;
            _defaultLeftNumberCellColor = _boardView.LeftNumberCells[x, y].GetComponent<SpriteRenderer>().color;
        }

        private void SetMistake(Vector3 position, bool isMistake)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                _mistakeBoard[coordinate.x, coordinate.y] = isMistake;

                _boardView.UserCellsView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().color = isMistake ? Color.red : Color.white;
            }
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
                    _boardView.UserCellsView[i, coordinate.y].GetComponent<SpriteRenderer>().color = _mistakeBoard[i, coordinate.y] == true ? Color.red : cellColor;

                for (int j = 0; j < _boardView.UserCellsView.GetLength(Constants.HeightDimension); j++)
                    _boardView.UserCellsView[coordinate.x, j].GetComponent<SpriteRenderer>().color = _mistakeBoard[coordinate.x, j] == true ? Color.red : cellColor;

                for (int i = 0; i < _boardView.TopNumberCells.GetLength(Constants.WidthDimension); i++)
                    _boardView.TopNumberCells[i, coordinate.y].GetComponent<SpriteRenderer>().color = topNumberCellColor;

                for (int j = 0; j < _boardView.LeftNumberCells.GetLength(Constants.HeightDimension); j++)
                    _boardView.LeftNumberCells[coordinate.x, j].GetComponent<SpriteRenderer>().color = leftNumberCellColor;
            }
        }

        private void OnClicked(Vector3 position)
        {
            MarkCell(position, _marked);
        }

        private void OnEmptied(Vector3 position)
        {
            MarkCell(position, _empty);
            SetMistake(position, false);
        }

        private void OnHoveredBegin(Vector3 position)
        {
            DrawGuides(position, _hoverCellColor, _hoverLeftNumberCellColor, _hoverTopNumberCellColor);
        }

        private void OnHoveredEnd(Vector3 position)
        {
            DrawGuides(position, _defaultCellColor, _defaultLeftNumberCellColor, _defaultTopNumberCellColor);
        }

        private void OnMarkMistake(Vector3 position)
        {
            SetMistake(position, true);
        }

        private void OnRemoveMistake(Vector3 position)
        {
            SetMistake(position, false);
        }
    }
}

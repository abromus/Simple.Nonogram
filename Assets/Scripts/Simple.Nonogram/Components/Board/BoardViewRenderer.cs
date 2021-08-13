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
        [SerializeField] private Material _lineMaterial;

        private BoardView _boardView;
        private Verifier _verifier;
        private LineRenderer _lineRenderer;
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
            InitializeAuxiliaryLines();
        }

        private void InitializeBoardView()
        {
            _boardView = GetComponent<BoardView>();

            _boardView.BoardClicked += OnBoardClicked;
            _boardView.BoardEmptied += OnBoardEmptied;
            _boardView.BoardHoveredBegin += OnBoardHoveredBegin;
            _boardView.BoardHoveredEnd += OnBoardHoveredEnd;
            _boardView.TopNumberBoardClicked += OnTopNumberBoardClicked;
            _boardView.LeftNumberBoardClicked += OnLeftNumberBoardClicked;
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

        private void InitializeAuxiliaryLines()
        {
            Color startColor = Color.grey;
            Color endColor = Color.grey;
            float width = 0.05f;
            float offsetX = _boardView.SpriteSize.Width / 2;
            float offsetY = _boardView.SpriteSize.Height / 2;
            string verticalNameGroup = "Vertical lines";
            string horizontalNameGroup = "Horizontal lines";
            string verticalBoardViewLineName = "Vertical BoardView line";
            string horizontalBoardViewLineName = "Horizontal BoardView line";
            string verticalNumberBoardLineName = "Vertical NumberBoard line";
            string horizontalNumberBoardLineName = "Horizontal NumberBoard line";
            GameObject verticalLines = new GameObject(verticalNameGroup);
            GameObject horizontalLines = new GameObject(horizontalNameGroup);

            verticalLines.transform.parent = transform;
            horizontalLines.transform.parent = transform;

            DrawBoardViewAuxiliaryLines(verticalLines,
                                        horizontalLines,
                                        verticalBoardViewLineName,
                                        horizontalBoardViewLineName,
                                        startColor,
                                        endColor,
                                        width,
                                        offsetX,
                                        offsetY);
            DrawNumberBoardAuxiliaryLines(verticalLines,
                                          horizontalLines,
                                          verticalNumberBoardLineName,
                                          horizontalNumberBoardLineName,
                                          startColor,
                                          endColor,
                                          width,
                                          offsetX,
                                          offsetY);
        }

        private bool TryFindCell(ICell[,] cells, Vector3 position, out Vector2Int coordinate)
        {
            bool isFind = false;
            coordinate = new Vector2Int(-1, -1);

            for (int i = 0; i < cells.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < cells.GetLength(Constants.HeightDimension); j++)
                    if (((MonoBehaviour)cells[i, j]).transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;
                    }

            return isFind;
        }

        private void MarkCell(Vector3 position, Sprite filledStateView)
        {
            if (TryFindCell(_boardView.UserCellsView, position, out Vector2Int coordinate))
            {
                SpriteRenderer spriteRenderer = _boardView.UserCellsView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>();

                spriteRenderer.sprite = spriteRenderer.sprite != filledStateView ? filledStateView : _blank;
                spriteRenderer.color = _hoverCellColor;
            }
        }

        private void MarkNumberCell(NumberCell[,] cells, Vector3 position)
        {
            if (TryFindCell(cells, position, out Vector2Int coordinate))
            {
                SpriteRenderer spriteRenderer = cells[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>();

                spriteRenderer.sprite = spriteRenderer.sprite == _blank ? _empty : _blank;
            }
        }

        private void SetMistake(Vector3 position, bool isMistake)
        {
            if (TryFindCell(_boardView.UserCellsView, position, out Vector2Int coordinate))
            {
                _mistakeBoard[coordinate.x, coordinate.y] = isMistake;

                _boardView.UserCellsView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().color = isMistake ? Color.red : Color.white;
            }
        }

        private void DrawGuides(Vector3 position, Color cellColor, Color leftNumberCellColor, Color topNumberCellColor)
        {
            if (TryFindCell(_boardView.UserCellsView, position, out Vector2Int coordinate))
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

        private void DrawBoardViewAuxiliaryLines(GameObject verticalLinesContainer, GameObject horizontalLinesContainer, string verticalLineName, string horizontalLineName, Color startColor, Color endColor, float width, float offsetX, float offsetY)
        {
            DrawVerticalAuxiliaryLines(_boardView.UserCellsView,
                                       verticalLinesContainer,
                                       verticalLineName,
                                       startColor,
                                       endColor,
                                       width,
                                       offsetX,
                                       offsetY,
                                       false);
            DrawHorizontalAuxiliaryLines(_boardView.UserCellsView,
                                         horizontalLinesContainer,
                                         horizontalLineName,
                                         startColor,
                                         endColor,
                                         width,
                                         offsetX,
                                         offsetY,
                                         false);
        }

        private void DrawNumberBoardAuxiliaryLines(GameObject verticalLinesContainer, GameObject horizontalLinesContainer, string verticalLineName, string horizontalLineName, Color startColor, Color endColor, float width, float offsetX, float offsetY)
        {
            DrawVerticalAuxiliaryLines(_boardView.TopNumberCells,
                                       verticalLinesContainer,
                                       verticalLineName,
                                       startColor,
                                       endColor,
                                       width,
                                       offsetX,
                                       offsetY,
                                       true);
            DrawHorizontalAuxiliaryLines(_boardView.LeftNumberCells,
                                         horizontalLinesContainer,
                                         horizontalLineName,
                                         startColor,
                                         endColor,
                                         width,
                                         offsetX,
                                         offsetY,
                                         true);
        }

        private void DrawVerticalAuxiliaryLines(ICell[,] cells, GameObject verticalLinesContainer, string verticalLineName, Color startColor, Color endColor, float width, float offsetX, float offsetY, bool isReverse)
        {
            DrawAuxiliaryLines(cells, verticalLinesContainer, verticalLineName, startColor, endColor, width, offsetX, offsetY, true, isReverse);
        }

        private void DrawHorizontalAuxiliaryLines(ICell[,] cells, GameObject horizontalLinesContainer, string horizontalLineName, Color startColor, Color endColor, float width, float offsetX, float offsetY, bool isReverse)
        {
            DrawAuxiliaryLines(cells, horizontalLinesContainer, horizontalLineName, startColor, endColor, width, offsetX, offsetY, false, isReverse);
        }

        private void DrawAuxiliaryLines(ICell[,] cells, GameObject linesContainer, string nameLineName, Color startColor, Color endColor, float width, float offsetX, float offsetY, bool isVertical = true, bool isReverse = false)
        {
            ICell[,] tempCells = isVertical ? cells : ArrayExtension.Transpose(cells);
            float startX;
            float endX;
            float startY;
            float endY;
            float z;
            int stepDivision = 5;
            int lineIndex = 0;
            int firstXIndex = 0;
            int lastXIndex = tempCells.GetLength(Constants.WidthDimension) - 1;
            int yIndex;
            int count = tempCells.GetLength(Constants.HeightDimension) / stepDivision - 1;
            int firstPoint = 0;
            int secondPoint = 1;
            int startDirecrionOffsetX = isVertical == false && isReverse ? 1 : -1;
            int endDirecrionOffsetX = isVertical == false && isReverse == false ? 1 : -1;
            int startDirecrionOffsetY = isVertical && isReverse ? -1 : 1;
            int endDirecrionOffsetY = isVertical && isReverse == false ? -1 : 1;

            for (int i = 0; i < count; i++)
            {
                _lineRenderer = new GameObject($"{nameLineName} {i}").AddComponent<LineRenderer>();
                _lineRenderer.transform.parent = linesContainer.transform;
                _lineRenderer.startColor = startColor;
                _lineRenderer.endColor = endColor;
                _lineRenderer.startWidth = width;
                _lineRenderer.endWidth = width;
                _lineRenderer.material = _lineMaterial;

                lineIndex++;
                yIndex = lineIndex * stepDivision;

                startX = ((MonoBehaviour)tempCells[firstXIndex, yIndex]).transform.position.x + startDirecrionOffsetX * offsetX;
                endX = ((MonoBehaviour)tempCells[lastXIndex, yIndex]).transform.position.x + endDirecrionOffsetX * offsetX;
                startY = ((MonoBehaviour)tempCells[firstXIndex, yIndex]).transform.position.y + startDirecrionOffsetY * offsetY;
                endY = ((MonoBehaviour)tempCells[lastXIndex, yIndex]).transform.position.y + endDirecrionOffsetY * offsetY;
                z = transform.position.z - Constants.Epsilon;

                _lineRenderer.SetPosition(firstPoint, new Vector3(startX, startY, z));
                _lineRenderer.SetPosition(secondPoint, new Vector3(endX, endY, z));
            }
        }

        private void OnBoardClicked(Vector3 position)
        {
            MarkCell(position, _marked);
        }

        private void OnBoardEmptied(Vector3 position)
        {
            MarkCell(position, _empty);
            SetMistake(position, false);
        }

        private void OnBoardHoveredBegin(Vector3 position)
        {
            DrawGuides(position, _hoverCellColor, _hoverLeftNumberCellColor, _hoverTopNumberCellColor);
        }

        private void OnBoardHoveredEnd(Vector3 position)
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

        private void OnTopNumberBoardClicked(Vector3 position)
        {
            MarkNumberCell(_boardView.TopNumberCells, position);
        }

        private void OnLeftNumberBoardClicked(Vector3 position)
        {
            MarkNumberCell(_boardView.LeftNumberCells, position);
        }
    }
}

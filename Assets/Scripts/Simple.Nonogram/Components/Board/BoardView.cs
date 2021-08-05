using System;

using Simple.Nonogram.Core;

using TMPro;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private string _pathToFile;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private NumberCell _numberCellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _numberCellsContainer;

        private AnswerBoard _answerBoard;
        private UserBoard _userBoard;
        private NumberBoard _numberBoard;
        private Cell[,] _userCellsView;
        private NumberCell[,] _topNumberCells;
        private NumberCell[,] _leftNumberCells;
        private GameObject _topNumberCellsContainer;
        private GameObject _leftNumberCellsContainer;
        private Bounds _bounds;
        private SizeF _spriteSize;

        public AnswerBoard AnswerBoard => _answerBoard;
        public UserBoard UserBoard => _userBoard;
        public Cell[,] UserCellsView => _userCellsView;
        public NumberCell[,] TopNumberCells => _topNumberCells;
        public NumberCell[,] LeftNumberCells => _leftNumberCells;
        public Bounds Bounds => _bounds;
        public SizeF SpriteSize => _spriteSize;

        public event Action<Vector3> Clicked;
        public event Action<Vector3> Emptied;
        public event Action<Vector3> HoveredBegin;
        public event Action<Vector3> HoveredEnd;

        private void Awake()
        {
            _answerBoard = new AnswerBoard(_pathToFile);
            _userBoard = new UserBoard(_answerBoard);
            _numberBoard = new NumberBoard(_answerBoard);

            _topNumberCellsContainer = new GameObject(nameof(_topNumberCellsContainer));
            _topNumberCellsContainer.transform.parent = _numberCellsContainer;
            _leftNumberCellsContainer = new GameObject(nameof(_leftNumberCellsContainer));
            _leftNumberCellsContainer.transform.parent = _numberCellsContainer;

            _userCellsView = new Cell[_answerBoard.Height, _answerBoard.Width];
            _topNumberCells = new NumberCell[_numberBoard.TopNumberCells.GetLength(Constants.WidthDimension),
                                            _numberBoard.TopNumberCells.GetLength(Constants.HeightDimension)];
            _leftNumberCells = new NumberCell[_numberBoard.LeftNumberCells.GetLength(Constants.WidthDimension),
                                            _numberBoard.LeftNumberCells.GetLength(Constants.HeightDimension)];

            Initialize();

            DefineBoardPositions();
        }

        private void Initialize()
        {
            CalculateSpriteSize();
            InitializeBoardView();
        }

        private void CalculateSpriteSize()
        {
            float widthSprite = 1;
            float heightSprite = 1;

            if (_cellPrefab.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                int numberDecimal = 2;

                widthSprite = Convert.ToSingle(Math.Round(spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit, numberDecimal));
                heightSprite = Convert.ToSingle(Math.Round(spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit, numberDecimal));
            }

            _spriteSize = new SizeF(widthSprite, heightSprite);
        }

        private void InitializeBoardView()
        {
            FillArray(_userCellsView, _cellPrefab, transform.position, Vector2.down, _cellsContainer);

            for (int i = 0; i < _userCellsView.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < _userCellsView.GetLength(Constants.HeightDimension); j++)
                {
                    _userCellsView[i, j].Clicked += OnClicked;
                    _userCellsView[i, j].Emptied += OnEmptied;
                    _userCellsView[i, j].HoveredBegin += OnHoveredBegin;
                    _userCellsView[i, j].HoveredEnd += OnHoveredEnd;
                }

            InitializeTopNumberCells();
            InitializeLeftNumberCells();
        }

        private void InitializeTopNumberCells()
        {
            InitializeNumberCells(_topNumberCells,
                                _numberCellPrefab,
                                new Vector3(transform.position.x, transform.position.y + _spriteSize.Height, transform.position.z),
                                Vector2.up,
                                _topNumberCellsContainer.transform,
                                _numberBoard.TopNumberCells);
        }

        private void InitializeLeftNumberCells()
        {
            Vector2 left = new Vector2(-1, -1);

            InitializeNumberCells(_leftNumberCells,
                              _numberCellPrefab,
                              new Vector3(transform.position.x - _spriteSize.Width, transform.position.y, transform.position.z),
                              left,
                              _leftNumberCellsContainer.transform,
                              _numberBoard.LeftNumberCells);
        }

        private void InitializeNumberCells(NumberCell[,] numberCellsView, NumberCell numberCellPrefab, Vector3 startPosition, Vector2 direction, Transform parent, int[,] numberCells)
        {
            FillArray(numberCellsView, numberCellPrefab, startPosition, direction, parent);

            for (int i = 0; i < numberCells.GetLength(Constants.WidthDimension); i++)
                for (int j = 0; j < numberCells.GetLength(Constants.HeightDimension); j++)
                    if (numberCells[i, j] > Constants.ZeroCount)
                        numberCellsView[i, j].GetComponentInChildren<TMP_Text>().text = numberCells[i, j].ToString();
        }

        private void FillArray(MonoBehaviour[,] array, MonoBehaviour prefab, Vector3 startPosition, Vector2 direction, Transform parent)
        {
            int negativeSign = -1;
            int positiveSign = 1;
            int directionX = direction.x < Constants.ZeroCount ? negativeSign : positiveSign;
            int directionY = direction.y < Constants.ZeroCount ? negativeSign : positiveSign;

            for (int i = 0; i < array.GetLength(Constants.WidthDimension); i++)
            {
                for (int j = 0; j < array.GetLength(Constants.HeightDimension); j++)
                {
                    array[i, j] = Instantiate(prefab,
                                              new Vector3(startPosition.x + directionX * j * _spriteSize.Width,
                                                          startPosition.y + directionY * i * _spriteSize.Height,
                                                          startPosition.z),
                                              Quaternion.identity);

                    array[i, j].transform.parent = parent;
                }
            }
        }

        private void DefineBoardPositions()
        {
            Vector2Int topNumberCellsSize = new Vector2Int(_topNumberCells.GetLength(Constants.WidthDimension),
                                                           _topNumberCells.GetLength(Constants.HeightDimension));
            Vector2Int leftNumberCellsSize = new Vector2Int(_leftNumberCells.GetLength(Constants.WidthDimension),
                                                            _leftNumberCells.GetLength(Constants.HeightDimension));
            int numberCellsContainerIndex = 0;
            int topNumberCellsContainerIndex = 0;
            int leftNumberCellsContainerIndex = 1;
            int topCellIndex = topNumberCellsSize.x * topNumberCellsSize.y - 1;
            int leftCellIndex = leftNumberCellsSize.y * leftNumberCellsSize.x - 1;

            _bounds.min = transform.GetChild(numberCellsContainerIndex).GetChild(leftNumberCellsContainerIndex).GetChild(leftCellIndex).position;
            _bounds.max = transform.GetChild(numberCellsContainerIndex).GetChild(topNumberCellsContainerIndex).GetChild(topCellIndex).position;
        }

        private void SetUserBoardCellState(Vector3 position, CellState filledState)
        {
            int x = Mathf.Abs((int)(position.x / _spriteSize.Width));
            int y = Mathf.Abs((int)(position.y / _spriteSize.Height));
            CellState state = _userBoard.Cells[y, x].State != filledState ? filledState : CellState.Blank;

            _userBoard.Cells[y, x].SetState(state);
        }

        private void OnClicked(Vector3 position)
        {
            SetUserBoardCellState(position, CellState.Marked);

            Clicked?.Invoke(position);
        }

        private void OnEmptied(Vector3 position)
        {
            SetUserBoardCellState(position, CellState.Empty);

            Emptied?.Invoke(position);
        }

        private void OnHoveredBegin(Vector3 position)
        {
            HoveredBegin?.Invoke(position);
        }

        private void OnHoveredEnd(Vector3 position)
        {
            HoveredEnd?.Invoke(position);
        }
    }
}

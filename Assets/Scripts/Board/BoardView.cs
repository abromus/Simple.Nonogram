using System;

using Simple.Nonogram.Core;

using TMPro;

using UnityEngine;

namespace Simple.Nonogram
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private string _pathToFile;
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private NumberCell _numberCellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [SerializeField] private Transform _numberCellsContainer;

        private Board _board;
        private NumberBoard _numberBoard;
        private Cell[,] _userCellsView;
        private Core.Cell[,] _userCells;
        private NumberCell[,] _topNumberCells;
        private NumberCell[,] _leftNumberCells;
        private GameObject _topNumberCellsContainer;
        private GameObject _leftNumberCellsContainer;
        private Bounds _bounds;
        private SizeF _spriteSize;

        private readonly int _widthDimension = 0;
        private readonly int _heightDimension = 1;

        public Bounds Bounds => _bounds;
        public SizeF SpriteSize => _spriteSize;
        public Core.Cell[,] UserCells => _userCells;
        public Cell[,] UserCellsView => _userCellsView;
        public NumberCell[,] TopNumberCells => _topNumberCells;
        public NumberCell[,] LeftNumberCells => _leftNumberCells;

        public event Action<Vector3> Clicked;
        public event Action<Vector3> Emptied;
        public event Action<Vector3> HoveredBegin;
        public event Action<Vector3> HoveredEnd;

        private void Awake()
        {
            _board = new Board(_pathToFile);
            _numberBoard = new NumberBoard(_board);
            _userCells = new Core.Cell[_board.Cells.GetLength(_heightDimension), _board.Cells.GetLength(_widthDimension)];

            _topNumberCellsContainer = new GameObject(nameof(_topNumberCellsContainer));
            _topNumberCellsContainer.transform.parent = _numberCellsContainer;
            _leftNumberCellsContainer = new GameObject(nameof(_leftNumberCellsContainer));
            _leftNumberCellsContainer.transform.parent = _numberCellsContainer;

            _userCellsView = new Cell[_board.Width, _board.Height];
            _topNumberCells = new NumberCell[_numberBoard.TopNumberCells.GetLength(_widthDimension),
                                            _numberBoard.TopNumberCells.GetLength(_heightDimension)];
            _leftNumberCells = new NumberCell[_numberBoard.LeftNumberCells.GetLength(_widthDimension),
                                            _numberBoard.LeftNumberCells.GetLength(_heightDimension)];

            Init();

            DefineBoardPositions();
        }

        private void Init()
        {
            CalculateSpriteSize();
            InitUserBoard();
            InitBoard();
            InitTopNumberCells();
            InitLeftNumberCells();
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

        private void InitUserBoard()
        {
            for (int i = 0; i < _userCells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _userCells.GetLength(_heightDimension); j++)
                    _userCells[i, j] = new Core.Cell(CellState.Blank);
        }

        private void InitBoard()
        {
            FillArray(_userCellsView, _cellPrefab, transform.position, Vector2.down, _cellsContainer);

            for (int i = 0; i < _userCellsView.GetLength(_widthDimension); i++)
                for (int j = 0; j < _userCellsView.GetLength(_heightDimension); j++)
                {
                    _userCellsView[i, j].Clicked += OnClicked;
                    _userCellsView[i, j].Emptied += OnEmptied;
                    _userCellsView[i, j].HoveredBegin += OnHoveredBegin;
                    _userCellsView[i, j].HoveredEnd += OnHoveredEnd;
                }
        }

        private void InitTopNumberCells()
        {
            FillArray(_topNumberCells,
                      _numberCellPrefab,
                      new Vector3(transform.position.x, transform.position.y + _spriteSize.Height, transform.position.z),
                      Vector2.up,
                      _topNumberCellsContainer.transform);

            for (int i = 0; i < _numberBoard.TopNumberCells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _numberBoard.TopNumberCells.GetLength(_heightDimension); j++)
                    if (_numberBoard.TopNumberCells[i, j] > 0)
                        _topNumberCells[i, j].GetComponentInChildren<TMP_Text>().text = _numberBoard.TopNumberCells[i, j].ToString();
        }

        private void InitLeftNumberCells()
        {
            Vector2 left = new Vector2(-1, -1);

            FillArray(_leftNumberCells,
                      _numberCellPrefab,
                      new Vector3(transform.position.x - _spriteSize.Width, transform.position.y, transform.position.z),
                      left,
                      _leftNumberCellsContainer.transform);

            for (int i = 0; i < _numberBoard.LeftNumberCells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _numberBoard.LeftNumberCells.GetLength(_heightDimension); j++)
                    if (_numberBoard.LeftNumberCells[i, j] > 0)
                        _leftNumberCells[i, j].GetComponentInChildren<TMP_Text>().text = _numberBoard.LeftNumberCells[i, j].ToString();
        }

        private void FillArray(MonoBehaviour[,] array, MonoBehaviour prefab, Vector3 startPosition, Vector2 direction, Transform parent)
        {
            int directionX = direction.x < 0 ? -1 : 1;
            int directionY = direction.y < 0 ? -1 : 1;
            for (int i = 0; i < array.GetLength(_widthDimension); i++)
            {
                for (int j = 0; j < array.GetLength(_heightDimension); j++)
                {
                    array[i, j] = Instantiate(prefab,
                                              new Vector3(startPosition.x + directionX * i * _spriteSize.Width,
                                                          startPosition.y + directionY * j * _spriteSize.Height,
                                                          startPosition.z),
                                              Quaternion.identity);

                    array[i, j].transform.parent = parent;
                }
            }
        }

        private void DefineBoardPositions()
        {
            Vector2Int topNumberCellsSize = new Vector2Int(_topNumberCells.GetLength(_widthDimension), _topNumberCells.GetLength(_heightDimension));
            Vector2Int leftNumberCellsSize = new Vector2Int(_leftNumberCells.GetLength(_widthDimension), _leftNumberCells.GetLength(_heightDimension));
            int numberCellsContainerIndex = 0;
            int topNumberCellsContainerIndex = 0;
            int leftNumberCellsContainerIndex = 1;
            int topCellIndex = topNumberCellsSize.x * topNumberCellsSize.y - 1;
            int leftCellIndex = leftNumberCellsSize.y * leftNumberCellsSize.x - 1;

            _bounds.min = transform.GetChild(numberCellsContainerIndex).GetChild(leftNumberCellsContainerIndex).GetChild(leftCellIndex).position;
            _bounds.max = transform.GetChild(numberCellsContainerIndex).GetChild(topNumberCellsContainerIndex).GetChild(topCellIndex).position;
        }

        private void OnClicked(Vector3 position)
        {
            Clicked?.Invoke(position);
        }

        private void OnEmptied(Vector3 position)
        {
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

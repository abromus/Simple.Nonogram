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
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;
        [SerializeField] private Sprite _empty;

        private Board _board;
        private NumberBoard _numberBoard;
        private Cell[,] _cells;
        private Core.Cell[,] _userBoard;
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

        private void Awake()
        {
            _board = new Board(_pathToFile);
            _numberBoard = new NumberBoard(_board);
            _userBoard = new Core.Cell[_board.Grid.GetLength(_heightDimension), _board.Grid.GetLength(_widthDimension)];

            _topNumberCellsContainer = new GameObject(nameof(_topNumberCellsContainer));
            _topNumberCellsContainer.transform.parent = _numberCellsContainer;
            _leftNumberCellsContainer = new GameObject(nameof(_leftNumberCellsContainer));
            _leftNumberCellsContainer.transform.parent = _numberCellsContainer;

            _cells = new Cell[_board.Width, _board.Height];
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
            for (int i = 0; i < _userBoard.GetLength(_widthDimension); i++)
                for (int j = 0; j < _userBoard.GetLength(_heightDimension); j++)
                    _userBoard[i, j] = new Core.Cell(CellState.Blank);
        }

        private void InitBoard()
        {
            FillArray(_cells, _cellPrefab, transform.position, Vector2.down, _cellsContainer);

            for (int i = 0; i < _cells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _cells.GetLength(_heightDimension); j++)
                {
                    //_cells[i, j].GetComponent<SpriteRenderer>().sprite = _board.Grid[j, i].State == CellState.Blank ? _blank : _marked;
                    _cells[i, j].GetComponent<SpriteRenderer>().sprite = _blank;
                    _cells[i, j].Clicked += OnClicked;
                    _cells[i, j].Emptied += OnEmptied;
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
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                CellState state = _userBoard[coordinate.x, coordinate.y].State == CellState.Blank ? CellState.Marked : CellState.Blank;
                Sprite stateView = _userBoard[coordinate.x, coordinate.y].State == CellState.Blank ? _marked : _blank;

                _userBoard[coordinate.x, coordinate.y].SetState(state);
                _cells[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().sprite = stateView;
            }
        }

        private void OnEmptied(Vector3 position)
        {
            if (TryFindCell(position, out Vector2Int coordinate))
            {
                CellState state = _userBoard[coordinate.x, coordinate.y].State == CellState.Blank ? CellState.Empty : CellState.Blank;
                Sprite stateView = _userBoard[coordinate.x, coordinate.y].State == CellState.Blank ? _empty : _blank;

                _userBoard[coordinate.x, coordinate.y].SetState(state);
                _cells[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().sprite = stateView;
            }
        }

        private bool TryFindCell(Vector3 position, out Vector2Int coordinate)
        {
            bool isFind = false;
            coordinate = new Vector2Int(-1, -1);

            for (int i = 0; i < _cells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _cells.GetLength(_heightDimension); j++)
                    if (_cells[i, j].transform.position == position)
                    {
                        coordinate = new Vector2Int(i, j);
                        isFind = true;
                    }

            return isFind;
        }
    }
}

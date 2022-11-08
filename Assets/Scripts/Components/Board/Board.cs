using System;
using Simple.Nonogram.Core;
using Simple.Nonogram.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private NonogramConfig _config;
        [Space]
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private Transform _cellsContainer;
        [Space]
        [SerializeField] private NumberCell _numberCellPrefab;
        [SerializeField] private Transform _numberCellsContainer;

        private AnswerBoard _answerBoard;
        private UserBoard _userBoard;
        private NumberBoard _numberBoard;
        private Cell[,] _userBoardView;
        private NumberCell[,] _top;
        private NumberCell[,] _left;
        private Bounds _bounds;
        private SizeF _spriteSize;
        private bool _isClicked;
        private PointerEventData.InputButton _button;

        public AnswerBoard AnswerBoard => _answerBoard;
        public UserBoard UserBoard => _userBoard;
        public Cell[,] UserBoardView => _userBoardView;
        public NumberCell[,] Top => _top;
        public NumberCell[,] Left => _left;
        public Bounds Bounds => _bounds;
        public SizeF SpriteSize => _spriteSize;

        public event Action<Vector3, PointerEventData.InputButton> BoardClicked;
        public event Action<Vector3, PointerEventData.InputButton> TopClicked;
        public event Action<Vector3, PointerEventData.InputButton> LeftClicked;
        public event Action<Vector3> BoardHoveredBegin;
        public event Action<Vector3> BoardHoveredEnd;

        private void Awake()
        {
            _answerBoard = new AnswerBoard(_config.PathToFile);
            _userBoard = new UserBoard(_answerBoard);
            _numberBoard = new NumberBoard(_answerBoard);

            _userBoardView = new Cell[_userBoard.Height, _userBoard.Width];
            _top = new NumberCell[_numberBoard.Top.GetLength((int)Dimension.Width), _numberBoard.Top.GetLength((int)Dimension.Height)];
            _left = new NumberCell[_numberBoard.Left.GetLength((int)Dimension.Width), _numberBoard.Left.GetLength((int)Dimension.Height)];

            Initialize();
        }

        private void Initialize()
        {
            CalculateSpriteSize();

            InitializeUserBoardView();
            InitializeNumberBoards();

            CalculateBounds();
        }

        private void InitializeNumberBoards()
        {
            var nameTop = "Top";
            var nameLeft = "Left";

            var startPositionTop = transform.position;
            var startPositionLeft = transform.position;
            var directionTop = Vector2.up;
            var directionLeft = new Vector2(-1, -1);

            startPositionTop.y += _spriteSize.Height;
            startPositionLeft.x -= _spriteSize.Width;

            InitializeTop(nameTop, startPositionTop, directionTop);
            InitializeLeft(nameLeft, startPositionLeft, directionLeft);
        }

        private void CalculateSpriteSize()
        {
            var widthSprite = 1f;
            var heightSprite = 1f;

            if (_cellPrefab.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                var digits = 2;

                widthSprite = Convert.ToSingle(Math.Round(spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit, digits));
                heightSprite = Convert.ToSingle(Math.Round(spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit, digits));
            }

            _spriteSize = new SizeF(widthSprite, heightSprite);
        }

        private void InitializeUserBoardView()
        {
            ArrayExtension.FillArray(_userBoardView, _cellPrefab, transform.position, Vector2.down, _cellsContainer, _spriteSize);

            for (int i = 0; i < _userBoardView.GetLength((int)Dimension.Width); i++)
                for (int j = 0; j < _userBoardView.GetLength((int)Dimension.Height); j++)
                {
                    _userBoardView[i, j].HoveredBegin += OnBoardHoveredBegin;
                    _userBoardView[i, j].HoveredEnd += OnBoardHoveredEnd;
                    _userBoardView[i, j].PointerDown += OnBoardPointerDown;
                    _userBoardView[i, j].PointerUp += OnBoardPointerUp;
                }
        }

        private void InitializeTop(string name, Vector3 startPosition, Vector2 direction)
        {
            InitializeNumberCells(_top, name, _numberCellPrefab, startPosition, direction, _numberBoard.Top, OnTopPointerDown);
        }

        private void InitializeLeft(string name, Vector3 startPosition, Vector2 direction)
        {
            InitializeNumberCells(_left, name, _numberCellPrefab, startPosition, direction, _numberBoard.Left, OnLeftPointerDown);
        }

        private void InitializeNumberCells(
            NumberCell[,] cellsView,
            string name,
            NumberCell prefab,
            Vector3 startPosition,
            Vector2 direction,
            int[,] cells,
            Action<Vector3, PointerEventData.InputButton> onPointerDown)
        {
            var ñontainer = new GameObject(name);
            ñontainer.transform.parent = _numberCellsContainer.transform;

            ArrayExtension.FillArray(cellsView, prefab, startPosition, direction, ñontainer.transform, _spriteSize);

            for (int i = 0; i < cells.GetLength((int)Dimension.Width); i++)
                for (int j = 0; j < cells.GetLength((int)Dimension.Height); j++)
                    if (cells[i, j] > 0)
                    {
                        cellsView[i, j].GetComponentInChildren<TMP_Text>().text = cells[i, j].ToString();
                        cellsView[i, j].PointerDown += onPointerDown;
                    }
        }

        private void CalculateBounds()
        {
            var containerIndex = 0;
            var topContainerIndex = 0;
            var leftContainerIndex = 1;

            var topCellsSize = new Vector2Int(_top.GetLength((int)Dimension.Width), _top.GetLength((int)Dimension.Height));
            var leftCellsSize = new Vector2Int(_left.GetLength((int)Dimension.Width), _left.GetLength((int)Dimension.Height));
            var topCellIndex = topCellsSize.x * topCellsSize.y - 1;
            var leftCellIndex = leftCellsSize.y * leftCellsSize.x - 1;

            _bounds.min = transform.GetChild(containerIndex).GetChild(leftContainerIndex).GetChild(leftCellIndex).position;
            _bounds.max = transform.GetChild(containerIndex).GetChild(topContainerIndex).GetChild(topCellIndex).position;
        }

        private void SetState(Vector3 position, CellState filledState)
        {
            var x = Mathf.Abs((int)(position.x / _spriteSize.Width));
            var y = Mathf.Abs((int)(position.y / _spriteSize.Height));
            var state = _userBoard.Cells[y, x].State != filledState ? filledState : CellState.Blank;

            _userBoard.Cells[y, x].SetState(state);
        }

        private void CheckDragging(Vector3 position)
        {
            if (_isClicked)
                OnBoardClicked(position, _button);
        }

        private void OnBoardClicked(Vector3 position, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
                SetState(position, CellState.Marked);
            else if (button == PointerEventData.InputButton.Right)
                SetState(position, CellState.Empty);

            BoardClicked?.Invoke(position, button);
        }

        private void OnBoardHoveredBegin(Vector3 position, PointerEventData.InputButton button)
        {
            if (_isClicked)
                CheckDragging(position);

            BoardHoveredBegin?.Invoke(position);
        }

        private void OnBoardHoveredEnd(Vector3 position, PointerEventData.InputButton button)
        {
            BoardHoveredEnd?.Invoke(position);
        }

        private void OnBoardPointerDown(Vector3 position, PointerEventData.InputButton button)
        {
            _isClicked = true;
            _button = button;

            OnBoardClicked(position, button);
        }

        private void OnBoardPointerUp(Vector3 position, PointerEventData.InputButton button)
        {
            _isClicked = false;
        }

        private void OnTopPointerDown(Vector3 position, PointerEventData.InputButton button)
        {
            TopClicked?.Invoke(position, button);
        }

        private void OnLeftPointerDown(Vector3 position, PointerEventData.InputButton button)
        {
            LeftClicked?.Invoke(position, button);
        }
    }
}

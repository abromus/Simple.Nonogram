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

        private Board _board;
        private NumberBoard _numberBoard;
        private Cell[,] _cells;
        private NumberCell[,] _topNumberCells;
        private NumberCell[,] _leftNumberCells;

        private readonly int _widthDimension = 0;
        private readonly int _heightDimension = 1;


        private void Awake()
        {
            _board = new Board(_pathToFile);
            _numberBoard = new NumberBoard(_board);

            _cells = new Cell[_board.Width, _board.Height];
            _topNumberCells = new NumberCell[_numberBoard.TopNumberCells.GetLength(_widthDimension),
                                            _numberBoard.TopNumberCells.GetLength(_heightDimension)];
            _leftNumberCells = new NumberCell[_numberBoard.LeftNumberCells.GetLength(_widthDimension),
                                            _numberBoard.LeftNumberCells.GetLength(_heightDimension)];

            Init();
        }

        private void Init()
        {
            float widthSprite = 1;
            float heightSprite = 1;

            if (_cellPrefab.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                widthSprite = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
                heightSprite = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
            }

            InitBoard(widthSprite, heightSprite);
            InitTopNumberCells(widthSprite, heightSprite);
            InitLeftNumberCells(widthSprite, heightSprite);
        }

        private void InitBoard(float widthSprite, float heightSprite)
        {
            FillArray(_cells,
                      _cellPrefab,
                      transform.position,
                      Vector2.down,
                      widthSprite,
                      heightSprite,
                      _cellsContainer);

            for (int i = 0; i < _cells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _cells.GetLength(_heightDimension); j++)
                    _cells[i, j].GetComponent<SpriteRenderer>().sprite = _board.Grid[j, i].State == CellState.Blank ? _blank : _marked;
        }

        private void InitTopNumberCells(float widthSprite, float heightSprite)
        {
            FillArray(_topNumberCells,
                      _numberCellPrefab,
                      new Vector3(transform.position.x, transform.position.y + heightSprite, transform.position.z),
                      Vector2.up,
                      widthSprite,
                      heightSprite,
                      _numberCellsContainer);

            for (int i = 0; i < _numberBoard.TopNumberCells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _numberBoard.TopNumberCells.GetLength(_heightDimension); j++)
                    if (_numberBoard.TopNumberCells[i, j] > 0)
                        _topNumberCells[i, j].GetComponentInChildren<TMP_Text>().text = _numberBoard.TopNumberCells[i, j].ToString();
        }

        private void InitLeftNumberCells(float widthSprite, float heightSprite)
        {
            Vector2 left = new Vector2(-1, -1);

            FillArray(_leftNumberCells,
                      _numberCellPrefab,
                      new Vector3(transform.position.x - widthSprite, transform.position.y, transform.position.z),
                      left,
                      widthSprite,
                      heightSprite,
                      _numberCellsContainer);

            for (int i = 0; i < _numberBoard.LeftNumberCells.GetLength(_widthDimension); i++)
                for (int j = 0; j < _numberBoard.LeftNumberCells.GetLength(_heightDimension); j++)
                    if (_numberBoard.LeftNumberCells[i, j] > 0)
                        _leftNumberCells[i, j].GetComponentInChildren<TMP_Text>().text = _numberBoard.LeftNumberCells[i, j].ToString();
        }

        private void FillArray(MonoBehaviour[,] array, MonoBehaviour prefab, Vector3 startPosition, Vector2 direction, float widthSprite, float heightSprite, Transform parent)
        {
            int directionX = direction.x < 0 ? -1 : 1;
            int directionY = direction.y < 0 ? -1 : 1;
            for (int i = 0; i < array.GetLength(_widthDimension); i++)
            {
                for (int j = 0; j < array.GetLength(_heightDimension); j++)
                {
                    array[i, j] = Instantiate(prefab,
                                              new Vector3(startPosition.x + directionX * i * widthSprite,
                                                          startPosition.y + directionY * j * heightSprite,
                                                          startPosition.z),
                                              Quaternion.identity);

                    array[i, j].transform.parent = parent;
                }
            }
        }
    }
}

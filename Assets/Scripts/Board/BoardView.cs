
using UnityEngine;

namespace Simple.Nonogram
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private string _pathToFile;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;

        private Board _board;
        private GameObject[,] _grid;

        private void Start()
        {
            _board = new Board(_pathToFile);
            _grid = new GameObject[_board.Width, _board.Height];
            InitBoard();
        }

        private void InitBoard()
        {
            float widthSprite = 1;
            float heightSprite = 1; 

            if(_prefab.TryGetComponent(out SpriteRenderer spriteRenderer))
            {
                widthSprite = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;
                heightSprite = spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;
            }

            for (int i = 0; i < _board.Width; i++)
            {
                for (int j = 0; j < _board.Height; j++)
                {
                    _grid[i, j] = Instantiate(_prefab,
                                              new Vector3(transform.position.x + i * widthSprite,
                                                          transform.position.y - j * heightSprite,
                                                          transform.position.z),
                                              Quaternion.identity);

                    _grid[i, j].GetComponent<SpriteRenderer>().sprite = _board.Grid[j, i].State == CellState.Blank ? _blank : _marked;
                    _grid[i, j].transform.parent = transform;
                }
            }
        }
    }
}

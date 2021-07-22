using UnityEngine;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(BoardView))]
    public class BoardViewMovement : MonoBehaviour
    {
        [SerializeField] private SpriteMask _mask;
        [SerializeField] private Camera _camera;
        [SerializeField] [Range(0, 2)] private float _horizontalSpeed = 0.05f;
        [SerializeField] [Range(0, 2)] private float _verticalSpeed = 0.05f;

        private BoardView _board;
        private Vector3 _startMovePosition;
        private Vector3 _minBoardPosition;
        private Vector3 _maxBoardPosition;

        private void Start()
        {
            _board = GetComponent<BoardView>();
            DefineBoardPositions();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _startMovePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            else if (Input.GetMouseButton(0))
                Move();
        }

        private void DefineBoardPositions()
        {
            if (_board.transform.childCount != 0)
            {
                int cellIndex = 1;
                int firstIndex = 0;
                int lastIndex = _board.transform.GetChild(cellIndex).childCount - 1;

                Transform firstChild = _board.transform.GetChild(cellIndex).GetChild(firstIndex);
                Transform lastChild = _board.transform.GetChild(cellIndex).GetChild(lastIndex);
                Vector3 sizeCell = firstChild.GetComponent<SpriteRenderer>().bounds.size;

                _maxBoardPosition = _board.transform.position;
                _minBoardPosition = new Vector3(_maxBoardPosition.x - (lastChild.position.x - firstChild.position.x + sizeCell.x) + _mask.bounds.size.x,
                                                _maxBoardPosition.y + (firstChild.position.y - lastChild.position.y + sizeCell.y) - _mask.bounds.size.y,
                                                _maxBoardPosition.z);
            }
        }

        private void Move()
        {
            float positionX = _camera.ScreenToWorldPoint(Input.mousePosition).x - _startMovePosition.x;
            float positionY = _camera.ScreenToWorldPoint(Input.mousePosition).y - _startMovePosition.y;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x + positionX * _horizontalSpeed,
                                                        _minBoardPosition.x,
                                                        _maxBoardPosition.x),
                                             Mathf.Clamp(transform.position.y + positionY * _verticalSpeed,
                                                        _maxBoardPosition.y,
                                                        _minBoardPosition.y),
                                             transform.position.z);
        }
    }
}

using Simple.Nonogram.Core;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private BoardView _board;
        [SerializeField] [Range(0, 1)] private float _horizontalSpeed = 0.05f;
        [SerializeField] [Range(0, 1)] private float _verticalSpeed = 0.05f;

        private Camera _camera;
        private Bounds _bounds;
        private Vector3 _startPosition;
        private int _moveButton = Constants.MiddleButton;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            CalculateBounds();

            if (_bounds.min.x < _board.Bounds.min.x && _bounds.max.x > _board.Bounds.max.x && _bounds.min.y == _board.Bounds.min.y)
                transform.position = new Vector3(_bounds.max.x, _bounds.min.y, _camera.transform.position.z);
            else if (_bounds.min.x < _board.Bounds.min.x && _bounds.max.x > _board.Bounds.max.x && _bounds.min.y > _board.Bounds.min.y)
                transform.position = new Vector3(_bounds.max.x, _bounds.max.y, _camera.transform.position.z);
            else if (_bounds.min.x > _board.Bounds.min.x && _bounds.max.x < _board.Bounds.max.x && _bounds.min.y == _board.Bounds.min.y)
                transform.position = new Vector3(_bounds.min.x, _bounds.min.y, _camera.transform.position.z);
            else if (_bounds.min.x > _board.Bounds.min.x && _bounds.max.x < _board.Bounds.max.x)
                transform.position = new Vector3(_bounds.min.x, _bounds.max.y, _camera.transform.position.z);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(_moveButton))
                _startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            else if (Input.GetMouseButton(_moveButton))
                Move();
        }

        private void CalculateBounds()
        {
            Vector2 bottomLeft = _camera.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 topRight = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth, _camera.pixelHeight));
            float x1 = _board.Bounds.min.x - bottomLeft.x - _board.SpriteSize.Width;
            float x2 = _board.Bounds.max.x - topRight.x + _board.SpriteSize.Width;
            float y1 = _board.Bounds.min.y - bottomLeft.y - _board.SpriteSize.Height;
            float y2 = _board.Bounds.max.y - topRight.y + _board.SpriteSize.Height;
            float z = _camera.transform.position.z;

            _bounds.min = new Vector3(x1 < x2 ? x1 : x2, y1 < y2 ? y1 : y2, z);
            _bounds.max = new Vector3(x2 > x1 ? x2 : x1, y2 > y1 ? y2 : y1, z);
        }

        private void Move()
        {
            float positionX = _camera.ScreenToWorldPoint(Input.mousePosition).x - _startPosition.x;
            float positionY = _camera.ScreenToWorldPoint(Input.mousePosition).y - _startPosition.y;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x - positionX * _horizontalSpeed, _bounds.min.x, _bounds.max.x),
                                             Mathf.Clamp(transform.position.y - positionY * _verticalSpeed, _bounds.min.y, _bounds.max.y),
                                             transform.position.z);
        }
    }
}

using Simple.Nonogram.Core;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] [Range((float)Number.Zero, (float)Number.One)] private float _horizontalSpeed = 0.5f;
        [SerializeField] [Range((float)Number.Zero, (float)Number.One)] private float _verticalSpeed = 0.5f;

        private Camera _camera;
        private Bounds _bounds;
        private Vector3 _startPosition;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            Initialize();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown((int)PointerEventData.InputButton.Middle))
                _startPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            else if (Input.GetMouseButton((int)PointerEventData.InputButton.Middle))
                Move();
        }

        private void Initialize()
        {
            CalculateBounds();

            Vector2 topLeft = _camera.ScreenToWorldPoint(new Vector2((int)Number.Zero, _camera.pixelHeight));
            float x = _board.Bounds.min.x - topLeft.x - _board.SpriteSize.Width;
            float y = _board.Bounds.max.y - topLeft.y + _board.SpriteSize.Height;

            transform.position = new Vector3(Mathf.Clamp(x, _bounds.min.x, _bounds.max.x),
                                             Mathf.Clamp(y, _bounds.min.y, _bounds.max.y),
                                             transform.position.z);
        }

        private void CalculateBounds()
        {
            Vector2 bottomLeft = _camera.ScreenToWorldPoint(new Vector2((int)Number.Zero, (int)Number.Zero));
            Vector2 topRight = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth, _camera.pixelHeight));
            float x1 = _board.Bounds.min.x - bottomLeft.x - _board.SpriteSize.Width;
            float x2 = _board.Bounds.max.x - topRight.x + _board.SpriteSize.Width;
            float y1 = _board.Bounds.min.y - bottomLeft.y - _board.SpriteSize.Height;
            float y2 = _board.Bounds.max.y - topRight.y + _board.SpriteSize.Height;
            float z = _camera.transform.position.z;

            _bounds.min = new Vector3(Mathf.Min(x1, x2), Mathf.Min(y1, y2), z);
            _bounds.max = new Vector3(Mathf.Max(x1, x2), Mathf.Max(y1, y2), z);
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

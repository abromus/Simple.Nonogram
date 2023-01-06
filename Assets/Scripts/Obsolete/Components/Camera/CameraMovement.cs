using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CameraZoom))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] [Range(0f, 1f)] private float _horizontalSpeed = 0.5f;
        [SerializeField] [Range(0f, 1f)] private float _verticalSpeed = 0.5f;
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraZoom _zoom;

        private CameraBounds _cameraBounds;
        private Vector3 _startPosition;
        private Bounds _currentBounds;
        private BoundsStrategy _boundsStrategy;

        private void Start()
        {
            _zoom.ZoomChanged += OnZoomChanged;
            _cameraBounds = new CameraBounds(_camera, _board.Bounds, _board.SpriteSize);

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
            var topLeft = _camera.ScreenToWorldPoint(new Vector2(0, _camera.pixelHeight));
            var x = _board.Bounds.min.x - topLeft.x - _board.SpriteSize.Width;
            var y = _board.Bounds.max.y - topLeft.y + _board.SpriteSize.Height;

            transform.position = new Vector3(
                Mathf.Clamp(x, _cameraBounds.Min.x, _cameraBounds.Max.x),
                Mathf.Clamp(y, _cameraBounds.Min.y, _cameraBounds.Max.y),
                transform.position.z);

            DefineStrategy();
        }

        private void DefineStrategy()
        {
            if (_board.Bounds.min.x > _cameraBounds.Min.x)
            {
                if (_board.Bounds.min.y == _cameraBounds.Min.y && _board.Bounds.max.y == _cameraBounds.Max.y)
                    SetStrategy(new SmallBoardStrategy(_camera, _cameraBounds));
                else
                    SetStrategy(new HighBoardStrategy(_camera, _cameraBounds));
            }
            else
            {
                if (_cameraBounds.Max.y > 0)
                    SetStrategy(new WideBoardStrategy(_camera, _cameraBounds));
                else
                    SetStrategy(new BigBoardStrategy(_camera, _cameraBounds));
            }
        }

        private void SetStrategy(BoundsStrategy strategy)
        {
            _boundsStrategy = strategy;
        }

        private void Move()
        {
            var positionX = _camera.ScreenToWorldPoint(Input.mousePosition).x - _startPosition.x;
            var positionY = _camera.ScreenToWorldPoint(Input.mousePosition).y - _startPosition.y;

            _currentBounds = _boundsStrategy.Calculate();

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x - positionX * _horizontalSpeed, _currentBounds.min.x, _currentBounds.max.x),
                Mathf.Clamp(transform.position.y - positionY * _verticalSpeed, _currentBounds.min.y, _currentBounds.max.y),
                transform.position.z);
        }

        private void OnZoomChanged()
        {
            Move();
        }
    }
}

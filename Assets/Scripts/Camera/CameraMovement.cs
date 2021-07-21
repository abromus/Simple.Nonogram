using UnityEngine;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(Camera))]
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private BoardView _board;
        [SerializeField] [Range(1, 20)] private float _horizontalSpeed = 1.0f;
        [SerializeField] [Range(1, 20)] private float _verticalSpeed = 1.0f;

        private Camera _camera;
        private Vector3 _startMovePosition;
        private Vector3 _startCameraPosition;
        private Vector3 _lastChildPosition;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            _startCameraPosition = _camera.transform.position;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startMovePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

                if(_board.transform.childCount != 0)
                {
                    int lastChildIndex = _board.transform.childCount - 1;
                    _lastChildPosition = _board.transform.GetChild(lastChildIndex).position;

                }
            }
            else if (Input.GetMouseButton(0))
            {
                Move();
            }
        }

        private void Move()
        {
            float positionX = _camera.ScreenToWorldPoint(Input.mousePosition).x - _startMovePosition.x;
            float positionY = _camera.ScreenToWorldPoint(Input.mousePosition).y - _startMovePosition.y;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x - positionX * _horizontalSpeed,
                                                        _startCameraPosition.x,
                                                        _lastChildPosition.x - _startCameraPosition.x),
                                             Mathf.Clamp(transform.position.y - positionY * _verticalSpeed,
                                                        _startCameraPosition.y,
                                                        _lastChildPosition.y - _startCameraPosition.y),
                                             transform.position.z);
        }
    }
}

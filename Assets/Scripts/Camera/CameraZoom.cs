using UnityEngine;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float _wheelSpeed = 50f;
        [SerializeField] private float _minSize = 1f;
        [SerializeField] private float _maxSize = 10f;

        private Camera _camera;
        private float _mouseScrollWheel;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            _mouseScrollWheel = Input.GetAxis("Mouse ScrollWheel");

            if (_mouseScrollWheel != 0)
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + _mouseScrollWheel * _wheelSpeed * Time.deltaTime, _minSize, _maxSize);
        }
    }
}

using Simple.Nonogram.Core;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private float _wheelSpeed = 200f;
        [SerializeField] private float _minSize = 1f;
        [SerializeField] private float _maxSize = 10f;

        private Camera _camera;
        private float _mouseScrollWheel;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            if (_camera.TryGetComponent(out Physics2DRaycaster _) == false)
                _camera.gameObject.AddComponent<Physics2DRaycaster>();
        }

        private void Update()
        {
            _mouseScrollWheel = Input.GetAxis(Constants.MouseAxis);

            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + _mouseScrollWheel * _wheelSpeed * Time.deltaTime, _minSize, _maxSize);
        }
    }
}

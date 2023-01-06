using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Camera), typeof(Physics2DRaycaster))]
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _wheelSpeed = 200f;
        [SerializeField] private float _minSize = 1f;
        [SerializeField] private float _maxSize = 10f;

        private float _mouseScrollWheel;

        private readonly string _mouseAxis = "Mouse ScrollWheel";

        public event Action ZoomChanged;

        private void Update()
        {
            _mouseScrollWheel = Input.GetAxis(_mouseAxis);

            if (_mouseScrollWheel == 0)
                return;

            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + _mouseScrollWheel * _wheelSpeed * Time.deltaTime, _minSize, _maxSize);

            ZoomChanged?.Invoke();
        }
    }
}

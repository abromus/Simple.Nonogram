using Simple.Nonogram.Core;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class CameraBounds
    {
        private Bounds _bounds;

        private readonly Camera _camera;
        private readonly Bounds _boardBounds;
        private readonly SizeF _spriteSize;

        public Vector3 Min => _bounds.min;
        public Vector3 Max => _bounds.max;

        public CameraBounds(Camera camera, Bounds boardBounds, SizeF spriteSize)
        {
            _camera = camera;
            _boardBounds = boardBounds;
            _spriteSize = spriteSize;

            CalculateBounds();
        }

        private void CalculateBounds()
        {
            Vector2 bottomLeft = _camera.ScreenToWorldPoint(new Vector2((int)Number.Zero, (int)Number.Zero));
            Vector2 topRight = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth, _camera.pixelHeight));
            float x1 = _boardBounds.min.x - bottomLeft.x - _spriteSize.Width;
            float x2 = _boardBounds.max.x - topRight.x + _spriteSize.Width;
            float y1 = _boardBounds.min.y - bottomLeft.y - _spriteSize.Height;
            float y2 = _boardBounds.max.y - topRight.y + _spriteSize.Height;
            float z = _camera.transform.position.z;

            _bounds.min = new Vector3(Mathf.Min(x1, x2), Mathf.Min(y1, y2), z);
            _bounds.max = new Vector3(Mathf.Max(x1, x2), Mathf.Max(y1, y2), z);
        }
    }
}

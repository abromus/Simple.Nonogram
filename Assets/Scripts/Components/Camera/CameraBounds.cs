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
            var bottomLeft = _camera.ScreenToWorldPoint(new Vector2(0, 0));
            var topRight = _camera.ScreenToWorldPoint(new Vector2(_camera.pixelWidth, _camera.pixelHeight));
            var x1 = _boardBounds.min.x - bottomLeft.x - _spriteSize.Width;
            var x2 = _boardBounds.max.x - topRight.x + _spriteSize.Width;
            var y1 = _boardBounds.min.y - bottomLeft.y - _spriteSize.Height;
            var y2 = _boardBounds.max.y - topRight.y + _spriteSize.Height;
            var z = _camera.transform.position.z;

            _bounds.min = new Vector3(Mathf.Min(x1, x2), Mathf.Min(y1, y2), z);
            _bounds.max = new Vector3(Mathf.Max(x1, x2), Mathf.Max(y1, y2), z);
        }
    }
}

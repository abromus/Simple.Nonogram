using System;

using Simple.Nonogram.Core;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    public abstract class BoundsStrategy
    {
        private readonly Camera _camera;
        private readonly CameraBounds _cameraBounds;
        private readonly float _defaultOrthographicSize;
        private readonly float _signX1;
        private readonly float _signX2;
        private readonly float _signY1;
        private readonly float _signY2;

        protected BoundsStrategy(Camera camera, CameraBounds cameraBounds, Tuple<bool, bool> signs)
        {
            _camera = camera;
            _cameraBounds = cameraBounds;
            _defaultOrthographicSize = camera.orthographicSize;

            _signX1 = signs.Item1 ? 1 : -1;
            _signX2 = (int)Number.MinusOne * _signX1;
            _signY1 = signs.Item2 ? 1 : -1;
            _signY2 = (int)Number.MinusOne * _signY1;
        }

        public Bounds Calculate()
        {
            float differenceOrthographicSize = _defaultOrthographicSize - _camera.orthographicSize;
            float x1 = _cameraBounds.Min.x + _signX1 * differenceOrthographicSize * _camera.aspect;
            float x2 = _cameraBounds.Max.x + _signX2 * differenceOrthographicSize * _camera.aspect;
            float y1 = _cameraBounds.Min.y + _signY1 * differenceOrthographicSize;
            float y2 = _cameraBounds.Max.y + _signY2 * differenceOrthographicSize;
            float z = _camera.transform.position.z;

            return new Bounds {
                min = new Vector3(Mathf.Min(x1, x2), Mathf.Min(y1, y2), z),
                max = new Vector3(Mathf.Max(x1, x2), Mathf.Max(y1, y2), z)
            };
        }
    }
}
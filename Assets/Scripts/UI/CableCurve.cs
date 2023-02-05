using System;
using UnityEngine;

namespace Simple.Nonogram.UI
{
    public class CableCurve
    {
        private Vector2[] _points;
        private Vector2 _start;
        private Vector2 _end;
        private bool _regen;

        private readonly float _slack;
        private readonly int _steps;
        private readonly Vector2[] _emptyCurve = new Vector2[] { Vector2.zero, Vector2.zero };

        public CableCurve(Vector2[] inputPoints)
        {
            _points = inputPoints;
            _start = inputPoints[0];
            _end = inputPoints[1];
            _regen = true;
            _slack = 0.5f;
            _steps = 20;
        }

        public Vector2[] GetPoints()
        {
            if (!_regen)
                return _points;

            if (_steps < 2 || Vector2.Distance(new Vector2(_end.x, _start.y), _start) == 0.0f)
                return _emptyCurve;

            _points = CalculatePoints();

            _regen = false;

            return _points;
        }

        private Vector2[] CalculatePoints()
        {
            var points = new Vector2[_steps];

            var hypotenuse = Vector2.Distance(_end, _start);
            var cathetus = Vector2.Distance(new Vector2(_end.x, _start.y), _start);
            var distanceY = _end.y - _start.y;
            var l = hypotenuse + _slack;

            var zTarget = Mathf.Sqrt(Mathf.Pow(l, 2.0f) - Mathf.Pow(_end.y - _start.y, 2.0f)) / cathetus;
            var z = GetZ(zTarget);

            var a = cathetus / z / 2.0f;
            var p = (cathetus - a * Mathf.Log((l + distanceY) / (l - distanceY))) / 2.0f;
            var q = (_end.y + _start.y - l * (float)Math.Cosh(z) / (float)Math.Sinh(z)) / 2.0f;

            for (int i = 0; i < _steps; i++)
            {
                var steps = i / (float)_steps;
                points[i] = new Vector2(Mathf.Lerp(_start.x, _end.x, steps), a * (float)Math.Cosh((steps * cathetus - p) / a) + q);
            }

            return points;
        }

        private float GetZ(float zTarget)
        {
            var loops = 30;
            var iterationCount = 10;
            var found = false;

            var z = 0.0f;
            var zStep = 100.0f;

            for (int i = 0; i < loops; i++)
            {
                for (int j = 0; j < iterationCount; j++)
                {
                    var zTest = z + zStep;
                    var zTestTarget = (float)Math.Sinh(zTest) / zTest;

                    if (float.IsInfinity(zTestTarget))
                        continue;

                    if (zTarget < zTestTarget)
                        break;

                    found = zTarget == zTestTarget;
                    z = zTest;
                }

                if (found)
                    break;

                zStep *= 0.1f;
            }

            return z;
        }
    }
}

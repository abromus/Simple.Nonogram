using System.Collections.Generic;
using UnityEngine;

namespace Simple.Nonogram.UI
{
    public class BezierPath
    {
        private int _curveCount;

        private readonly List<Vector2> _controlPoints;
        private readonly Vector2[] _pointsToDraw;
        private readonly int _segmentsPerCurve;
        private readonly BezierType _mode;
        private readonly int _curvePointsCount = 3;

        private readonly float _epsilon = 0.0001f;
        private readonly float _minimumSqrDistance = 0.01f;
        private readonly float _divisionThreshold = -0.99f;

        public BezierPath(Vector2[] pointsToDraw, int segmentsPerCurve, BezierType mode)
        {
            _controlPoints = new List<Vector2>();
            _pointsToDraw = pointsToDraw;
            _segmentsPerCurve = segmentsPerCurve;
            _mode = mode;
        }

        public Vector2[] GetPoints()
        {
            SetControlPoints(_pointsToDraw);

            var drawingPoints = _mode == BezierType.Basic
                ? GetDrawingPointsBasic()
                : _mode == BezierType.Improved
                    ? GetDrawingPointsImproved()
                    : GetDrawingPoints();

            return drawingPoints.ToArray();
        }

        private void SetControlPoints(Vector2[] newControlPoints)
        {
            _controlPoints.Clear();
            _controlPoints.AddRange(newControlPoints);
            _curveCount = (_controlPoints.Count - 1) / _curvePointsCount;
        }

        private List<Vector2> GetDrawingPointsBasic()
        {
            var drawingPoints = new List<Vector2> { CalculatePoint(0, 0) };

            for (int i = 1; i < _curveCount; i++)
                for (int j = 1; j <= _segmentsPerCurve; j++)
                {
                    var t = j / (float)_segmentsPerCurve;
                    drawingPoints.Add(CalculatePoint(i, t));
                }

            return drawingPoints;
        }

        private List<Vector2> GetDrawingPointsImproved()
        {
            var drawingPoints = new List<Vector2>();
            var step = 3;

            for (int i = 0; i < _controlPoints.Count - step; i += step)
            {
                var point0 = _controlPoints[i];
                var point1 = _controlPoints[i + 1];
                var point2 = _controlPoints[i + 2];
                var point3 = _controlPoints[i + 3];

                if (i == 0)
                    drawingPoints.Add(CalculateCubicCurve(0, point0, point1, point2, point3));

                for (int j = 1; j <= _segmentsPerCurve; j++)
                    drawingPoints.Add(CalculateCubicCurve(j / (float)_segmentsPerCurve, point0, point1, point2, point3));
            }

            return drawingPoints;
        }

        private List<Vector2> GetDrawingPoints()
        {
            var drawingPoints = new List<Vector2>();

            for (int i = 0; i < _curveCount; i++)
            {
                var curveDrawingPoints = FindDrawingPoints(i);

                if (i != 0)
                    curveDrawingPoints.RemoveAt(0);

                drawingPoints.AddRange(curveDrawingPoints);
            }

            return drawingPoints;
        }

        private Vector2 CalculatePoint(int curveIndex, float t)
        {
            var nodeIndex = curveIndex * 3;

            var point0 = _controlPoints[nodeIndex];
            var point1 = _controlPoints[nodeIndex + 1];
            var point2 = _controlPoints[nodeIndex + 2];
            var point3 = _controlPoints[nodeIndex + 3];

            return CalculateCubicCurve(t, point0, point1, point2, point3);
        }

        private Vector2 CalculateCubicCurve(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            var u = 1 - t;
            var tt = t * t;
            var uu = u * u;
            var uuu = uu * u;
            var ttt = tt * t;

            var p = uuu * p0;

            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }

        private List<Vector2> FindDrawingPoints(int curveIndex)
        {
            var pointList = new List<Vector2>();
            var firstIndex = 0;
            var secondIndex = 1;
            var insertionIndex = 1;

            var left = CalculatePoint(curveIndex, firstIndex);
            var right = CalculatePoint(curveIndex, secondIndex);

            pointList.Add(left);
            pointList.Add(right);

            FindDrawingPoints(curveIndex, firstIndex, secondIndex, pointList, insertionIndex);

            return pointList;
        }

        private int FindDrawingPoints(int curveIndex, float firstIndex, float secondIndex, List<Vector2> pointList, int insertionIndex)
        {
            var leftPoint = CalculatePoint(curveIndex, firstIndex);
            var rightPoint = CalculatePoint(curveIndex, secondIndex);

            if ((leftPoint - rightPoint).sqrMagnitude < _minimumSqrDistance)
                return 0;

            var middleIndex = (firstIndex + secondIndex) / 2;
            var middlePoint = CalculatePoint(curveIndex, middleIndex);

            var leftDirection = (leftPoint - middlePoint).normalized;
            var rightDirection = (rightPoint - middlePoint).normalized;

            var pointsAddedCount = 0;

            if (Vector2.Dot(leftDirection, rightDirection) <= _divisionThreshold && Mathf.Abs(middleIndex) >= _epsilon)
                return pointsAddedCount;

            pointsAddedCount += FindDrawingPoints(curveIndex, firstIndex, middleIndex, pointList, insertionIndex);
            pointList.Insert(insertionIndex + pointsAddedCount, middlePoint);
            pointsAddedCount++;
            pointsAddedCount += FindDrawingPoints(curveIndex, middleIndex, secondIndex, pointList, insertionIndex + pointsAddedCount);

            return pointsAddedCount;
        }
    }
}

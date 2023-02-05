using System.Collections.Generic;
using Simple.Nonogram.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasRenderer))]
    public class UiLineRenderer : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private ResolutionMode _improveResolution;
        [SerializeField] private float _resolution;
        [SerializeField] private bool _useNativeSize;

        [SerializeField] private Vector2[] _points;

        [SerializeField] private bool _lineList;
        [SerializeField] private bool _relativeSize;
        [SerializeField] private bool _lineCaps;
        [SerializeField, Min(0.01f)] private float _lineThickness = 2f;
        [SerializeField, Range(1f, 180f)] private float _minBevelDegrees = 30;

        [SerializeField, Min(0)] private int _bezierSegmentsPerCurve = 10;
        [SerializeField] private BezierType _bezierMode = BezierType.None;

        private Sprite _activeSprite;
        private float _pixelsPerUnit;
        private float _minBevelRadians;

        private UV _uv;

        private readonly Sprite _overrideSprite;
        private readonly float _eventAlphaThreshold = 1f;

        protected override void Awake()
        {
            _activeSprite = _overrideSprite != null ? _overrideSprite : _sprite;

            var spritePixelsPerUnit = _activeSprite ? _activeSprite.pixelsPerUnit : 100f;
            var referencePixelsPerUnit = canvas ? canvas.referencePixelsPerUnit : 100f;
            _pixelsPerUnit = spritePixelsPerUnit / referencePixelsPerUnit;

            _minBevelRadians = _minBevelDegrees * Mathf.Deg2Rad;

            useLegacyMeshGeneration = false;

            _uv = new UV(_activeSprite);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            SetAllDirty();

            if (_points.Length == 0)
                _points = new Vector2[1];
        }

        public void SetPoints(Vector2[] points)
        {
            _points = points;

            SetAllDirty();

            Rebuild(CanvasUpdate.Layout);
        }

        protected override void OnPopulateMesh(VertexHelper vertexHelper)
        {
            if (_points == null || _points.Length == 0)
                return;

            vertexHelper.Clear();

            PopulateMesh(vertexHelper, _points);
        }


        private void PopulateMesh(VertexHelper vertexHelper, Vector2[] pointsToDraw)
        {
            if (_bezierMode != BezierType.None && _bezierMode != BezierType.Catenary && pointsToDraw.Length > 3)
                pointsToDraw = new BezierPath(pointsToDraw, _bezierSegmentsPerCurve, _bezierMode).GetPoints();
            else if (_bezierMode == BezierType.Catenary && pointsToDraw.Length == 2)
                pointsToDraw = new CableCurve(pointsToDraw).GetPoints();

            if (_improveResolution != ResolutionMode.None)
                pointsToDraw = IncreaseResolution(pointsToDraw);

            FillVertexes(vertexHelper, pointsToDraw);

            vertexHelper.CheckCurrentVertCount();
        }

        private void FillVertexes(VertexHelper vertexHelper, Vector2[] pointsToDraw)
        {
            var segments = GetSegments(pointsToDraw);

            for (int i = 0; i < segments.Count; i++)
            {
                if (!_lineList && i < segments.Count - 1)
                {
                    var join = CalculateMiterPoint(segments[i], segments[i + 1]);
                    vertexHelper.AddUIVertexQuad(join);
                }

                vertexHelper.AddUIVertexQuad(segments[i]);
            }
        }

        private List<UIVertex[]> GetSegments(Vector2[] pointsToDraw)
        {
            var segments = new List<UIVertex[]>();
            var sizeX = _relativeSize ? rectTransform.rect.width : 1f;
            var sizeY = _relativeSize ? rectTransform.rect.height : 1f;
            var offsetX = -rectTransform.pivot.x * sizeX;
            var offsetY = -rectTransform.pivot.y * sizeY;

            for (int i = 1; i < pointsToDraw.Length; i++)
            {
                var start = new Vector2(pointsToDraw[i - 1].x * sizeX + offsetX, pointsToDraw[i - 1].y * sizeY + offsetY);
                var end = new Vector2(pointsToDraw[i].x * sizeX + offsetX, pointsToDraw[i].y * sizeY + offsetY);

                if (_lineList && _lineCaps || !_lineList && _lineCaps && i == 1)
                    segments.Add(CreateLineCap(start, end, SegmentType.Start));

                segments.Add(CreateLineSegment(start, end, SegmentType.Middle));

                if (_lineList && _lineCaps || !_lineList && _lineCaps && i == pointsToDraw.Length - 1)
                    segments.Add(CreateLineCap(start, end, SegmentType.End));

                if (_lineList)
                    i++;
            }

            return segments;
        }

        private UIVertex[] CalculateMiterPoint(UIVertex[] firstVertices, UIVertex[] secondVertices)
        {
            SetVertexPosition(ref firstVertices, ref secondVertices);

            var join = new UIVertex[] { firstVertices[2], firstVertices[3], secondVertices[0], secondVertices[1] };

            return join;
        }


        private void SetVertexPosition(ref UIVertex[] firstVertices, ref UIVertex[] secondVertices)
        {
            var firstVector = firstVertices[1].position - firstVertices[2].position;
            var secondVector = secondVertices[2].position - secondVertices[1].position;

            var angle = Vector2.Angle(firstVector, secondVector) * Mathf.Deg2Rad;
            var halfAngle = angle / 2f;
            var doubleTangent = Mathf.Tan(halfAngle) * 2f;

            var miterDistance = _lineThickness / doubleTangent;
            var doubleMiterDistance = miterDistance * 2f;

            if (doubleMiterDistance >= firstVector.magnitude || doubleMiterDistance >= secondVector.magnitude || angle <= _minBevelRadians)
                return;

            var sign = Mathf.Sign(Vector3.Cross(firstVector.normalized, secondVector.normalized).z);
            var normalizedMiterDistance = sign * miterDistance * firstVector.normalized;

            var miterPointA = firstVertices[2].position - normalizedMiterDistance;
            var miterPointB = firstVertices[3].position + normalizedMiterDistance;

            if (sign < 0)
            {
                firstVertices[2].position = miterPointA;
                secondVertices[1].position = miterPointA;
            }
            else
            {
                firstVertices[3].position = miterPointB;
                secondVertices[0].position = miterPointB;
            }
        }

        private Vector2[] IncreaseResolution(Vector2[] input)
        {
            var _inputList = new List<Vector2>(input);
            var _outputList = new List<Vector2>();

            if (_improveResolution == ResolutionMode.PerLine)
                _outputList = IncreaseResolutionPerLine(_inputList);
            else if (_improveResolution == ResolutionMode.PerSegment)
                _outputList = IncreaseResolutionPerSegment(_inputList);

            return _outputList.ToArray();
        }

        private List<Vector2> IncreaseResolutionPerLine(List<Vector2> input)
        {
            var totalDistance = 0f;

            for (int i = 0; i < input.Count - 1; i++)
                totalDistance += Vector2.Distance(input[i], input[i + 1]);

            ResolutionToNativeSize(totalDistance);

            var increments = totalDistance / _resolution;

            var outputList = new List<Vector2>();

            for (int i = 0; i < input.Count - 1; i++)
            {
                var firstPoint = input[i];

                outputList.Add(firstPoint);

                var secondPoint = input[i + 1];
                var segmentDistance = Vector2.Distance(firstPoint, secondPoint) / increments;

                for (int j = 0; j < segmentDistance; j++)
                    outputList.Add(Vector2.Lerp(firstPoint, secondPoint, j / segmentDistance));

                outputList.Add(secondPoint);
            }

            return outputList;
        }

        private List<Vector2> IncreaseResolutionPerSegment(List<Vector2> input)
        {
            var outputList = new List<Vector2>();

            for (int i = 0; i < input.Count - 1; i++)
            {
                var firstPoint = input[i];

                outputList.Add(firstPoint);

                var secondPoint = input[i + 1];

                ResolutionToNativeSize(Vector2.Distance(firstPoint, secondPoint));

                for (int j = 1; j < _resolution; j++)
                    outputList.Add(Vector2.Lerp(firstPoint, secondPoint, j / _resolution));

                outputList.Add(secondPoint);
            }

            return outputList;
        }

        private void ResolutionToNativeSize(float distance)
        {
            if (!_useNativeSize)
                return;

            _resolution = distance / (_activeSprite.rect.width / _pixelsPerUnit);
            _lineThickness = _activeSprite.rect.height / _pixelsPerUnit;
        }

        private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, SegmentType type)
        {
            return type == SegmentType.Start
                ? CreateStartSegment(start, end)
                : type == SegmentType.End
                    ? CreateEndSegment(start, end)
                    : null;
        }

        private UIVertex[] CreateStartSegment(Vector2 start, Vector2 end)
        {
            var calculatedStart = start - (end - start).normalized * _lineThickness / 2;

            return CreateLineSegment(calculatedStart, start, SegmentType.Start);
        }

        private UIVertex[] CreateEndSegment(Vector2 start, Vector2 end)
        {
            var calculatedEnd = end + (end - start).normalized * _lineThickness / 2;

            return CreateLineSegment(end, calculatedEnd, SegmentType.End);
        }

        private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, SegmentType type)
        {
            Vector2 offset = new Vector2(start.y - end.y, end.x - start.x).normalized * _lineThickness / 2;

            var vertex1 = start - offset;
            var vertex2 = start + offset;
            var vertex3 = end + offset;
            var vertex4 = end - offset;

            var vertices = new[] { vertex1, vertex2, vertex3, vertex4 };
            var uvs = _uv.GetUvs(type);

            return SetVbo(vertices, uvs);
        }

        private UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] vbo = new UIVertex[4];

            for (int i = 0; i < vertices.Length; i++)
            {
                var vert = UIVertex.simpleVert;
                vert.color = color;
                vert.position = vertices[i];
                vert.uv0 = uvs[i];
                vbo[i] = vert;
            }

            return vbo;
        }

        #region ILayoutElement Interface
        public virtual void CalculateLayoutInputHorizontal() { }

        public virtual void CalculateLayoutInputVertical() { }

        public virtual float minWidth => 0;

        public virtual float preferredWidth => _activeSprite == null ? 0 : _activeSprite.rect.size.x / _pixelsPerUnit;

        public virtual float flexibleWidth => -1;

        public virtual float minHeight => 0;

        public virtual float preferredHeight => _activeSprite == null ? 0 : _activeSprite.rect.size.y / _pixelsPerUnit;

        public virtual float flexibleHeight => -1;

        public virtual int layoutPriority => 0;
        #endregion

        #region ICanvasRaycastFilter Interface
        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (_eventAlphaThreshold >= 1)
                return true;

            if (_activeSprite == null)
                return true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out Vector2 local);

            var rect = GetPixelAdjustedRect();

            local.x += rectTransform.pivot.x * rect.width;
            local.y += rectTransform.pivot.y * rect.height;

            local = MapCoordinate(local, rect);

            var spriteRect = _activeSprite.textureRect;
            var normalized = new Vector2(local.x / spriteRect.width, local.y / spriteRect.height);

            var x = Mathf.Lerp(spriteRect.x, spriteRect.xMax, normalized.x) / _activeSprite.texture.width;
            var y = Mathf.Lerp(spriteRect.y, spriteRect.yMax, normalized.y) / _activeSprite.texture.height;

            return _activeSprite.texture.GetPixelBilinear(x, y).a >= _eventAlphaThreshold;
        }

        private Vector2 MapCoordinate(Vector2 local, Rect rect)
        {
            return new Vector2(local.x * rect.width, local.y * rect.height);
        }
        #endregion
    }
}

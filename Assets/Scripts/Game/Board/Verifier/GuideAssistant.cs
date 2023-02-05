using Simple.Nonogram.UI;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class GuideAssistant : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private UiLineRenderer _topGuide;
        [SerializeField] private UiLineRenderer _bottomGuide;
        [SerializeField] private UiLineRenderer _leftGuide;
        [SerializeField] private UiLineRenderer _rightGuide;

        private RectTransform _boardRect;
        private RectTransform _leftNumbersRect;
        private RectTransform _topNumbersRect;
        private Vector2 _cellSize;

        public void SetData(RectTransform boardRect, RectTransform leftNumbersRect, RectTransform topNumbersRect, Vector2 cellSize)
        {
            _boardRect = boardRect;
            _leftNumbersRect = leftNumbersRect;
            _topNumbersRect = topNumbersRect;
            _cellSize = cellSize;

            Resize();

            DrawGuides(Vector2.zero);
            SetActiveGuides(false);
        }

        public void DrawGuides(Vector2 point)
        {
            SetActiveGuides(true);

            DrawHorizontalGuides(point);
            DrawVerticalGuides(point);
        }

        private void DrawHorizontalGuides(Vector2 point)
        {
            var xMin = _rect.rect.width / 2 - _boardRect.rect.width;
            var xMax = _rect.rect.width / 2;
            var yMin = point.y + _boardRect.rect.height - _rect.rect.height / 2;
            var yMax = yMin + _cellSize.y;

            var startBottomPosition = new Vector2(xMin, yMin);
            var endBottomPosition = new Vector2(xMax, yMin);
            var startTopPosition = new Vector2(xMin, yMax);
            var endTopPosition = new Vector2(xMax, yMax);

            _topGuide.SetPoints(new Vector2[] { startTopPosition, endTopPosition });

            _bottomGuide.SetPoints(new Vector2[] { startBottomPosition, endBottomPosition });
        }

        private void DrawVerticalGuides(Vector2 point)
        {
            var xMin = point.x - _boardRect.rect.width + _rect.rect.width / 2;
            var xMax = xMin + _cellSize.x;
            var yMin = -_rect.rect.height / 2;
            var yMax = _boardRect.rect.height + yMin;

            var startLeftPosition = new Vector2(xMin, yMin);
            var endLeftPosition = new Vector2(xMin, yMax);
            var startRightPosition = new Vector2(xMax, yMin);
            var endRightPosition = new Vector2(xMax, yMax);

            _leftGuide.SetPoints(new Vector2[] { startLeftPosition, endLeftPosition });

            _rightGuide.SetPoints(new Vector2[] { startRightPosition, endRightPosition });
        }

        private void Resize()
        {
            CellUtils.SetSizeWithCurrentAnchors(_rect,
                _leftNumbersRect.rect.width + _topNumbersRect.rect.width,
                _leftNumbersRect.rect.height + _topNumbersRect.rect.height);
        }

        private void SetActiveGuides(bool active)
        {
            _topGuide.gameObject.SetActive(active);
            _bottomGuide.gameObject.SetActive(active);
            _leftGuide.gameObject.SetActive(active);
            _rightGuide.gameObject.SetActive(active);
        }
    }
}

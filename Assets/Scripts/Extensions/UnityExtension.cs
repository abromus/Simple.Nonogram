using UnityEngine;

namespace Simple.Nonogram.Extension
{
    public static class UnityExtension
    {
        public static RectTransform SetSizeWithCurrentAnchors(this RectTransform rectTransform, float width, float height)
        {
            var sizeDelta = rectTransform.sizeDelta;
            var parentSize = GetParentSize(rectTransform);

            sizeDelta.x = Mathf.Abs(width - parentSize.x * (rectTransform.anchorMax.x - rectTransform.anchorMin.x));
            sizeDelta.y = Mathf.Abs(height - parentSize.y * (rectTransform.anchorMax.y - rectTransform.anchorMin.y));
            rectTransform.sizeDelta = sizeDelta;

            return rectTransform;
        }

        private static Vector2 GetParentSize(RectTransform rectTransform)
        {
            var rectTransformParent = rectTransform.parent.GetComponent<RectTransform>();

            return rectTransformParent != null ? rectTransformParent.rect.size : Vector2.zero;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Extensions
{
    public static class UnityExtension
    {
        public static RectTransform UpdateRectTransform(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.sizeDelta = new Vector2(sizeDelta.y, sizeDelta.x);

            return rectTransform;
        }

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
            var rectTransformParent = (RectTransform)rectTransform.parent;

            return rectTransformParent ? rectTransformParent.rect.size : Vector2.zero;
        }

        public static LayoutElement UpdateSizes(this LayoutElement layoutElement, float preferredWidth, float preferredHeight, float flexibleWidth, float flexibleHeight)
        {
            layoutElement.preferredWidth = preferredWidth;
            layoutElement.preferredHeight = preferredHeight;
            layoutElement.flexibleWidth = flexibleWidth;
            layoutElement.flexibleHeight = flexibleHeight;

            return layoutElement;
        }

        public static LayoutElement UpdateSize(this LayoutElement layoutElement, float minWidth, float minHeight)
        {
            layoutElement.minWidth = minWidth;
            layoutElement.minHeight = minHeight;

            return layoutElement;
        }

        public static void MarkDontDestroyOnLoad(this Object dontDestroyOnLoadObject)
        {
            DontDestroyOnLoad(dontDestroyOnLoadObject);
        }

        public static void DontDestroyOnLoad(Object dontDestroyOnLoadObject)
        {
//#if SNGC_USE_DONT_DESTROY_ON_LOAD
            switch (dontDestroyOnLoadObject)
            {
                case Component component:
                    component.transform.SetParent(null);
                    break;
                case GameObject gameObject:
                    gameObject.transform.SetParent(null);
                    break;
            }

            Object.DontDestroyOnLoad(dontDestroyOnLoadObject);
//#endif
        }
    }
}

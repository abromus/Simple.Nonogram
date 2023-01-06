using System.Collections.Generic;
using System.Linq;
using Simple.Nonogram.Core;
using UnityEngine;

namespace Simple.Nonogram.Components
{
    public class NonogramView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private RectTransform _contentRectTransform;

        [Header("Settings")]
        [SerializeField] private List<NonogramElement> _elements;
        [SerializeField] [Min(0f)] private float _offset;

        public NonogramElement Add(NonogramElement element)
        {
            var createdElement = Instantiate(element, _contentRectTransform);

            createdElement.transform.localPosition = CalculateLocalPosition(createdElement);
            _elements.Add(createdElement);

            UpdateSize(createdElement);

            return createdElement;
        }

        private Vector3 CalculateLocalPosition(NonogramElement element)
        {
            var tempElement = element;
            var tempElementPosition = _elements?.Count > 0 ? _elements.Last().transform.localPosition : element.transform.localPosition;

            tempElementPosition.y -= _elements?.Count == 0 ? _offset : tempElement.Height + _offset;

            return tempElementPosition;
        }

        private void UpdateSize(NonogramElement element)
        {
            var contentHeight = _elements.Count == 1
                ? _contentRectTransform.rect.height + _offset
                : _contentRectTransform.rect.height + element.Height + _offset;

            _contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
        }
    }
}

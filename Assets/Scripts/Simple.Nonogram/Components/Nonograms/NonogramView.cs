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
        [SerializeField] [Min((float)Number.Zero)] private float _offset;

        public NonogramElement Add(NonogramElement element)
        {
            NonogramElement createdElement = Instantiate(element, _contentRectTransform);

            createdElement.transform.localPosition = CalculateLocalPosition(createdElement);
            _elements.Add(createdElement);

            UpdateSize(createdElement);

            return createdElement;
        }

        private Vector3 CalculateLocalPosition(NonogramElement element)
        {
            NonogramElement tempElement = element;
            Vector3 tempElementPosition = _elements?.Count > (int)Number.Zero ? _elements.Last().transform.localPosition : element.transform.localPosition;

            tempElementPosition.y -= _elements?.Count == (int)Number.Zero ? _offset : tempElement.Height + _offset;

            return tempElementPosition;
        }

        private void UpdateSize(NonogramElement element)
        {
            float contentHeight = _elements.Count == (int)Number.One
                ? _contentRectTransform.rect.height + _offset
                : _contentRectTransform.rect.height + element.Height + _offset;

            _contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
        }
    }
}

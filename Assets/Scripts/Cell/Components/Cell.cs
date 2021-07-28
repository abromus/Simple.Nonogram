using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private Color _hoverColor = new Color(0.52f, 0.45f, 0.45f);

        public event Action<Vector3> Clicked;
        public event Action<Vector3> Emptied;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _spriteRenderer.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _spriteRenderer.color = _defaultColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Clicked?.Invoke(transform.position);
            else if (eventData.button == PointerEventData.InputButton.Right)
                Emptied?.Invoke(transform.position);
        }
    }
}

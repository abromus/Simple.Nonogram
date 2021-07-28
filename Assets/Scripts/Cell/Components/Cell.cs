using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private Color _hoverColor = new Color(0.52f, 0.45f, 0.45f);

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
    }
}

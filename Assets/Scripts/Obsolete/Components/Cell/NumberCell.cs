using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public class NumberCell : MonoBehaviour, IClickableCell
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _text;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public TMP_Text Text => _text;

        public event Action<Vector3, PointerEventData.InputButton> PointerDown;
        public event Action<Vector3, PointerEventData.InputButton> PointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(transform.position, eventData.button);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(transform.position, eventData.button);
        }
    }
}

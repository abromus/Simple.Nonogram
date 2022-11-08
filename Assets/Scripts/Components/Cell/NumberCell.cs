using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public class NumberCell : MonoBehaviour, IClickableCell
    {
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

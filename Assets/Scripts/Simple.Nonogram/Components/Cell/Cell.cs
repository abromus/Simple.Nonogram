using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public class Cell : MonoBehaviour, ICell
    {
        public event Action<Vector3, PointerEventData.InputButton> Clicked;
        public event Action<Vector3, PointerEventData.InputButton> Emptied;
        public event Action<Vector3, PointerEventData.InputButton> HoveredBegin;
        public event Action<Vector3, PointerEventData.InputButton> HoveredEnd;
        public event Action<Vector3, PointerEventData.InputButton> PointerDown;
        public event Action<Vector3, PointerEventData.InputButton> PointerUp;

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredBegin?.Invoke(transform.position, eventData.button);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredEnd?.Invoke(transform.position, eventData.button);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Clicked?.Invoke(transform.position, eventData.button);
            else if (eventData.button == PointerEventData.InputButton.Right)
                Emptied?.Invoke(transform.position, eventData.button);
        }

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

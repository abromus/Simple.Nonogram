using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Cell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action<Vector3> Clicked;
        public event Action<Vector3> Emptied;
        public event Action<Vector3> HoveredBegin;
        public event Action<Vector3> HoveredEnd;

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoveredBegin?.Invoke(transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoveredEnd?.Invoke(transform.position);
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

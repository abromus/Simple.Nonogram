using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    interface ICell : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector3, PointerEventData.InputButton> Clicked;
        public event Action<Vector3, PointerEventData.InputButton> Emptied;
        public event Action<Vector3, PointerEventData.InputButton> HoveredBegin;
        public event Action<Vector3, PointerEventData.InputButton> HoveredEnd;
        public event Action<Vector3, PointerEventData.InputButton> PointerDown;
        public event Action<Vector3, PointerEventData.InputButton> PointerUp;
    }
}

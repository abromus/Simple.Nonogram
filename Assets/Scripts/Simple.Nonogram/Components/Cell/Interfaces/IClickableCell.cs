using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public interface IClickableCell : ICell, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<Vector3, PointerEventData.InputButton> PointerDown;
        public event Action<Vector3, PointerEventData.InputButton> PointerUp;
    }
}

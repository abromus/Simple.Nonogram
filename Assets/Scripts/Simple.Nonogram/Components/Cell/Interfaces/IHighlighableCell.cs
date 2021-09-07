using System;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public interface IHighlighableCell : ICell, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Vector3, PointerEventData.InputButton> HoveredBegin;
        public event Action<Vector3, PointerEventData.InputButton> HoveredEnd;
    }
}

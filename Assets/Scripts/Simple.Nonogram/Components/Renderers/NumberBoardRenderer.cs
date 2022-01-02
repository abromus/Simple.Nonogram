using Simple.Nonogram.Extension;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    public class NumberBoardRenderer
    {
        private readonly NumberCell[,] _cells;
        private readonly Sprite _blank;
        private readonly Sprite _empty;

        public NumberBoardRenderer(NumberCell[,] cells, Sprite blank, Sprite empty)
        {
            _cells = cells;
            _blank = blank;
            _empty = empty;
        }

        public void Mark(Vector3 position, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left || button == PointerEventData.InputButton.Right)
                if (ArrayExtension.TryFindCell(_cells, position, out Vector2Int coordinate))
                    SetSprite(coordinate, _empty);
        }

        public void OnClicked(Vector3 position, PointerEventData.InputButton button)
        {
            Mark(position, button);
        }

        private void SetSprite(Vector2Int coordinate, Sprite filledState)
        {
            SpriteRenderer spriteRenderer = _cells[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = spriteRenderer.sprite != filledState ? filledState : _blank;
        }
    }
}

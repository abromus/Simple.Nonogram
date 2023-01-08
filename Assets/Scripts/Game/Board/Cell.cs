using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class Cell : MonoBehaviour, ICell
    {
        [SerializeField] private RectTransform _rectTransform;

        public Vector2 Size => _rectTransform.rect.size;
    }
}

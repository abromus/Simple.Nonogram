using TMPro;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class NumberCell : MonoBehaviour, ICell
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TMP_Text _text;

        public Vector2 Size => _rectTransform.rect.size;

        public void SetData(string value)
        {
            _text.text = value;
        }
    }
}

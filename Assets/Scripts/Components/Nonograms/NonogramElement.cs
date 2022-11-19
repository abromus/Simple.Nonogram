using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Components
{
    public class NonogramElement : MonoBehaviour
    {
        [SerializeField] private RectTransform _transform;
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _size;
        [Space]
        [SerializeField] private Image _image;
        [Space]
        [SerializeField] private Button _button;

        public float Height => _transform.rect.height;
        public Button Button => _button;

        public void SetTitle(string title)
        {
            _title.text = title;
        }

        public void SetSize(Vector2Int size)
        {
            _size.text = $"{size.x}x{size.y}";
        }

        public void SetImage(Sprite image)
        {
            _image.sprite = image;
        }
    }
}

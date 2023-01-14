using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class Cell : MonoBehaviour, ICell
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private List<CellInfo> _images;

        private CellType _cellType;

        public Vector2 Size => _rectTransform.rect.size;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonCLick);
        }

        public void ChangeCellType(CellType cellType)
        {
            _cellType = cellType;
        }

        private void OnButtonCLick()
        {
            ChangeSprite();
        }

        private void ChangeSprite()
        {
            var sprite = GetSprite(_cellType);

            _image.sprite = _image.sprite != sprite ? sprite : null;
        }

        private Sprite GetSprite(CellType cellType)
        {
            var cellInfo = _images.Find(info => info.Type == cellType);

            return cellInfo?.Sprite;
        }
    }
}

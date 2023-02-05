using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class Cell : MonoBehaviour, ICell
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _button;
        [SerializeField] private Image _cellImage;
        [SerializeField] private Image _image;
        [SerializeField] private List<CellInfo> _images;

        private CellType _cellType;
        private CellType _cellClickedType;

        private readonly Subject<Cell> _onClick = new Subject<Cell>();
        private readonly Subject<Vector2> _onPointerEnter = new Subject<Vector2>();

        public IObservable<Cell> OnClick => _onClick;
        public IObservable<Vector2> OnPointerEnter => _onPointerEnter;

        public RectTransform RectTransform => _rectTransform;
        public Vector2 Size => _rectTransform.rect.size;
        public CellType CellType => _cellType;
        public Image Image => _cellImage;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClick);
            _button.OnPointerEnterAsObservable().Subscribe(_ => OnButtonPointerEnter()).AddTo(this);
        }

        public void ChangeCellType(CellType cellType)
        {
            _cellClickedType = cellType;
        }

        public string GetState()
        {
            var markedSprite = GetSprite(CellType.Marked);
            return _image.sprite == markedSprite ? "1" : "0";
        }

        private void OnButtonClick()
        {
            ChangeSprite();

            _onClick.OnNext(this);
        }

        private void OnButtonPointerEnter()
        {
            _onPointerEnter.OnNext(_rectTransform.anchoredPosition);
        }

        private void ChangeSprite()
        {
            var sprite = GetSprite(_cellClickedType);

            _image.sprite = _image.sprite != sprite ? sprite : null;
            _cellType = _cellType == _cellClickedType ? CellType.None : _cellClickedType;
        }

        private Sprite GetSprite(CellType cellType)
        {
            var cellInfo = _images.Find(info => info.Type == cellType);

            return cellInfo?.Sprite;
        }
    }
}

using Cysharp.Threading.Tasks;
using Simple.Nonogram.Core.Services;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;

        [Space]
        [SerializeField] private CellBoard _cellBoard;
        [SerializeField] private Verifier _verifier;
        [SerializeField] private NumberBoard _numberBoard;
        [SerializeField] private GuideAssistant _guideAssistant;
        [SerializeField] private PictureBoard _pictureBoard;

        private ICompositionRoot _root;
        private IWorld _world;
        private NonogramController _nonogramController;

        public void SetData(NonogramController nonogramController)
        {
            _nonogramController = nonogramController;

            _cellBoard.SetData(_nonogramController.CurrentNonogram);
            //_verifier.SetData();
            _numberBoard.SetData(_nonogramController.CurrentNonogram);
            _guideAssistant.SetData(_cellBoard.Rect, _numberBoard.LeftNumberCells, _numberBoard.TopNumberCells, _cellBoard.CellSize);
            _pictureBoard.SetData(_numberBoard.LeftNumberCells.rect.width, _numberBoard.TopNumberCells.rect.height);
            
            ResizeBoard();

            _cellBoard.OnClick.Subscribe(_guideAssistant.DrawGuides).AddTo(this);
            _cellBoard.OnPointerEnter.Subscribe(_guideAssistant.DrawGuides).AddTo(this);
        }

        public void ChangeCellType(CellType cellType)
        {
            _cellBoard.ChangeCellType(cellType);
        }

        private void ResizeBoard()
        {
            var width = _numberBoard.NumberCells.rect.width + xOffset * 2;
            var height = _numberBoard.NumberCells.rect.height + yOffset * 2;

            ResizeBoard(_rectTransform, width, height);
            ResizeBoard(_parentRectTransform, width, height);
        }

        private void ResizeBoard(RectTransform _rectTransform, float width, float height)
        {
            CellUtils.SetSizeWithCurrentAnchors(_rectTransform, width, height);
        }
    }
}

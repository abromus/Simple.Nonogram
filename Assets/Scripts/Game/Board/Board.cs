using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;

        [Space]
        [SerializeField] private CellBoard _cellBoard;
        [SerializeField] private NumberBoard _numberBoard;
        [SerializeField] private PictureBoard _pictureBoard;

        private ICompositionRoot _root;
        private IWorld _world;
        private NonogramController _nonogramController;

        private void Awake()
        {
            _root = DI.GetCompositionRoot(CompositionTag.Game);
            _world = _root.Get<IWorld>();
            _nonogramController = _world.NonogramController;
        }

        private void Start()
        {
            SetData();

            ResizeBoard();
        }

        private void SetData()
        {
            _cellBoard.SetData(_nonogramController.CurrentNonogram);
            _numberBoard.SetData(_nonogramController.CurrentNonogram);
            _pictureBoard.SetData(_numberBoard.LeftNumberCells.rect.width, _numberBoard.TopNumberCells.rect.height);
        }

        private void ResizeBoard()
        {
            CellUtils.SetSizeWithCurrentAnchors(
                _rectTransform,
                _numberBoard.NumberCells.rect.width + xOffset * 2,
                _numberBoard.NumberCells.rect.height + yOffset * 2);
        }
    }
}

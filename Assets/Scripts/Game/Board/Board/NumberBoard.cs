using Simple.Nonogram.Nonograms;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class NumberBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform _numberCellsContainer;
        [SerializeField] private NumberCellsGroup _topGroup;
        [SerializeField] private NumberCellsGroup _leftGroup;

        public RectTransform NumberCells => _numberCellsContainer;
        public RectTransform TopNumberCells => _topGroup.CellsContainer;
        public RectTransform LeftNumberCells => _leftGroup.CellsContainer;

        public void SetData(NonogramInfo nonogram)
        {
            _topGroup.SetData(nonogram, true);
            _leftGroup.SetData(nonogram, false);

            ResizeNumberCells();
        }

        private void ResizeNumberCells()
        {
            CellUtils.SetSizeWithCurrentAnchors(
                _numberCellsContainer,
                _leftGroup.CellsContainer.rect.width + _topGroup.CellsContainer.rect.width,
                _leftGroup.CellsContainer.rect.height + _topGroup.CellsContainer.rect.height);
        }
    }
}

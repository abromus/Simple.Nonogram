using System.Collections.Generic;
using Simple.Nonogram.Nonograms;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class CellBoard : MonoBehaviour
    {
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private RectTransform _cellsContainer;
        [SerializeField] private GridLayoutGroup _cellsGroup;

        private NonogramInfo _nonogram;
        private List<Cell> _cells;

        public void SetData(NonogramInfo nonogram)
        {
            _nonogram = nonogram;

            Initialize();
        }

        public void ChangeCellType(CellType cellType)
        {
            foreach (var cell in _cells)
                cell.ChangeCellType(cellType);
        }

        private void Initialize()
        {
            InstantiateCells(_nonogram.Size.x, _nonogram.Size.y);
            SetCellSize();
            SetSizeWithCurrentAnchors(_nonogram.Size.x, _nonogram.Size.y);
        }

        private void InstantiateCells(int width, int height)
        {
            _cells = CellUtils.InstantiateCells(_cellPrefab, _cellsContainer, width, height);
        }

        private void SetCellSize()
        {
            CellUtils.SetCellSize(_cellsGroup, _cellPrefab.Size.x, _cellPrefab.Size.y);
        }

        private void SetSizeWithCurrentAnchors(int width, int height)
        {
            CellUtils.SetSizeWithCurrentAnchors(
                _cellsContainer,
                _cellPrefab.Size.x * width,
                _cellPrefab.Size.y * height);
        }
    }
}

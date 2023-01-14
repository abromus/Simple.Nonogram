using System.Collections.Generic;
using System.Text;
using Simple.Nonogram.Core.Services;
using Simple.Nonogram.Nonograms;
using Simple.Nonogram.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class GenerationBoard : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _parentRectTransform;
        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;

        [Space]
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private RectTransform _cellsContainer;
        [SerializeField] private GridLayoutGroup _cellsGroup;

        private ICompositionRoot _root;
        private GenerationNonogramInfo _nonogramInfo;
        private NonogramSettings _settings;
        private List<Cell> _cells;

        public void SetData(ICompositionRoot root, GenerationNonogramInfo info)
        {
            _root = root;
            _nonogramInfo = info;
            _settings = _root.Get<NonogramSettings>();

            Initialize();

            ResizeBoard();
        }

        public void ChangeCellType(CellType cellType)
        {
            foreach (var cell in _cells)
                cell.ChangeCellType(cellType);
        }

        public void GenerateNonogram(string name)
        {
            var path = $"{_settings.PathToGenerationFolder}/{name}";
            var file = CreateCellsInfo();

            var nonogramMeta = new NonogramMeta();
            nonogramMeta.Safe(path, file);
        }

        private void Initialize()
        {
            InstantiateCells(_nonogramInfo.Width, _nonogramInfo.Height);
            SetCellSize();
            SetSizeWithCurrentAnchors(_nonogramInfo.Width, _nonogramInfo.Height);
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

        private void ResizeBoard()
        {
            var width = _cellsContainer.rect.width + xOffset * 2;
            var height = _cellsContainer.rect.height + yOffset * 2;

            ResizeBoard(_rectTransform, width, height);
            ResizeBoard(_parentRectTransform, width, height);
        }

        private void ResizeBoard(RectTransform _rectTransform, float width, float height)
        {
            CellUtils.SetSizeWithCurrentAnchors(_rectTransform, width, height);
        }

        private List<string> CreateCellsInfo()
        {
            var cells = new List<string>();
            var row = new StringBuilder();

            for (int j = 0; j < _nonogramInfo.Height; j++)
            {
                row.Clear();

                for (int i = 0; i < _nonogramInfo.Width; i++)
                    row.Append(_cells[j * _nonogramInfo.Width + i].GetState());

                cells.Add(row.ToString());
            }

            return cells;
        }
    }
}

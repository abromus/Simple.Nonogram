using System.Collections.Generic;
using System.Linq;
using Simple.Nonogram.Core;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Nonograms;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class NumberCellsGroup : MonoBehaviour
    {
        [SerializeField] private NumberCell _cellPrefab;
        [SerializeField] private RectTransform _cellsContainer;
        [SerializeField] private GridLayoutGroup _cellsGroup;

        private List<NumberCell> _cells;

        public RectTransform CellsContainer => _cellsContainer;
        public GridLayoutGroup LayoutGroup => _cellsGroup;

        private NonogramInfo _nonogram;
        private bool _isTop;
        private int _width;
        private int _height;

        public void SetData(NonogramInfo nonogram, bool isTop)
        {
            _nonogram = nonogram;
            _isTop = isTop;

            Initialize();
        }

        private void Initialize()
        {
            var cells = CalculateCells(_isTop);

            _width = _isTop ? _nonogram.Size.x : cells.GetLength((int)Dimension.Height);
            _height = _isTop ? cells.GetLength((int)Dimension.Height) : _nonogram.Size.y;

            SetCellSize();
            SetSizeWithCurrentAnchors(_width, _height);
            InstantiateCells(_width, _height);
            FillCells(_cells, cells);
        }

        private int[,] CalculateCells(bool isTop)
        {
            var nonogram = isTop ? _nonogram.Nonogram : _nonogram.Nonogram.Transpose();
            var width = isTop ? _nonogram.Size.x : _nonogram.Size.y;
            var height = isTop ? _nonogram.Size.y : _nonogram.Size.x;
            var marked = '1';
            var countMarked = 0;
            var cells = new List<List<int>>();

            for (int i = 0; i < width; i++)
            {
                var numbers = new List<int>();
                for (int j = 0; j < height; j++)
                {
                    if (nonogram.ElementAt(j).ElementAt(i) == marked)
                    {
                        countMarked++;

                        if (j == height - 1)
                            AddNumbers(numbers, ref countMarked);
                    }
                    else if (nonogram.ElementAt(j).ElementAt(i) != marked && countMarked > 0)
                    {
                        AddNumbers(numbers, ref countMarked);
                    }
                }

                cells.Add(numbers);
            }

            return cells.ToArray<int>();
        }

        private void AddNumbers(List<int> numbers, ref int countMarked)
        {
            numbers.Add(countMarked);
            countMarked = 0;
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

        private void FillCells(List<NumberCell> numberCells, int[,] cells)
        {
            var width = cells.GetLength((int)Dimension.Width);
            var height = cells.GetLength((int)Dimension.Height);
            var blank = 0;

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    var value = cells[i, j] == blank ? string.Empty : cells[i, j].ToString();
                    numberCells[width * j + i].SetData(value);
                }
        }
    }
}

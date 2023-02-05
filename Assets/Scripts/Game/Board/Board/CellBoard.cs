using System;
using System.Collections.Generic;
using Simple.Nonogram.Nonograms;
using UniRx;
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

        private readonly Subject<Cell> _onClick = new Subject<Cell>();
        private readonly Subject<Vector2> _onPointerEnter = new Subject<Vector2>();

        public IObservable<Cell> OnClick => _onClick;
        public IObservable<Vector2> OnPointerEnter => _onPointerEnter;

        public Vector2 CellSize => _cellPrefab.Size;
        public RectTransform Rect => _cellsContainer;

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

            foreach (var cell in _cells)
            {
                cell.OnClick.Subscribe(_onClick.OnNext);
                cell.OnPointerEnter.Subscribe(_onPointerEnter.OnNext);
            }
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

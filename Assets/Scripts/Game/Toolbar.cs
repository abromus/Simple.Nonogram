using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private Button _markedButton;
        [SerializeField] private Button _emptyButton;

        private CellType _cellType = CellType.Marked;
        private Subject<CellType> _cellTypeChanged = new Subject<CellType>();

        public IObservable<CellType> CellTypeChanged => _cellTypeChanged;

        private void Awake()
        {
            _markedButton.OnClickAsObservable().Subscribe(OnMarkedButtonClick).AddTo(this);
            _emptyButton.OnClickAsObservable().Subscribe(OnEmptyButtonClick).AddTo(this);
        }

        private void ChangeCellType(CellType type)
        {
            _cellType = type;
            _cellTypeChanged.OnNext(_cellType);
        }

        private void OnMarkedButtonClick(Unit _)
        {
            ChangeCellType(CellType.Marked);
        }

        private void OnEmptyButtonClick(Unit _)
        {
            ChangeCellType(CellType.Empty);
        }
    }
}

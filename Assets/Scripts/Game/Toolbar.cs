using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class Toolbar : MonoBehaviour
    {
        [SerializeField] private Button _markedButton;
        [SerializeField] private Button _emptyButton;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _generationButton;

        private CellType _cellType = CellType.Marked;
        private Subject<CellType> _cellTypeChanged = new Subject<CellType>();
        private Subject<string> _generated = new Subject<string>();

        public IObservable<CellType> CellTypeChanged => _cellTypeChanged;
        public IObservable<string> Generated => _generated;

        private void Awake()
        {
            _markedButton.OnClickAsObservable().Subscribe(_ => OnMarkedButtonClick()).AddTo(this);
            _emptyButton.OnClickAsObservable().Subscribe(_ => OnEmptyButtonClick()).AddTo(this);
            _generationButton?.OnClickAsObservable().Subscribe(_ => OnGenerationButtonClick()).AddTo(this);
        }

        private void ChangeCellType(CellType type)
        {
            _cellType = type;
            _cellTypeChanged.OnNext(_cellType);
        }

        private void OnMarkedButtonClick()
        {
            ChangeCellType(CellType.Marked);
        }

        private void OnEmptyButtonClick()
        {
            ChangeCellType(CellType.Empty);
        }

        private void OnGenerationButtonClick()
        {
            var name = _inputField.text;
            _generated.OnNext(name);
        }
    }
}

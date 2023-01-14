using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class GenerationNonogramSettings : MonoBehaviour
    {
        [SerializeField] private CustomSlider _widthSlider;
        [SerializeField] private CustomSlider _heightSlider;
        [SerializeField] private Button _generationButton;

        private CompositeDisposable _subscription;
        private Subject<GenerationNonogramInfo> _created = new Subject<GenerationNonogramInfo>();

        public IObservable<GenerationNonogramInfo> Created => _created;

        private void Awake()
        {
            _subscription = new CompositeDisposable();

            _generationButton.OnClickAsObservable().Subscribe(_ => GenerateBoard()).AddTo(_subscription);
        }

        private void GenerateBoard()
        {
            var info = new GenerationNonogramInfo((int)_widthSlider.Value, (int)_heightSlider.Value);

            _created.OnNext(info);
        }
    }
}

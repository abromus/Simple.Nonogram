using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class CustomSlider : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _value;

        private CompositeDisposable _subscription;

        public float Value => _slider.value;

        private void Awake()
        {
            _subscription = new CompositeDisposable();

            _slider.OnValueChangedAsObservable().Subscribe(value => OnValueChanged(value)).AddTo(_subscription);
        }

        private void OnValueChanged(float value)
        {
            _value.text = value.ToString();
        }
    }
}

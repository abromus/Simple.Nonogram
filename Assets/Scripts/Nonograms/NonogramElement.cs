using Simple.Nonogram.Infrastructure.Delegates;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Nonograms
{
    public class NonogramElement : MonoBehaviour
    {
        [SerializeField] private RectTransform _transform;
        [Space]
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _size;
        [Space]
        [SerializeField] private Button _button;

        private Block _block;

        public Button Button => _button;

        private void Awake()
        {
            Button.OnClickAsObservable()
                .Subscribe(_ => _block.SafeInvoke())
                .AddTo(this);
        }

        public void Inject(string title, Vector2Int size, Block block)
        {
            _title.text = title;
            _size.text = $"{size.x}x{size.y}";
            _block = block;
        }
    }
}

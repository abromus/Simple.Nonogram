using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Menu
{
    public class TutorialMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nonograms;
        [SerializeField] private float _topOffset;
        [SerializeField] private float _bottomOffset;

        [SerializeField] private NonogramButton _nonogramButtonPrefab;

        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
        }

        private async void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                var nonogram = Instantiate(_nonogramButtonPrefab, _nonograms);
                nonogram.name = $"nonogram{i + 1}";
            }

            await UniTask.WaitForEndOfFrame(this);

            var height = _nonograms.rect.height + _topOffset + _bottomOffset;
            _content.SetSizeWithCurrentAnchors(_content.rect.width, height);
        }

        private void OnBackButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}

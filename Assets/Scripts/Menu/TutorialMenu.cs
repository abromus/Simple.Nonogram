using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Nonograms;
using Simple.Nonogram.Settings;
using UniRx;
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

        [Space]
        [SerializeField] private NonogramElement _nonogramElementPrefab;

        private void Awake()
        {
            _backButton.OnClickAsObservable()
                .Subscribe(_ => OnBackButtonClick())
                .AddTo(this);
        }

        private async void Start()
        {
            await LoadNonograms();
        }

        private async UniTask LoadNonograms()
        {
            InstantiateNonograms();

            await UniTask.WaitForEndOfFrame(this);

            ResizeContent();
        }

        private void InstantiateNonograms()
        {
            var infos = GetNonogramInfos();

            for (int i = 0; i < infos.Count; i++)
            {
                var nonogram = Instantiate(_nonogramElementPrefab, _nonograms);
                var j = i +1;

                nonogram.Inject(infos[i].Name, infos[i].Size, () =>
                {
                    DebugExtension.LogError($"Nonogram {j}");
                    //SceneManager.LoadScene(_levelSceneName, LoadSceneMode.Single);
                });
            }
        }

        private List<NonogramInfo> GetNonogramInfos()
        {
            var root = DI.GetCompositionRoot(CompositionTag.Game);
            var nonogramSettings = root.Get<NonogramSettings>();
            var nonogramMeta = new NonogramMeta();
            var infos = nonogramMeta.Load(nonogramSettings.PathToTutorialFolder);

            return infos;
        }

        private void ResizeContent()
        {
            var height = _nonograms.rect.height + _topOffset + _bottomOffset;
            _content.SetSizeWithCurrentAnchors(_content.rect.width, height);
        }

        private void OnBackButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}

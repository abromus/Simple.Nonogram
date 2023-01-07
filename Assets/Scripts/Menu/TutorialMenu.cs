using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.StateMachine;
using Simple.Nonogram.Nonograms;
using Simple.Nonogram.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Menu
{
    public class TutorialMenu : MonoBehaviour
    {
        private const string GameScene = "Game";

        [SerializeField] private Button _backButton;
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nonograms;
        [SerializeField] private float _topOffset;
        [SerializeField] private float _bottomOffset;

        [Space]
        [SerializeField] private NonogramElement _nonogramElementPrefab;

        private ICompositionRoot _root;
        private IGameStateMachine _stateMachine;

        private void Awake()
        {
            _root = DI.GetCompositionRoot(CompositionTag.Game);
            _stateMachine = _root.Get<IGameStateMachine>();

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

            var gameInfo = new SceneInfo(GameScene, null);

            for (int i = 0; i < infos.Count; i++)
            {
                var nonogram = Instantiate(_nonogramElementPrefab, _nonograms);

                nonogram.Inject(infos[i].Name, infos[i].Size, () => OnNonogramClick(gameInfo));
            }
        }

        private List<NonogramInfo> GetNonogramInfos()
        {
            var nonogramSettings = _root.Get<NonogramSettings>();
            var nonogramMeta = new NonogramMeta();
            var infos = nonogramMeta.Load(nonogramSettings.PathToTutorialFolder);

            return infos;
        }

        private void ResizeContent()
        {
            var height = _nonograms.rect.height + _topOffset + _bottomOffset;
            _content.SetSizeWithCurrentAnchors(_content.rect.width, height);
        }

        private void OnNonogramClick(SceneInfo gameInfo)
        {
            _stateMachine.Enter<SceneLoaderState, SceneInfo>(gameInfo);
        }

        private void OnBackButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}

using Cysharp.Threading.Tasks;
using Simple.Nonogram.Core.Services;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Nonograms;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
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
        private NonogramController _nonorgamController;

        private void Awake()
        {
            _root = DI.GetCompositionRoot(CompositionTag.Game);
            _stateMachine = _root.Get<IGameStateMachine>();

            var world = _root.Get<IWorld>();
            _nonorgamController = world.NonogramController;

            _backButton.OnClickAsObservable()
                .Subscribe(_ => OnBackButtonClick())
                .AddTo(this);
        }

        private void Start()
        {
            LoadNonograms();
        }

        private async void LoadNonograms()
        {
            InstantiateNonograms();

            await UniTask.WaitForEndOfFrame(this);

            ResizeContent();
        }

        private void InstantiateNonograms()
        {
            var nonograms = _nonorgamController.TutorialNonograms;

            var gameInfo = new SceneInfo(GameScene, null);

            foreach (var nonogram in nonograms)
            {
                var nonogramView = Instantiate(_nonogramElementPrefab, _nonograms);

                nonogramView.Inject(nonogram.Name, nonogram.Size, () => OnNonogramClick(nonogram, gameInfo));
            }
        }

        private async void ResizeContent()
        {
            await UniTask.WaitForEndOfFrame(this);

            var height = _nonograms.rect.height + _topOffset + _bottomOffset;
            _content.SetSizeWithCurrentAnchors(_content.rect.width, height);
        }

        private void OnNonogramClick(NonogramInfo nonogram, SceneInfo gameInfo)
        {
            _nonorgamController.CurrentNonogram = nonogram;

            _stateMachine.Enter<SceneLoaderState, SceneInfo>(gameInfo);
        }

        private void OnBackButtonClick()
        {
            gameObject.SetActive(false);
        }
    }
}

using Simple.Nonogram.Core.Services;
using Simple.Nonogram.Extension;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class GenerationGameBoard : MonoBehaviour
    {
        [SerializeField] private GenerationNonogramSettings _settings;
        [SerializeField] private GenerationBoard _board;
        [SerializeField] private GameObject _boardContainer;
        [SerializeField] private Toolbar _toolbar;

        private ICompositionRoot _root;

        private CompositeDisposable _generationSubscription;
        private CompositeDisposable _subscription;

        private void Awake()
        {
            _generationSubscription = new CompositeDisposable();
            _subscription = new CompositeDisposable();

            _root = DI.GetCompositionRoot(CompositionTag.Game);

            _settings.Created.Subscribe(info => OnSettingsCreated(info)).AddTo(_generationSubscription);
        }

        private void OnSettingsCreated(GenerationNonogramInfo info)
        {
            HideSettings();

            InitializeBoard(info);

            InitializeToolbar();
        }

        private void OnDestroy()
        {
            _generationSubscription = _generationSubscription.SafeUnsubscribe();
            _subscription = _subscription.SafeUnsubscribe();
        }

        private void HideSettings()
        {
            _settings.gameObject.SetActive(false);

            _generationSubscription = _generationSubscription.SafeUnsubscribe();
        }

        private void InitializeBoard(GenerationNonogramInfo info)
        {
            _boardContainer.SetActive(true);
            _board.SetData(_root, info);
        }

        private void InitializeToolbar()
        {
            _toolbar.gameObject.SetActive(true);
            _toolbar.CellTypeChanged.Subscribe(cellType => OnCellTypeChanged(cellType)).AddTo(_subscription);
            _toolbar.Generated.Subscribe(name => OnGenerated(name)).AddTo(_subscription);
        }

        private void OnCellTypeChanged(CellType cellType)
        {
            _board.ChangeCellType(cellType);
        }

        private void OnGenerated(string name)
        {
            _board.GenerateNonogram(name);
        }
    }
}

using Simple.Nonogram.Core.Services;
using Simple.Nonogram.Extension;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Toolbar _toolbar;

        private ICompositionRoot _root;
        private IWorld _world;
        private NonogramController _nonogramController;

        private CompositeDisposable _subscription;

        private void Awake()
        {
            _root = DI.GetCompositionRoot(CompositionTag.Game);
            _world = _root.Get<IWorld>();
            _nonogramController = _world.NonogramController;

            _subscription = new CompositeDisposable();
            _toolbar.CellTypeChanged.Subscribe(cellType => OnCellTypeChanged(cellType)).AddTo(_subscription);
        }

        private void Start()
        {
            SetData();
        }

        private void OnDestroy()
        {
            _subscription = _subscription.SafeUnsubscribe();
        }

        private void SetData()
        {
            _board.SetData(_nonogramController);
        }

        private void OnCellTypeChanged(CellType cellType)
        {
            _board.ChangeCellType(cellType);
        }
    }
}

using Simple.Nonogram.Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class MainMenu : MonoBehaviour
    {
        private const string GenerationScene = "Generation";

        [SerializeField] private NonogramsMenu _nonogramsMenuPrefab;

        [SerializeField] private Button _buttonToPlay;
        [SerializeField] private Button _buttonToGenerateNonograms;

        private ICompositionRoot _root;
        private IGameStateMachine _stateMachine;

        private NonogramsMenu _nonogramsMenu;

        private void Awake()
        {
            _root = DI.GetCompositionRoot(CompositionTag.Game);
            _stateMachine = _root.Get<IGameStateMachine>();

            _buttonToPlay.onClick.AddListener(PlayGame);
            _buttonToGenerateNonograms.onClick.AddListener(GenerateNonogram);
        }

        private void PlayGame()
        {
            ShowNonogramsMenu();

            gameObject.SetActive(false);
        }

        private void ShowNonogramsMenu()
        {
            if (_nonogramsMenu == null)
                _nonogramsMenu = Instantiate(_nonogramsMenuPrefab, transform.parent);

            _nonogramsMenu.gameObject.SetActive(true);
            _nonogramsMenu.SetData(_root);
        }

        private void GenerateNonogram()
        {
            var gameInfo = new SceneInfo(GenerationScene, null);
            _stateMachine.Enter<SceneLoaderState, SceneInfo>(gameInfo);
        }
    }
}

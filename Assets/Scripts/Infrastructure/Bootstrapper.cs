using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Infrastructure.Services;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private MonoInjector _injector;

        private Game _game;

        private async void Awake()
        {
            await CreateGame();
            await CreateCompositionRoot();

            EnterNextState();

            DontDestroyOnLoad(this);
        }

        private async Task CreateGame()
        {
            _game = new Game(_injector);

            await UniTask.WaitForEndOfFrame(this);
        }

        private async Task CreateCompositionRoot()
        {
            _game.CreateCompositionRoot();

            await UniTask.WaitUntil(() => _game.IsInitialized);
        }

        private void EnterNextState()
        {
            _game.StateMachine.Enter<BootstrapState>();
        }
    }
}

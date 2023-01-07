using Cysharp.Threading.Tasks;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.StateMachine;

namespace Simple.Nonogram.Infrastructure
{
    public class Game
    {
        private readonly MonoInjector _injector;

        private GameStateMachine _stateMachine;
        private bool _isInitialized;

        public GameStateMachine StateMachine => _stateMachine;
        public bool IsInitialized => _isInitialized;

        public Game(MonoInjector injector)
        {
            _injector = injector;

            DI.OnRootCreated += OnRootCreated;
        }

        public void CreateCompositionRoot()
        {
            DI.CreateCompositionRoot(CompositionTag.Game);
        }

        private async void OnRootCreated(CompositionRoot root)
        {
            await UniTask.WaitUntil(() => _injector.IsInitialized);

            _stateMachine = new GameStateMachine(new SceneLoader(), root);

            DI.OnRootCreated -= OnRootCreated;

            _isInitialized = true;
        }
    }
}

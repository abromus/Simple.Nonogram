using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;

namespace Simple.Nonogram.Infrastructure.States
{
    public class BootstrapState : IEnterState, IExitState
    {
        private const string InitialSceneName = "InitialScene";
        private const string MainMenuSceneName = "MainMenuScene";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;
        private readonly LoadingController _loadingController;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root, LoadingController loadingController)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
            _loadingController = loadingController;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            _root.Add<IGameStateMachine>(_stateMachine);
            _root.Add<ILoadingController>(_loadingController);
            _loadingController.Initialize();
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadSceneState, string>(MainMenuSceneName);
        }
    }
}

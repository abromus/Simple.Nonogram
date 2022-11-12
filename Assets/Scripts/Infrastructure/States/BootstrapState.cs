using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;

namespace Simple.Nonogram.Infrastructure.States
{
    public class BootstrapState : IEnterState
    {
        private const string InitialSceneName = "Initial";
        private const string LevelDataSceneName = "LevelData";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
        }

        public void Exit() { }

        private void RegisterServices()
        {
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(LevelDataSceneName);
        }
    }
}

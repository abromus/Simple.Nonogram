using Simple.Nonogram.Infrastructure.Services;

namespace Simple.Nonogram.Infrastructure.States
{
    public class BootstrapState : IEnterState
    {
        private const string InitialSceneName = "Initial";
        private const string LevelDataSceneName = "LevelData";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ServiceLocator _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ServiceLocator services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            //_services.Add<IGameFactory>(new GameFactory());
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(LevelDataSceneName);
        }
    }
}

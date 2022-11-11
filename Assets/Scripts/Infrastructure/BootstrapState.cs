namespace Simple.Nonogram.Infrastructure
{
    public class BootstrapState : IEnterState
    {
        private const string InitialSceneName = "Initial";
        private const string LevelDataSceneName = "LevelData";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();

            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
        }

        public void Exit() { }

        private void RegisterServices() { }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadLevelState, string>(LevelDataSceneName);
        }
    }
}

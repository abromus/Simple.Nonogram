namespace Simple.Nonogram.Infrastructure.States
{
    public class BootstrapState : IEnterState
    {
        private const string InitialSceneName = "InitialScene";

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

            _sceneLoader.Load(InitialSceneName, EnterSceneLoaderState);
        }

        public void Exit() { }

        private void RegisterServices() { }

        private void EnterSceneLoaderState()
        {
            var mainMenuInfo = new SceneInfo("MainMenu", () => _stateMachine.Enter<MainMenuState>());

            _stateMachine.Enter<SceneLoaderState, SceneInfo>(mainMenuInfo);
        }
    }
}

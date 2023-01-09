namespace Simple.Nonogram.Core.Services
{
    public class BootstrapState : IEnterState
    {
        private const string BootstrapSceneName = "Bootstrap";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
        }

        public void Enter()
        {
            RegisterServices();

            _sceneLoader.Load(BootstrapSceneName, OnSceneLoad);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            _root.Add<IGameStateMachine>(_stateMachine);
        }

        private void OnSceneLoad()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}

namespace Simple.Nonogram.Infrastructure.Services
{
    public class MainMenuState : IEnterState
    {
        private const string MainMenuScene = "MainMenu";

        private GameStateMachine _stateMachine;

        public MainMenuState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {
            var mainMenuInfo = new SceneInfo(MainMenuScene, null);

            _stateMachine.Enter<SceneLoaderState, SceneInfo>(mainMenuInfo);
        }

        public void Exit() { }
    }
}

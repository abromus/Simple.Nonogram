using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;

namespace Simple.Nonogram.Infrastructure.States
{
    public class GameLoopState : IEnterState, IExitState
    {
        private GameStateMachine _gameStateMachine;
        private SceneLoader _sceneLoader;
        private ICompositionRoot _root;

        public GameLoopState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _gameStateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}

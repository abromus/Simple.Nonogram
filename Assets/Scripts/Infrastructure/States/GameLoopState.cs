using Simple.Nonogram.Infrastructure.Services.DependencyInjection;

namespace Simple.Nonogram.Infrastructure.States
{
    public class GameLoopState : IEnterState, IExitState
    {
        private GameStateMachine _gameStateMachine;
        private ICompositionRoot _root;

        public GameLoopState(GameStateMachine stateMachine, ICompositionRoot root)
        {
            _gameStateMachine = stateMachine;
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

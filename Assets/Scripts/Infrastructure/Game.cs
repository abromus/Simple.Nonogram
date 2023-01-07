using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.StateMachine;

namespace Simple.Nonogram.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game()
        {
            StateMachine = new GameStateMachine(new SceneLoader(), DI.CreateCompositionRoot(CompositionTag.Game));
        }
    }
}

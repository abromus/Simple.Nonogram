using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;
using Simple.Nonogram.Infrastructure.States;

namespace Simple.Nonogram.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game()
        {
            StateMachine = new GameStateMachine(new SceneLoader(), DI.CreateCompositionRoot(CompositionTag.Root));
        }
    }
}

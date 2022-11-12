using Simple.Nonogram.Infrastructure.Services;
using Simple.Nonogram.Infrastructure.States;

namespace Simple.Nonogram.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game()
        {
            StateMachine = new GameStateMachine(new SceneLoader(), ServiceLocator.Contrainer);
        }
    }
}

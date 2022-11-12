using Simple.Nonogram.Configuration;

namespace Simple.Nonogram.Infrastructure.Services.Application
{
    public class GameManager : IGameManager
    {
        private const string GameKey = "game";

        private GameWorld _world;
        public GameConfiguration _gameConfiguration;
        private IGameProvider _gameProvider;

        public GameWorld World => _world;

        public GameManager(GameConfiguration gameConfiguration, IGameProvider gameProvider)
        {
            _gameConfiguration = gameConfiguration;
            _gameProvider = gameProvider;

            _world = new GameWorld();
        }

        public void TickClickSystems()
        {
        }

        public void Tick(float deltaTime)
        {
            _world.DeltaTime = deltaTime;
        }
    }
}

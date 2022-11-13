using Simple.Nonogram.Infrastructure.Services.Loading;
using Simple.Nonogram.Infrastructure.States;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private LoadingController _loadingController;

        private Game _game;

        private void Awake()
        {
            _game = new Game(_loadingController);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}

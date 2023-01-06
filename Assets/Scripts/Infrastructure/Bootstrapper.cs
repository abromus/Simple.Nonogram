using Simple.Nonogram.Infrastructure.States;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private Game _game;

        private void Awake()
        {
            _game = new Game();
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}

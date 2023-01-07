using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.StateMachine
{
    public class MainMenuState : IEnterState
    {
        public void Enter()
        {
            Debug.LogError($"MainMenuState");
        }

        public void Exit() { }
    }
}

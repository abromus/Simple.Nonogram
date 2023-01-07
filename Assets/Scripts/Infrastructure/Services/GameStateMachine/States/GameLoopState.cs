using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.StateMachine
{
    public class GameLoopState : IEnterState
    {
        public void Enter()
        {
            Debug.LogError($"GameLoopState");
        }

        public void Exit() { }
    }
}

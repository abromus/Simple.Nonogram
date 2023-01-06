using UnityEngine;

namespace Simple.Nonogram.Infrastructure.States
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

using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services
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

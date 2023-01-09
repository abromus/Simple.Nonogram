using UnityEngine;

namespace Simple.Nonogram.Core.Services
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

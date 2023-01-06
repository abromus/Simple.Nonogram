using UnityEngine;

namespace Simple.Nonogram.Infrastructure.States
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

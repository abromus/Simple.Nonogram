using System;
using System.Collections.Generic;

namespace Simple.Nonogram.Core.Services
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IExitState _currentState;

        public GameStateMachine(SceneLoader sceneLoader, ICompositionRoot root)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, root),
                [typeof(SceneLoaderState)] = new SceneLoaderState(sceneLoader),
                [typeof(MainMenuState)] = new MainMenuState(this),
                [typeof(GameLoopState)] = new GameLoopState(),
            };
        }

        public void Enter<Tstate>() where Tstate : class, IEnterState
        {
            var state = ChangeState<Tstate>();
            state.Enter();
        }

        public void Enter<Tstate, TPayload>(TPayload payload) where Tstate : class, IEnterState<TPayload>
        {
            var state = ChangeState<Tstate>();
            state.Enter(payload);
        }

        private Tstate ChangeState<Tstate>() where Tstate : class, IExitState
        {
            _currentState?.Exit();

            var state = GetState<Tstate>();
            _currentState = state;

            return state;
        }

        private Tstate GetState<Tstate>() where Tstate : class, IState
        {
            return _states[typeof(Tstate)] as Tstate;
        }
    }
}

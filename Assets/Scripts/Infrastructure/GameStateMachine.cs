using System;
using System.Collections.Generic;

namespace Simple.Nonogram.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IExitState _currentState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(sceneLoader)
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

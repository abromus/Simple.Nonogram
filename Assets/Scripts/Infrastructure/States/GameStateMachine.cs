using System;
using System.Collections.Generic;
using Simple.Nonogram.Infrastructure.Services;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;

namespace Simple.Nonogram.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine, IService
    {
        private readonly Dictionary<Type, IState> _states;

        private IExitState _currentState;
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public GameStateMachine(SceneLoader sceneLoader, ICompositionRoot root, LoadingController loadingController)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, root, loadingController),
                [typeof(LoadSceneState)] = new LoadSceneState(this, sceneLoader, root),
                [typeof(GameLoopState)] = new GameLoopState(this, root)
            };

            _isInitialized = true;
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

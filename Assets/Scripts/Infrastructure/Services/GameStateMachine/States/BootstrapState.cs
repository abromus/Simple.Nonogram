﻿using Simple.Nonogram.Infrastructure.Services.DependencyInjection;

namespace Simple.Nonogram.Infrastructure.Services.StateMachine
{
    public class BootstrapState : IEnterState
    {
        private const string InitialSceneName = "InitialScene";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
        }

        public void Enter()
        {
            RegisterServices();

            _sceneLoader.Load(InitialSceneName, EnterSceneLoaderState);
        }

        public void Exit() { }

        private void RegisterServices()
        {
            _root.Add<IGameStateMachine>(_stateMachine);
        }

        private void EnterSceneLoaderState()
        {
            _stateMachine.Enter<MainMenuState>();
        }
    }
}

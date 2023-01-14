﻿using Cysharp.Threading.Tasks;
using Simple.Nonogram.Game;
using Simple.Nonogram.Core.Services;

namespace Simple.Nonogram.Core
{
    public class Game
    {
        private readonly MonoInjector _injector;

        private GameStateMachine _stateMachine;
        private World _world;
        private bool _isInitialized;

        public GameStateMachine StateMachine => _stateMachine;
        public bool IsInitialized => _isInitialized;

        public Game(MonoInjector injector)
        {
            _injector = injector;

            DI.RootCreated += OnRootCreated;
        }

        public void CreateCompositionRoot()
        {
            DI.CreateCompositionRoot(CompositionTag.Game);
        }

        private async void OnRootCreated(CompositionRoot root)
        {
            await UniTask.WaitUntil(() => _injector.IsInitialized);

            _stateMachine = new GameStateMachine(new SceneLoader(), root);
            _world = new World(root);

            DI.RootCreated -= OnRootCreated;

            _isInitialized = true;
        }
    }
}

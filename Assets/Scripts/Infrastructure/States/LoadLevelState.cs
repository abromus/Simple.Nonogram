using System;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;

namespace Simple.Nonogram.Infrastructure.States
{
    public class LoadLevelState : IEnterState<string>, IExitState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;
        private readonly ILoadingController _loadingController;

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
            _loadingController = _root.Get<ILoadingController>();
        }

        public void Enter(string sceneName)
        {
            _loadingController.ShowLoader();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadingController.HideLoader();
        }

        private void OnLoaded()
        {
            _stateMachine.Enter<GameLoopState>();
        }
    }
}

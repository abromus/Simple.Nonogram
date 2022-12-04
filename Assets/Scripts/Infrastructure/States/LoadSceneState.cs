using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;
using Simple.Nonogram.Infrastructure.Services.SceneManagement;
using UnityEngine.SceneManagement;

namespace Simple.Nonogram.Infrastructure.States
{
    public class LoadSceneState : IEnterState<string>, IExitState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly ICompositionRoot _root;
        private readonly ILoadingController _loadingController;

        private readonly SceneConfiguration _sceneConfiguration;

        public LoadSceneState(GameStateMachine stateMachine, SceneLoader sceneLoader, ICompositionRoot root)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _root = root;
            _loadingController = _root.GetService<ILoadingController>();
            _sceneConfiguration = _root.GetConfiguration<SceneConfiguration>();
        }

        public void Enter(string sceneName)
        {
            _loadingController.ShowLoader();
            _sceneLoader.Load(sceneName, LoadSceneMode.Additive, OnLoaded);
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

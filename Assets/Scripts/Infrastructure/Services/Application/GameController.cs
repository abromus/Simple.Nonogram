using System;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Simple.Nonogram.Configuration;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Delegates;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.Application
{
    public class GameController : IGameProvider, IGameAdapter, IService
    {
        private CompositionRoot _root;
        private GameManager _gameManager;
        private Subject<Null> _onViewUpdate = new Subject<Null>();
        private GameConfiguration _gameConfiguration;
        private AppController _appController;

        private Coroutine _screenSleepCoroutine;

        public GameWorld World => _gameManager.World;

        public IObservable<Null> OnViewUpdate => _onViewUpdate;

        public IGameManager GameManager => _gameManager;

        public void InitGameController(string mainConfigString, CompositionRoot root, Block<GameController> onSuccess, AppController appController)
        {
            _root = root;
            _appController = appController;

            Parse(mainConfigString, () => ContinueCreateGameController(() =>
            {
                onSuccess.SafeInvoke(this);
            }));
        }

        private void ContinueCreateGameController(Block onSuccess)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            CreateGame();
            onSuccess.SafeInvoke();
#else
            CreateGameAsync(onSuccess, unitsConfig);
#endif
        }

        private async void Parse(string mainConfigString, Block onSuccess)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await Task.Run(async () => await CreateConfigurationAsync(mainConfigString));

            DebugExtension.Log("Parse Elapsed Seconds: " + stopwatch.Elapsed.TotalSeconds);

            onSuccess.SafeInvoke();
        }

        private async Task CreateConfigurationAsync(string mainConfigString)
        {
            _gameConfiguration = await GameConfiguration.PreInitialize(mainConfigString);
        }

        private async void CreateGameAsync(Block onSuccess)
        {
            await Task.Run(() => CreateGame());

            onSuccess.SafeInvoke();
        }

        private void CreateGame()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            _gameManager = new GameManager(_gameConfiguration, this);

            DebugExtension.Log("CreateGame Elapsed Seconds: " + stopwatch.Elapsed.TotalSeconds);
        }

        public void Tick(float deltaTime)
        {
            _gameManager.TickClickSystems();

            _gameManager.Tick(deltaTime);

            _onViewUpdate.OnNext(null);
        }

        public void OnPause(bool paused)
        {
            if (!paused)
            {
                ResetSleepTimer();
            }
            else
            {
                if (_screenSleepCoroutine != null) _appController.StopCoroutine(_screenSleepCoroutine);
            }
        }

        public void OnQuit() { }

        private IEnumerator SleepTimer()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            yield return new WaitForSeconds(1800);

            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            _screenSleepCoroutine = null;
        }

        private void ResetSleepTimer()
        {
            if (_screenSleepCoroutine != null)
                _appController.StopCoroutine(_screenSleepCoroutine);

            _screenSleepCoroutine = _appController.StartCoroutine(SleepTimer());
        }
    }
}

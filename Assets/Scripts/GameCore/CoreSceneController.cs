using System;
using System.Collections;
using System.Diagnostics;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure;
using Simple.Nonogram.Infrastructure.Delegates;
using Simple.Nonogram.Infrastructure.Services.Application;
using Simple.Nonogram.Infrastructure.Services.Application.AppEvents;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using Simple.Nonogram.Infrastructure.Services.Loading;
using UniRx;
using UnityEngine;

namespace Simple.Nonogram.GameCore
{
    public class CoreSceneController : AppController, IAppQuitListener
    {
        public static Stopwatch StopWatch;

        public static bool IsWeakDevice => SystemInfo.systemMemorySize < WeakRamMin;
        public static int InitializeCounter { get; private set; }
        public static int MaxInitializeCounter { get; } = 3;

#if UNITY_IOS
        public const int WeakRamMin = 1500;
#else
        public const int WeakRamMin = 3300;
#endif

        private CompositionRoot _root;
        private SceneLoader _sceneLoader;
        private bool _destroyed;
        private AppCore _appCore;
        private GameController _gameController;

        private Coroutine _checkCrashOnLoadCoroutine;
        private string _loadingLog = "";

        private Subject<Null> _onGameInitialized = new Subject<Null>();

        private Stopwatch initGameStopWatch;

        public IObservable<Null> OnGameInitialized => _onGameInitialized;
       
        internal void Inject(AppCore appCore)
        {
            _appCore = appCore;
        }

        protected override void Awake()
        {
            base.Awake();

            _checkCrashOnLoadCoroutine = StartCoroutine(CheckCrashOnLoad());

            StopWatch = new Stopwatch();
            StopWatch.Start();
        }

        private IEnumerator CheckCrashOnLoad()
        {
            yield return new WaitForSeconds(60f);

            DebugExtension.LogError("CheckCrashOnLoad " + _loadingLog);

            _checkCrashOnLoadCoroutine = null;
        }

        public async void Initialize()
        {
            //DOTween.SetTweensCapacity(1000, 500);
            ///////////_root = DI.CreateCompositionRoot(CompositionTag.Root);

            //_sceneLoader = new SceneLoader(this);

            //_root.Add(_sceneLoader);
            _root.Add((AppController)this);

            _loadingLog += "StartInitialize\n";
        }

        protected override void InitGame(Block<IGameAdapter> onSuccess, FailBlock onFail)
        {
            _loadingLog += "StartInitGame\n";

            InitializeCounter++;

            initGameStopWatch = new Stopwatch();
            initGameStopWatch.Start();

            //var mainConfig = sqbaFiles.TextByName("main_defaults.json");

            if (_gameController == null)
            {
                var stop = new Stopwatch();
                stop.Start();
                var gameController = new GameController();
                /*gameController.InitGameController(mainConfig,
                    _root,
                    newGameController =>
                    {
                        InitializeCounter++;
                        UnityEngine.Debug.Log("InitGameController Elapsed Seconds: " + stop.Elapsed.TotalSeconds);
                        ContinueInitGame(newGameController, onSuccess);
                    },
                    this);*/

                return;
            }
        }

        private void ContinueInitGame(GameController gameController, Block<IGameAdapter> onSuccess)
        {
            _gameController = gameController;
            _root.Add<IGameProvider>(gameController);
        }

        protected override void LaunchGame()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (IsWeakDevice) Application.targetFrameRate = 30;

            _loadingLog += "StartLaunchGame\n";
            if (_destroyed) return;

            _appCore.CoreControllerLoaded();
            LoadScenes(stopwatch);
        }

        private void OnDestroy()
        {
            if (_destroyed)
                return;

            ((IDisposable)_sceneLoader).Dispose();

            if (_root != null)
            {
                DI.DestroyCompositionRoot(_root);
                _root = null;
            }

            _destroyed = true;
        }

        private void LoadScenes(Stopwatch stopWatch)
        {
            _loadingLog += "LoadScene\n";
            if (_checkCrashOnLoadCoroutine != null)
            {
                StopCoroutine(_checkCrashOnLoadCoroutine);
                _checkCrashOnLoadCoroutine = null;
            }

            //_sceneLoader.LoadScene(WorldSceneLoadParameters.Default, null, null);
        }

        protected void Update()
        {
            if (_gameController != null)
                _gameController.Tick(Time.deltaTime);
        }

        protected override void AppPaused()
        {
            if (IsInitialized)
                _gameController.OnPause(true);
        }

        protected override void AppResumed()
        {
            if (IsInitialized)
                _gameController.OnPause(false);
        }

        public void OnAppQuit()
        {
            _gameController.OnQuit();
        }
    }
}

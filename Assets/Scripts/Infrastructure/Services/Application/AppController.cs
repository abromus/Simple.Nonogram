using System;
using System.Collections.Generic;
using System.Threading;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Services.Application.AppEvents;
using Simple.Nonogram.Infrastructure.Delegates;
using UnityEngine;
using UnityEngine.Events;
using Simple.Nonogram.GameCore;

namespace Simple.Nonogram.Infrastructure.Services.Application
{
    public abstract class AppController : MonoBehaviour, IAppPauseListener, IAppResumeListener, IService
    {
        [SerializeField, Range(30, 120)] private int _frameRate = 60;
        [SerializeField] private bool _useRefreshRateAsTargetFrameRate;
        [SerializeField] private List<GameObject> _dontDestroyOnLoadList;
        [SerializeField] private bool _initOnStart = true;
        [SerializeField] private CoreCanvasController _coreCanvas;

        private IGameAdapter _gameAdapter;

        private readonly UnityEvent _onInitializeComplete = new UnityEvent();

        private bool _isInitialized;
        private bool _isStartInitialized;

        /// <summary>
        /// Инициализировано ли приложение
        /// </summary>
        public bool IsInitialized
        {
            get => _isInitialized;
            private set
            {
                if (_isInitialized == value)
                    return;

                _isInitialized = value;

                if (_isInitialized)
                    _onInitializeComplete.Invoke();
            }
        }

        /// <summary>
        /// Событие, которое вызывается при инициализации приложения
        /// </summary>
        public UnityEvent OnInitializeComplete => _onInitializeComplete;

        protected CoreCanvasController CoreCanvas => _coreCanvas;

        public int Priority => (int)AppEventPriority.Game;

        public void StartInitialize()
        {
            var defaultOptions = new AppInitializationOptions();
            StartInitialize(defaultOptions);
        }

        public async void StartInitialize(AppInitializationOptions options)
        {
            if (_isStartInitialized)
                return;

            _isStartInitialized = true;
        }

        protected virtual void Awake()
        {
            this.MarkDontDestroyOnLoad();

            foreach (var item in _dontDestroyOnLoadList)
                item.MarkDontDestroyOnLoad();

            SetFrameRate();
            SetNumberFormat();
            InitializeCalendars();
        }

        protected virtual void Start()
        {
            if (_initOnStart)
                StartInitialize();
        }

        /// <summary>
        /// В этом методе инициализируется вся игра (создаётся главный менеджер, подкачиваются конфиги, загружается сейв)
        /// </summary>
        /// <param name="onSuccess">вызывается при успешной инициализации</param>
        /// <param name="onFail">вызывается при неудачной инициализации</param>
        protected abstract void InitGame(Block<IGameAdapter> onSuccess, FailBlock onFail);

        /// <summary>
        /// В этом методе начинается игра (например: загружается игровая сцена)
        /// </summary>
        protected abstract void LaunchGame();

        private void GameFullInitialize()
        {
            InitGame(
                adapter =>
                {
                    _gameAdapter = adapter;
                },
                (c, m) =>
                {
                    DebugExtension.Log($"GameFullInitialize error. Code - \"{c}\"; Message - \"{m}\"");
                    DebugExtension.Exception("ErrorAlert. AppController. Error in InitGame.");
                });
        }

        private void PreLaunchGame()
        {
            _isInitialized = true;

            LaunchGame();
        }
        #region App events handlers

        public void OnAppPaused()
        {
            if (!_isInitialized)
                return;

            AppPaused();
        }

        protected virtual void AppPaused() { }

        public void OnAppResumed()
        {
            if (_isInitialized)
                AppResumed();
        }

        protected virtual void AppResumed() { }

        #endregion App events handlers

        private void SetFrameRate()
        {
            UnityEngine.Application.targetFrameRate = _useRefreshRateAsTargetFrameRate ? Screen.currentResolution.refreshRate : _frameRate;
        }

        private void SetNumberFormat()
        {
            var culture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
        }

        private void InitializeCalendars()
        {
            new System.Globalization.ChineseLunisolarCalendar();
            new System.Globalization.HebrewCalendar();
            new System.Globalization.HijriCalendar();
            new System.Globalization.JapaneseCalendar();
            new System.Globalization.JapaneseLunisolarCalendar();
            new System.Globalization.KoreanCalendar();
            new System.Globalization.KoreanLunisolarCalendar();
            new System.Globalization.PersianCalendar();
            new System.Globalization.TaiwanCalendar();
            new System.Globalization.TaiwanLunisolarCalendar();
            new System.Globalization.ThaiBuddhistCalendar();
            new System.Globalization.UmAlQuraCalendar();
        }
    }
}

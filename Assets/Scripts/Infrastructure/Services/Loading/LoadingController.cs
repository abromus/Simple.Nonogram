using System;
using System.Collections.Generic;
using Simple.Nonogram.Infrastructure.Delegates;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public class LoadingController : MonoBehaviour, ILoadingController
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private LoadingScreen[] allScreens = null;

        private LoadingScreen _activeScreen;
        private HashSet<object> _loaderHoldRequesters = new HashSet<object>();
        private LoaderParams _currentLoaderParams;

        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public bool IsHasAnyScreen => _activeScreen != null;

        public void Initialize()
        {
            _canvas.enabled = true;

            DontDestroyOnLoad(this);
        }

        public void ShowLoader(LoaderParams loaderParams = null, Block onComplete = null, bool withFade = true, int timeout = 0, Block timeoutBlock = null)
        {
            if (loaderParams == null)
                loaderParams = StartupLoaderParams.Startup;

            _currentLoaderParams = loaderParams;

            LoadingScreen newScreen = GetScreenForParams(loaderParams);

            if (loaderParams.IsFirstLoad)
            {
                _activeScreen = newScreen;
                newScreen.Show(onComplete, timeout: timeout, timeoutBlock: timeoutBlock);

                return;
            }

            if (_activeScreen == newScreen)
                _activeScreen = null;

            var oldScreen = _activeScreen;
            _activeScreen = newScreen;

            if (oldScreen != null)
                oldScreen.Hide();

            if (_activeScreen != null)
            {
                _activeScreen.SetParameters(loaderParams);
                _activeScreen.Show(onComplete, withFade, timeout, timeoutBlock);
            }

            _canvas.enabled = true;
        }

        public void HideLoader(Type loaderType, bool force = false)
        {
            if (loaderType != _currentLoaderParams.GetType())
                return;

            _loaderHoldRequesters.Clear();

            if (_activeScreen != null)
                _activeScreen.Hide(force);

            _activeScreen = null;
        }

        public void HideLoader(bool force = false)
        {
            _loaderHoldRequesters.Clear();

            if (_activeScreen != null)
                _activeScreen.Hide(force);

            _activeScreen = null;
        }

        public void RequestHoldLoader(object requester)
        {
            if (requester != null)
                _loaderHoldRequesters.Add(requester);
        }

        public void RequestHideLoader(object requester)
        {
            if (requester == null || !_loaderHoldRequesters.Remove(requester))
                return;

            if (_activeScreen != null && _loaderHoldRequesters.Count == 0)
                _activeScreen.Hide();
        }

        private LoadingScreen GetScreenForParams(LoaderParams loaderParams)
        {
            foreach (var screen in allScreens)
                if (screen.Type == loaderParams.Type)
                    return screen;

            return null;
        }
    }
}

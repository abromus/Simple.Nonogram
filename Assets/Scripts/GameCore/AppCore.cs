using System;
using Simple.Nonogram.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Simple.Nonogram.GameCore
{
    public sealed class AppCore
    {
        public static int FirsSceneId = 0;
        private const int CoreSceneBuildIndex = 1;

        private const string CoreControllerTag = "GameController";

        [SerializeField, Range(30, 60)] private int _frameRate = 60;

        private AppCore()
        {
            FirsSceneId = SceneManager.GetActiveScene().buildIndex;

            Application.targetFrameRate = _frameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            var scene = SceneManager.GetActiveScene();

            SceneManager.sceneLoaded += InitializeCoreController;
            SceneManager.LoadSceneAsync(CoreSceneBuildIndex, LoadSceneMode.Additive);
        }

        private void InitializeCoreController(Scene scene, LoadSceneMode mode)
        {
            DebugExtension.Log("App core loaded");

            SceneManager.sceneLoaded -= InitializeCoreController;

            var coreController = GameObject.FindGameObjectWithTag(CoreControllerTag).GetComponent<CoreSceneController>();
            coreController.Inject(this);
            coreController.Initialize();
        }

        internal void CoreControllerLoaded()
        {
            // SceneManager.UnloadSceneAsync(launchSceneBuildIndex);
        }
    }
}

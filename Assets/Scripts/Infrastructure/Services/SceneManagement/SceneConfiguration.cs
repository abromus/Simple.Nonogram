using System;
using System.Collections.Generic;
using Simple.Nonogram.Configuration;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneConfiguration", menuName = "Configurations/Scene configuration")]
    public class SceneConfiguration : ScriptableConfiguration
    {
        [SerializeField] private List<SceneInfo> sceneTypes;

        private enum SceneType
        {
            Initial,
            MainMenu,
            Level
        }

        [Serializable]
        private class SceneInfo
        {
            public SceneType Type;
            public string SceneName;
        }
    }
}

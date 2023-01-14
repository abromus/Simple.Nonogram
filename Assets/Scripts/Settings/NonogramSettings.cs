using Simple.Nonogram.Core.Services;
using UnityEngine;

namespace Simple.Nonogram.Settings
{
    [CreateAssetMenu(fileName = "NonogramSettings", menuName = "Settings/NonogramSettings", order = 51)]
    public class NonogramSettings : ScriptableObject, IScriptableObject
    {
        [SerializeField] private string _pathToTutorialFolder;
        [SerializeField] private string _pathToGenerationFolder;

        public string PathToTutorialFolder => _pathToTutorialFolder;
        public string PathToGenerationFolder => _pathToGenerationFolder;
    }
}

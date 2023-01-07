using Simple.Nonogram.Infrastructure.Services;
using UnityEngine;

namespace Simple.Nonogram.Settings
{
    [CreateAssetMenu(fileName = "NonogramSettings", menuName = "Settings/NonogramSettings", order = 51)]
    public class NonogramSettings : ScriptableObject, IScriptableObject
    {
        [SerializeField] private string _pathToTutorialFolder;

        public string PathToTutorialFolder => _pathToTutorialFolder;
    }
}

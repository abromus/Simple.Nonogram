using UnityEngine;

namespace Simple.Nonogram.Components
{
    [CreateAssetMenu(fileName = "NonogramConfiguration", menuName = "Configurations/Nonogram configuration")]
    public class NonogramConfiguration : ScriptableObject
    {
        [SerializeField] private string _pathToFile;

        public string PathToFile => _pathToFile;

        public void SetPathToFile(string path)
        {
            _pathToFile = path;
        }
    }
}

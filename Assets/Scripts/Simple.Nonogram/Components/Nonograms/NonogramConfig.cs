using UnityEngine;

namespace Simple.Nonogram.Components
{
    [CreateAssetMenu(fileName = "NonogramConfig", menuName = "Create nonogram config")]
    public class NonogramConfig : ScriptableObject
    {
        [SerializeField] private string _pathToFile;

        public string PathToFile => _pathToFile;

        public void SetPathToFile(string path)
        {
            _pathToFile = path;
        }
    }
}

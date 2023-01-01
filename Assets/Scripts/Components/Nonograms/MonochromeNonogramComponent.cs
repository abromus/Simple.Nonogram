using System.Collections.Generic;
using System.IO;
using Simple.Nonogram.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Simple.Nonogram.Components
{
    public class MonochromeNonogramComponent : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private NonogramView _nonogramView;
        [SerializeField] private NonogramElement _prefab;
        [SerializeField] private NonogramConfig _config;

        private void Awake()
        {
            var _path = "\\Trash\\Nonograms";
            var _levelSceneName = "Level";

            var metaFiles = LoadNonogramsMeta(_path);

            for (int i = 0; i < metaFiles.Count; i++)
            {
                var element = _nonogramView.Add(_prefab);
                var j = i;

                element.SetTitle(metaFiles[i].Name);
                element.SetSize(metaFiles[i].Size);

                element.Button.onClick.AddListener(() =>
                {
                    _config.SetPathToFile(metaFiles[j].FullPath);

                    SceneManager.LoadScene(_levelSceneName, LoadSceneMode.Single);
                });
            }
        }

        private List<NonogramInfo> LoadNonogramsMeta(string path)
        {
            var extension = "*.txt";

            var files = Directory.GetFiles(Application.dataPath + path, extension);
            var nonogramsMeta = new List<NonogramInfo>();

            foreach (string file in files)
            {
                var lines = NonogramFile.LoadFile(file);
                var image = MakeImage(file);
                var size = lines == NonogramFile.EmptyFile
                    ? new Vector2Int(0, 0)
                    : new Vector2Int(lines[0].Length, lines.Count);

                var temp = new NonogramInfo()
                {
                    FullPath = file,
                    Name = Path.GetFileNameWithoutExtension(file),
                    Size = size,
                    Image = image
                };

                nonogramsMeta.Add(temp);
            }

            return nonogramsMeta;
        }

        private Image MakeImage(string _)
        {
            return null;
        }
    }
}

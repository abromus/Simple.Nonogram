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
        [SerializeField] private NonogramElement _prefab;
        [SerializeField] private RectTransform _prefabParent;
        [SerializeField] private NonogramConfig _config;

        private readonly string _path = "\\Trash\\Nonograms";
        private readonly string _levelSceneName = "Level";

        private void Awake()
        {
            var metaFiles = LoadNonogramsMeta(_path);

            for (int i = 0; i < metaFiles.Count; i++)
            {
                var element = Instantiate(_prefab, _prefabParent);
                var actionButton = element.GetButton();
                var j = i;

                element.SetTitle(metaFiles[i].Name);
                element.SetSize(metaFiles[i].Size);

                actionButton.onClick.AddListener(() =>
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

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

        private readonly string _path = "\\Resources\\Nonograms";
        private readonly string _levelSceneName = "Level";

        private void Awake()
        {
            List<NonogramInfo> metaFiles = LoadNonogramsMeta(_path);

            for (int i = (int)Number.Zero; i < metaFiles.Count; i++)
            {
                NonogramElement element = _nonogramView.Add(_prefab);
                Button actionButton = element.GetButton();
                int j = i;

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
            const string extension = "*.txt";

            string[] files = Directory.GetFiles(Application.dataPath + path, extension);
            List<NonogramInfo> nonogramsMeta = new List<NonogramInfo>();

            foreach (string file in files)
            {
                List<string> lines = NonogramFile.LoadFile(file);
                Image image = MakeImage(file);
                Vector2Int size = lines == NonogramFile.EmptyFile
                    ? new Vector2Int((int)Number.Zero, (int)Number.Zero)
                    : new Vector2Int(lines[(int)Number.Zero].Length, lines.Count);

                NonogramInfo temp = new NonogramInfo()
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

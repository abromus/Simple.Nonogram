using System;
using System.Collections.Generic;
using System.IO;
using Simple.Nonogram.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Simple.Nonogram.Components.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _generateButton;

        [SerializeField] private NonogramElement _nonogramPrefab;
        [SerializeField] private RectTransform _nonogramPrefabParent;
        [SerializeField] private RectTransform _nonogramContainer;
        [SerializeField] private NonogramConfiguration _configuration;

        private readonly string _path = "\\Trash\\Nonograms";
        private readonly string _levelSceneName = "Level";
        private readonly string _nonogramGeneratorSceneName = "NonogramGeneratorScene";

        private void Awake()
        {
            _playButton.onClick.AddListener(Play);
            _generateButton.onClick.AddListener(GenerateNonogram);

            _nonogramContainer.gameObject.SetActive(false);
        }

        private void Play()
        {
            _playButton.gameObject.SetActive(false);
            _generateButton.gameObject.SetActive(false);

            _nonogramContainer.gameObject.SetActive(true);
            gameObject.SetActive(false);

            CreateNonograms();
        }

        private void GenerateNonogram()
        {
            SceneManager.LoadScene(_nonogramGeneratorSceneName, LoadSceneMode.Single);
        }

        private void CreateNonograms()
        {
            var metaFiles = LoadNonogramsMeta(_path);

            for (int i = 0; i < metaFiles.Count; i++)
            {
                var element = Instantiate(_nonogramPrefab, _nonogramPrefabParent);
                var j = i;

                element.SetTitle(metaFiles[i].Name);
                element.SetSize(metaFiles[i].Size);

                element.Button.onClick.AddListener(() =>
                {
                    _configuration.SetPathToFile(metaFiles[j].FullPath);

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

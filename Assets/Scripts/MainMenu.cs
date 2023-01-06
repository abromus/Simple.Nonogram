using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Simple.Nonogram.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _buttonToPlay;
        [SerializeField] private Button _buttonToGenerateNonograms;

        private void Awake()
        {
            _buttonToPlay.onClick.AddListener(PlayGame);
            _buttonToGenerateNonograms.onClick.AddListener(GenerateNonogram);
        }

        private void PlayGame()
        {
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        private void GenerateNonogram()
        {
            throw new NotImplementedException();
        }
    }
}

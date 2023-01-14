using System;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NonogramsMenu _nonogramsMenuPrefab;

        [SerializeField] private Button _buttonToPlay;
        [SerializeField] private Button _buttonToGenerateNonograms;

        private NonogramsMenu _nonogramsMenu;

        private void Awake()
        {
            _buttonToPlay.onClick.AddListener(PlayGame);
            _buttonToGenerateNonograms.onClick.AddListener(GenerateNonogram);
        }

        private void PlayGame()
        {
            ShowNonogramsMenu();

            gameObject.SetActive(false);
        }

        private void ShowNonogramsMenu()
        {
            if (_nonogramsMenu == null)
                _nonogramsMenu = Instantiate(_nonogramsMenuPrefab, transform.parent);

            _nonogramsMenu.gameObject.SetActive(true);
        }

        private void GenerateNonogram()
        {
            throw new NotImplementedException();
        }
    }
}

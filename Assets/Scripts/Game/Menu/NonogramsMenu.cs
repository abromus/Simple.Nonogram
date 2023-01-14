using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public class NonogramsMenu : MonoBehaviour
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _tutorialButton;
        [SerializeField] private Button _monochromeNonogramsButton;
        [SerializeField] private Button _multicolorNonogramsButton;

        [SerializeField] private TutorialMenu _tutorialMenuPrefab;

        private TutorialMenu _tutorialMenu;

        private void Awake()
        {
            _backButton.onClick.AddListener(OnBackButtonClick);
            _tutorialButton.onClick.AddListener(OnTutorialButtonClick);
        }

        private void OnBackButtonClick()
        {
            gameObject.SetActive(false);
        }

        private void OnTutorialButtonClick()
        {
            ShowTutorialMenu();

            gameObject.SetActive(false);
        }

        private void ShowTutorialMenu()
        {
            if (_tutorialMenu == null)
                _tutorialMenu = Instantiate(_tutorialMenuPrefab, transform.parent);

            _tutorialMenu.gameObject.SetActive(true);
        }
    }
}

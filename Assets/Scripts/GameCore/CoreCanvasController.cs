using UnityEngine;

namespace Simple.Nonogram.GameCore
{
    public class CoreCanvasController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup coreCanvasGroup;
        [SerializeField] private CanvasGroup gameCanvasGroup;

        [Space]
        [SerializeField] private RectTransform coreRoot;
        [SerializeField] private RectTransform gameRoot;
        [SerializeField] private RectTransform alertsRoot;

        //public PanelRootData CoreRootData => new PanelRootData(coreRoot, coreCanvasGroup);
        //public PanelRootData GameRootData => new PanelRootData(gameRoot, gameCanvasGroup);

        public CanvasGroup CoreCanvasGroup => coreCanvasGroup;
        public CanvasGroup GameCanvasGroup => gameCanvasGroup;

        public RectTransform CoreRoot => coreRoot;
        public RectTransform GameRoot => gameRoot;
        public RectTransform AlertsRoot => alertsRoot;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}

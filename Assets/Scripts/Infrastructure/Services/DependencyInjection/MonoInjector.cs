using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public class MonoInjector : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _behaviours;
        [SerializeField] private ScriptableObject[] _scriptables;
        [SerializeField] private string _injectTag;

        private void Awake()
        {
            DI.OnRootCreated += OnRootCreated;
        }

        private void OnDestroy()
        {
            DI.OnRootCreated -= OnRootCreated;
        }

        public string InjectTag
        {
            get => _injectTag;
            set => _injectTag = value;
        }

        private void OnRootCreated(CompositionRoot root)
        {
            if (!string.Equals(_injectTag, root.Tag, System.StringComparison.InvariantCulture))
                return;

            foreach (var behaviour in _behaviours)
            {
                var type = behaviour.GetType();
                root.Add(type, behaviour);
            }

            foreach (var scriptable in _scriptables)
            {
                var type = scriptable.GetType();
                root.Add(type, scriptable);
            }
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

namespace Simple.Nonogram.Core.Services
{
    public class MonoInjector : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _behaviours;
        [SerializeField] private ScriptableObject[] _scriptables;
        [SerializeField] private string _injectTag;

        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        private void Awake()
        {
            DI.RootCreated += OnRootCreated;
        }

        private void OnDestroy()
        {
            DI.RootCreated -= OnRootCreated;
        }

        private void OnRootCreated(CompositionRoot root)
        {
            if (!string.Equals(_injectTag, root.Tag, StringComparison.InvariantCulture))
                return;

            AddBehaviours(root);
            AddScriptables(root);

            _isInitialized = true;
        }

        private void AddBehaviours(CompositionRoot root)
        {
            Add(root, _behaviours);
        }

        private void AddScriptables(CompositionRoot root)
        {
            Add(root, _scriptables);
        }

        private void Add(CompositionRoot root, IEnumerable enumerators)
        {
            foreach (var enumerator in enumerators)
            {
                var type = enumerator.GetType();
                root.Add(type, enumerator);
            }
        }
    }
}

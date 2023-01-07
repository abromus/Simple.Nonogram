using System;
using System.Collections;
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

        private void OnRootCreated(CompositionRoot root)
        {
            if (!string.Equals(_injectTag, root.Tag, StringComparison.InvariantCulture))
                return;

            AddBehaviours(root);
            AddScriptables(root);
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

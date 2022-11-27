using System.Collections.Generic;
using Simple.Nonogram.Configuration;
using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public class MonoInjector : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> _services;
        [SerializeField] private List<ScriptableConfiguration> _configurations;
        [SerializeField] private string _injectTag;

        public string InjectTag
        {
            get => _injectTag;
            set => _injectTag = value;
        }

        private void OnValidate()
        {
            for (int i = 0; i < _services.Count; i++)
                if (_services[i].GetComponent<IService>() == null)
                {
                    _services.RemoveAt(i);
                    i--;
                }
        }

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
            if (!string.Equals(_injectTag, root.Tag, System.StringComparison.InvariantCulture))
                return;

            foreach (var service in _services)
                root.AddService(service as IService);

            foreach (var configuration in _configurations)
                root.AddConfiguration(configuration);
        }
    }
}

using System;
using System.Collections.Generic;
using Simple.Nonogram.Extension;

namespace Simple.Nonogram.Infrastructure.Services
{
    public sealed class CompositionRoot : ICompositionRoot, IDisposable
    {
        private CompositionRoot _dependencyRoot;
        private List<CompositionRoot> _childs;

        private Dictionary<Type, object> _objects = new Dictionary<Type, object>();

        public CompositionRoot DependencyRoot =>_dependencyRoot;

        public string Tag { get; private set; }

        public bool Disposed { get; private set; }

        public CompositionRoot(string tag)
        {
            Tag = tag;
        }

        public void Add<TService>(TService service) where TService : class, IService
        {
            if (service == null)
                return;

            Add(typeof(TService), service);
        }

        public void Add(Type type, object service)
        {
            _objects[type] = service;
        }

        public TService Get<TService>() where TService : class, IService
        {
            if (Disposed)
                return null;

            var type = typeof(TService);

            return _objects.TryGetValue(type, out object service)
                ? service as TService
                : _dependencyRoot != null
                    ? _dependencyRoot.Get<TService>()
                    : null;
        }

        public void SetDependency(CompositionRoot dependencyRoot)
        {
            if (Disposed)
            {
                DebugExtension.LogWarning($"Can't set dependency to Disposed CompositionRoot. Tag = \"{Tag}\"");
                return;
            }

            if (dependencyRoot == this)
            {
                DebugExtension.LogWarning($"Can't set as dependency root as self. Tag = \"{Tag}\"");
                return;
            }

            if (dependencyRoot == null || dependencyRoot.Disposed)
            {
                DebugExtension.LogWarning("Can't set null or Disposed Root");
                return;
            }

            if (HasChild(dependencyRoot))
            {
                DebugExtension.LogWarning($"Can't set as dependency own child. Tag = \"{Tag}\", DependencyTag = \"{dependencyRoot.Tag}\"");
                return;
            }

            _dependencyRoot = dependencyRoot;
            _dependencyRoot.AddChild(this);
        }

        public void Dispose()
        {
            if (Disposed)
                return;

            _objects.Clear();
            _objects = null;

            if (_childs != null)
                ClearChilds();

            if (_dependencyRoot != null && !_dependencyRoot.Disposed && _dependencyRoot._childs != null)
            {
                _dependencyRoot._childs.Remove(this);
                _dependencyRoot = null;
            }

            Disposed = true;
        }

        private void ClearChilds()
        {
            foreach (var child in _childs)
            {
                if (child == null || child.Disposed)
                    continue;

                if (child.DependencyRoot == this)
                    child.ClearDependencyRoot();
            }

            _childs.Clear();
            _childs = null;
        }

        private void AddChild(CompositionRoot child)
        {
            if (Disposed)
                return;

            if (_childs == null)
                _childs = new List<CompositionRoot>();

            _childs.Add(child);
        }

        private bool HasChild(CompositionRoot child)
        {
            return !Disposed && _childs != null && _childs.Contains(child);
        }

        private void ClearDependencyRoot()
        {
            _dependencyRoot = null;
        }
    }
}

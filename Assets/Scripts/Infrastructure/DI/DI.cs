using System;
using System.Collections.Generic;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public static class DI
    {
        private static readonly Dictionary<string, CompositionRoot> Roots = new Dictionary<string, CompositionRoot>();

        private static string LastRootTag;
        private static CompositionRoot LastRoot;

        public static event Block<CompositionRoot> OnRootCreated;

        public static CompositionRoot CreateCompositionRoot(string compositionTag)
        {
            if (HasRootWithTag(compositionTag))
            {
                DebugExtension.LogError($"Create CompositionRoot with tag \"{compositionTag}\" twice.");
                return null;
            }
            else
            {
                var root = new CompositionRoot(compositionTag);
                Roots[compositionTag] = root;

                OnRootCreated?.Invoke(root);

                return root;
            }
        }

        public static ICompositionRoot GetCompositionRoot(string compositionTag)
        {
            if (string.Equals(LastRootTag, compositionTag, StringComparison.InvariantCulture))
            {
                if (LastRoot != null && !LastRoot.Disposed)
                    return LastRoot;
                else
                    ClearLastRoot();
            }

            LastRootTag = compositionTag;
            LastRoot = Roots[LastRootTag];

            return LastRoot;
        }

        public static void DestroyCompositionRoot(CompositionRoot root)
        {
            if (root == null || root.Disposed)
                return;

            Roots.Remove(root.Tag);

            if (LastRoot == root)
                ClearLastRoot();

             root.Dispose();
        }

        public static void SetRootDependency(CompositionRoot root, string dependencyTag)
        {
            Roots.TryGetValue(dependencyTag, out CompositionRoot dependency);

            root.SetDependency(dependency);
        }

        private static bool HasRootWithTag(string compositionTag)
        {
            return Roots.ContainsKey(compositionTag);
        }

        private static void ClearLastRoot()
        {
            LastRootTag = null;
            LastRoot = null;
        }
    }
}

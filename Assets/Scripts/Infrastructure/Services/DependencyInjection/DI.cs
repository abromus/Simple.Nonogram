using System;
using System.Collections.Generic;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public static class DI
    {
        private static Dictionary<string, CompositionRoot> RootByTag = new Dictionary<string, CompositionRoot>();

        private static string LastRootTag;
        private static CompositionRoot LastRoot;

        public static event Block<CompositionRoot> OnRootCreated;

        public static ICompositionRoot GetCompositionRoot(string compositionTag)
        {
            if (string.Equals(LastRootTag, compositionTag, StringComparison.InvariantCulture))
            {
                if (LastRoot != null && !LastRoot.Disposed)
                    return LastRoot;

                LastRootTag = null;
                LastRoot = null;
            }

            LastRootTag = compositionTag;

            return LastRoot = RootByTag[LastRootTag];
        }

        public static bool HasRootWithTag(string compositionTag)
        {
            return RootByTag.ContainsKey(compositionTag);
        }

        public static CompositionRoot CreateCompositionRoot(string compositionTag)
        {
            if (HasRootWithTag(compositionTag))
            {
                DebugExtension.LogError($"Create CompositionRoot with tag \"{compositionTag}\" twice.");
                return null;
            }

            var root = new CompositionRoot(compositionTag);
            RootByTag[compositionTag] = root;

            OnRootCreated?.Invoke(root);

            return root;
        }

        public static void DestroyCompositionRoot(CompositionRoot root)
        {
            if (root == null || root.Disposed)
                return;

            RootByTag.Remove(root.Tag);

            if (LastRoot == root)
            {
                LastRootTag = null;
                LastRoot = null;
            }

            ((IDisposable)root).Dispose();
        }

        public static void SetRootDependency(CompositionRoot root, string dependencyTag)
        {
            RootByTag.TryGetValue(dependencyTag, out CompositionRoot dependency);

            root.SetDependency(dependency);
        }
    }
}

using System.Threading;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Infrastructure.Delegates;
using UniRx;
using UnityEngine.SceneManagement;

namespace Simple.Nonogram.Infrastructure
{
    public class SceneLoader
    {
        private CancellationToken _subscription;

        public void Load(string name, Block onSuccess = null)
        {
            LoadScene(name, onSuccess);
        }

        private void LoadScene(string name, Block onSuccess = null)
        {
            if (SceneManager.GetActiveScene().name != name)
            {
                SceneManager.LoadSceneAsync(name)
                    .AsAsyncOperationObservable()
                    .Do(x => DebugExtension.Log($"Scene \"{name}\" progress: {x.progress}"))
                    .Subscribe(_ => DebugExtension.Log($"Scene \"{name}\" loaded"))
                    .AddTo(_subscription);
            }
            else
            {
                DebugExtension.Log($"Scene \"{name}\" already loaded!");
            }

            onSuccess.SafeInvoke();
        }
    }
}

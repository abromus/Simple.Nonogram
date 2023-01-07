using System.Threading;
using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extension;
using Simple.Nonogram.Infrastructure.Delegates;
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

        private async void LoadScene(string name, Block onSuccess = null)
        {
            if (SceneManager.GetActiveScene().name != name)
            {
                var nextSceneOperation = SceneManager.LoadSceneAsync(name);

                await UniTask.WaitUntil(() => nextSceneOperation.isDone);

                DebugExtension.Log($"Scene \"{name}\" loaded!");
            }
            else
            {
                DebugExtension.Log($"Scene \"{name}\" already loaded!");
            }

            onSuccess.SafeInvoke();
        }
    }
}

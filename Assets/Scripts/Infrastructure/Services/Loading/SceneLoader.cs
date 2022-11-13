using Cysharp.Threading.Tasks;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.Infrastructure.Delegates;
using UnityEngine.SceneManagement;

namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public class SceneLoader : IService
    {
        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        public SceneLoader()
        {
            _isInitialized = true;
        }

        public async void Load(string name, Block onSuccess = null)
        {
            await LoadScene(name, onSuccess);
        }

        private async UniTask LoadScene(string name, Block onSuccess = null, FailBlock onFail = null)
        {
            if (SceneManager.GetActiveScene().name != name)
            {
                var nextSceneOperation = SceneManager.LoadSceneAsync(name);

                await UniTask.WaitUntil(() => nextSceneOperation.isDone);
            }

            onSuccess.SafeInvoke();
        }
    }
}

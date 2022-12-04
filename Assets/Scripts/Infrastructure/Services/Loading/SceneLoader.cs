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

        public async void Load(string name, LoadSceneMode loadSceneMode, Block onSuccess = null, FailBlock onFail = null)
        {
            await LoadScene(name, loadSceneMode, onSuccess, onFail);
        }

        private async UniTask LoadScene(string name, LoadSceneMode loadSceneMode, Block onSuccess = null, FailBlock onFail = null)
        {
            if (SceneManager.GetActiveScene().name != name)
            {
                var nextSceneOperation = SceneManager.LoadSceneAsync(name, loadSceneMode);

                await UniTask.WaitUntil(() => nextSceneOperation.isDone);
            }

            onSuccess.SafeInvoke();
        }
    }
}

﻿using Cysharp.Threading.Tasks;
using Simple.Nonogram.Infrastructure.Delegates;
using UnityEngine.SceneManagement;

namespace Simple.Nonogram.Infrastructure
{
    public class SceneLoader
    {
        public async void Load(string name, Block onSuccess = null)
        {
            await LoadScene(name, onSuccess);
        }

        private async UniTask LoadScene(string name, Block onSuccess = null)
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
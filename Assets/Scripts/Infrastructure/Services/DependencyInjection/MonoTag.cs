using UnityEngine;

namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MonoInjector))]
    public abstract class MonoTag : MonoBehaviour
    {
#if !UNITY_EDITOR
        private void Awake()
        {
            Destroy(this);
        }
#else
        protected abstract string InjectTag { get; }

        private void Awake()
        {
            if (UnityEngine.Application.isPlaying)
            {
                Destroy(this);
                return;
            }

            SetupTag();
        }

        private void Start()
        {
            SetupTag();
        }

        private void Update()
        {
            SetupTag();
        }

        private void SetupTag()
        {
            GetComponent<MonoInjector>().InjectTag = InjectTag;
        }

#endif
    }
}

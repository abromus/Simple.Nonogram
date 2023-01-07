using UnityEngine;

namespace Simple.Nonogram.Infrastructure
{
    public class Launcher : MonoBehaviour
    {
        [SerializeField] private Bootstrapper _bootstrapperPrefab;

        private void Start()
        {
            var bootstrapper = FindObjectOfType<Bootstrapper>();

            if (bootstrapper == null)
                Instantiate(_bootstrapperPrefab);

            Destroy(gameObject);
        }
    }
}

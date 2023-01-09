using UnityEngine;

namespace Simple.Nonogram.Core
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

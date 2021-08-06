using System;

using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(BoardView))]
    public class Verifier : MonoBehaviour
    {
        private Core.Verifier _verifier;
        private BoardView _boardView;

        public event Action<Vector3> MarkMistake;
        public event Action<Vector3> RemoveMistake;

        private void Start()
        {
            _boardView = GetComponent<BoardView>();

            _verifier = new Core.Verifier(_boardView);
            _verifier.MarkMistake += OnMarkMistake;
            _verifier.RemoveMistake += OnRemoveMistake;
        }

        private void OnMarkMistake(Vector3 position)
        {
            MarkMistake?.Invoke(position);
        }

        private void OnRemoveMistake(Vector3 position)
        {
            RemoveMistake?.Invoke(position);
        }
    }
}

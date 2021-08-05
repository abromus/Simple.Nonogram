using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(BoardView))]
    public class Verifier : MonoBehaviour
    {
        private Core.Verifier _verifier;
        private BoardView _boardView;

        private void Start()
        {
            _boardView = GetComponent<BoardView>();

            _verifier = new Core.Verifier(_boardView);
        }
    }
}

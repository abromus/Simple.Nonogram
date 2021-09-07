using UnityEngine;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Board))]
    [RequireComponent(typeof(Verifier))]
    public class VerifierRenderer : MonoBehaviour
    {
        private Board _board;
        private Verifier _verifier;
        private Cell[,] _userBoard;
        private Color _mistakeColor = Color.red;
        private Color _hoverColor = new Color(0.52f, 0.45f, 0.45f);

        private void Awake()
        {
            _board = GetComponent<Board>();
            _verifier = GetComponent<Verifier>();
        }

        private void Start()
        {
            _userBoard = _board.UserBoardView;
        }

        private void OnEnable()
        {
            _verifier.MarkMistake += OnMarkMistake;
            _verifier.RemoveMistake += OnRemoveMistake;
        }

        private void OnDisable()
        {
            _verifier.MarkMistake -= OnMarkMistake;
            _verifier.RemoveMistake -= OnRemoveMistake;
        }

        private void SetMistake(Vector2Int coordinate, bool isMistake)
        {
            _userBoard[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>().color = isMistake ? _mistakeColor : _hoverColor;
        }

        private void OnMarkMistake(Vector2Int coordinate)
        {
            SetMistake(coordinate, true);
        }

        private void OnRemoveMistake(Vector2Int coordinate)
        {
            SetMistake(coordinate, false);
        }
    }
}

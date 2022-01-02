using System;
using Simple.Nonogram.Extension;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Board))]
    public class Verifier : MonoBehaviour
    {
        private Board _board;
        private Core.Verifier _verifier;

        public bool[,] MistakeBoard => _verifier.MistakeBoard;

        public event Action<Vector2Int> MarkMistake;
        public event Action<Vector2Int> RemoveMistake;

        private void Awake()
        {
            _board = GetComponent<Board>();
            _verifier = new Core.Verifier(_board.AnswerBoard, _board.UserBoard);
        }

        private void Start()
        {
            _board.BoardClicked += OnClicked;
            _verifier.MarkMistake += OnMarkMistake;
            _verifier.RemoveMistake += OnRemoveMistake;
        }

        private void CheckCell(Vector2Int coordinate)
        {
            _verifier.CheckCell(coordinate);
        }

        private void OnClicked(Vector3 position, PointerEventData.InputButton arg2)
        {
            if (ArrayExtension.TryFindCell(_board.UserBoardView, position, out Vector2Int coordinate))
                CheckCell(coordinate);
        }

        private void OnMarkMistake(Vector2Int position)
        {
            MarkMistake?.Invoke(position);
        }

        private void OnRemoveMistake(Vector2Int position)
        {
            RemoveMistake?.Invoke(position);
        }
    }
}

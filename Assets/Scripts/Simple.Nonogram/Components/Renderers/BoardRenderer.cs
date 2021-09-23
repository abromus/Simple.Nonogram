using Simple.Nonogram.Core;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Simple.Nonogram.Components
{
    [RequireComponent(typeof(Board))]
    [RequireComponent(typeof(Verifier))]
    public class BoardRenderer : MonoBehaviour
    {
        [SerializeField] private Sprite _blank;
        [SerializeField] private Sprite _marked;
        [SerializeField] private Sprite _empty;
        [SerializeField] private Material _lineMaterial;

        private Board _board;
        private Verifier _verifier;
        private GuideRenderer _guideRenderer;
        private AuxiliaryLinesRenderer _auxiliaryLinesRenderer;
        private NumberBoardRenderer _topRenderer;
        private NumberBoardRenderer _leftRenderer;

        private void Awake()
        {
            _board = GetComponent<Board>();
            _verifier = GetComponent<Verifier>();

            _topRenderer = new NumberBoardRenderer(_board.Top, _blank, _empty);
            _leftRenderer = new NumberBoardRenderer(_board.Left, _blank, _empty);
        }

        private void Start()
        {
            _guideRenderer = new GuideRenderer(_board, _verifier);
            _auxiliaryLinesRenderer = new AuxiliaryLinesRenderer(_board, transform, _lineMaterial, Color.grey, Color.grey);

            _board.BoardClicked += OnBoardClicked;
            _board.BoardHoveredBegin += OnBoardHoveredBegin;
            _board.BoardHoveredEnd += OnBoardHoveredEnd;
            _board.TopClicked += _topRenderer.OnClicked;
            _board.LeftClicked += _leftRenderer.OnClicked;

            _auxiliaryLinesRenderer.Draw();
        }

        private void MarkCell(Vector3 position, Sprite filledState)
        {
            if (ArrayExtension.TryFindCell(_board.UserBoardView, position, out Vector2Int coordinate))
                SetSprite(coordinate, filledState);
        }

        private void SetSprite(Vector2Int coordinate, Sprite filledState)
        {
            SpriteRenderer spriteRenderer = _board.UserBoardView[coordinate.x, coordinate.y].GetComponent<SpriteRenderer>();

            spriteRenderer.sprite = spriteRenderer.sprite != filledState ? filledState : _blank;
        }

        private void OnBoardClicked(Vector3 position, PointerEventData.InputButton button)
        {
            if (button == PointerEventData.InputButton.Left)
                MarkCell(position, _marked);
            else if (button == PointerEventData.InputButton.Right)
                MarkCell(position, _empty);
        }

        private void OnBoardHoveredBegin(Vector3 position)
        {
            _guideRenderer.DrawGuides(position, true);
        }

        private void OnBoardHoveredEnd(Vector3 position)
        {
            _guideRenderer.DrawGuides(position, false);
        }
    }
}

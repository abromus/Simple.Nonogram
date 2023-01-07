using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private Cell _cellPrefab;
        [SerializeField] private RectTransform _cellsContainer;

        [SerializeField] private NumberCell _numberCellPrefab;
        [SerializeField] private RectTransform _topNumberCellsContainer;
        [SerializeField] private RectTransform _leftNumberCellsContainer;

        [SerializeField] private RectTransform _rectTransform;
    }
}

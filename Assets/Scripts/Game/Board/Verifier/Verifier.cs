using System;
using Simple.Nonogram.Nonograms;
using UnityEngine;

namespace Simple.Nonogram.Game
{
    public class Verifier : MonoBehaviour
    {
        private NonogramInfo _nonogram;

        public void SetData(NonogramInfo nonogram)
        {
            _nonogram = nonogram;
        }

        public void CheckCell(Cell cell)
        {
            var x = (int)(cell.RectTransform.anchoredPosition.x / cell.Size.x);
            var y = (int)(-cell.RectTransform.anchoredPosition.y / cell.Size.y) - 1;
            var empty = '0';
            var marked = '1';

            if (cell.CellType == CellType.None
                || _nonogram.Nonogram[y][x] == marked && cell.CellType == CellType.Marked
                || _nonogram.Nonogram[y][x] == empty && cell.CellType == CellType.Empty)
            {
                cell.Image.color = Color.white;
            }
            else
            {
                cell.Image.color = Color.red;
            }
        }
    }
}

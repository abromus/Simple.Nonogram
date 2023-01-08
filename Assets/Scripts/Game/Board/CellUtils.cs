using System.Collections.Generic;
using Simple.Nonogram.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace Simple.Nonogram.Game
{
    public static class CellUtils
    {
        public static List<T> InstantiateCells<T>(T prefab, RectTransform container, int width, int height) where T : MonoBehaviour
        {
            var items = new List<T>();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    var item = Object.Instantiate(prefab, container);
                    items.Add(item);
                }

            return items;
        }

        public static void SetSizeWithCurrentAnchors(RectTransform container, float width, float height)
        {
            container.SetSizeWithCurrentAnchors(width, height);
        }

        public static void SetCellSize(GridLayoutGroup group, float x, float y)
        {
            group.cellSize = new Vector2(x, y);
        }
    }
}

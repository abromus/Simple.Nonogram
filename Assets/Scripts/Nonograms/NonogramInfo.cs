using System.Collections.Generic;
using UnityEngine;

namespace Simple.Nonogram.Nonograms
{
    public struct NonogramInfo
    {
        public readonly List<string> Nonogram;
        public readonly string Name;
        public readonly Vector2Int Size;

        public NonogramInfo(List<string> nonogram, string name)
        {
            Nonogram = nonogram;
            Name = name;
            Size = nonogram == null
                ? new Vector2Int(0, 0)
                : new Vector2Int(nonogram[0].Length, nonogram.Count);
        }
    }
}

using UnityEngine;

namespace Simple.Nonogram.Nonograms
{
    public struct NonogramInfo
    {
        public string FullPath;
        public string Name;
        public Vector2Int Size;

        public NonogramInfo(string fullPath, string name, Vector2Int size)
        {
            FullPath = fullPath;
            Name = name;
            Size = size;
        }
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Simple.Nonogram.Nonograms
{
    public class NonogramMeta
    {
        private const string Extension = "*.txt";

        public List<NonogramInfo> Load(string path)
        {
            var files = GetFiles(path, Extension);

            var metas = Load(files);

            return metas;
        }

        private List<NonogramInfo> Load(string[] files)
        {
            var nonogramsMeta = new List<NonogramInfo>();

            foreach (string file in files)
                nonogramsMeta.Add(CreateNonogramInfo(file));

            return nonogramsMeta;
        }

        private NonogramInfo CreateNonogramInfo(string file)
        {
            var nonogramFile = new NonogramFile();
            var nonogram = nonogramFile.LoadFile(file);
            var name = GetName(file);
            var size = GetSize(nonogram);

            var nonogramInfo = new NonogramInfo(file, name, size);

            return nonogramInfo;
        }

        private string[] GetFiles(string path, string extension)
        {
            return Directory.GetFiles(Application.dataPath + path, extension);
        }

        private string GetName(string file)
        {
            return Path.GetFileNameWithoutExtension(file);
        }

        private static Vector2Int GetSize(List<string> nonogram)
        {
            return nonogram == null
                ? new Vector2Int(0, 0)
                : new Vector2Int(nonogram[0].Length, nonogram.Count);
        }
    }
}

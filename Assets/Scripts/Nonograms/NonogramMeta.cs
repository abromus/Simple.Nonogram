using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Simple.Nonogram.Nonograms
{
    public class NonogramMeta
    {
        private const string Extension = "txt";
        private const string SearchPattern = "*." + Extension;

        public List<NonogramInfo> Load(string path)
        {
            var files = GetFiles(path, SearchPattern);

            var metas = Load(files);

            return metas;
        }

        public void Safe(string path, List<string> file)
        {
            var nonogramFile = new NonogramFile();
            var pathWithExtension = $"{Application.dataPath}{path}.{Extension}";

            nonogramFile.SafeFile(pathWithExtension, file);
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

            var nonogramInfo = new NonogramInfo(nonogram, name);

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
    }
}

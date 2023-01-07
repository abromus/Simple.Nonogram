using System;
using System.Collections.Generic;
using System.IO;
using Simple.Nonogram.Extension;

namespace Simple.Nonogram.Nonograms
{
    public class NonogramFile
    {
        public List<string> LoadFile(string path)
        {
            return string.IsNullOrEmpty(path) ? null : TryLoadFile(path);
        }

        private List<string> TryLoadFile(string path)
        {
            var lines = new List<string>();

            try
            {
                using StreamReader stream = new StreamReader(path);

                do
                {
                    lines.Add(stream.ReadLine());
                } while (!stream.EndOfStream);
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"NonogramFile.LoadFile: {exception.Message}.");
            }

            return lines;
        }
    }
}

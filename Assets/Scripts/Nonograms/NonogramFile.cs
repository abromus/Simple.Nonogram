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

        public void SafeFile(string path, List<string> file)
        {
            if (path == null || file == null)
                return;

            SafeFileAsync(path, file);
        }

        private List<string> TryLoadFile(string path)
        {
            var lines = new List<string>();

            try
            {
                using StreamReader stream = new StreamReader(path);

                do
                {
                    lines.Add(stream.ReadLineAsync().Result);
                } while (!stream.EndOfStream);
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"NonogramFile.LoadFile: {exception.Message}.");
            }

            return lines;
        }

        private async void SafeFileAsync(string path, List<string> file)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(path, false);

                foreach (var line in file)
                    await writer.WriteLineAsync(line);
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"NonogramFile.SafeFileAsync: {exception.Message}.");
            }
        }
    }
}

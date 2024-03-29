﻿using System;
using System.Collections.Generic;
using System.IO;
using Simple.Nonogram.Extensions;

namespace Simple.Nonogram.Core
{
    public static class NonogramFile
    {
        public static List<string> EmptyFile => null;

        public static List<string> LoadFile(string path)
        {
            var lines = new List<string>();

            if (string.IsNullOrEmpty(path))
                return EmptyFile;

            try
            {
                using var stream = new StreamReader(path);

                do
                {
                    lines.Add(stream.ReadLine());
                } while (stream.EndOfStream == false);
            }
            catch (Exception exception)
            {
                DebugExtension.LogError($"NonogramFile.LoadFile: {exception.Message}.");
            }

            return lines;
        }
    }
}

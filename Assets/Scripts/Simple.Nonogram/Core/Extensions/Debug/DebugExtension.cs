using System;

using UnityEngine;

namespace Simple.Nonogram.Core
{
    public static class DebugExtension
    {
        public static void Log(string message)
        {
            Debug.Log($"{DateTime.Now}. {message}");
        }

        public static void LogError(string message)
        {
            Debug.LogError($"{DateTime.Now}. {message}");
        }
    }
}

using System;
using UnityEngine;

namespace Simple.Nonogram.Core
{
    public static class DebugExtension
    {
        public static void Log(string message, string color = "white")
        {
            Debug.Log($"{DateTime.Now}. <color={color}>{message}</color>");
        }

        public static void LogWarning(string message, string color = "yellow")
        {
            Debug.LogWarning($"{DateTime.Now}. <color={color}>{message}</color>");
        }

        public static void LogError(string message, string color = "red")
        {
            Debug.LogError($"{DateTime.Now}. <color={color}>{message}</color>");
        }
    }
}

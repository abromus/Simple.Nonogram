using System;
using UnityEngine;

namespace Simple.Nonogram.Extensions
{
    public static class DebugExtension
    {
        public static void Log(string message, string color = "white")
        {
            Debug.Log($"{DateTime.Now}. <color={color}>{message}</color>");
        }

        public static void Log(string message, Color color)
        {
            Log(message, color.ToString());
        }

        public static void LogWarning(string message, string color = "yellow")
        {
            Debug.LogWarning($"{DateTime.Now}. <color={color}>{message}</color>");
        }

        public static void LogWarning(string message, Color color)
        {
            LogWarning(message, color.ToString());
        }

        public static void LogError(string message, string color = "red")
        {
            Debug.LogError($"{DateTime.Now}. <color={color}>{message}</color>");
        }

        public static void LogError(string message, Color color)
        {
            LogError(message, color.ToString());
        }

        public static void Exception(string exceptionMessage)
        {
            var exception = new Exception(exceptionMessage);

            LogError(exception.Message);
        }
    }
}

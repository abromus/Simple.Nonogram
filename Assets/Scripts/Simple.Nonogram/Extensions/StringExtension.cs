using Simple.Nonogram.Core;
using System;
using System.Linq;

namespace Simple.Nonogram.Extension
{
    public static class StringExtension
    {
        public static string ConvertRgbToHex(float[] values)
        {
            return values.Aggregate("", (current, value) => current + ConvertToHex(value));
        }

        public static string ConvertToHex(float value)
        {
            byte[] bytes = BitConverter.GetBytes((int)value);
            int i = BitConverter.ToInt32(bytes, (int)Number.Zero);

            return i.ToString("X2");
        }

        public static string[] Split(string text, int countCharacters)
        {
            int count = (int)Math.Ceiling((float)text.Length / countCharacters);
            int index = (int)Number.Zero;
            string[] strings = new string[count];

            for (int i = (int)Number.Zero; i < strings.Length; i++)
            {
                if (index + countCharacters >= text.Length)
                    countCharacters = text.Length - index;

                strings[i] = text.Substring(index, countCharacters);

                index += countCharacters;
            }

            return strings;
        }

        public static float[] ConvertToFloat(string[] array)
        {
            float[] tempArray = new float[array.Length];

            for (int i = (int)Number.Zero; i < array.Length; i++)
                tempArray[i] = uint.Parse(array[i], System.Globalization.NumberStyles.AllowHexSpecifier);

            return tempArray;
        }
    }
}

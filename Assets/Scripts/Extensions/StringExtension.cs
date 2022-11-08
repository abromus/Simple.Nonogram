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
            var bytes = BitConverter.GetBytes((int)value);
            var i = BitConverter.ToInt32(bytes, 0);

            return i.ToString("X2");
        }

        public static string[] Split(string text, int countCharacters)
        {
            var count = (int)Math.Ceiling((float)text.Length / countCharacters);
            var index = 0;
            var strings = new string[count];

            for (int i = 0; i < strings.Length; i++)
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
            var tempArray = new float[array.Length];

            for (int i = 0; i < array.Length; i++)
                tempArray[i] = float.Parse(array[i], System.Globalization.NumberStyles.AllowHexSpecifier);

            return tempArray;
        }
    }
}

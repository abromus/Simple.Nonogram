namespace Simple.Nonogram.Core
{
    public static class ArrayExtension
    {
        public static T[,] Transpose<T>(T[,] array)
        {
            int width = array.GetLength(Constants.WidthDimension);
            int height = array.GetLength(Constants.HeightDimension);
            T[,] transposedArray = new T[height, width];

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    transposedArray[i, j] = array[j, i];

            return transposedArray;
        }
    }
}

namespace Simple.Nonogram.Game
{
    public readonly struct GenerationNonogramInfo
    {
        public readonly int Width;
        public readonly int Height;

        public GenerationNonogramInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}

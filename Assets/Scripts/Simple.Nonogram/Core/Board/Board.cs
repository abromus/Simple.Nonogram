namespace Simple.Nonogram.Core
{
    public abstract class Board
    {
        public Cell[,] Cells { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
    }
}

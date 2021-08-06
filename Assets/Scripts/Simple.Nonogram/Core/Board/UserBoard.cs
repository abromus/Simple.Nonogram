namespace Simple.Nonogram.Core
{
    public class UserBoard : Board
    {
        public UserBoard(AnswerBoard answerBoard)
        {
            Width = answerBoard.Width;
            Height = answerBoard.Height;

            Initialize();
        }

        private void Initialize()
        {
            Cells = new Cell[Height, Width];

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    Cells[i, j] = new Cell();
        }
    }
}

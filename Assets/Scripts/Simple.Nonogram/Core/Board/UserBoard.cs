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

            for (int i = (int)Number.Zero; i < Height; i++)
                for (int j = (int)Number.Zero; j < Width; j++)
                    Cells[i, j] = new Cell();
        }
    }
}

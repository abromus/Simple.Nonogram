namespace Simple.Nonogram.Core
{
    public enum CellState
    {
        Blank,      //Пустая
        Empty,      //Крест (X)
        Marked,     //Помечена
        Unknown,    //Знак вопроса
        Dotted,     //Временно помечена (точка)
        Solid,      //Временно помечена (горизонтальная черта)
        Dashed      //Временно помечена (тире)
    }
}

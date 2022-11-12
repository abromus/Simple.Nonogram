namespace Simple.Nonogram.Infrastructure.Delegates
{
    public delegate void Block();

    public delegate void FailBlock(long errorCode = -1, string message = null);

    public delegate void Block<in T>(T obj);

    public delegate void Block<in T1, in T2>(T1 t1, T2 t2);
}

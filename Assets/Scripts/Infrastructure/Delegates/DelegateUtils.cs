namespace Simple.Nonogram.Infrastructure.Delegates
{
    public delegate void Block();

    public delegate void FailBlock(long errorCode = -1, string message = null);

    public delegate void Block<in T>(T obj);

    public delegate void Block<in T1, in T2>(T1 t1, T2 t2);

    public static class DelegateUtils
    {
        public static void SafeInvoke(this Block block)
        {
            block?.Invoke();
        }

        public static void SafeInvoke(this FailBlock block)
        {
            block?.Invoke();
        }

        public static void SafeInvoke(this FailBlock block, long errorCode, string errorMessage)
        {
            block?.Invoke(errorCode, errorMessage);
        }

        public static void SafeInvoke<T>(this Block<T> block, T obj)
        {
            block?.Invoke(obj);
        }

        public static void SafeInvoke<T1, T2>(this Block<T1, T2> block, T1 t1, T2 t2)
        {
            block?.Invoke(t1, t2);
        }
    }
}

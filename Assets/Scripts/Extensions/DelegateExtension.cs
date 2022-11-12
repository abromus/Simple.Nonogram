using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Extensions
{

    public static class DelegateExtension
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
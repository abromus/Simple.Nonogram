using System;

namespace Simple.Nonogram.Extension
{
    public static class UniRxUtils
    {
        public static T SafeUnsubscribe<T>(this T subscribe) where T : class, IDisposable
        {
            subscribe?.Dispose();

            subscribe = null;

            return subscribe;
        }
    }
}

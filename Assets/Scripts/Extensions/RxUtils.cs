using System;
using UniRx;

namespace Simple.Nonogram.Extensions
{
    public static class RxUtils
    {
        public static void SafeUnsubscribe(ref IDisposable subscribe)
        {
            subscribe?.Dispose();

            subscribe = null;
        }

        public static void SafeUnsubscribe(ref CompositeDisposable subscribe)
        {
            SafeUnsubscribe(ref subscribe);
        }
    }
}

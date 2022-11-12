using System.Collections.Generic;

namespace Simple.Nonogram.Infrastructure.Services.Application.AppEvents
{
    public class AppEventPriorityComparer<T> : IComparer<T> where T : IAppEventPriority
    {
        public int Compare(T x, T y)
        {
            return x == null
                ? -1
                : y == null
                    ? 1
                    : x.Priority - y.Priority;
        }
    }
}

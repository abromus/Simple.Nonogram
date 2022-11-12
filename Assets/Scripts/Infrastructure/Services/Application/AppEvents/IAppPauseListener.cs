namespace Simple.Nonogram.Infrastructure.Services.Application.AppEvents
{
    public interface IAppPauseListener : IAppEventPriority
    {
        void OnAppPaused();
    }
}

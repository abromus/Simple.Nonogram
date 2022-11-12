namespace Simple.Nonogram.Infrastructure.Services.Application.AppEvents
{
    public interface IAppQuitListener : IAppEventPriority
    {
        void OnAppQuit();
    }
}

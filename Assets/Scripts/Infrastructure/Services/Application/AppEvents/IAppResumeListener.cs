namespace Simple.Nonogram.Infrastructure.Services.Application.AppEvents
{
    public interface IAppResumeListener : IAppEventPriority
    {
        void OnAppResumed();
    }
}

namespace Simple.Nonogram.Infrastructure.Services.Application.AppEvents
{
    public enum AppEventPriority
    {
        Chronometer = -10,
        Save = -9,
        Game = 0,
        LocalNotification,

        CoreDefaults,
        Ads,
        Analytics
    }
}

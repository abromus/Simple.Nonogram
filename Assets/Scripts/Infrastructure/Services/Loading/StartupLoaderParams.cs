namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public sealed class StartupLoaderParams : LoaderParams
    {
        public static readonly StartupLoaderParams Startup = new StartupLoaderParams();

        public override LoaderType Type => LoaderType.Startup;

        private StartupLoaderParams(bool isFirstLoad = false)
        {
            SetFirstLoad(isFirstLoad);
        }
    }
}

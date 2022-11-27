namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public interface ICompositionRoot
    {
        public string Tag { get; }

        public void AddService<T>(T service) where T : class, IService;
        public void AddConfiguration<T>(T configuration) where T : class, IConfiguration;
        public T GetService<T>() where T : class, IService;
        public T GetConfiguration<T>() where T : class, IConfiguration;
    }
}

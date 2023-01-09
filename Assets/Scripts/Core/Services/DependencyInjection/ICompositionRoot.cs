namespace Simple.Nonogram.Core.Services
{
    public interface ICompositionRoot
    {
        public string Tag { get; }

        public void Add<T>(T obj) where T : class, IService;
        public T Get<T>() where T : class, IService;
    }

}

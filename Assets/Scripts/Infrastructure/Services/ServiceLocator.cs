namespace Simple.Nonogram.Infrastructure.Services
{
    public class ServiceLocator
    {
        private static ServiceLocator _instance;

        public static ServiceLocator Contrainer => _instance ??= new ServiceLocator();

        public void Add<TService>(TService implementation) where TService : IService
        {
            Implementation<TService>.ServiceInstance = implementation;
        }

        public IService Get<TService>() where TService : IService
        {
            return Implementation<TService>.ServiceInstance;
        }

        private static class Implementation<TService> where TService : IService
        {
            public static TService ServiceInstance;
        }
    }
}
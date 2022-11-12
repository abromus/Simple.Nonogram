using System;

namespace Simple.Nonogram.Infrastructure.Services.Application
{
    public interface IGameProvider : IService
    {
        GameWorld World { get; }
        IObservable<Null> OnViewUpdate { get; }
    }
}

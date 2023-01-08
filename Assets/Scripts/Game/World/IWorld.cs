using Simple.Nonogram.Infrastructure.Services;

namespace Simple.Nonogram.Game
{
    public interface IWorld : IService
    {
        NonogramController NonogramController { get; }
    }
}

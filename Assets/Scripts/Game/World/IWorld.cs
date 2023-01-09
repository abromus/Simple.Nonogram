using Simple.Nonogram.Core.Services;

namespace Simple.Nonogram.Game
{
    public interface IWorld : IService
    {
        NonogramController NonogramController { get; }
    }
}

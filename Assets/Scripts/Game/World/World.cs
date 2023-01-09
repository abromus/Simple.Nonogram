using Simple.Nonogram.Core.Services;
using Simple.Nonogram.Settings;

namespace Simple.Nonogram.Game
{
    public class World : IWorld
    {
        private readonly ICompositionRoot _root;
        private readonly NonogramController _nonogramController;

        public NonogramController NonogramController => _nonogramController;

        public World(ICompositionRoot root)
        {
            _root = root;
            _root.Add<IWorld>(this);

            _nonogramController = new NonogramController(_root.Get<NonogramSettings>());
        }
    }
}

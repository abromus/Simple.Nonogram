using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Infrastructure.Services.StateMachine
{
    public class SceneInfo
    {
        public string Name;
        public Block OnSuccess;

        public SceneInfo(string name, Block onSuccess)
        {
            Name = name;
            OnSuccess = onSuccess;
        }
    }
}

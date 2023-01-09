namespace Simple.Nonogram.Core.Services
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

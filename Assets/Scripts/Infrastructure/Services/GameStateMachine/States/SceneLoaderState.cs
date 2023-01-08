namespace Simple.Nonogram.Infrastructure.Services
{
    public class SceneLoaderState : IEnterState<SceneInfo>
    {
        private readonly SceneLoader _sceneLoader;

        public SceneLoaderState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter(SceneInfo sceneInfo)
        {
            _sceneLoader.Load(sceneInfo.Name, sceneInfo.OnSuccess);
        }

        public void Exit() { }
    }
}

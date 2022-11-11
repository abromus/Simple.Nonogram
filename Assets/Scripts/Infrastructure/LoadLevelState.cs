namespace Simple.Nonogram.Infrastructure
{
    public class LoadLevelState : IEnterState<string>
    {
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Enter(string sceneName)
        {
            _sceneLoader.Load(sceneName);
        }

        public void Exit() { }
    }
}

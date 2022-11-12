using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public sealed class StandartLoadingScreen : LoadingScreen
    {
        public override LoaderType Type => LoaderType.Standard;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }

        public override void SetParameters(LoaderParams loaderParams) { }

        public override void Show(Block onComplete, bool withFade = true, int timeout = 0, Block timeoutBlock = null)
        {
            base.Show(onComplete, withFade, timeout, timeoutBlock);
        }

        public override void Hide(bool force = false)
        {
            base.Hide(force);
        }
    }
}

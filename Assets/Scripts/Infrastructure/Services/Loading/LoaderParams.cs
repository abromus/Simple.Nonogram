namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public abstract class LoaderParams
    {
        private bool _isFirstLoad = false;

        public abstract LoaderType Type { get; }

        public bool IsFirstLoad => _isFirstLoad;

        public void SetFirstLoad(bool isFirstLoad)
        {
            _isFirstLoad = isFirstLoad;
        }
    }
}

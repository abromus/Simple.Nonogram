using System;
using Simple.Nonogram.Infrastructure.Delegates;

namespace Simple.Nonogram.Infrastructure.Services.Loading
{
    public interface ILoadingController : IService
    {
        public void ShowLoader(LoaderParams loaderParams = null, Block onComplete = null, bool withFade = true, int timeout = 0, Block timeoutBlock = null);
        public void HideLoader(Type loaderType, bool force = false);
        public void HideLoader(bool force = false);
        public void RequestHideLoader(object requester);
        public void RequestHoldLoader(object requester);
    }
}

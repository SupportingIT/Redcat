using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Redcat.App.ViewModels;

namespace Redcat.App
{
    public class RedcatApp : MvxApplication
    {
        public override void Initialize()
        {
            var appStart = new MvxAppStart<HomeViewModel>();
            Mvx.RegisterSingleton<IMvxAppStart>(appStart);
        }
    }
}

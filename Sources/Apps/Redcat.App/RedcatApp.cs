using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
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

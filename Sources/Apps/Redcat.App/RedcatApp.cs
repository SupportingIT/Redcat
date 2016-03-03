using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Redcat.App.ViewModels;

namespace Redcat.App
{
    public class RedcatApp : MvxApplication
    {
        //private ICommunicator communicator;

        public override void Initialize()
        {
            var appStart = new MvxAppStart<MainMenuViewModel>();
            Mvx.RegisterSingleton<IMvxAppStart>(appStart);
        }
    }
}

using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Platform;
using System.Windows.Threading;
using Cirrious.MvvmCross.Wpf.Views;
using Cirrious.CrossCore;
using Redcat.App.Services;
using Redcat.App.Wpf.Views;
using Redcat.App.ViewModels;

namespace Redcat.App.Wpf
{
    public class Setup : MvxWpfSetup
    {
        public Setup(Dispatcher dispatcher, IMvxWpfViewPresenter presenter) : base(dispatcher, presenter)
        { }

        protected override IMvxApplication CreateApp()
        {
            return new RedcatApp();
        }

        public override void Initialize()
        {
            base.Initialize();
            Mvx.RegisterSingleton<IAccountService>(new AccountService());

            ProtocolInfoProvider protocolProvider = new ProtocolInfoProvider();
            protocolProvider.RegisterProtocol("XMPP", typeof(XmppSettingsViewModel), typeof(XmppSettingsViewModel));
            Mvx.RegisterSingleton<IProtocolInfoProvider>(protocolProvider);
        }
    }
}

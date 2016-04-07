using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Wpf.Platform;
using MvvmCross.Wpf.Views;
using Redcat.App.Services;
using Redcat.App.Wpf.Services;
using Redcat.Core.Channels;
using Redcat.Core.Net;
using Redcat.Xmpp;
using Redcat.Xmpp.Channels;
using Redcat.Xmpp.Negotiators;
using System;
using System.Windows.Threading;

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
            Mvx.RegisterType<IConnectionSettingsRepository, ConnectionSettingsRepository>();            
            Mvx.RegisterType<IXmppChannelFactory, XmppChannelFactory>();
            Mvx.RegisterType<IStreamChannelFactory, TcpChannelFactory>();
            Mvx.RegisterType<Func<ISaslCredentials>>(() => new SaslCredentialsProvider().GetCredentials);
            Mvx.RegisterSingleton(new XmppCommunicator(Mvx.Resolve<IXmppChannelFactory>()));
        }
    }
}

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Redcat.App.Services;
using Redcat.Xmpp;

namespace Redcat.App.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        public SettingsViewModel(IConnectionSettingsRepository repository)
        {
            Connection = new ConnectionSettingsViewModel(repository);
            Xmpp = new XmppCommunicatorViewModel(Mvx.Resolve<XmppCommunicator>(), repository);
        }        

        public ConnectionSettingsViewModel Connection { get; }

        public XmppCommunicatorViewModel Xmpp { get; }
    }
}

using Cirrious.MvvmCross.ViewModels;
using Redcat.Core;

namespace Redcat.App.ViewModels
{
    public class XmppSettingsViewModel : MvxViewModel
    {
        private ConnectionSettings settings;

        public void Init(ConnectionSettings settings)
        {
            this.settings = settings;
        }

        public ConnectionSettings Settings => (settings ?? (settings = new ConnectionSettings()));

        public string Username
        {
            get { return Settings.Username; }
            set
            {
                Settings.Username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        public string Domain
        {
            get { return Settings.Domain; }
            set
            {
                Settings.Domain = value;
                RaisePropertyChanged(() => Domain);
            }
        }

        public string Resource
        {
            get { return Settings.GetString("Resource"); }
            set
            {
                Settings.Set("Resource", value);
                RaisePropertyChanged(() => Resource);
            }
        }
    }
}

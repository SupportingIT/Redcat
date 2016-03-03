using MvvmCross.Core.ViewModels;
using Redcat.App.Services;
using Redcat.Core;

namespace Redcat.App.ViewModels
{
    public class ConnectionSettingsViewModel : MvxViewModel
    {
        private IConnectionSettingsRepository repository;
        private ConnectionSettings settings;

        public ConnectionSettingsViewModel(IConnectionSettingsRepository repository)
        {
            this.repository = repository;
            SaveCommand = new MvxCommand(Save);
        }

        private ConnectionSettings Settings
        {
            get
            {
                if (settings == null) settings = repository.Get();                
                return settings;
            }
        }

        public string Name
        {
            get { return Settings.Name; }
            set { Settings.Name = value; }
        }

        public string Domain
        {
            get { return Settings.Domain; }
            set { Settings.Domain = value; }
        }

        public string Host
        {
            get { return Settings.Host; }
            set { Settings.Host = value; }
        }

        public int Port
        {
            get { return Settings.Port; }
            set { Settings.Port = value; }
        }

        public IMvxCommand SaveCommand { get; }

        private void Save()
        {
            repository.Save(Settings);
        }
    }    
}

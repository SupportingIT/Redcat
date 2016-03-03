using MvvmCross.Core.ViewModels;
using Redcat.App.Services;

namespace Redcat.App.ViewModels
{
    public class SettingsViewModel : MvxViewModel
    {
        public SettingsViewModel(IConnectionSettingsRepository repository)
        {
            Connection = new ConnectionSettingsViewModel(repository);
        }        

        public ConnectionSettingsViewModel Connection { get; }
    }
}

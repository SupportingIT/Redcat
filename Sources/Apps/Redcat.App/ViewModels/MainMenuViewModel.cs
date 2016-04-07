using MvvmCross.Core.ViewModels;

namespace Redcat.App.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        public MainMenuViewModel()
        {
            HomeCommand = new MvxCommand(Home);
            SettingsCommand = new MvxCommand(Settings);
        }

        public IMvxCommand HomeCommand { get; }

        public IMvxCommand SettingsCommand { get; }

        private void Home()
        {
            ShowViewModel<HomeViewModel>();
        }

        private void Settings()
        {
            ShowViewModel<SettingsViewModel>();
        }
    }
}

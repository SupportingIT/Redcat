using Cirrious.MvvmCross.ViewModels;
using System;

namespace Redcat.App.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        public MainMenuViewModel()
        {
            SettingsCommand = new MvxCommand(Settings);
        }

        public IMvxCommand SettingsCommand { get; }

        private void Settings()
        {
            throw new NotImplementedException();
        }
    }
}

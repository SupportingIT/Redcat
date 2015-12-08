using Cirrious.MvvmCross.ViewModels;

namespace Redcat.App.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        public MainMenuViewModel()
        {
            HomeCommand = new MvxCommand(Home);
            ManageAccountsCommand = new MvxCommand(ManageAccounts);
        }

        public IMvxCommand HomeCommand { get; }

        public IMvxCommand ManageAccountsCommand { get; }

        private void Home()
        {            
        }

        private void ManageAccounts()
        {            
        }
    }
}

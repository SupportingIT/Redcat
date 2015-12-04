using Prism.Commands;
using Prism.Regions;
using System.Windows.Input;

namespace Redcat.Communicator.ViewModels
{
    public class MainMenuViewModel
    {
        private IRegionManager regionManager;

        public MainMenuViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            HomeCommand = new DelegateCommand(Home);
            ManageAccountsCommand = new DelegateCommand(ManageAccounts);
        }

        public ICommand HomeCommand { get; }

        public ICommand ManageAccountsCommand { get; }

        private void Home()
        {
            regionManager.RequestNavigate(RegionNames.MainContent, ViewNames.Home);
        }

        private void ManageAccounts()
        {
            regionManager.RequestNavigate(RegionNames.MainContent, ViewNames.AccountList);
        }
    }
}

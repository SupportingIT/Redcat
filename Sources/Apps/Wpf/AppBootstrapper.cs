using System.Windows;
using Prism.Unity;
using Redcat.Communicator.Views;
using Prism.Regions;

namespace Redcat.Communicator
{
    public class AppBootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return new ShellView();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            RegisterRegions();
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        private void RegisterRegions()
        {
            IRegionManager rm = (IRegionManager)Container.Resolve(typeof(IRegionManager), null);
            rm.AddToRegion(RegionNames.MainMenu, new MainMenuView());
            rm.AddToRegion(RegionNames.StatusBar, new StatusBarView());
        }
    }
}

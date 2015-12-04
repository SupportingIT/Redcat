using System.Windows;
using Prism.Unity;
using Redcat.Communicator.Views;
using Prism.Regions;
using Prism.Modularity;
using System.Collections.Generic;
using Redcat.Core.Communication;
using Microsoft.Practices.Unity;
using Redcat.Core.Net;
using Redcat.Communicator.Services;

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
            rm.RegisterViewWithRegion(RegionNames.MainMenu, typeof(MainMenuView));

            rm.RegisterViewWithRegion(RegionNames.MainContent, typeof(HomeView));
            rm.RegisterViewWithRegion(RegionNames.MainContent, typeof(AccountListView));
            rm.RegisterViewWithRegion(RegionNames.MainContent, typeof(NewAccountView));

            rm.RegisterViewWithRegion(RegionNames.StatusBar, typeof(StatusBarView));            
        }

        protected override void ConfigureModuleCatalog()
        {            
            ModuleCatalog.AddModule(new ModuleInfo("xmpp", typeof(XmppModule).AssemblyQualifiedName));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.RegisterInstance<ICollection<IChannelFactory>>(new List<IChannelFactory>());
            Container.RegisterInstance<IChannelFactory<IStreamChannel>>(new TcpChannelFactory());
            Container.RegisterInstance<IAccountService>(new AccountService());
        }
    }
}

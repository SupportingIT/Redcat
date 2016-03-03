using System.Windows;
using System;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Wpf.Views;
using Redcat.App.ViewModels;

namespace Redcat.App.Wpf
{
    public partial class App : Application
    {
        private bool isInitialized = false;

        protected override void OnActivated(EventArgs e)
        {
            if (!isInitialized) Initialize();
            base.OnActivated(e);
        }

        private void Initialize()
        {
            IMvxWpfViewPresenter presenter = CreatePresenter();
            Setup setup = new Setup(Dispatcher, presenter);
            setup.Initialize();
                        
            IMvxAppStart start = Mvx.Resolve<IMvxAppStart>();
            start.Start();
            
            presenter.Show(new MvxViewModelRequest<MainMenuViewModel>(null, null, null));

            isInitialized = true;
        }

        private IMvxWpfViewPresenter CreatePresenter()
        {
            ContentControl mainContent = (ContentControl)MainWindow.FindName("MainContent");
            WindowCommands mainMenu = new WindowCommands();
            ((MainWindow)MainWindow).RightWindowCommands = mainMenu;
            var presenter = new RedcatWpfViewPresenter(Dispatcher, mainContent, mainMenu);
            return presenter;
        }
    }
}

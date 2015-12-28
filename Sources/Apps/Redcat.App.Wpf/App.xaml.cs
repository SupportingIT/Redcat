using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;
using System.Windows;
using System;
using Redcat.App.ViewModels;
using System.Windows.Controls;
using Redcat.App.Wpf.Views;

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
            InitializeMainMenu();
                        
            IMvxAppStart start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            isInitialized = true;
        }

        private IMvxWpfViewPresenter CreatePresenter()
        {
            ContentControl mainContent = (ContentControl)MainWindow.FindName("MainContent");
            Window dialogWindow = Resources["DialogWindow"] as Window;
            var presenter = new RedcatWpfViewPresenter(Dispatcher, mainContent, dialogWindow);
            presenter.AddDialogView<AccountListView>();
            presenter.AddDialogView<NewAccountView>();            

            return presenter;
        }

        private void InitializeMainMenu()
        {
            ContentControl mainMenu = (ContentControl)MainWindow.FindName("MainMenu");
            IMvxSimpleWpfViewLoader loader = Mvx.Resolve<IMvxSimpleWpfViewLoader>();
            var mainMenuView = loader.CreateView(new MvxViewModelRequest { ViewModelType = typeof(MainMenuViewModel) });
            mainMenu.Content = mainMenuView;
        }
    }
}

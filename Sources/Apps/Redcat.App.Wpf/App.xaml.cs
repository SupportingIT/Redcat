using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;
using System.Windows;

namespace Redcat.App.Wpf
{
    public partial class App : Application
    {
        private bool isInitialized = false;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (!isInitialized) Initialize();
        }

        private void Initialize()
        {
            IMvxWpfViewPresenter presenter = new MvxSimpleWpfViewPresenter(MainWindow);
            Setup setup = new Setup(Dispatcher, presenter);
            setup.Initialize();

            IMvxAppStart start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            isInitialized = true;
        }
    }
}

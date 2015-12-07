using Cirrious.MvvmCross.Wpf.Platform;
using Cirrious.MvvmCross.Wpf.Views;
using System.Windows.Threading;
using Cirrious.MvvmCross.ViewModels;
using Redcat.App;

namespace Redcat.Communicator
{
    public class Setup : MvxWpfSetup
    {
        public Setup(Dispatcher dispatcher, IMvxWpfViewPresenter presenter) : base(dispatcher, presenter)
        { }

        protected override IMvxApplication CreateApp()
        {
            return new RedcatApp();
        }
    }
}

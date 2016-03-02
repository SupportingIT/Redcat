using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Platform;
using System.Windows.Threading;
using Cirrious.MvvmCross.Wpf.Views;
using Cirrious.CrossCore;

namespace Redcat.App.Wpf
{
    public class Setup : MvxWpfSetup
    {
        public Setup(Dispatcher dispatcher, IMvxWpfViewPresenter presenter) : base(dispatcher, presenter)
        { }

        protected override IMvxApplication CreateApp()
        {
            return new RedcatApp();
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}

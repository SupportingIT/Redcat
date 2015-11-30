using Prism;
using System.Windows;

namespace Redcat.Communicator
{    
    public partial class App : Application
    {
        private Bootstrapper bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bootstrapper = new AppBootstrapper();
            bootstrapper.Run();
        }
    }
}

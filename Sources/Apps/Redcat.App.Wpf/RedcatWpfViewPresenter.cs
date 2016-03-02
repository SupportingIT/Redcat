using Cirrious.MvvmCross.Wpf.Views;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Redcat.App.Wpf.Views;

namespace Redcat.App.Wpf
{
    public class RedcatWpfViewPresenter : MvxWpfViewPresenter
    {
        private ContentControl mainContent;
        private ItemsControl mainMenu;
        private Dispatcher dispatcher;

        public RedcatWpfViewPresenter(Dispatcher dispatcher, ContentControl mainContent, ItemsControl mainMenu)
        {
            this.dispatcher = dispatcher;
            this.mainContent = mainContent;
            this.mainMenu = mainMenu;
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            if (frameworkElement is MainMenuView)
            {
                mainMenu.Items.Add(frameworkElement);
                frameworkElement.Visibility = Visibility.Visible;                
                return;
            }
            mainContent.Content = frameworkElement;
        }        
    }
}

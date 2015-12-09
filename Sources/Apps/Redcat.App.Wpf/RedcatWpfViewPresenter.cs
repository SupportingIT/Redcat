using Cirrious.MvvmCross.Wpf.Views;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using System.Windows.Threading;

namespace Redcat.App.Wpf
{
    public class RedcatWpfViewPresenter : MvxWpfViewPresenter
    {
        private ContentControl mainContent;
        private ICollection<Type> dialogViews;
        private Dispatcher dispatcher;
        private Window dialogWindow;

        public RedcatWpfViewPresenter(Dispatcher dispatcher, ContentControl mainContent, Window dialogWindow)
        {
            this.mainContent = mainContent;
            this.dialogWindow = dialogWindow;
            dialogViews = new List<Type>();
            this.dispatcher = dispatcher;
        }        

        public void AddDialogView<T>() where T : IMvxView
        {
            dialogViews.Add(typeof(T));
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            if (dialogViews.Contains(frameworkElement.GetType()))
            {                
                dispatcher.InvokeAsync(() => {
                    dialogWindow.Content = frameworkElement;
                    if (!dialogWindow.IsVisible) dialogWindow.ShowDialog();
                });
                
                return;
            }

            mainContent.Content = frameworkElement;
        }
    }
}

using Cirrious.MvvmCross.Wpf.Views;
using System.Windows.Controls;
using System;
using System.Windows;
using System.Collections.Generic;
using Cirrious.MvvmCross.Views;
using System.Windows.Threading;
using Cirrious.MvvmCross.ViewModels;

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
            this.dispatcher = dispatcher;
            this.mainContent = mainContent;
            this.dialogWindow = dialogWindow;
            dialogViews = new List<Type>();            
        }        

        public void AddDialogView<T>() where T : IMvxView
        {
            dialogViews.Add(typeof(T));
        }

        public override void Present(FrameworkElement frameworkElement)
        {
            if (IsDialogView(frameworkElement))
            {
                ShowDialogView(frameworkElement);                
                return;
            }

            mainContent.Content = frameworkElement;
        }

        private bool IsDialogView(FrameworkElement view)
        {
            return dialogViews.Contains(view.GetType());
        }

        private void ShowDialogView(FrameworkElement view)
        {
            dispatcher.InvokeAsync(() => {
                dialogWindow.Content = view;
                if (!dialogWindow.IsVisible) dialogWindow.ShowDialog();
                return;
            });
        }        

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            base.ChangePresentation(hint);
        }

        private void ShowProtocolSettingsView(FrameworkElement view)
        {
        }
    }
}

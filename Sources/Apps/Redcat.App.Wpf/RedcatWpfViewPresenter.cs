using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using Redcat.App.Wpf.Views;
using MvvmCross.Wpf.Views;
using System.Collections.Generic;
using System;
using MvvmCross.Core.ViewModels;

namespace Redcat.App.Wpf
{
    public class RedcatWpfViewPresenter : MvxWpfViewPresenter
    {
        private ContentControl mainContent;
        private ItemsControl mainMenu;
        private Window dialogWindow;
        private Dispatcher dispatcher;
        private List<Type> dialogViews;

        public RedcatWpfViewPresenter(Dispatcher dispatcher, ContentControl mainContent, ItemsControl mainMenu, Window dialogWindow)
        {
            dialogViews = new List<Type>();
            this.dialogWindow = dialogWindow;
            this.dispatcher = dispatcher;
            this.mainContent = mainContent;
            this.mainMenu = mainMenu;
        }

        public void AddDialogView<T>()
        {
            dialogViews.Add(typeof(T));
        }

        public override void Present(FrameworkElement view)
        {
            if (IsDialogView(view))
            {
                dialogWindow.Content = view;
                dialogWindow.ShowDialog();
                return;
            }
            if (view is MainMenuView)
            {
                mainMenu.Items.Add(view);
                view.Visibility = Visibility.Visible;                
                return;
            }
            mainContent.Content = view;
        }

        private bool IsDialogView(FrameworkElement view)
        {
            return dialogViews.Contains(view.GetType());
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                dialogWindow.Close();
            }
            base.ChangePresentation(hint);
        }
    }
}

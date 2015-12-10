using Cirrious.MvvmCross.Wpf.Views;
using System.Windows;

namespace Redcat.App.Wpf.Views
{
    public partial class AccountListView : MvxWpfView
    {
        public AccountListView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Parent is DialogWindow) ((DialogWindow)Parent).Hide();
        }
    }
}

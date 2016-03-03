using MahApps.Metro.Controls;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;

namespace Redcat.App.Wpf
{
    public partial class MainWindow : MetroWindow, IMvxView
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public IMvxViewModel ViewModel
        {
            get; set;
        }
    }
}

using Cirrious.MvvmCross.Views;
using MahApps.Metro.Controls;
using System.Windows;
using Cirrious.MvvmCross.ViewModels;
using System;

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

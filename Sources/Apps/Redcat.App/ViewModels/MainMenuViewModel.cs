using Cirrious.MvvmCross.ViewModels;
using System;

namespace Redcat.App.ViewModels
{
    public class MainMenuViewModel : MvxViewModel
    {
        public MainMenuViewModel()
        {
            HomeCommand = new MvxCommand(Home);
        }

        public IMvxCommand HomeCommand { get; }         

        private void Home()
        {
            ShowViewModel<HomeViewModel>();
        }
    }
}

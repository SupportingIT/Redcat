﻿using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Wpf.Views;
using System.Windows;
using System;
using Redcat.App.ViewModels;
using System.Windows.Controls;

namespace Redcat.App.Wpf
{
    public partial class App : Application
    {
        private bool isInitialized = false;

        protected override void OnActivated(EventArgs e)
        {
            if (!isInitialized) Initialize();
            base.OnActivated(e);
        }

        private void Initialize()
        {
            
            ContentControl mainContent = (ContentControl)MainWindow.FindName("MainContent");

            IMvxWpfViewPresenter presenter = new MvxSimpleWpfViewPresenter(mainContent);
            Setup setup = new Setup(Dispatcher, presenter);
            setup.Initialize();
            InitializeMainMenu();
                        
            IMvxAppStart start = Mvx.Resolve<IMvxAppStart>();
            start.Start();

            isInitialized = true;
        }

        private void InitializeMainMenu()
        {
            ContentControl mainMenu = (ContentControl)MainWindow.FindName("MainMenu");
            IMvxSimpleWpfViewLoader loader = Mvx.Resolve<IMvxSimpleWpfViewLoader>();
            var mainMenuView = loader.CreateView(new MvxViewModelRequest { ViewModelType = typeof(MainMenuViewModel) });
            mainMenu.Content = mainMenuView;
        }
    }
}
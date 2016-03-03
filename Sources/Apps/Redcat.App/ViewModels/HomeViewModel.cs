using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public HomeViewModel()
        {
            RootViews = new[] { new Tuple<string, Type>("XMPP", typeof(XmppCommunicatorViewModel)) };
            ShowViewCommand = new MvxCommand(ShowView);
        }

        public IEnumerable<Tuple<string, Type>> RootViews { get; }

        public Tuple<string, Type> SelectedView { get; set; }

        public IMvxCommand ShowViewCommand { get; }

        private void ShowView()
        {
            ShowViewModel<XmppCommunicatorViewModel>();
        }
    }    
}

using MvvmCross.Core.ViewModels;
using Redcat.App.Models;
using System;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class HomeViewModel : MvxViewModel
    {
        public HomeViewModel()
        {
            ProtocolModules = new List<ProtocolModule> { new ProtocolModule("XMPP", typeof(XmppCommunicatorViewModel)) };
            ShowProtocolModuleCommand = new MvxCommand<ProtocolModule>(ShowProtocolModule);
        }        

        public IEnumerable<ProtocolModule> ProtocolModules { get; }

        public IMvxCommand ShowProtocolModuleCommand { get; }

        private void ShowProtocolModule(ProtocolModule module)
        {
            ShowViewModel(module.HomeViewModel);
        }
    }    
}

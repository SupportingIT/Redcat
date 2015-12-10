using Cirrious.MvvmCross.ViewModels;
using Redcat.App.Services;
using Redcat.Core;
using System;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class NewAccountViewModel : MvxViewModel
    {
        private IAccountService accountService;
        private IProtocolInfoProvider protocolInfoProvider;
        private IEnumerable<string> protocols;
        private string selectedProtocol;
        private ConnectionSettings settings;

        public NewAccountViewModel(IAccountService accountService, IProtocolInfoProvider protocolInfoProvider)
        {
            this.accountService = accountService;
            this.protocolInfoProvider = protocolInfoProvider;
            CreateAccountCommand = new MvxCommand(CreateAccount);
        }

        public string AccountName { get; set; }

        public IEnumerable<string> Protocols => (protocols ?? (protocols = protocolInfoProvider.GetProtocolsName()));

        public string SelectedProtocol
        {
            get { return selectedProtocol; }
            set
            {
                selectedProtocol = value;
                Type viewModel = protocolInfoProvider.GetViewModelTypeForNewSettings(selectedProtocol);                
                ShowViewModel(viewModel, ConnectionSettings);
            }
        }

        public ConnectionSettings ConnectionSettings => (settings ?? (settings = new ConnectionSettings()));

        public IMvxCommand CreateAccountCommand { get; }

        private void CreateAccount()
        {
            throw new NotImplementedException();
        }
    }
}

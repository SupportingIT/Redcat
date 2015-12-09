using Cirrious.MvvmCross.ViewModels;
using Redcat.App.Services;
using System;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class NewAccountViewModel : MvxViewModel
    {
        private IAccountService accountService;
        private IProtocolInfoProvider protocolInfoProvider;
        private IEnumerable<string> protocols;

        public NewAccountViewModel(IAccountService accountService, IProtocolInfoProvider protocolInfoProvider)
        {
            this.accountService = accountService;
            this.protocolInfoProvider = protocolInfoProvider;
            CreateAccountCommand = new MvxCommand(CreateAccount);
        }

        public string AccountName { get; set; }

        public IEnumerable<string> Protocols => (protocols ?? (protocols = protocolInfoProvider.GetProtocolsName()));

        public string SelectedProtocol { get; set; }

        public IMvxCommand CreateAccountCommand { get; }

        private void CreateAccount()
        {
            throw new NotImplementedException();
        }
    }
}

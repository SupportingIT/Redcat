using Cirrious.MvvmCross.ViewModels;
using Redcat.App.Models;
using Redcat.App.Services;
using System.Collections.Generic;

namespace Redcat.App.ViewModels
{
    public class NewAccountViewModel : MvxViewModel
    {
        private IAccountService accountService;
        private IProtocolService protocolService;
        private IEnumerable<string> protocols;
        private string selectedProtocol;

        public NewAccountViewModel(IAccountService accountService, IProtocolService protocolService)
        {
            this.accountService = accountService;
            this.protocolService = protocolService;
            CreateAccountCommand = new MvxCommand(CreateAccount);
            CloseCommand = new MvxCommand(Close);
        }

        public string AccountName { get; set; }

        public IEnumerable<string> Protocols => (protocols ?? (protocols = protocolService.GetProtocolsName()));

        public string SelectedProtocol
        {
            get { return selectedProtocol; }
            set
            {
                selectedProtocol = value;
                ConnectionSettings = protocolService.GetViewModelForNewSettings(selectedProtocol);
                MvxPresentationHint hint = new ShowProtocolSettingsHint(ConnectionSettings);
                ChangePresentation(hint);
            }
        }

        public ProtocolSettingsViewModel ConnectionSettings { get; private set; }

        public IMvxCommand CreateAccountCommand { get; }

        public IMvxCommand CloseCommand { get; }

        private void CreateAccount()
        {
            Account account = new Account
            {
                Name = AccountName,
                Protocol = SelectedProtocol                
            };
            accountService.AddAccount(account);
            Close();
        }

        private void Close()
        {
            ChangePresentation(new MvxClosePresentationHint(this));
        }
    }
}

using Prism.Commands;
using Redcat.Communicator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Redcat.Communicator.ViewModels
{
    public class NewAccountViewModel
    {
        private IAccountService accountService;

        public NewAccountViewModel(IAccountService accountService)
        {
            this.accountService = accountService;            
            CreateAccountCommand = new DelegateCommand(CreateAccount);
        }

        public string AccountName { get; set; }

        public IEnumerable<string> Protocols { get; } = Enumerable.Range(0, 10).Select(i => "Protocol " + i);

        public string SelectedProtocol { get; set; }

        public ICommand CreateAccountCommand { get; }

        private void CreateAccount()
        {
            throw new NotImplementedException();
        }
    }
}

using Cirrious.MvvmCross.ViewModels;
using Redcat.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.App.ViewModels
{
    public class NewAccountViewModel : MvxViewModel
    {
        private IAccountService accountService;

        public NewAccountViewModel(IAccountService accountService)
        {
            this.accountService = accountService;            
            
        }

        public string AccountName { get; set; }

        public IEnumerable<string> Protocols { get; } = Enumerable.Range(0, 10).Select(i => "Protocol " + i);

        public string SelectedProtocol { get; set; }

        public IMvxCommand CreateAccountCommand { get; }

        private void CreateAccount()
        {
            throw new NotImplementedException();
        }
    }
}

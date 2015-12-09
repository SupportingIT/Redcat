using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Redcat.App.Models;
using Cirrious.MvvmCross.ViewModels;
using Redcat.App.Services;

namespace Redcat.App.ViewModels
{
    public class AccountListViewModel : MvxViewModel
    {
        private IAccountService accountService;
        private ICollection<Account> accounts;

        public AccountListViewModel(IAccountService accountService)
        {
            this.accountService = accountService;
            NewAccountCommand = new MvxCommand(NewAccount);
            EditAccountCommand = new MvxCommand<Account>(EditAccount);
            DeleteAccountCommand = new MvxCommand<Account>(DeleteAccount);
        }

        public ICollection<Account> Accounts => (accounts ?? (accounts = new ObservableCollection<Account>(accountService.GetAccounts())));

        public IMvxCommand NewAccountCommand { get; }

        public IMvxCommand EditAccountCommand { get; }

        public IMvxCommand DeleteAccountCommand { get; }

        private void NewAccount()
        {
            ShowViewModel<NewAccountViewModel>();
        }

        public void EditAccount(Account account)
        {
            throw new NotImplementedException();
        }

        private void DeleteAccount(Account account)
        {
            Accounts.Remove(account);
        }
    }
}

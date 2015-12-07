using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Redcat.App.Services;
using Redcat.App.Models;
using Cirrious.MvvmCross.ViewModels;

namespace Redcat.App.ViewModels
{
    public class AccountListViewModel : MvxViewModel
    {
        public AccountListViewModel()
        {            
            Accounts = new ObservableCollection<Account>();            
        }

        public ICollection<Account> Accounts { get; }

        public object NewAccountCommand { get; }

        public object EditAccountCommand { get; }

        public object DeleteAccountCommand { get; }

        public object CloseCommand { get; }        

        private void NewAccount()
        {            
        }

        public void EditAccount(Account account)
        {
            throw new NotImplementedException();
        }

        private void DeleteAccount(Account account)
        {
            Accounts.Remove(account);
        }

        private void Close()
        {
            
        }
    }
}

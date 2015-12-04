using System;
using System.Windows.Input;
using Prism.Commands;
using System.Collections.Generic;
using Redcat.Communicator.Models;
using System.Collections.ObjectModel;
using Prism.Regions;
using Redcat.Communicator.Services;

namespace Redcat.Communicator.ViewModels
{
    public class AccountListViewModel
    {
        private IRegionManager regionManager;

        public AccountListViewModel(IRegionManager regionManager, IAccountService accountService)
        {
            NewAccountCommand = new DelegateCommand(NewAccount);
            EditAccountCommand = new DelegateCommand<Account>(EditAccount);
            DeleteAccountCommand = new DelegateCommand<Account>(DeleteAccount);
            CloseCommand = new DelegateCommand(Close);
            Accounts = new ObservableCollection<Account>(accountService.GetAccounts());
            this.regionManager = regionManager;
        }

        public ICollection<Account> Accounts { get; }

        public ICommand NewAccountCommand { get; }

        public ICommand EditAccountCommand { get; }

        public ICommand DeleteAccountCommand { get; }

        public ICommand CloseCommand { get; }        

        private void NewAccount()
        {
            regionManager.RequestNavigate(RegionNames.MainContent, ViewNames.NewAccount);
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

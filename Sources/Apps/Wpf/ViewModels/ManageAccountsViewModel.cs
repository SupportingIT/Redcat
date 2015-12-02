using System;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Input;
using Prism.Commands;
using System.Collections.Generic;
using Redcat.Communicator.Models;
using System.Collections.ObjectModel;

namespace Redcat.Communicator.ViewModels
{
    public class ManageAccountsViewModel : IInteractionRequestAware
    {
        public ManageAccountsViewModel()
        {
            NewAccountCommand = new DelegateCommand(NewAccount);
            EditAccountCommand = new DelegateCommand<Account>(EditAccount);
            DeleteAccountCommand = new DelegateCommand<Account>(DeleteAccount);
            CloseCommand = new DelegateCommand(Close);
            Accounts = new ObservableCollection<Account>();
        }

        public ICollection<Account> Accounts { get; }

        public ICommand NewAccountCommand { get; }

        public ICommand EditAccountCommand { get; }

        public ICommand DeleteAccountCommand { get; }

        public ICommand CloseCommand { get; }

        public Action FinishInteraction { get; set; }

        public INotification Notification { get; set; }

        private void NewAccount()
        {
            Accounts.Add(new Account() { Name = "account", Protocol = "Proto" });
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
            FinishInteraction();
        }
    }
}

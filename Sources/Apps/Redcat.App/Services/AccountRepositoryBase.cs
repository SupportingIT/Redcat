using Redcat.App.Models;
using System.Collections.Generic;
using System;

namespace Redcat.App.Services
{
    public abstract class AccountRepositoryBase : IAccountRepository
    {
        private ICollection<Account> accounts;

        protected ICollection<Account> Accounts
        {
            get
            {
                if (accounts == null)
                {
                    var accList = RetreiveAccounts();
                    if (accList == null) accounts = new List<Account>();
                    accounts = new List<Account>(accList);
                }
                return accounts;
            }
        }

        public void Add(Account account)
        {
            Accounts.Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return Accounts;
        }

        public void Remove(Account account)
        {
            Accounts.Remove(account);
        }

        protected abstract IEnumerable<Account> RetreiveAccounts();

        protected abstract void StoreAccounts(IEnumerable<Account> accounts);
    }
}

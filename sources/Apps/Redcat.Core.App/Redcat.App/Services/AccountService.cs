using Redcat.App.Models;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public class AccountService : IAccountService
    {
        private List<Account> accounts = new List<Account>();

        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return accounts;
        }
    }
}

using System.Collections.Generic;
using Redcat.Communicator.Models;

namespace Redcat.Communicator.Services
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

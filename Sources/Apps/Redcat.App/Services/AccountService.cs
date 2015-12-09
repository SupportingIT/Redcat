using Redcat.App.Models;
using System.Collections.Generic;
using System.Linq;

namespace Redcat.App.Services
{
    public class AccountService : IAccountService
    {
        private List<Account> accounts = new List<Account>(Enumerable.Range(0, 3).Select(i => new Account { Name = $"Account {i}", Protocol = $"Protocol {i}" }));

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

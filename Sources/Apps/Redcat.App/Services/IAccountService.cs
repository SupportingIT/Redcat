using Redcat.App.Models;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts();
        void AddAccount(Account account);
        void DeleteAccount(Account account);
        void UpdateAccount(Account account);
    }
}

using Redcat.App.Models;
using System.Collections.Generic;

namespace Redcat.App.Services
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
        void Add(Account account);
        void Remove(Account account);
    }
}

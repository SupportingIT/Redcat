using Redcat.Communicator.Models;
using System.Collections.Generic;

namespace Redcat.Communicator.Services
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts();
        void AddAccount(Account account);
    }
}

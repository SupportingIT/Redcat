using System;
using System.Collections.Generic;
using Redcat.App.Models;
using Redcat.App.Services;
using Newtonsoft.Json;
using System.IO;

namespace Redcat.App.Wpf
{
    public class AccountStorage : AccountRepositoryBase
    {
        private string fileName;

        public AccountStorage(string fileName)
        {
            this.fileName = fileName;
        }

        protected override IEnumerable<Account> RetreiveAccounts()
        {
            return JsonConvert.DeserializeObject<IEnumerable<Account>>(File.ReadAllText(fileName));
        }

        protected override void StoreAccounts(IEnumerable<Account> accounts)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(accounts));
        }
    }
}

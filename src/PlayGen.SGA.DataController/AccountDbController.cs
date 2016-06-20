using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PlayGen.SGA.Contracts.Controllers;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.DataAccess;
using PlayGen.SGA.DataController.Exceptions;

namespace PlayGen.SGA.DataController
{
    public class AccountDbController : DbController
    {
        public AccountDbController(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public Account Create(Account account)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                context.Accounts.Add(account);
                SaveChanges(context);

                return account;
            }
        }

        public IEnumerable<Account> Get(string[] names)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var accounts = context.Accounts
                    .Where(a => names.Contains(a.Name))
                    .Include(a => a.User);

                return accounts.ToList();
            }
        }

        public void Delete(int[] id)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var accounts = context.Accounts.Where(g => id.Contains(g.Id));
                context.Accounts.RemoveRange(accounts);
                SaveChanges(context);
            }
        }
    }
}

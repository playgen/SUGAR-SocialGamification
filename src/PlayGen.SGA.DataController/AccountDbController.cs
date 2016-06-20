using System;
using System.Collections.Generic;
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

                var hasConflicts = context.Accounts.Any(a => a.Name == account.Name);

                if (hasConflicts)
                {
                    throw new DuplicateRecordException($"An account with the name {account.Name} already exists.");
                }
                
                context.Accounts.Add(account);
                context.SaveChanges();

                return account;
            }
        }

        public IEnumerable<Account> Get(string[] names)
        {
            using (var context = new SGAContext(NameOrConnectionString))
            {
                SetLog(context);

                var accounts = context.Accounts.Where(a => names.Contains(a.Name));

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
                context.SaveChanges();
            }
        }
    }
}

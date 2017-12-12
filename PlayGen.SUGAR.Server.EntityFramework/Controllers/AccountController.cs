using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class AccountController : DbController
	{
		public AccountController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public Account Create(Account account)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedActor(account.User);

				context.Accounts.Add(account);
				SaveChanges(context);

				return account;
			}
		}

		public List<Account> Get(string[] names, int sourceId)
		{
			using (var context = ContextFactory.Create())
			{
				var accounts = context.Accounts
					.Where(a => names.Contains(a.Name) && a.AccountSourceId == sourceId)
					.Include(a => a.User);

				return accounts.ToList();
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var account = context.Accounts.Find(id);
				if (account == null)
				{
					throw new MissingRecordException($"No Account exists with Id: {id}");
				}
				context.Accounts.Remove(account);
				SaveChanges(context);
			}
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class AccountSourceController : DbController
	{
		public AccountSourceController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<AccountSource> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var sources = context.AccountSources.ToList();
				return sources;
			}
		}

		public AccountSource Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var source = context.AccountSources.Find(id);
				return source;
			}
		}

		public AccountSource Get(string token)
		{
			using (var context = ContextFactory.Create())
			{
				var source = context.AccountSources.SingleOrDefault(s => s.Token == token);
				return source;
			}
		}

		public AccountSource Create(AccountSource source)
		{
			using (var context = ContextFactory.Create())
			{
				context.AccountSources.Add(source);
				SaveChanges(context);

				return source;
			}
		}

		public void Update(AccountSource source)
		{
			using (var context = ContextFactory.Create())
			{
				context.AccountSources.Update(source);
				SaveChanges(context);
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				// todo should we not also be deleting all data associated with this game?
				var source = context.AccountSources.Find(id);
				if (source == null)
				{
					throw new MissingRecordException($"No AccountSource exists with Id: {id}");
				}
				context.AccountSources.Remove(source);
				SaveChanges(context);
			}
		}
	}
}

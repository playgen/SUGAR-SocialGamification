using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;
using Remotion.Linq.Utilities;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class UserController : DbController
	{
		public UserController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<User> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var users = context.Users
					.IncludeAll()
					.ToList();

				return users;
			}
		}

		public List<User> Search(string name, bool exactMatch = false)
		{
			using (var context = ContextFactory.Create())
			{
				List<User> users;

				if (!exactMatch)
				{
					users = context.Users
						.IncludeAll()
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();
				}
				else
				{
					users = context.Users
						.IncludeAll()
						.Where(g => g.Name == name).ToList();
				}

				return users;
			}
		}

		public User Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var user = context.Users
					.IncludeAll()
					.FirstOrDefault(u => u.Id == id);

				return user;
			}
		}

		public User Create(User user, SUGARContext context = null)
		{
			var didCreate = false;
			if (context == null)
			{
				context = ContextFactory.Create();
				didCreate = true;
			}
			
			if (context.Users.Any(g => g.Name == user.Name))
			{
				throw new DuplicateRecordException($"A user with the name: \"{user.Name}\" already exists.");
			}

			context.Users.Add(user);

			if (didCreate)
			{
				context.SaveChanges();
				context.Dispose();
			}

			return user;
		}

		public User Update(User user)
		{
			using (var context = ContextFactory.Create())
			{
				context.Users.Update(user);
				context.SaveChanges();
				return user;
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var user = context.Users.Find(id);
				if (user == null)
				{
					throw new MissingRecordException($"No User exists with Id: {id}");
				}
				context.Users.Remove(user);
				context.SaveChanges();
			}
		}
	}
}

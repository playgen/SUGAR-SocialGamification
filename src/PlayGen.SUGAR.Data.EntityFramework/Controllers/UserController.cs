using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class UserController : DbController
	{
		public UserController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public IEnumerable<User> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var users = context.Users.ToList();

				return users;
			}
		}

		public IEnumerable<User> Search(string name, bool exactMatch = false)
		{
			using (var context = ContextFactory.Create())
			{
				IEnumerable<User> users;

				if (!exactMatch)
				{
					users = context.Users
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();
				}
				else
				{
					users = context.Users
						.Where(g => g.Name == name).ToList();
				}
				
				return users;
			}
		}

		public User Search(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var user = context.Users.Find(id);

				return user;
			}
		}

		public void Create(User user)
		{
			using (var context = ContextFactory.Create())
			{
				if (context.Users.Any(g => g.Name == user.Name))
				{
					throw new DuplicateRecordException($"A user with the name: \"{user.Name}\" already exists.");
				}
				
				context.Users.Add(user);
				SaveChanges(context);
			}
		}

		public void Update(User user)
		{
			using (var context = ContextFactory.Create())
			{
				var existing = context.Users.Find(user.Id);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;
					existing.Name = user.Name;
					SaveChanges(context);
				}
				else
				{
					throw new MissingRecordException($"The existing user with ID {user.Id} could not be found.");
				}
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var user = context.Users
					.Where(g => id == g.Id);

				context.Users.RemoveRange(user);
				SaveChanges(context);
			}
		}
	}
}

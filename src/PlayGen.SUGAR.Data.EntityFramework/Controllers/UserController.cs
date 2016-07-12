using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class UserController : OLD_DbController
	{
		public UserController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<User> Get()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var users = context.Users.ToList();

				return users;
			}
		}

		public IEnumerable<User> Search(string name, bool exactMatch = false)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var user = context.Users.Find(id);

				return user;
			}
		}

		public void Create(User user)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				context.Users.Add(user);
				SaveChanges(context);
			}
		}

		public void Update(User user)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var user = context.Users
					.Where(g => id == g.Id);

				context.Users.RemoveRange(user);
				SaveChanges(context);
			}
		}
	}
}

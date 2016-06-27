using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	/// <summary>
	/// Performs DB operations on the User entity
	/// </summary>
	public class UserController : DbController
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

		public IEnumerable<User> Search(string name)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var users = context.Users
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();

				return users;
			}
		}

		public User Search(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var user = context.Users
					.SingleOrDefault(g => id == g.Id);

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

		public void Delete(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var user = context.Users
					.Where(u => id.Contains(u.Id)).ToList();

				context.Users.RemoveRange(user);
				SaveChanges(context);
			}
		}
	}
}

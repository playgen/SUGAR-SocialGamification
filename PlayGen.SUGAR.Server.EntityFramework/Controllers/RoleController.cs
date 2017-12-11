using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class RoleController : DbController
	{
		public RoleController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Role> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var roles = context.Roles.ToList();
				return roles;
			}
		}

		public List<Role> Get(ClaimScope scope)
		{
			using (var context = ContextFactory.Create())
			{
				var roles = context.Roles.Where(r => r.ClaimScope == scope).ToList();
				return roles;
			}
		}

		public Role GetDefault(string roleName)
		{
			using (var context = ContextFactory.Create())
			{
				var role = context.Roles.FirstOrDefault(r => r.Name == roleName && r.Default);
				return role;
			}
		}

		public Role Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var role = context.Roles.Find(id);
				return role;
			}
		}

		public Role Create(Role role)
		{
			using (var context = ContextFactory.Create())
			{
				context.Roles.Add(role);
				SaveChanges(context);

				return role;
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				// todo also delete all data associated with this role?
				var role = context.Roles.Find(id);
				if (role == null)
				{
					throw new MissingRecordException($"No Role exists with Id: {id}");
				}
				context.Roles.Remove(role);
				SaveChanges(context);
			}
		}
	}
}

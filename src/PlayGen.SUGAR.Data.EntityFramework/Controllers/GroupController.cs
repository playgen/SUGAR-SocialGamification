using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	/// <summary>
	/// Performs DB operations on the Group entity
	/// </summary>
	public class GroupController : DbController
	{
		public GroupController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Group> Get()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Groups.ToList();

				return groups;
			}
		}

		public IEnumerable<Group> Search(string name)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Groups
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();

				return groups;
			}
		}

		public Group Search(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var group = context.Groups
					.SingleOrDefault(g => id == g.Id);

				return group;
			}
		}

		public void Create(Group group)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				context.Groups.Add(group);
				SaveChanges(context);
			}
		}

		public void Delete(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var group = context.Groups
					.Where(g => id == g.Id);

				context.Groups.RemoveRange(group);
				SaveChanges(context);
			}
		}
	}
}

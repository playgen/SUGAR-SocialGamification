using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
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

		public IEnumerable<Group> Get(string[] names)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Groups.Where(g => names.Contains(g.Name)).ToList();

				return groups;
			}
		}

		public IEnumerable<Group> Get(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Groups.Where(g => id.Contains(g.Id)).ToList();

				return groups;
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

		public void Delete(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Groups.Where(g => id.Contains(g.Id)).ToList();

				context.Groups.RemoveRange(groups);
				SaveChanges(context);
			}
		}
	}
}

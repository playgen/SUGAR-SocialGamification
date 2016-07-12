using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GroupController : OLD_DbController
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

				var group = context.Groups.Find(id);

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

		public void Update(Group group)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var existing = context.Groups.Find(group.Id);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;
					existing.Name = group.Name;
					SaveChanges(context);
				}
				else
				{
					throw new MissingRecordException($"The existing group with ID {group.Id} could not be found.");
				}
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

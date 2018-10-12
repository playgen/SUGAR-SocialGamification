using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class GroupController : DbController
	{
		public GroupController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Group> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var groups = context.Groups
					.IncludeAll()
					.ToList();

				return groups;
			}
		}

		public List<Group> Get(string name)
		{
			using (var context = ContextFactory.Create())
			{
				var groups = context.Groups
					.IncludeAll()
					.Where(g => g.Name.ToLower().Contains(name.ToLower()))
					.ToList();

				return groups;
			}
		}

		public Group Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var group = context.Groups
					.IncludeAll()
					.FirstOrDefault(g => g.Id == id);


				return group;
			}
		}

		public Group Create(Group group, SUGARContext context = null)
		{
			var didCreateContext = false;
			if(context == null)
			{
				context = ContextFactory.Create();
				didCreateContext = true;
			}

			context.Groups.Add(group);

			if (didCreateContext)
			{
				context.SaveChanges();
				context.Dispose();
			}

			return group;
		}

		public void Update(Group group)
		{
			using (var context = ContextFactory.Create())
			{
				context.Groups.Update(group);
				context.SaveChanges();
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var group = context.Groups.Find(id);
				if (group == null)
				{
					throw new MissingRecordException($"No Group exists with Id: {id}");
				}
				context.Groups.Remove(group);
				context.SaveChanges();
			}
		}
	}
}

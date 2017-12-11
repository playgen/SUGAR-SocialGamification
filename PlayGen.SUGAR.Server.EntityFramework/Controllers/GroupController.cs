using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

		public Group Create(Group group)
		{
			using (var context = ContextFactory.Create())
			{
				context.Groups.Add(group);
				SaveChanges(context);

				return group;
			}
		}

		public void Update(Group group)
		{
			using (var context = ContextFactory.Create())
			{
				context.Groups.Update(group);
				SaveChanges(context);
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
				SaveChanges(context);
			}
		}
	}
}

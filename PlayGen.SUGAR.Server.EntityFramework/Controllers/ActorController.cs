using System.Collections.Generic;
using System.Linq;

using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class ActorController : DbController
	{
		public ActorController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Actor> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var groups = context.Actors.ToList();

				return groups;
			}
		}

		public Actor Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var actor = context.Actors.Find(id);

				return actor;
			}
		}

		public void Create(Actor actor)
		{
			using (var context = ContextFactory.Create())
			{
				context.Actors.Add(actor);
				SaveChanges(context);
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var actor = context.Actors.Find(id);
				if (actor == null)
				{
					throw new MissingRecordException($"No Actor exists with Id: {id}");
				}
				context.Actors.Remove(actor);
				SaveChanges(context);
			}
		}
	}
}

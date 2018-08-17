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
				var actors = context.Actors
					.Where(a => !a.Private)
					.ToList();

				return actors;
			}
		}

		public Actor Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var actor = context.Actors.Find(id);
				
				return actor.Private
					? null
					: actor;	
			}
		}

		public void Create(Actor actor)
		{
			using (var context = ContextFactory.Create())
			{
				context.Actors.Add(actor);
				context.SaveChanges();
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
				context.SaveChanges();
			}
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
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
				var actor = context.Actors.Find(context, id);

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

		public void Delete(int[] id)
		{
			using (var context = ContextFactory.Create())
			{
				var actors = context.Actors.Where(a => id.Contains(a.Id)).ToList();

				context.Actors.RemoveRange(actors);
				SaveChanges(context);
			}
		}
	}
}

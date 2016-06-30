using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class ActorController : DbController
	{
		public ActorController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Actor> Get()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var groups = context.Actors.ToList();

				return groups;
			}
		}

		public Actor Get(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var actor = context.Actors.Find(id);

				return actor;
			}
		}

		public void Create(Actor actor)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				context.Actors.Add(actor);
				SaveChanges(context);
			}
		}

		public void Delete(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var actors = context.Actors.Where(a => id.Contains(a.Id)).ToList();

				context.Actors.RemoveRange(actors);
				SaveChanges(context);
			}
		}
	}
}

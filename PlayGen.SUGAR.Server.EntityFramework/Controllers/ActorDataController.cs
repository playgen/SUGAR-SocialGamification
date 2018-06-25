using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Extensions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class ActorDataController : DbController
	{
		public ActorDataController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public bool KeyExists(int gameId, int actorId, string key)
		{
			using (var context = ContextFactory.Create())
			{
				return context.FilterByActorId(actorId)
					.FilterByGameId(gameId)
					.FilterByKey(key)
					.Any();
			}
		}

		public List<ActorData> Get(int gameId, int actorId, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.FilterByActorId(actorId)
					.FilterByGameId(gameId)
					.FilterByKeys(keys)
					.ToList();
				return data;
			}
		}

		public ActorData Create(ActorData data)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedGame(data.GameId);
				context.HandleDetatchedActor(data.ActorId);

				context.ActorData.Add(data);
				context.SaveChanges();

				return data;
			}
		}

		public void Update(ActorData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
				context.ActorData.Update(updatedData);
				context.SaveChanges();
			}
		}
	}
}
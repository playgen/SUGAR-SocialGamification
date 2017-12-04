using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
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

		public bool KeyExists(int? gameId, int? actorId, string key)
		{
			using (var context = ContextFactory.Create())
			{
				return context.FilterByActorId(actorId)
					.FilterByGameId(gameId)
					.FilterByKey(key)
					.Any();
			}
		}

		public List<ActorData> Get(int? gameId = null, int? actorId = null, ICollection<string> keys = null)
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
				SaveChanges(context);

				return data;
			}
		}

		public void Update(ActorData updatedData)
		{
			using (var context = ContextFactory.Create())
			{
				// todo replace with entire block with: (and update unit tests)
				// context.[tablename].Update(entity);
				// context.SaveChanges();

				var existingData = context.ActorData
					.Find(updatedData.Id);

				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedData.Value;

				SaveChanges(context);
			}
		}
	}
}
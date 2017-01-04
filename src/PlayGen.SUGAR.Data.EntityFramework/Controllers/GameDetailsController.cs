using System.Collections.Generic;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using System.Linq;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameDetailsController : DbController
	{
		public GameDetailsController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public bool KeyExists(int gameId, string key)
		{
			using (var context = ContextFactory.Create())
			{
				return context.GameDetails.FilterByGameId(gameId)
					.FilterByKey(key)
					.Any();
			}
		}

		public List<GameDetails> Get(int gameId, ICollection<string> keys = null)
		{
			using (var context = ContextFactory.Create())
			{
				var data = context.GameDetails.FilterByGameId(gameId)
					.FilterByKeys(keys)
					.ToList();
				return data;
			}
		}

		public GameDetails Create(GameDetails details)
		{
			using (var context = ContextFactory.Create())
			{
				context.HandleDetatchedActor(details.GameId);

				context.GameDetails.Add(details);
				SaveChanges(context);

				return details;
			}
		}

		public void Update(GameDetails updatedDetails)
		{
			using (var context = ContextFactory.Create())
			{
				// todo replace with entire block with: (and update unit tests)
				// context.[tablename].Update(entity);
				// context.SaveChanges();

				var existingData = context.GameDetails
					.Find(context, updatedDetails.Id);

				if (existingData == null)
				{
					throw new MissingRecordException("Cannot find the object to update.");
				}

				existingData.Value = updatedDetails.Value;

				SaveChanges(context);
			}
		}
	}
}
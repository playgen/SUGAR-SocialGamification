using PlayGen.SUGAR.Data.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class ContextExtensions
	{
		public static void HandleDetatchedGame(this SUGARContext context, int? gameId)
		{
			if (gameId != null && gameId != 0)
			{
				var game = context.Games.FirstOrDefault(a => a.Id == gameId.Value);
				if (game != null && context.Entry(game).State == EntityState.Detached)
				{
					context.Games.Attach(game);
				}
			}
		}

		public static void HandleDetatchedActor(this SUGARContext context, int? actorId)
		{
			if (actorId != null)
			{
				var actor = context.Actors.FirstOrDefault(a => a.Id == actorId.Value);
				if (actor != null && context.Entry(actor).State == EntityState.Detached)
				{
					context.Actors.Attach(actor);
				}
			}
		}

		public static void HandleDetatchedActor(this SUGARContext context, Actor actor)
		{
			if (actor != null && context.Entry(actor).State == EntityState.Detached)
			{
				context.Actors.Attach(actor);
			}
		}

		public static void HandleDetatchedGameData(this SUGARContext context, GameData gameData)
		{
			if (gameData != null && context.Entry(gameData).State == EntityState.Detached)
			{
				context.GameData.Attach(gameData);
			}
		}
    }
}

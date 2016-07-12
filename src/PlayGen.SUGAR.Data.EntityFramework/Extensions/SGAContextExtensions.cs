using PlayGen.SUGAR.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SUGAR.Data.EntityFramework.Extensions
{
	public static class SGAContextExtensions
	{
		public static void HandleDetatchedGame(this SGAContext context, int? gameId)
		{
			if (gameId != null && gameId != 0)
			{
				var game = context.Games.First(a => a.Id == gameId.Value);
				if (game != null && context.Entry(game).State == EntityState.Detached)
				{
					context.Games.Attach(game);
				}
			}
		}

		public static void HandleDetatchedActor(this SGAContext context, int? actorId)
		{
			if (actorId != null)
			{
				var actor = context.Actors.First(a => a.Id == actorId.Value);
				if (actor != null && context.Entry(actor).State == EntityState.Detached)
				{
					context.Actors.Attach(actor);
				}
			}
		}

		public static void HandleDetatchedActor(this SGAContext context, Actor actor)
		{
			if (actor != null && context.Entry(actor).State == EntityState.Detached)
			{
				context.Actors.Attach(actor);
			}
		}

		public static void HandleDetatchedGameData(this SGAContext context, GameData gameData)
		{
			if (gameData != null && context.Entry(gameData).State == EntityState.Detached)
			{
				context.GameData.Attach(gameData);
			}
		}
	}
}

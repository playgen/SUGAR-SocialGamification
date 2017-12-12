using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.EntityFramework.Controllers
{
	public class GameController : DbController
	{
		public GameController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public List<Game> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var games = context.Games.ToList();
				return games;
			}
		}

		public List<Game> Search(string name)
		{
			using (var context = ContextFactory.Create())
			{
				var games = context.Games
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();
				return games;
			}
		}

		public Game Get(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var game = context.Games.Find(id);
				return game;
			}
		}

		public Game Create(Game game)
		{
			using (var context = ContextFactory.Create())
			{
				context.Games.Add(game);
				SaveChanges(context);

				return game;
			}
		}

		public void Update(Game game)
		{
			using (var context = ContextFactory.Create())
			{
				context.Games.Update(game);
				SaveChanges(context);
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				// todo should we not also be deleting all data associated with this game?
				var game = context.Games.Find(id);
				if (game == null)
				{
					throw new MissingRecordException($"No Game exists with Id: {id}");
				}
				context.Games.Remove(game);
				SaveChanges(context);
			}
		}
	}
}

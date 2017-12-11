using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
				// todo replace with entire block with: (and update unit tests)
				// context.[tablename].Update(entity);
				// context.SaveChanges();

				var existing = context.Games.Find(game.Id);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;
					existing.Name = game.Name;
					SaveChanges(context);
				}
				else
				{
					throw new MissingRecordException($"The existing game with ID {game.Id} could not be found.");
				}
			}
		}

		public void Delete(int id)
		{
			using (var context = ContextFactory.Create())
			{
				// todo why are we removing multiple games?
				// todo should we not also be deleting all data associated with this game?
				var game = context.Games
					.Where(g => id == g.Id);

				context.Games.RemoveRange(game);
				SaveChanges(context);
			}
		}
	}
}

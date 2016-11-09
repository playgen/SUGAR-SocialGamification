using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameController : DbController
	{
		public GameController(SUGARContextFactory contextFactory)
            : base(contextFactory)
        {
        }

        public IEnumerable<Game> Get()
		{
			using (var context = ContextFactory.Create())
			{
				var games = context.Games.ToList();
				return games;
			}
		}

		public IEnumerable<Game> Search(string name)
		{
			using (var context = ContextFactory.Create())
			{
				var games = context.Games
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();
				return games;
			}
		}

		public Game Search(int id)
		{
			using (var context = ContextFactory.Create())
			{
				var game = context.Games.Find(context, id);
				return game;
			}
		}

		public void Create(Game game)
		{
			using (var context = ContextFactory.Create())
			{
				context.Games.Add(game);
				SaveChanges(context);
			}
		}

		public void Update(Game game)
		{
			using (var context = ContextFactory.Create())
			{
				var existing = context.Games.Find(context, game.Id);

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

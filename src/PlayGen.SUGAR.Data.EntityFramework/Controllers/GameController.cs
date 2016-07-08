using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GameController : DbController
	{
		public GameController(string nameOrConnectionString) 
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Game> Get()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var games = context.Games.ToList();
				return games;
			}
		}

		public IEnumerable<Game> Search(string name)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var games = context.Games
					.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();
				return games;
			}
		}

		public Game Search(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var game = context.Games.Find(id);
				return game;
			}
		}

		public void Create(Game game)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);
				
				context.Games.Add(game);
				SaveChanges(context);
			}
		}

		public void Update(Game game)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

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
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var game = context.Games
					.Where(g => id == g.Id);

				context.Games.RemoveRange(game);
				SaveChanges(context);
			}
		}
	}
}

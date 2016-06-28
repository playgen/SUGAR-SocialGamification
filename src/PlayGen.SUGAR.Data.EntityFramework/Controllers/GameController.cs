using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Data.Model;

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

		/// <summary>
		/// Retrieve game multiple records by name from the database
		/// </summary>
		/// <param name="partialName"></param>
		/// <returns></returns>
		public IEnumerable<Game> Find(string partialName)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var games = context.Games.Where(g => g.Name.ToLower().Contains(partialName.ToLower())).ToList();
				return games;
			}
		}

		/// <summary>
		/// Create a new game record in the database.
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public void Create(Game game)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);
				
				context.Games.Add(game);
				SaveChanges(context);
			}
		}

		/// <summary>
		/// Delete a game record from the database.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var games = context.Games.Where(g => id.Contains(g.Id)).ToList();

				context.Games.RemoveRange(games);
				SaveChanges(context);
			}
		}
	}
}

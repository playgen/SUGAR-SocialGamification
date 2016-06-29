using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class AchievementController : DbController
	{
		public AchievementController(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Achievement> GetGlobal()
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievements = context.Achievements.Where(a => a.GameId == null).ToList();
				return achievements;
			}
		}


		public IEnumerable<Achievement> GetByGame(int gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievements = context.Achievements.Where(a => a.GameId == gameId).ToList();
				return achievements;
			}
		}

		public Achievement Get(int achievementId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievement = context.Achievements.Find(achievementId);
				return achievement;
			}
		}

		public Achievement Create(Achievement achievement)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Achievements.Any(a => a.Name == achievement.Name && a.GameId == achievement.GameId);

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An achievement with the name {achievement.Name} for this game already exists.");
				}

				/*var gameExists = context.Games.Any(g => g.Id == achievement.GameId);

				if (!gameExists)
				{
					throw new MissingRecordException("The provided game does not exist.");
				}*/

				context.Achievements.Add(achievement);
				SaveChanges(context);
				return achievement;
			}
		}

		public void Delete(int id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievement = context.Achievements.Find(id);
				if (achievement != null)
				{
					context.Achievements.Remove(achievement);
					SaveChanges(context);
				}
			}
		}
	}
}

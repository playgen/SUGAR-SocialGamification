using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class GroupAchievementController : DbController
	{
		public GroupAchievementController(string nameOrConnectionString) : base(nameOrConnectionString)
		{
		}

		public IEnumerable<GroupAchievement> GetByGame(int gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievements = context.GroupAchievements
					.Where(a => a.GameId == gameId).ToList();
				return achievements;
			}
		}

		public IEnumerable<GroupAchievement> Get(int[] achievementIds)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievements = context.GroupAchievements
					.Where(a => achievementIds.Contains(a.Id)).ToList();
				return achievements;
			}
		}

		public GroupAchievement Create(GroupAchievement achievement)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var hasConflicts = context.GroupAchievements
					.Any(a => a.Name == achievement.Name 
					&& a.GameId == achievement.GameId);

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An achievement with the name {achievement.Name} for this game already exists.");
				}

				var gameExists = context.Games.Any(g => g.Id == achievement.GameId);

				if (!gameExists)
				{
					throw new MissingRecordException("The provided game does not exist.");
				}

				context.GroupAchievements.Add(achievement);
				SaveChanges(context);
				return achievement;
			}
		}

		public void Delete(int[] id)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var achievement = context.GroupAchievements
					.Where(g => id.Contains(g.Id)).ToList();

				context.GroupAchievements.RemoveRange(achievement);
				SaveChanges(context);
			}
		}
	}
}

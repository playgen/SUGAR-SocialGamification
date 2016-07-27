using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

		public IEnumerable<Achievement> GetByGame(int? gameId)
		{
			using (var context = new SUGARContext(NameOrConnectionString))
			{
				SetLog(context);
				gameId = gameId ?? 0;

				var achievements = context.Achievements.Where(a => a.GameId == gameId).ToList();
				return achievements;
			}
		}

		public Achievement Get(string token, int? gameId)
		{
			using (var context = new SUGARContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;
				
				var achievement = context.Achievements.Find(token, gameId);
				return achievement;
			}
		}

		public Achievement Create(Achievement achievement)
		{
			using (var context = new SUGARContext(NameOrConnectionString))
			{
				SetLog(context);

				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Achievements.Any(a => (a.Name == achievement.Name && a.GameId == achievement.GameId)
									|| (a.Token == achievement.Token && a.GameId == achievement.GameId));

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"An achievement with the name {achievement.Name} or token {achievement.Token} for this game already exists.");
				}

				context.Achievements.Add(achievement);
				SaveChanges(context);
				return achievement;
			}
		}

		public void Update(Achievement achievement)
		{
			using (var context = new SUGARContext(NameOrConnectionString))
			{
				SetLog(context);

				var existing = context.Achievements.Find(achievement.Token, achievement.GameId);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;

					var hasConflicts = context.Achievements.Where(a => (a.Name == achievement.Name && a.GameId == achievement.GameId));

					if (hasConflicts.Count() > 0)
					{
						if (hasConflicts.Any(a => a.Token != achievement.Token))
						{
							throw new DuplicateRecordException($"An achievement with the name {achievement.Name} for this game already exists.");
						}
					}

					existing.Name = achievement.Name;
					existing.CompletionCriteriaCollection = achievement.CompletionCriteriaCollection;
					existing.RewardCollection = achievement.RewardCollection;
					existing.Description = achievement.Description;
					existing.ActorType = achievement.ActorType;
					existing.GameId = achievement.GameId;
					existing.Token = achievement.Token;

					SaveChanges(context);
				} else
				{
					throw new MissingRecordException($"The existing achievement with token {achievement.Token} and game ID {achievement.GameId} could not be found.");
				}
			}
		}
		
		public void Delete(string token, int? gameId)
		{
			using (var context = new SUGARContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;

				var achievement = context.Achievements.Find(token, gameId);
				if (achievement != null)
				{
					context.Achievements.Remove(achievement);
					SaveChanges(context);
				}
			}
		}
	}
}

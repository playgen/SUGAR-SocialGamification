using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.ContextScope.Interfaces;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class AchievementController : DbController
	{
		public AchievementController(IAmbientContextLocator ambientContextLocator)
			: base(ambientContextLocator)
		{
		}
		
		public IEnumerable<Achievement> GetByGame(int? gameId)
		{
			gameId = gameId ?? 0;

			var achievements = context.Achievements.Where(a => a.GameId == gameId).ToList();
			return achievements;
		}

		public Achievement Get(string token, int? gameId)
		{
			gameId = gameId ?? 0;

			var achievement = context.Achievements.Find(token, gameId);
			return achievement;
		}

		public Achievement Create(Achievement achievement)
		{
			//TODO: refine duplicate text for actor type and game id
			var hasConflicts = context.Achievements.Any(a => (a.Name == achievement.Name && a.GameId == achievement.GameId)
								|| (a.Token == achievement.Token && a.GameId == achievement.GameId));

			if (hasConflicts)
			{
				throw new DuplicateRecordException($"An achievement with the name {achievement.Name} or token {achievement.Token} for this game already exists.");
			}

			context.Achievements.Add(achievement);

			return achievement;			
		}

		public void Update(Achievement achievement)
		{
			var existing = context.Achievements.Find(achievement.Token, achievement.GameId);

			if (existing != null)
			{
				context.Entry(existing).State = EntityState.Modified;

				existing.Name = achievement.Name;
				existing.CompletionCriteriaCollection = achievement.CompletionCriteriaCollection;
				existing.RewardCollection = achievement.RewardCollection;
				existing.Description = achievement.Description;
				existing.ActorType = achievement.ActorType;
				existing.GameId = achievement.GameId;
				existing.Token = achievement.Token;
				// TODO Move SaveChanges(context);
			}
			else
			{
				throw new MissingRecordException($"The existing achievement with token {achievement.Token} and game ID {achievement.GameId} could not be found.");
			}
		}
		
		public void Delete(string token, int? gameId)
		{
			gameId = gameId ?? 0;

			var achievement = context.Achievements.Find(token, gameId);
			if (achievement != null)
			{
				context.Achievements.Remove(achievement);
				// TODO Move SaveChanges(context);
			}
		}
	}
}

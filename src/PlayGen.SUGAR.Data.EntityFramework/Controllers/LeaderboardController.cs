using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using System.Data.Entity;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class LeaderboardController : OLD_DbController
	{
		public LeaderboardController(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
		}

		public IEnumerable<Leaderboard> GetByGame(int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);
				gameId = gameId ?? 0;

				var leaderboards = context.Leaderboards.Where(l => l.GameId == gameId).ToList();
				return leaderboards;
			}
		}

		public Leaderboard Get(string token, int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;

				var leaderboard = context.Leaderboards.Find(token, gameId);
				return leaderboard;
			}
		}

		public Leaderboard Create(Leaderboard leaderboard)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Leaderboards.Any(l => (l.Name == leaderboard.Name && l.GameId == leaderboard.GameId)
									|| (l.Token == leaderboard.Token && l.GameId == leaderboard.GameId));

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"A leaderboard with the name {leaderboard.Name} or token {leaderboard.Token} for this game already exists.");
				}

				hasConflicts = ((int)leaderboard.LeaderboardType < 3 && ((int)leaderboard.GameDataType == 1 || (int)leaderboard.GameDataType == 2)) ||
								((int)leaderboard.LeaderboardType > 2 && ((int)leaderboard.GameDataType == 0 || (int)leaderboard.GameDataType == 3)) ? false : true;

				if (hasConflicts)
				{
					throw new System.ArgumentException($"A leaderboard cannot be created with LeaderboardType {leaderboard.LeaderboardType.ToString()} and GameDataType{leaderboard.GameDataType.ToString()} as it would always return zero results.");
				}

				context.Leaderboards.Add(leaderboard);
				SaveChanges(context);
				return leaderboard;
			}
		}

		public void Update(Leaderboard leaderboard)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				var existing = context.Leaderboards.Find(leaderboard.Token, leaderboard.GameId);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;

					existing.Name = leaderboard.Name;
					existing.Key = leaderboard.Key;
					existing.ActorType = leaderboard.ActorType;
					existing.CriteriaScope = leaderboard.CriteriaScope;
					existing.GameDataType = leaderboard.GameDataType;
					existing.GameId = leaderboard.GameId;
					existing.LeaderboardType = leaderboard.LeaderboardType;
					existing.Token = leaderboard.Token;

					SaveChanges(context);
				}
				else
				{
					throw new MissingRecordException($"The existing leaderboard with token {leaderboard.Token} and game ID {leaderboard.GameId} could not be found.");
				}
			}
		}
		
		public void Delete(string token, int? gameId)
		{
			using (var context = new SGAContext(NameOrConnectionString))
			{
				SetLog(context);

				gameId = gameId ?? 0;

				var leaderboard = context.Leaderboards.Find(token, gameId);
				if (leaderboard != null)
				{
					context.Leaderboards.Remove(leaderboard);
					SaveChanges(context);
				}
			}
		}
	}
}
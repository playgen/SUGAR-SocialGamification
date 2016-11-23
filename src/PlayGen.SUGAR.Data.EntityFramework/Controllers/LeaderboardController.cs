using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Extensions;

namespace PlayGen.SUGAR.Data.EntityFramework.Controllers
{
	public class LeaderboardController : DbController
	{
		public LeaderboardController(SUGARContextFactory contextFactory)
			: base(contextFactory)
		{
		}

		public IEnumerable<Leaderboard> GetByGame(int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var leaderboards = context.Leaderboards.Where(l => l.GameId == gameId).ToList();
				return leaderboards;
			}
		}

		public Leaderboard Get(string token, int? gameId)
		{
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var leaderboard = context.Leaderboards.Find(context, token, gameId);
				return leaderboard;
			}
		}

		public Leaderboard Create(Leaderboard leaderboard)
		{
			using (var context = ContextFactory.Create())
			{
				//TODO: refine duplicate text for actor type and game id
				var hasConflicts = context.Leaderboards.Any(l => (l.Name == leaderboard.Name && l.GameId == leaderboard.GameId)
									|| (l.Token == leaderboard.Token && l.GameId == leaderboard.GameId));

				if (hasConflicts)
				{
					throw new DuplicateRecordException($"A leaderboard with the name {leaderboard.Name} or token {leaderboard.Token} for this game already exists.");
				}

				hasConflicts = ((int)leaderboard.LeaderboardType < 3 && ((int)leaderboard.SaveDataType == 1 || (int)leaderboard.SaveDataType == 2)) ||
								((int)leaderboard.LeaderboardType > 2 && ((int)leaderboard.SaveDataType == 0 || (int)leaderboard.SaveDataType == 3)) ? false : true;

				if (hasConflicts)
				{
					throw new System.ArgumentException($"A leaderboard cannot be created with LeaderboardType {leaderboard.LeaderboardType.ToString()} and SaveDataType{leaderboard.SaveDataType.ToString()} as it would always return zero results.");
				}

				context.Leaderboards.Add(leaderboard);
				SaveChanges(context);
				return leaderboard;
			}
		}

		public void Update(Leaderboard leaderboard)
		{
			using (var context = ContextFactory.Create())
			{
				var existing = context.Leaderboards.Find(context, leaderboard.Token, leaderboard.GameId);

				if (existing != null)
				{
					context.Entry(existing).State = EntityState.Modified;

					var hasConflicts = context.Leaderboards.Where(l => (l.Name == leaderboard.Name && l.GameId == leaderboard.GameId));

					if (hasConflicts.Any())
					{
						if (hasConflicts.Any(a => a.Token != leaderboard.Token))
						{
							throw new DuplicateRecordException($"A leaderboard with the name {leaderboard.Name} for this game already exists.");
						}
					} else
					{
						var hasTypeConflicts = ((int)leaderboard.LeaderboardType < 3 && ((int)leaderboard.SaveDataType == 1 || (int)leaderboard.SaveDataType == 2)) ||
								((int)leaderboard.LeaderboardType > 2 && ((int)leaderboard.SaveDataType == 0 || (int)leaderboard.SaveDataType == 3)) ? false : true;

						if (hasTypeConflicts)
						{
							throw new System.ArgumentException($"A leaderboard cannot be updated to use LeaderboardType {leaderboard.LeaderboardType.ToString()} and SaveDataType{leaderboard.SaveDataType.ToString()}, as it would always return zero results.");
						}
					}

					existing.Name = leaderboard.Name;
					existing.Key = leaderboard.Key;
					existing.ActorType = leaderboard.ActorType;
					existing.CriteriaScope = leaderboard.CriteriaScope;
					existing.SaveDataType = leaderboard.SaveDataType;
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
			using (var context = ContextFactory.Create())
			{
				gameId = gameId ?? 0;

				var leaderboard = context.Leaderboards.Find(context, token, gameId);
				if (leaderboard != null)
				{
					context.Leaderboards.Remove(leaderboard);
					SaveChanges(context);
				}
			}
		}
	}
}
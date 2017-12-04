// todo remove any references to the contracts project

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Extensions;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class LeaderboardController : CriteriaEvaluator
	{
		protected readonly EntityFramework.Controllers.ActorController ActorController;
		protected readonly EntityFramework.Controllers.GroupController GroupController;
		protected readonly EntityFramework.Controllers.UserController UserController;

		private readonly ILogger _logger;

		// todo replace db controller usage with core controller usage (all cases except for leaderbaordDbController)
		public LeaderboardController(
			ILogger<LeaderboardController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			GroupMemberController groupMemberCoreController,
			UserFriendController userFriendCoreController,
			EntityFramework.Controllers.ActorController actorController,
			EntityFramework.Controllers.GroupController groupController,
			EntityFramework.Controllers.UserController userController,
			SUGARContextFactory contextFactory)
			: base(evaluationDataLogger, contextFactory, groupMemberCoreController, userFriendCoreController)
		{
			_logger = logger;
			ActorController = actorController;
			GroupController = groupController;
			UserController = userController;
		}

		public List<LeaderboardStandingsResponse> GetStandings(Leaderboard leaderboard, LeaderboardStandingsRequest request)
		{
			if (leaderboard == null)
			{
				throw new MissingRecordException("The provided leaderboard does not exist.");
			}
			if (request.PageLimit <= 0)
			{
				throw new ArgumentException("You must request at least one ranking from the leaderboard.");
			}
			var standings = GatherStandings(leaderboard, request);

			_logger.LogInformation($"{standings?.Count} Standings for Leaderboard: {leaderboard?.Token}");

			return standings;
		}

		protected List<LeaderboardStandingsResponse> GatherStandings(Leaderboard leaderboard, LeaderboardStandingsRequest request)
		{
			var actors = GetActors(request.LeaderboardFilterType, leaderboard.ActorType, request.ActorId);

			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, leaderboard.EvaluationDataCategory);

			List<LeaderboardStandingsResponse> typeResults;

			switch (leaderboard.LeaderboardType)
			{
				case LeaderboardType.Highest:
					typeResults = EvaluateHighest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				case LeaderboardType.Lowest:
					typeResults = EvaluateLowest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				case LeaderboardType.Cumulative:
					typeResults = EvaluateCumulative(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				case LeaderboardType.Count:
					typeResults = EvaluateCount(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				case LeaderboardType.Earliest:
					typeResults = EvaluateEarliest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				case LeaderboardType.Latest:
					typeResults = EvaluateLatest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, leaderboard.EvaluationDataCategory, request);
					break;

				default:
					return null;
			}

			var results = FilterResults(typeResults, request.PageLimit, request.PageOffset, request.LeaderboardFilterType, request.ActorId);

			_logger.LogInformation($"{results?.Count} Standings for Leaderboard: {leaderboard?.Token}");

			return results;
		}

		protected List<ActorResponse> GetActors(LeaderboardFilterType filter, ActorType actorType, int? actorId)
		{
			var actors = Enumerable.Empty<ActorResponse>().ToList();

			switch (filter)
			{
				case LeaderboardFilterType.Top:
					break;
				case LeaderboardFilterType.Near:
					if (!actorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather those ranked near them");
					}
					var provided = ActorController.Get(actorId.Value);
					if (provided == null || actorType != ActorType.Undefined && provided.ActorType != actorType)
					{
						throw new ArgumentException("The provided ActorId cannot compete on this leaderboard.");
					}
					break;
				case LeaderboardFilterType.Friends:
					if (!actorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather rankings among friends");
					}
					if (actorType == ActorType.Group)
					{
						throw new ArgumentException("This leaderboard cannot filter by friends");
					}
					var user = ActorController.Get(actorId.Value);
					if (user == null || user.ActorType != ActorType.User)
					{
						throw new ArgumentException("The provided ActorId is not an user.");
					}
					break;
				case LeaderboardFilterType.GroupMembers:
					if (!actorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather rankings among group members");
					}
					if (actorType == ActorType.Group)
					{
						throw new ArgumentException("This leaderboard cannot filter by group members");
					}

					// todo this doesn't get the group!
					var group = ActorController.Get(actorId.Value);
					if (group == null || group.ActorType != ActorType.Group)
					{
						throw new ArgumentException("The provided ActorId is not a group.");
					}

					// todo and then doesn't return the members of that group!
					// todo what happens in the case where a user is in multiple groups?
					break;
			}

			switch (actorType)
			{
				case ActorType.Undefined:
					actors = ActorController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = GetName(a.Id, a.ActorType)
					}).ToList();
					break;

				case ActorType.User:
					actors = UserController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = a.Name
					}).ToList();
					break;

				case ActorType.Group:
					actors = GroupController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = a.Name
					}).ToList();
					break;
			}

			_logger.LogDebug($"{actors?.Count} Actors for Filter: {filter}, ActorType: {actorType}, ActorId: {actorId}");

			return actors;
		}

		protected string GetName(int id, ActorType actorType)
		{
			switch (actorType)
			{
				case ActorType.User:
					return UserController.Get(id).Name;

				case ActorType.Group:
					return GroupController.Get(id).Name;

				default:
					return "";
			}
		}

		protected List<LeaderboardStandingsResponse> EvaluateHighest(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Max<long>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				case EvaluationDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Max<float>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value)
						.Where(r => float.Parse(r.Value) > 0).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> EvaluateLowest(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Min<long>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				case EvaluationDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Min<float>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderBy(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> EvaluateCumulative(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Sum<long>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				case EvaluationDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.Sum<float>(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> EvaluateCount(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.CountKeys(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				case EvaluationDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.CountKeys(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> EvaluateEarliest(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.TryGetEarliestKey(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).SerializeToString()
					}).ToList();
					break;

				case EvaluationDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.TryGetEarliestKey(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).SerializeToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderBy(r => r.Value)
						.Where(r => DateTime.Parse(r.Value) != default(DateTime)).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> EvaluateLatest(EvaluationDataController evaluationDataController, List<ActorResponse> actors, int? gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, EvaluationDataCategory evaluationDataCategory, LeaderboardStandingsRequest request)
		{
			List<LeaderboardStandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.TryGetLatestKey(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).SerializeToString()
					}).ToList();
					break;

				case EvaluationDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.TryGetLatestKey(gameId, r.Id, key, evaluationDataType, evaluationDataCategory, request.DateStart, request.DateEnd).SerializeToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value)
						.Where(r => DateTime.Parse(r.Value) != default(DateTime)).ToList();

			_logger.LogDebug($"{results?.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<LeaderboardStandingsResponse> FilterResults(List<LeaderboardStandingsResponse> typeResults, int limit, int offset, LeaderboardFilterType filter, int? actorId)
		{
			int position;
			switch (filter)
			{
				case LeaderboardFilterType.Top:
					typeResults = typeResults.Skip(offset * limit).Take(limit).ToList();
					position = (offset * limit);
					return typeResults.Select(s => new LeaderboardStandingsResponse {
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					}).ToList();
				case LeaderboardFilterType.Near:
					var typeResultList = typeResults as List<LeaderboardStandingsResponse> ?? typeResults.ToList();
					bool actorCheck = actorId != null && typeResultList.Any(r => r.ActorId == actorId.Value);
					if (actorCheck)
					{
						int actorPosition = typeResultList.TakeWhile(r => r.ActorId != actorId.Value).Count();
						offset += actorPosition / limit;
					}
					typeResults = typeResultList.Skip(offset * limit).Take(limit).ToList();
					position = (offset * limit);
					return typeResults.Select(s => new LeaderboardStandingsResponse {
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					}).ToList();
				case LeaderboardFilterType.Friends:
					position = (offset * limit);
					var overall = typeResults.Select(s => new LeaderboardStandingsResponse {
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
					if (actorId != null)
					{
						var friends = UserFriendCoreController.GetFriends(actorId.Value).Select(r => r.Id).ToList();
						friends.Add(actorId.Value);
						var friendsOnly = overall.Where(r => friends.Contains(r.ActorId));
						friendsOnly = friendsOnly.Skip(offset * limit).Take(limit);
						return friendsOnly.ToList();
					}
					return Enumerable.Empty<LeaderboardStandingsResponse>().ToList();
				case LeaderboardFilterType.GroupMembers:
					position = (offset * limit);
					var all = typeResults.Select(s => new LeaderboardStandingsResponse {
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
					if (actorId != null)
					{
						var members = GroupMemberCoreController.GetMembers(actorId.Value).Select(r => r.Id);
						var membersOnly = all.Where(r => members.Contains(r.ActorId));
						membersOnly = membersOnly.Skip(offset * limit).Take(limit);
						return membersOnly.ToList();
					}
					return Enumerable.Empty<LeaderboardStandingsResponse>().ToList();
				default:
					return null;
			}
		}
	}
}
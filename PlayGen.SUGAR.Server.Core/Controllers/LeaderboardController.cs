using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Extensions;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;

namespace PlayGen.SUGAR.Server.Core.Controllers
{
	public class LeaderboardController : CriteriaEvaluator
	{
		protected readonly ActorController ActorController;
		protected readonly GroupController GroupController;
		protected readonly UserController UserController;

		private readonly ILogger _logger;

		public LeaderboardController(
			ILogger<LeaderboardController> logger,
			ILogger<EvaluationDataController> evaluationDataLogger,
			RelationshipController relationshipCoreController,
			ActorController actorController,
			GroupController groupController,
			UserController userController,
			SUGARContextFactory contextFactory)
			: base(evaluationDataLogger, contextFactory, relationshipCoreController)
		{
			_logger = logger;
			ActorController = actorController;
			GroupController = groupController;
			UserController = userController;
		}

		public List<StandingsResponse> GetStandings(Leaderboard leaderboard, StandingsRequest request)
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

			_logger.LogInformation($"{standings?.Count} Standings for Leaderboard: {leaderboard.Token}");

			return standings;
		}

		protected List<StandingsResponse> GatherStandings(Leaderboard leaderboard, StandingsRequest request)
		{
			var evaluationDataController = new EvaluationDataController(EvaluationDataLogger, ContextFactory, leaderboard.EvaluationDataCategory);

			var actors = GetActors(evaluationDataController, leaderboard, request);
			
			List<StandingsResponse> typeResults;

			switch (leaderboard.LeaderboardType)
			{
				case LeaderboardType.Highest:
					typeResults = EvaluateHighest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				case LeaderboardType.Lowest:
					typeResults = EvaluateLowest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				case LeaderboardType.Cumulative:
					typeResults = EvaluateCumulative(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				case LeaderboardType.Count:
					typeResults = EvaluateCount(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				case LeaderboardType.Earliest:
					typeResults = EvaluateEarliest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				case LeaderboardType.Latest:
					typeResults = EvaluateLatest(evaluationDataController, actors, leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.LeaderboardType, leaderboard.EvaluationDataType, request);
					break;

				default:
					return null;
			}

			var results = FilterResults(typeResults, request.PageLimit, request.PageOffset, request.LeaderboardFilterType, request.ActorId);

			_logger.LogInformation($"{results?.Count} Standings for Leaderboard: {leaderboard.Token}");

			return results;
		}

		protected List<Actor> GetActors(EvaluationDataController evaluationDataController, Leaderboard leaderboard, StandingsRequest request)
		{
			switch (request.LeaderboardFilterType)
			{
				case LeaderboardFilterType.Top:
					break;
				case LeaderboardFilterType.Near:
					if (!request.ActorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather those ranked near them");
					}
					var provided = ActorController.Get(request.ActorId.Value);
					if (provided == null || leaderboard.ActorType != ActorType.Undefined && provided.ActorType != leaderboard.ActorType)
					{
						throw new ArgumentException("The provided ActorId cannot compete on this leaderboard.");
					}
					break;
				case LeaderboardFilterType.Friends:
					if (!request.ActorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather rankings among friends");
					}
					if (leaderboard.ActorType == ActorType.Group)
					{
						throw new ArgumentException("This leaderboard cannot filter by friends");
					}
					var user = ActorController.Get(request.ActorId.Value);
					if (user == null || user.ActorType != ActorType.User)
					{
						throw new ArgumentException("The provided ActorId is not an user.");
					}
					break;
				case LeaderboardFilterType.GroupMembers:
					if (!request.ActorId.HasValue)
					{
						throw new ArgumentException("An ActorId has to be passed in order to gather rankings among group members");
					}
					if (leaderboard.ActorType == ActorType.Group)
					{
						throw new ArgumentException("This leaderboard cannot filter by group members");
					}
					var group = ActorController.Get(request.ActorId.Value);
					if (group == null || group.ActorType != ActorType.Group)
					{
						throw new ArgumentException("The provided ActorId is not a group.");
					}
					break;
			}
			// get all valid actors (have evaluationDataKey evaluation data in game gameId)
			var validActors = evaluationDataController.GetGameKeyActors(leaderboard.GameId, leaderboard.EvaluationDataKey, leaderboard.EvaluationDataType, request.DateStart, request.DateEnd);
			var actors = validActors.Select(a => ActorController.Get(a)).Where(a => a != null).ToList();

			_logger.LogDebug($"{actors.Count} Actors for Filter: {request.LeaderboardFilterType}, ActorType: {leaderboard.ActorType}, ActorId: {request.ActorId}");

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
					return string.Empty;
			}
		}

		protected List<StandingsResponse> EvaluateHighest(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			if (request.MultiplePerActor)
			{
				results = GetAllActorResults(evaluationDataController, actors, gameId, key, evaluationDataType, request);
			}
			else {
				switch (evaluationDataType)
				{
				case EvaluationDataType.Long:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetMax(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.Value
						}).ToList();
					break;
				case EvaluationDataType.Float:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetMax(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.Value
						}).ToList();
					break;
				default:
					return null;
				}
			}

			results = results.OrderByDescending(r => float.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<StandingsResponse> EvaluateLowest(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			if (request.MultiplePerActor)
			{
				results = GetAllActorResults(evaluationDataController, actors, gameId, key, evaluationDataType, request);
			}
			else {
				switch (evaluationDataType)
				{
				case EvaluationDataType.Long:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetMin(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.Value
						}).ToList();
					break;

				case EvaluationDataType.Float:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetMin(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.Value
						}).ToList();
					break;
				default:
					return null;
				}
			}

			results = results.OrderBy(r => float.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<StandingsResponse> EvaluateCumulative(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.Long:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetSum<long>(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.ToString()
						}).ToList();
					break;

				case EvaluationDataType.Float:
					results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetSum<float>(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
						.Where(a => a.Value != null)
						.Select(a => new StandingsResponse
						{
							ActorId = a.Actor.Id,
							ActorName = a.Actor.Name,
							Value = a.Value.ToString()
						}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => float.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<StandingsResponse> EvaluateCount(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			switch (evaluationDataType)
			{
				case EvaluationDataType.String:
					results = actors.Select(r => new StandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.CountKeys(gameId, r.Id, key, evaluationDataType, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				case EvaluationDataType.Boolean:
					results = actors.Select(r => new StandingsResponse {
						ActorId = r.Id,
						ActorName = r.Name,
						Value = evaluationDataController.CountKeys(gameId, r.Id, key, evaluationDataType, request.DateStart, request.DateEnd).ToString()
					}).ToList();
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => float.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<StandingsResponse> EvaluateEarliest(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			if (request.MultiplePerActor)
			{
				results = GetAllActorResults(evaluationDataController, actors, gameId, key, evaluationDataType, request);
			}
			else
			{
				switch (evaluationDataType)
				{
					case EvaluationDataType.String:
						results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetEarliest(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
							.Where(a => a.Value != null)
							.Select(a => new StandingsResponse
							{
								ActorId = a.Actor.Id,
								ActorName = a.Actor.Name,
								Value = a.Value.DateCreated.SerializeToString()
							}).ToList();
						break;

					case EvaluationDataType.Boolean:
						results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetEarliest(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
							.Where(a => a.Value != null)
							.Select(a => new StandingsResponse
							{
								ActorId = a.Actor.Id,
								ActorName = a.Actor.Name,
								Value = a.Value.DateCreated.SerializeToString()
							}).ToList();
						break;

					default:
						return null;
				}
			}

			results = results.OrderBy(r => DateTime.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		protected List<StandingsResponse> EvaluateLatest(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, LeaderboardType type, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			List<StandingsResponse> results;
			if (request.MultiplePerActor)
			{
				results = GetAllActorResults(evaluationDataController, actors, gameId, key, evaluationDataType, request);
			}
			else
			{
				switch (evaluationDataType)
				{
					case EvaluationDataType.String:
						results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetLatest(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
							.Where(a => a.Value != null)
							.Select(a => new StandingsResponse
							{
								ActorId = a.Actor.Id,
								ActorName = a.Actor.Name,
								Value = a.Value.DateCreated.SerializeToString()
							}).ToList();
						break;

					case EvaluationDataType.Boolean:
						results = actors.Select(a => new { Actor = a, Value = evaluationDataController.TryGetLatest(gameId, a.Id, key, out var value, evaluationDataType, request.DateStart, request.DateEnd) ? value : null })
							.Where(a => a.Value != null)
							.Select(a => new StandingsResponse
							{
								ActorId = a.Actor.Id,
								ActorName = a.Actor.Name,
								Value = a.Value.DateCreated.SerializeToString()
							}).ToList();
						break;

					default:
						return null;
				}
			}

			results = results.OrderByDescending(r => DateTime.Parse(r.Value)).ToList();

			_logger.LogDebug($"{results.Count} Actors for GameId: {gameId}, Key: {key}, Leaderboard Type: {type}, Save Data Type: {evaluationDataType}");

			return results;
		}

		private List<StandingsResponse> GetAllActorResults(EvaluationDataController evaluationDataController, List<Actor> actors, int gameId, string key, EvaluationDataType evaluationDataType, StandingsRequest request)
		{
			return actors.SelectMany(a => evaluationDataController.List(gameId, a.Id, key, evaluationDataType, request.DateStart, request.DateEnd).Select(r => new StandingsResponse
			{
				ActorId = a.Id,
				ActorName = a.Name,
				Value = r.Value.ToString()
			})).ToList();
		}

		protected List<StandingsResponse> FilterResults(List<StandingsResponse> typeResults, int limit, int offset, LeaderboardFilterType filter, int? actorId)
		{
			int position;
			switch (filter)
			{
				case LeaderboardFilterType.Top:
					break;
				case LeaderboardFilterType.Near:
					if (actorId != null && typeResults.Any(r => r.ActorId == actorId.Value))
					{
						var actorPosition = typeResults.TakeWhile(r => r.ActorId != actorId.Value).Count();
						offset += actorPosition / limit;
					}
					else
					{
						typeResults = new List<StandingsResponse>();
					}
					break;
				case LeaderboardFilterType.Friends:
					if (actorId != null)
					{
						var friends = RelationshipCoreController.GetRelationships(actorId.Value, ActorType.User).Select(r => r.Id).ToList();
						friends.Add(actorId.Value);
						typeResults = typeResults.Where(r => friends.Contains(r.ActorId)).Skip(offset * limit).Take(limit).ToList();
					}
					else
					{
						typeResults = new List<StandingsResponse>();
					}
					break;
				case LeaderboardFilterType.GroupMembers:
					if (actorId != null)
					{
						var members = RelationshipCoreController.GetRelationships(actorId.Value, ActorType.User).Select(r => r.Id);
						typeResults = typeResults.Where(r => members.Contains(r.ActorId)).Skip(offset * limit).Take(limit).ToList();
					}
					else
					{
						typeResults = new List<StandingsResponse>();
					}
					break;
				default:
					return null;
			}
			typeResults = typeResults.Skip(offset * limit).Take(limit).ToList();
			position = offset * limit;
			return typeResults.Select(s => new StandingsResponse
			{
				ActorId = s.ActorId,
				ActorName = s.ActorName,
				Value = s.Value,
				Ranking = ++position
			}).ToList();
		}
	}
}
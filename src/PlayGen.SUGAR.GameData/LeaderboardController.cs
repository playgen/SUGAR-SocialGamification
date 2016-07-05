using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.Interfaces;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.GameData
{
	public class LeaderboardController : DataEvaluationController
	{
		protected readonly ActorController ActorController;
		protected readonly GroupController GroupController;
		protected readonly UserController UserController;

		public LeaderboardController(IGameDataController gameDataController,
			GroupRelationshipController groupRelationshipController,
			ActorController actorController,
			GroupController groupController,
			UserController userController)
			: base(gameDataController, groupRelationshipController)
		{
			ActorController = actorController;
			GroupController = groupController;
			UserController = userController;
		}

		public IEnumerable<LeaderboardStandingsResponse> GetStandings (Leaderboard leaderboard, LeaderboardStandingsRequest request)
		{
			if (leaderboard == null)
			{
				throw new MissingRecordException("The provided leaderboard does not exist.");
			}
			if (request.ActorId != null)
			{
				throw new NotImplementedException("Getting relative standings is not yet implemented");
			}
			var standings = GatherStandings(leaderboard, request);
			return standings;
		}

		protected IEnumerable<LeaderboardStandingsResponse> GatherStandings(Leaderboard leaderboard, LeaderboardStandingsRequest request)
		{
			IEnumerable<ActorResponse> actors = null;
			switch (leaderboard.ActorType)
			{
				case ActorType.Undefined:
					actors = ActorController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = GetName(a.Id, a.ActorType) });
					break;

				case ActorType.User:
					actors = UserController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = a.Name });
					break;

				case ActorType.Group:
					actors = GroupController.Get().Select(a => new ActorResponse {
						Id = a.Id,
						Name = a.Name });
					break;
			}

			IEnumerable<LeaderboardStandingsResponse> typeResults = Enumerable.Empty<LeaderboardStandingsResponse>();

			switch (leaderboard.LeaderboardType)
			{
				case LeaderboardType.Highest:
					typeResults = EvaluateHighest(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				case LeaderboardType.Lowest:
					typeResults = EvaluateLowest(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				case LeaderboardType.Cumulative:
					typeResults = EvaluateCumulative(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				case LeaderboardType.Count:
					typeResults = EvaluateCount(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				case LeaderboardType.Earliest:
					typeResults = EvaluateEarliest(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				case LeaderboardType.Latest:
					typeResults = EvaluateLatest(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);
					break;

				default:
					return null;
			}

			typeResults = typeResults.Skip(request.Offset * request.Limit).Take(request.Limit);
			int position = (request.Offset * request.Limit);
			var results = typeResults.Select(s => new LeaderboardStandingsResponse
			{
				ActorId = s.ActorId,
				ActorName = s.ActorName,
				Value = s.Value,
				Ranking = ++position
			});
			return results;
		}

		protected string GetName (int id, ActorType actorType)
		{
			switch (actorType)
			{
				case ActorType.User:
					return UserController.Search(id).Name;

				case ActorType.Group:
					return GroupController.Search(id).Name;

				default:
					return "";
			}
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateHighest(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.GetHighestLongs(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.GetHighestFloats(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value);
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateLowest(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.GetLowestLongs(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.GetLowestFloats(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderBy(r => r.Value);
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateCumulative(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.Long:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.SumLongs(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Float:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.SumFloats(gameId, r.Id, key, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value);
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateCount(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.CountKeys(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.CountKeys(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value)
						.Where(r => float.Parse(r.Value) > 0);
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateEarliest(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetEarliestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetEarliestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderBy(r => r.Value)
						.Where(r => DateTime.Parse(r.Value) != default(DateTime));
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateLatest(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			IEnumerable<LeaderboardStandingsResponse> results = Enumerable.Empty<LeaderboardStandingsResponse>();
			switch (gameDataType)
			{
				case GameDataType.String:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetLatestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				case GameDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetLatestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd).ToString()
					});
					break;

				default:
					return null;
			}

			results = results.OrderBy(r => r.Value)
						.Where(r => DateTime.Parse(r.Value) != default(DateTime));
			return results;
		}
	}
}
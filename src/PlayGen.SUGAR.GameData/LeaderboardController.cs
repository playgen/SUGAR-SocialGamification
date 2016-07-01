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
			switch (leaderboard.GameDataType)
			{
				case GameDataType.Boolean:
					return EvaluateBool(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);

				case GameDataType.String:
					return EvaluateString(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);

				case GameDataType.Float:
					return EvaluateFloat(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);

				case GameDataType.Long:
					return EvaluateLong(actors, leaderboard.GameId, leaderboard.Key, leaderboard.LeaderboardType, leaderboard.GameDataType, request);

				default:
					return null;
			}
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

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateLong(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			var sums = actors.Select(r => new {
				Actor = r,
				Value = GameDataController.SumLongs(gameId, r.Id, key)
			}).OrderByDescending(r => r.Value)
			.Skip(request.Offset * request.Limit).Take(request.Limit);
			int position = (request.Offset * request.Limit);
			var results = sums.Select(s => ToStandingsResponse(s.Actor, s.Value.ToString(), ++position));
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateFloat(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			var sums = actors.Select(r => new {
				Actor = r,
				Value = GameDataController.SumFloats(gameId, r.Id, key)
			}).OrderByDescending(r => r.Value)
			.Skip(request.Offset * request.Limit).Take(request.Limit);
			int position = (request.Offset * request.Limit);
			var results = sums.Select(s => ToStandingsResponse(s.Actor, s.Value.ToString(), ++position));
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateString(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			var sums = actors.Select(r => new {
				Actor = r,
				Value = GameDataController.CountKeys(gameId, r.Id, key, gameDataType)
			}).OrderByDescending(r => r.Value)
			.Where(a => a.Value > 0)
			.Skip(request.Offset * request.Limit).Take(request.Limit);
			int position = (request.Offset * request.Limit);
			var results = sums.Select(s => ToStandingsResponse(s.Actor, s.Value.ToString(), ++position));
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> EvaluateBool(IEnumerable<ActorResponse> actors, int? gameId, string key, LeaderboardType type, GameDataType gameDataType, LeaderboardStandingsRequest request)
		{
			var sums = actors.Select(r => new {
				Actor = r,
				Value = GameDataController.CountKeys(gameId, r.Id, key, gameDataType)
			}).OrderByDescending(r => r.Value)
			.Where(a => a.Value > 0)
			.Skip(request.Offset * request.Limit).Take(request.Limit);
			int position = (request.Offset * request.Limit);
			var results = sums.Select(s => ToStandingsResponse(s.Actor, s.Value.ToString(), ++position));
			return results;
		}

		protected LeaderboardStandingsResponse ToStandingsResponse (ActorResponse actor, string value, int position)
		{
			return new LeaderboardStandingsResponse
			{
				ActorId = actor.Id,
				ActorName = actor.Name,
				Value = value,
				Ranking = position
			};
		}
	}
}
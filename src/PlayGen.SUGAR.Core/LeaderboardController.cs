using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Gore
{
	public class LeaderboardController : DataEvaluationController
	{
		protected readonly ActorController ActorController;
		protected readonly GroupController GroupController;
		protected readonly UserController UserController;

		public LeaderboardController(GameDataController gameDataController,
			GroupRelationshipController groupRelationshipController,
			UserRelationshipController userRelationshipController,
			ActorController actorController,
			GroupController groupController,
			UserController userController)
			: base(gameDataController, groupRelationshipController, userRelationshipController)
		{
			ActorController = actorController;
			GroupController = groupController;
			UserController = userController;
		}

		public IEnumerable<LeaderboardStandingsResponse> GetStandings(Leaderboard leaderboard, LeaderboardStandingsRequest request)
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
			return standings;
		}

		protected IEnumerable<LeaderboardStandingsResponse> GatherStandings(Leaderboard leaderboard, LeaderboardStandingsRequest request)
		{
			IEnumerable<ActorResponse> actors = GetActors(request.LeaderboardFilterType, leaderboard.ActorType, request.ActorId);

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

			var results = FilterResults(typeResults, request.PageLimit, request.PageOffset, request.LeaderboardFilterType, request.ActorId);
			return results;
		}

		protected IEnumerable<ActorResponse> GetActors(LeaderboardFilterType filter, ActorType actorType, int? actorId)
		{
			IEnumerable<ActorResponse> actors = Enumerable.Empty<ActorResponse>();
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
					actors = ActorController.Get().Select(a => new ActorResponse
					{
						Id = a.Id,
						Name = GetName(a.Id, a.ActorType)
					});
					break;

				case ActorType.User:
					actors = UserController.Get().Select(a => new ActorResponse
					{
						Id = a.Id,
						Name = a.Name
					});
					break;

				case ActorType.Group:
					actors = GroupController.Get().Select(a => new ActorResponse
					{
						Id = a.Id,
						Name = a.Name
					});
					break;
			}

			return actors;
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

			results = results.OrderByDescending(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0);
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

			results = results.OrderBy(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0);
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

			results = results.OrderByDescending(r => float.Parse(r.Value))
						.Where(r => float.Parse(r.Value) > 0);
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

			results = results.OrderByDescending(r => float.Parse(r.Value))
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
						Value = GameDataController.TryGetEarliestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd)
									.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
					});
					break;

				case GameDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetEarliestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd)
									.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
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
						Value = GameDataController.TryGetLatestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd)
									.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
					});
					break;

				case GameDataType.Boolean:
					results = actors.Select(r => new LeaderboardStandingsResponse
					{
						ActorId = r.Id,
						ActorName = r.Name,
						Value = GameDataController.TryGetLatestKey(gameId, r.Id, key, gameDataType, request.DateStart, request.DateEnd)
									.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
					});
					break;

				default:
					return null;
			}

			results = results.OrderByDescending(r => r.Value)
						.Where(r => DateTime.Parse(r.Value) != default(DateTime));
			return results;
		}

		protected IEnumerable<LeaderboardStandingsResponse> FilterResults(IEnumerable<LeaderboardStandingsResponse> typeResults, int limit, int offset, LeaderboardFilterType filter, int? actorId)
		{
			int position = 0;
			switch (filter)
			{
				case LeaderboardFilterType.Top:
					typeResults = typeResults.Skip(offset * limit).Take(limit);
					position = (offset * limit);
					return typeResults.Select(s => new LeaderboardStandingsResponse
					{
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
				case LeaderboardFilterType.Near:
					bool actorCheck = typeResults.Any(r => r.ActorId == actorId.Value);
					if (actorCheck)
					{
						int actorPosition = typeResults.TakeWhile(r => r.ActorId != actorId.Value).Count();
						offset += actorPosition / limit;
					}
					typeResults = typeResults.Skip(offset * limit).Take(limit);
					position = (offset * limit);
					return typeResults.Select(s => new LeaderboardStandingsResponse
					{
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
				case LeaderboardFilterType.Friends:
					position = (offset * limit);
					var overall = typeResults.Select(s => new LeaderboardStandingsResponse
					{
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
					List<int> friends = UserRelationshipController.GetFriends(actorId.Value).Select(r => r.Id).ToList();
					friends.Add(actorId.Value);
					var friendsOnly = overall.Where(r => friends.Contains(r.ActorId));
					friendsOnly = friendsOnly.Skip(offset * limit).Take(limit);
					return friendsOnly;
				case LeaderboardFilterType.GroupMembers:
					position = (offset * limit);
					var all = typeResults.Select(s => new LeaderboardStandingsResponse
					{
						ActorId = s.ActorId,
						ActorName = s.ActorName,
						Value = s.Value,
						Ranking = ++position
					});
					IEnumerable<int> members = GroupRelationshipController.GetMembers(actorId.Value).Select(r => r.Id);
					var membersOnly = all.Where(r => members.Contains(r.ActorId));
					membersOnly = membersOnly.Skip(offset * limit).Take(limit);
					return membersOnly;
				default:
					return null;
			}
		}
	}
}
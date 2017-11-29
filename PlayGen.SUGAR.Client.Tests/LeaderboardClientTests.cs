using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class LeaderboardClientTests : ClientTestBase
	{
		[Fact]
		public void CanGetLeaderboardsByGame()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_GameGet");

			var leaderboardNames = new[]
			{
				"CanGetLeaderboardsByGame1",
				"CanGetLeaderboardsByGame2",
				"CanGetLeaderboardsByGame3"
			};

			foreach (var name in leaderboardNames)
			{
				var createRequest = new LeaderboardRequest
				{
					Token = name,
					GameId = game.Id,
					Name = name,
					Key = name,
					ActorType = ActorType.User,
					EvaluationDataType = EvaluationDataType.Long,
					CriteriaScope = CriteriaScope.Actor,
					LeaderboardType = LeaderboardType.Highest
				};

				SUGARClient.Leaderboard.Create(createRequest);
			}

			var getResponse = SUGARClient.Leaderboard.Get(game.Id);

			Assert.Equal(3, getResponse.Count());

			var getCheck = getResponse.Where(g => leaderboardNames.Any(ln => g.Name.Contains(ln)));

			Assert.Equal(3, getCheck.Count());
		}

		[Fact]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var getResponse = SUGARClient.Leaderboard.Get(-1);

			Assert.Empty(getResponse);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.Get("CannotGetNotExistingLeaderboard", -1);

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Get");

			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.Get("", game.Id));
		}

		[Fact]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.GetGlobal("CannotGetNotExistingGlobalLeaderboard");

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.GetGlobal(""));
		}

		[Fact]
		public void CanGetGlobalLeaderboardStandings()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetGlobalLeaderboardStandings",
				Name = "CanGetGlobalLeaderboardStandings",
				Key = "CanGetGlobalLeaderboardStandings",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetUser(SUGARClient.User, $"{nameof(LeaderboardClientTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CanGetLeaderboardStandings()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetLeaderboardStandings",
				GameId = game.Id,
				Name = "CanGetLeaderboardStandings",
				Key = "CanGetLeaderboardStandings",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetUser(SUGARClient.User, $"{nameof(LeaderboardClientTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboardStandings()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = "CannotGetNotExistingLeaderboardStandings",
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var group = Helpers.GetGroup(SUGARClient.Group, $"{nameof(LeaderboardClientTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = group.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = group.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithZeroPageLimit()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				Key = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 0,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetNearLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetNearLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetNearLeaderboardStandingsWithoutActorId",
				Key = "CannotGetNearLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetFriendsLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				Key = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Friends,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetGroupMemberLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				Key = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(LeaderboardClientTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var user = Helpers.GetUser(SUGARClient.User, $"{nameof(LeaderboardClientTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = user.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}
	}
}
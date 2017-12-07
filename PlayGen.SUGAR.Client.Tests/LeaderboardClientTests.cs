using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class LeaderboardClientTests : ClientTestBase
	{
		[Fact]
		public void CanGetLeaderboardsByGame()
		{
			var key = "Leaderboard_CanGetLeaderboardsByGame";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var getResponse = Fixture.SUGARClient.Leaderboard.Get(game.Id);

			Assert.Equal(3, getResponse.Count());

			var getCheck = getResponse.Where(g => g.Name.Contains(key));

			Assert.Equal(3, getCheck.Count());
		}

		[Fact]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var key = "Leaderboard_CannotGetLeaderboardsByNotExistingGame";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getResponse = Fixture.SUGARClient.Leaderboard.Get(-1);

			Assert.Empty(getResponse);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboard()
		{
			var key = "Leaderboard_CannotGetNotExistingLeaderboard";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getResponse = Fixture.SUGARClient.Leaderboard.Get(key, -1);

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var key = "Leaderboard_CannotGetLeaderboardWithEmptyToken";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Leaderboard.Get(string.Empty, game.Id));
		}

		[Fact]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var key = "Leaderboard_CannotGetNotExistingLeaderboard";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getResponse = Fixture.SUGARClient.Leaderboard.GetGlobal(key);

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			var key = "Leaderboard_CannotGetGlobalLeaderboardWithEmptyToken";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Leaderboard.GetGlobal(string.Empty));
		}

		[Fact]
		public void CanGetGlobalLeaderboardStandings()
		{
			var key = "Leaderboard_CanGetGlobalLeaderboardStandings";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var gameData = new EvaluationDataRequest
			{
				Key = key,
				EvaluationDataType = EvaluationDataType.Long,
				CreatingActorId = loggedInAccount.User.Id,
				Value = "5",
				GameId = Platform.GlobalId
			};

			Fixture.SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0, 
				GameId = Platform.GlobalId,
				MultiplePerActor = false
			};

			var standingsResponse = Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(loggedInAccount.User.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CanGetLeaderboardStandings()
		{
			var key = "Leaderboard_CanGetLeaderboardStandings";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var gameData = new EvaluationDataRequest
			{
				Key = key,
				EvaluationDataType = EvaluationDataType.Long,
				CreatingActorId = loggedInAccount.User.Id,
				Value = "5",
				GameId = game.Id
			};

			Fixture.SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0,
				GameId = game.Id,
				MultiplePerActor = false
			};

			var standingsResponse = Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(loggedInAccount.User.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CanGetMultipleLeaderboardStandingsForActor()
		{
			var key = "Leaderboard_CanGetMultipleLeaderboardStandingsForActor";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);
			var count = 10;
			for (var i = 1; i < count+1; i++)
			{
				var gameData = new EvaluationDataRequest
				{
					Key = key,
					EvaluationDataType = EvaluationDataType.Long,
					CreatingActorId = loggedInAccount.User.Id,
					Value = i.ToString(),
					GameId = game.Id
				};

				Fixture.SUGARClient.GameData.Add(gameData);
			}

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0,
				GameId = game.Id,
				MultiplePerActor = true
			};

			var standingsResponse = Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.Equal(count, standingsResponse.Count());
			Assert.Equal(count.ToString(), standingsResponse.First().Value);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboardStandings()
		{
			var key = "Leaderboard_CannotGetNotExistingLeaderboardStandings";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithoutToken()
		{
			var key = "Leaderboard_CannotGetStandingsWithoutToken";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 0,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithoutGameId()
		{
			var key = "Leaderboard_CannotGetStandingsWithoutGameId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithoutLeaderboardFilterType()
		{
			var key = "Leaderboard_CannotGetStandingsWithoutFilterType";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithoutPageLimit()
		{
			var key = "Leaderboard_CannotGetStandingsWithoutLimit";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithoutPageOffset()
		{
			var key = "Leaderboard_CannotGetStandingsWithoutOffset";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithIncorrectActorType()
		{
			var key = "Leaderboard_CannotGetStandingsWithIncorrectActorType";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var gameData = new EvaluationDataRequest
			{
				Key = key,
				EvaluationDataType = EvaluationDataType.Long,
				CreatingActorId = loggedInAccount.User.Id,
				Value = "5",
				GameId = game.Id
			};

			Fixture.SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				ActorId = loggedInAccount.User.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetLeaderboardStandingsWithZeroPageLimit()
		{
			var key = "Leaderboard_CannotGetLeaderboardStandingsWithZeroPageLimit";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 0,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetNearLeaderboardStandingsWithoutActorId()
		{
			var key = "Leaderboard_CannotGetNearLeaderboardStandingsWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetFriendsLeaderboardStandingsWithoutActorId()
		{
			var key = "Leaderboard_CannotGetFriendsLeaderboardStandingsWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Friends,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetGroupMemberLeaderboardStandingsWithoutActorId()
		{
			var key = "Leaderboard_CannotGetGroupMemberWithoutActorId";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Fact]
		public void CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType()
		{
			var key = "Leaderboard_CannotGetGroupMembersWithIncorrectActorType";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, key);

			var gameData = new EvaluationDataRequest
			{
				Key = key,
				EvaluationDataType = EvaluationDataType.Long,
				CreatingActorId = loggedInAccount.User.Id,
				Value = "5",
				GameId = game.Id
			};

			Fixture.SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = key,
				GameId = game.Id,
				ActorId = loggedInAccount.User.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		public LeaderboardClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
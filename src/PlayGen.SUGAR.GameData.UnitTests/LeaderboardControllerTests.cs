using System;
using System.Linq;
using System.Collections.Generic;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.GameData.UnitTests
{
	public class LeaderboardControllerTests : TestController, IClassFixture<GameDataFixture>
	{
		#region Configuration
		private readonly Data.EntityFramework.Controllers.LeaderboardController _leaderboardController;
		private readonly GameData.LeaderboardController _leaderboardEvaulationController;
		private readonly GameDataController _gameDataController;

		public LeaderboardControllerTests()
		{
			_leaderboardController = new Data.EntityFramework.Controllers.LeaderboardController(NameOrConnectionString);
			_leaderboardEvaulationController = new LeaderboardController(new GameDataController(NameOrConnectionString), 
				new GroupRelationshipController(NameOrConnectionString), new UserRelationshipController(NameOrConnectionString),
				new ActorController(NameOrConnectionString), new GroupController(NameOrConnectionString),
				new UserController(NameOrConnectionString));
			_gameDataController = new GameDataController(TestController.NameOrConnectionString);
		}
		#endregion

		#region Tests
		[Fact]
		public void GetLeaderboardSumLongSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardSumLongSpeedTest", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardSumFloatSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatSpeedTest", GameDataType.Float, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardHighLongSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardHighLongSpeedTest", GameDataType.Long, LeaderboardType.Highest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardHighFloatSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatSpeedTest", GameDataType.Float, LeaderboardType.Highest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLowLongSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLowLongSpeedTest", GameDataType.Long, LeaderboardType.Lowest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLowFloatSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatSpeedTest", GameDataType.Float, LeaderboardType.Lowest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardCountStringSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardCountStringSpeedTest", GameDataType.String, LeaderboardType.Count);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardCountBoolSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolSpeedTest", GameDataType.Boolean, LeaderboardType.Count);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardEarliestStringSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringSpeedTest", GameDataType.String, LeaderboardType.Earliest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardEarliestBoolSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolSpeedTest", GameDataType.Boolean, LeaderboardType.Earliest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLatestStringSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringSpeedTest", GameDataType.String, LeaderboardType.Latest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLatestBoolSpeedTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolSpeedTest", GameDataType.Boolean, LeaderboardType.Latest);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardNearFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterSpeedTest", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Near, 100, random.Next(1, 101));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);

		}

		[Fact]
		public void GetLeaderboardFriendFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterSpeedTest", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Friends, 100, random.Next(1, 101));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
		}

		[Fact]
		public void GetLeaderboardGroupMemberFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterSpeedTest", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.GroupMembers, 100, random.Next(101, 111));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);

		}

		[Fact]
		public void CreateLeaderboardStandingRequestWithInvalidLeaderboardId()
		{
			var filter = CreateLeaderboardStandingsRequest(-1, LeaderboardFilterType.Top, 100);

			Assert.Throws<MissingRecordException>(() => _leaderboardEvaulationController.GetStandings(null, filter));
		}

		[Fact]
		public void CreateLeaderboardStandingRequestWithInvalidLimit()
		{
			var leaderboard = CreateLeaderboard("CreateLeaderboardStandingRequestWithInvalidLimit", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateNearLeaderboardStandingRequestWithInvalidActorId()
		{
			var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, -1);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateNearLeaderboardStandingRequestWithInvalidActorType()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorType", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, random.Next(101, 111));

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateNearLeaderboardStandingRequestWithNoActorId()
		{
			var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithNoActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateFriendsLeaderboardStandingRequestWithInvalidActorId()
		{
			var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, -1);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateFriendsLeaderboardStandingRequestWithInvalidActorType()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorType", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, random.Next(101, 111));

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateFriendsLeaderboardStandingRequestWithNoActorId()
		{
			var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithNoActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateMembersLeaderboardStandingRequestWithInvalidActorId()
		{
			var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, -1);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateMembersLeaderboardStandingRequestWithInvalidActorType()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorType", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0, random.Next(1, 101));

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		[Fact]
		public void CreateMembersLeaderboardStandingRequestWithNoActorId()
		{
			var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithNoActorId", GameDataType.Long, LeaderboardType.Cumulative);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 0);

			Assert.Throws<ArgumentException>(() => _leaderboardEvaulationController.GetStandings(leaderboard, filter));

		}

		public void GetLeaderboardSumLongAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardSumLongAccuracyTest", GameDataType.Long, LeaderboardType.Cumulative, "GetLeaderboardSumLongAccuracyTest");

			CreateGameData("GetLeaderboardSumLongAccuracyTest", GameDataType.Long, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardSumFloatAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatAccuracyTest", GameDataType.Float, LeaderboardType.Cumulative, "GetLeaderboardSumFloatAccuracyTest");

			CreateGameData("GetLeaderboardSumFloatAccuracyTest", GameDataType.Float, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardHighLongAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardHighLongAccuracyTest", GameDataType.Long, LeaderboardType.Highest, "GetLeaderboardHighLongAccuracyTest");

			CreateGameData("GetLeaderboardHighLongAccuracyTest", GameDataType.Long, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardHighFloatAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatAccuracyTest", GameDataType.Float, LeaderboardType.Highest, "GetLeaderboardHighFloatAccuracyTest");

			CreateGameData("GetLeaderboardHighFloatAccuracyTest", GameDataType.Float, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardLowLongAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLowLongAccuracyTest", GameDataType.Long, LeaderboardType.Lowest, "GetLeaderboardLowLongAccuracyTest");

			CreateGameData("GetLeaderboardLowLongAccuracyTest", GameDataType.Long, leaderboard.GameId.Value, true);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[randomId - 1].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardLowFloatAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatAccuracyTest", GameDataType.Float, LeaderboardType.Lowest, "GetLeaderboardLowFloatAccuracyTest");

			CreateGameData("GetLeaderboardLowFloatAccuracyTest", GameDataType.Float, leaderboard.GameId.Value, true);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[randomId - 1].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardCountStringAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardCountStringAccuracyTest", GameDataType.String, LeaderboardType.Count, "GetLeaderboardCountStringAccuracyTest");

			CreateGameData("GetLeaderboardCountStringAccuracyTest", GameDataType.String, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardCountBoolAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolAccuracyTest", GameDataType.Boolean, LeaderboardType.Count, "GetLeaderboardCountBoolAccuracyTest");

			CreateGameData("GetLeaderboardCountBoolAccuracyTest", GameDataType.Boolean, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardEarliestStringAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringAccuracyTest", GameDataType.String, LeaderboardType.Earliest, "GetLeaderboardEarliestStringAccuracyTest");

			CreateGameData("GetLeaderboardEarliestStringAccuracyTest", GameDataType.String, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[randomId - 1].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardEarliestBoolAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolAccuracyTest", GameDataType.Boolean, LeaderboardType.Earliest, "GetLeaderboardEarliestBoolAccuracyTest");

			CreateGameData("GetLeaderboardEarliestBoolAccuracyTest", GameDataType.Boolean, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[randomId - 1].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardLatestStringAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringAccuracyTest", GameDataType.String, LeaderboardType.Latest, "GetLeaderboardLatestStringAccuracyTest");

			CreateGameData("GetLeaderboardLatestStringAccuracyTest", GameDataType.String, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardLatestBoolAccuracyTest()
		{
			var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolAccuracyTest", GameDataType.Boolean, LeaderboardType.Latest, "GetLeaderboardLatestBoolAccuracyTest");

			CreateGameData("GetLeaderboardLatestBoolAccuracyTest", GameDataType.Boolean, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Top, 100);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(100, standings.Length);

			for (int i = 0; i < 5; i++)
			{
				Random random = new Random();
				int randomId = random.Next(1, 101);
				Assert.Equal(randomId, standings[filter.Limit - randomId].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardNearFilterAccuracyTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterAccuracyTest", GameDataType.Long, LeaderboardType.Lowest, "GetLeaderboardNearFilterAccuracyTest");

			CreateGameData("GetLeaderboardNearFilterAccuracyTest", GameDataType.Long, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Near, 10, random.Next(1, 101));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(10, standings.Length);

			for (int i = 0; i < 10; i++)
			{
				int offset = (filter.ActorId.Value - 1) / 10;
				Assert.Equal(offset + i + 1, standings[i].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardFriendFilterAccuracyTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterAccuracyTest", GameDataType.Long, LeaderboardType.Lowest, "GetLeaderboardFriendFilterAccuracyTest");

			CreateGameData("GetLeaderboardFriendFilterAccuracyTest", GameDataType.Long, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.Friends, 100, random.Next(1, 91));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(21, standings.Length);

			for (int i = 0; i < 21; i++)
			{
				Assert.Equal(filter.ActorId.Value - 10 + i, standings[i].ActorId);
			}
		}

		[Fact]
		public void GetLeaderboardGroupMemberFilterAccuracyTest()
		{
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterAccuracyTest", GameDataType.Long, LeaderboardType.Lowest, "GetLeaderboardGroupMemberFilterAccuracyTest");

			CreateGameData("GetLeaderboardGroupMemberFilterAccuracyTest", GameDataType.Long, leaderboard.GameId.Value);

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Id, LeaderboardFilterType.GroupMembers, 100, random.Next(101, 111));

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(10, standings.Length);

			for (int i = 0; i < 10; i++)
			{
				int offset = (filter.ActorId.Value - 101) * 10;
				Assert.Equal(offset + i + 1, standings[i].ActorId);
			}
		}
		#endregion

		#region Helpers
		private Leaderboard CreateLeaderboard(string name, GameDataType dataType, LeaderboardType boardType, string customKey = "")
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = name,
				Token = name,
				Key = dataType.ToString(),
				GameId = random.Next(1, 101),
				ActorType = ActorType.User,
				GameDataType = dataType,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = boardType
			};

			if (customKey.Length > 0)
			{
				leaderboard.Key = customKey;
			}

			_leaderboardController.Create(leaderboard);
			return leaderboard;
		}

		private LeaderboardStandingsRequest CreateLeaderboardStandingsRequest(int id, LeaderboardFilterType filterType, int limit, int actorId = 0)
		{
			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = id,
				LeaderboardFilterType = filterType,
				Limit = limit,
				Offset = 0
			};

			if (actorId != 0)
			{
				filter.ActorId = actorId;
			}

			return filter;
		}

		private void CreateGameData(string key, GameDataType type, int gameId = 0, bool singular = false)
		{
			Random random = new Random();
			List<Data.Model.GameData> data = new List<Data.Model.GameData>();
			for (int i = 1; i <= 100; i++)
			{
				for (int j = i; j > 0; j--)
				{
					var gameData = new Data.Model.GameData
					{
						ActorId = i,
						GameId = gameId,
						Key = key,
						Value = j.ToString(),
						DataType = type
					};
					if (type == GameDataType.Float)
					{
						gameData.Value = (j * 0.01f).ToString();
					}
					else if (type == GameDataType.Boolean)
					{
						gameData.Value = (j % 2 == 0 ? true : false).ToString();
					}
					data.Add(gameData);
					if (singular)
					{
						break;
					}
				}
			}
			_gameDataController.Create(data.ToArray());
		}
		#endregion
	}
}
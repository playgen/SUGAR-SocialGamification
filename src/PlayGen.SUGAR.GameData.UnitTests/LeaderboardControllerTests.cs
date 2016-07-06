using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using Xunit;

namespace PlayGen.SUGAR.GameData.UnitTests
{
	public class LeaderboardControllerTests : TestController, IClassFixture<GameDataFixture>
	{
		#region Configuration
		private readonly Data.EntityFramework.Controllers.LeaderboardController _leaderboardController;
		private readonly GameData.LeaderboardController _leaderboardEvaulationController;

		public LeaderboardControllerTests()
		{
			_leaderboardController = new Data.EntityFramework.Controllers.LeaderboardController(NameOrConnectionString);
			_leaderboardEvaulationController = new LeaderboardController(new GameDataController(NameOrConnectionString), 
				new GroupRelationshipController(NameOrConnectionString), new UserRelationshipController(NameOrConnectionString),
				new ActorController(NameOrConnectionString), new GroupController(NameOrConnectionString),
				new UserController(NameOrConnectionString));
		}
		#endregion

		#region Tests
		[Fact]
		public void GetLeaderboardSumLongSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardSumLongSpeedTest",
				Token = "GetLeaderboardSumLongSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Cumulative
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardSumFloatSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardSumFloatSpeedTest",
				Token = "GetLeaderboardSumFloatSpeedTest",
				Key = "Float",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Cumulative
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardHighLongSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardHighLongSpeedTest",
				Token = "GetLeaderboardHighLongSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardHighFloatSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardHighFloatSpeedTest",
				Token = "GetLeaderboardHighFloatSpeedTest",
				Key = "Float",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLowLongSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLowLongSpeedTest",
				Token = "GetLeaderboardLowLongSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLowFloatSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLowFloatSpeedTest",
				Token = "GetLeaderboardLowFloatSpeedTest",
				Key = "Float",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardCountStringSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardCountStringSpeedTest",
				Token = "GetLeaderboardCountStringSpeedTest",
				Key = "String",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.String,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardCountBoolSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardCountBoolSpeedTest",
				Token = "GetLeaderboardCountBoolSpeedTest",
				Key = "Boolean",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Boolean,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardEarliestStringSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardEarliestStringSpeedTest",
				Token = "GetLeaderboardEarliestStringSpeedTest",
				Key = "String",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.String,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Earliest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardEarliestBoolSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardEarliestBoolSpeedTest",
				Token = "GetLeaderboardEarliestBoolSpeedTest",
				Key = "Boolean",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Boolean,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Earliest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLatestStringSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLatestStringSpeedTest",
				Token = "GetLeaderboardLatestStringSpeedTest",
				Key = "String",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.String,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Latest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardLatestBoolSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLatestBoolSpeedTest",
				Token = "GetLeaderboardLatestBoolSpeedTest",
				Key = "Boolean",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Boolean,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Latest
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);
			
		}

		[Fact]
		public void GetLeaderboardNearFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardNearFilterSpeedTest",
				Token = "GetLeaderboardNearFilterSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Cumulative
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				ActorId = random.Next(0, 100),
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);

		}

		[Fact]
		public void GetLeaderboardFriendFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardFriendFilterSpeedTest",
				Token = "GetLeaderboardFriendFilterSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Cumulative
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.Friends,
				ActorId = random.Next(0, 100),
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);

		}

		[Fact]
		public void GetLeaderboardGroupMemberFilterSpeedTest()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardGroupMemberFilterSpeedTest",
				Token = "GetLeaderboardGroupMemberFilterSpeedTest",
				Key = "Long",
				GameId = random.Next(0, 100),
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Cumulative
			};

			_leaderboardController.Create(leaderboard);

			var filter = new LeaderboardStandingsRequest
			{
				LeaderboardId = leaderboard.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				ActorId = random.Next(100, 110),
				Limit = 100,
				Offset = 0
			};

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter);

		}
		#endregion

		#region Helpers

		#endregion
	}
}
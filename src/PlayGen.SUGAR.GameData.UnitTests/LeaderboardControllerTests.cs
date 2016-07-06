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
		public void GetLeaderboardSumLong()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardSumLong",
				Token = "GetLeaderboardSumLong",
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
		public void GetLeaderboardSumFloat()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardSumFloat",
				Token = "GetLeaderboardSumFloat",
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
		public void GetLeaderboardHighLong()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardHighLong",
				Token = "GetLeaderboardHighLong",
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
		public void GetLeaderboardHighFloat()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardHighFloat",
				Token = "GetLeaderboardHighFloat",
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
		public void GetLeaderboardLowLong()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLowLong",
				Token = "GetLeaderboardLowLong",
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
		public void GetLeaderboardLowFloat()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLowFloat",
				Token = "GetLeaderboardLowFloat",
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
		public void GetLeaderboardCountString()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardCountString",
				Token = "GetLeaderboardCountString",
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
		public void GetLeaderboardCountBool()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardCountBool",
				Token = "GetLeaderboardCountBool",
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
		public void GetLeaderboardEarliestString()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardEarliestString",
				Token = "GetLeaderboardEarliestString",
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
		public void GetLeaderboardEarliestBool()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardEarliestBool",
				Token = "GetLeaderboardEarliestBool",
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
		public void GetLeaderboardLatestString()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLatestString",
				Token = "GetLeaderboardLatestString",
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
		public void GetLeaderboardLatestBool()
		{
			Random random = new Random();
			var leaderboard = new Leaderboard
			{
				Name = "GetLeaderboardLatestBool",
				Token = "GetLeaderboardLatestBool",
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
		#endregion

		#region Helpers

		#endregion
	}
}
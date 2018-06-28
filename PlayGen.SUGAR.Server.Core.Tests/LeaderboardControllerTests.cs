using System;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.Core.EvaluationEvents;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;
using DbControllerLocator = PlayGen.SUGAR.Server.EntityFramework.Tests.ControllerLocator;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	public class LeaderboardControllerTests : CoreTestFixtureBase
    {
        private const int GlobalAverageLeaderboardStandingsExecutionMilliseconds = 2000;
	    private const int GlobalLeaderboardStandingsExecutionCount = 5;
        private readonly Controllers.LeaderboardController _leaderboardCoreController = ControllerLocator.LeaderboardController;
        private readonly LeaderboardController _leaderboardDbController = DbControllerLocator.LeaderboardController;

	    public LeaderboardControllerTests(CoreTestFixture fixture) : base(fixture)
	    {
	    }

        [Fact]
		public void GetLeaderboardSumLongSpeedTest()
		{
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
			
            // Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
		}

        [Fact]
        public void GetLeaderboardSumFloatSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardHighLongSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Highest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardHighFloatSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Highest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

            // Act & Assert
            AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardLowLongSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Lowest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardLowFloatSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Lowest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardCountStringSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.String, LeaderboardType.Count);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardCountBoolSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Boolean, LeaderboardType.Count);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardEarliestStringSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.String, LeaderboardType.Earliest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardEarliestBoolSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Boolean, LeaderboardType.Earliest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardLatestStringSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.String, LeaderboardType.Latest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void GetLeaderboardLatestBoolSpeedTest()
        {
			// Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Boolean, LeaderboardType.Latest);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Theory]
		[InlineData(74)]
		[InlineData(8)]
        public void GetLeaderboardNearFilterSpeedTest(int userIndex)
        {
			// Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{userIndex}";
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, Fixture.SortedUsers[userIndex].Id);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Theory]
		[InlineData(18)]
		[InlineData(95)]
        public void GetLeaderboardFriendFilterSpeedTest(int userIndex)
        {
            // Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{userIndex}";
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.SortedUsers[userIndex].Id);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Theory]
		[InlineData(2)]
		[InlineData(8)]
        public void GetLeaderboardGroupMemberFilterSpeedTest(int groupIndex)
        {
            // Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{groupIndex}";
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, Fixture.SortedGroups[groupIndex].Id);

			// Act & Assert
			AssertUtil.ExecutionTimeAssert(() => _leaderboardCoreController.GetStandings(leaderboard, filter), GlobalAverageLeaderboardStandingsExecutionMilliseconds, GlobalLeaderboardStandingsExecutionCount);
        }

        [Fact]
        public void CantCreateLeaderboardStandingRequestWithInvalidLeaderboardId()
		{
			// Arrange
			var token = "";
            var filter = CreateLeaderboardStandingsRequest(token, -1, LeaderboardFilterType.Top);

			// Act & Assert
            Assert.Throws<MissingRecordException>(() => _leaderboardCoreController.GetStandings(null, filter));
        }

		[Fact]
        public void CantCreateNearLeaderboardStandingRequestWithInvalidActorId()
        {
			// Arrange
            var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, -1);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateNearLeaderboardStandingRequestWithNoActorId()
        {
			// Arrange
            var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateFriendsLeaderboardStandingRequestWithInvalidActorId()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, -1);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateFriendsLeaderboardStandingRequestWithInvalidActorType()
        {
            // Arrange
            var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
			// Using a group's id instead of a user's id to throw the error caught below
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.SortedGroups[0].Id);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateFriendsLeaderboardStandingRequestWithNoActorId()
        {
			// Arrange
            var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateMembersLeaderboardStandingRequestWithInvalidActorId()
        {
			// Arrange
            var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, -1);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
		public void CantCreateMembersLeaderboardStandingRequestWithInvalidActorType()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
			// Using a user's id instead of a group's id to throw the error caught below
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, Fixture.SortedUsers[0].Id);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

        [Fact]
        public void CantCreateMembersLeaderboardStandingRequestWithNoActorId()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0);

			// Act & Assert
            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));
        }

		[Fact]
        public void GetLeaderboardSumLongAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardSumFloatAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Cumulative);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardHighLongAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Highest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardHighFloatAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Highest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardLowLongAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Lowest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardLowFloatAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Float, LeaderboardType.Lowest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardCountStringAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.String, LeaderboardType.Count);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardCountBoolAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Boolean, LeaderboardType.Count);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);
			
			for(var i = 0; i < filter.PageLimit; i++)
	        {
                // Assumptions:
                // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
                // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
				// So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.
				
                Assert.Equal(Fixture.SortedUsers[Fixture.SortedUsers.Count - (i + 1)].Id, standings[i].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardEarliestStringAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.String, LeaderboardType.Earliest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardEarliestBoolAccuracyTest()
        {
            // Arrange
			var token = System.Reflection.MethodBase.GetCurrentMethod().Name;
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Boolean, LeaderboardType.Earliest);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
			Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

	        for (var i = 0; i < standings.Length; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
		        // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

		        Assert.Equal(Fixture.SortedUsers[i].Id, standings[i].ActorId);
	        }
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(22, 0)]
        [InlineData(58, 0)]
        [InlineData(9, 10)]
	    [InlineData(32, 31)]
	    [InlineData(46, 20)]
	    public void LimitsStandingsAndOffset(int pageLimit, int offset)
	    {
		    // Arrange
		    var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{pageLimit}_{offset}";
		    var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);

		    // Act
		    var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, limit: pageLimit, offset: offset);
		    var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

		    // Assert
		    Assert.Equal(pageLimit == 0 ? Fixture.SortedUsers.Count : filter.PageLimit, standings.Length);

		    for (var i = 0; i < standings.Length; i++)
		    {
			    // Assumptions:
			    // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
			    // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
			    // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

				Assert.Equal(Fixture.SortedUsers[(Fixture.SortedUsers.Count -1) - (i + offset)].Id, standings[i].ActorId);
		    }
        }

	    [Theory]
	    [InlineData(9, 0)]
	    [InlineData(32, 0)]
	    [InlineData(46, 0)]
        [InlineData(9, 10)]
	    [InlineData(32, 31)]
	    [InlineData(46, 20)]
	    public void LimitsStandingsAndOffsetNear(int pageLimit, int offset)
	    {
		    // Arrange
		    var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{pageLimit}_{offset}";
		    var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative);
		    var actorIndex = Fixture.SortedUsers.Count / 2;
            var actorId = Fixture.SortedUsers[actorIndex].Id;
		    var firstIndex = (actorIndex - ((pageLimit - 1) / 2)) - 1;

		    // Act
		    var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, actorId, pageLimit, offset);
		    var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            // Assert
		    Assert.Equal(pageLimit == 0 ? Fixture.SortedUsers.Count : filter.PageLimit, standings.Length);

            for (var i = 0; i < standings.Length; i++)
		    {
			    // Assumptions:
			    // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
			    // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
			    // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

			    Assert.Equal(Fixture.SortedUsers[(Fixture.SortedUsers.Count - 1) - (i + offset + firstIndex)].Id, standings[i].ActorId);
		    }
        }

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestStringAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, LeaderboardType.Latest, "GetLeaderboardLatestStringAccuracyTest");

        //	CreateEvaluationDataAscending("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, Fixture.SortedUsers.Count + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestBoolAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, LeaderboardType.Latest, "GetLeaderboardLatestBoolAccuracyTest");

        //	CreateEvaluationDataAscending("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, Fixture.SortedUsers.Count + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}
		
        [Theory]
		[InlineData(5)]
		[InlineData(63)]
        public void GetLeaderboardFriendFilterAccuracyTest(int userIndex)
        {
            // Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{userIndex}";
            var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Lowest);

            // Act
	        var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.SortedUsers[userIndex].Id);
	        var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            // Assert
            // Assumptions: Each user is created with [CoreTestFixture.FriendCount] friends
            Assert.Equal(CoreTestFixture.FriendCount + 1, standings.Length);

            // Assumptions:
            // Each user in Fixture.SortedUsers was created in sequence so the ActorId of each user in Fixture.SortedUsers is in ascending order.
            // Each user in Fixture.SortedUsers has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
            // So the leaderboard order should match that of [Fixture.SortedUsers reversed] for ascending evaluations.

	        for (var i = 0; i < standings.Length - 1; i++)
	        {
				Assert.True(standings[i].ActorId < standings[i + 1].ActorId);
	        }
        }

		[Theory]
		[InlineData(LeaderboardFilterType.Top, 15, 0, CoreTestFixture.UserCount)]
		[InlineData(LeaderboardFilterType.Top, 15, 1, CoreTestFixture.UserCount)]
		[InlineData(LeaderboardFilterType.Near, 15, 0, CoreTestFixture.UserCount)]
		[InlineData(LeaderboardFilterType.Near, 15, 1, CoreTestFixture.UserCount)]
        public void CanHandleActorIndexOutOfRange(LeaderboardFilterType filterType, int userIndex, int offset, int limit)
		{
			// Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{filterType}_{userIndex}_{offset}_{limit}";
			TestNoStandingsResult(token, filterType, userIndex, offset, limit);

		}

		[Theory]
		[InlineData(LeaderboardFilterType.Top, 15, 0, 10)]
		[InlineData(LeaderboardFilterType.Top, 15, 1, 10)]
		[InlineData(LeaderboardFilterType.Near, 15, 0, 10)]
		[InlineData(LeaderboardFilterType.Near, 15, 1, 10)]
		public void CanHandleNoResultsForKey(LeaderboardFilterType filterType, int userIndex, int offset, int limit)
		{
			// Arrange
			var token = $"{System.Reflection.MethodBase.GetCurrentMethod().Name}_{filterType}_{userIndex}_{offset}_{limit}";
			TestNoStandingsResult(token, filterType, userIndex, offset, limit);

		}

        private void TestNoStandingsResult(string token, LeaderboardFilterType filterType, int userIndex, int offset, int limit)
		{
			// Arrange
			var leaderboard = CreateLeaderboard(token, EvaluationDataType.Long, LeaderboardType.Cumulative, $"{token}_ExpectedNoDataMatch");

			// Act
			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, filterType, Fixture.SortedUsers[userIndex].Id, offset, limit);
			var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
			Assert.Empty(standings);
		}

        /* todo: fix
        [Fact]
        public void GetLeaderboardGroupMemberFilterAccuracyTest()
        {
            // this part of the leaderboard logic doesn't currently work correctly
            
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardGroupMemberFilterAccuracyTest");

			var users = CreateUsers(EvaluationDataFixture.SortedUsers.Count, "GetLeaderboardGroupMemberFilterAccuracyTest");
			CreateEvaluationDataAscending(users, "GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);
		    CreateGroupMembership("GetLeaderboardGroupMemberFilterAccuracyTest", users);
            
			var pageLimit = EvaluationDataFixture.SortedUsers.Count/3;
			var actorId = users.ElementAt(random.Next(0, users.Count())).Id;

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, pageLimit, actorId);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(Fixture.SortedUsers.Count, standings.Length);

			for (int i = 0; i < filter.PageLimit; i++)
			{
				int offset = (filter.ActorId.Value - EvaluationDataFixture.SortedUsers.Count - 1) * filter.PageLimit;
				Assert.Equal(offset + i + 1, standings[i].ActorId);
			}
        }*/

        #region Helpers
        private Leaderboard CreateLeaderboard(string token, EvaluationDataType dataType, LeaderboardType boardType, string overrideDataKey = null)
        {
            var leaderboard = new Leaderboard
            {
                Name = token,
                Token = token,
                EvaluationDataKey = overrideDataKey ?? Fixture.GenerateEvaluationDataKey(dataType),
                GameId = Fixture.EvaluationDataGameId,
                ActorType = ActorType.User,
                EvaluationDataType = dataType,
                CriteriaScope = CriteriaScope.Actor,
                LeaderboardType = boardType
            };
			
            _leaderboardDbController.Create(leaderboard);
            return leaderboard;
        }

        private StandingsRequest CreateLeaderboardStandingsRequest(string token, int gameId, LeaderboardFilterType filterType, int actorId = 0, int limit = 0, int offset = 0)
        {
            var filter = new StandingsRequest
            {
                LeaderboardToken = token,
                GameId = gameId,
                LeaderboardFilterType = filterType,
				PageLimit = limit,
				PageOffset = offset
            };

            if (actorId != 0)
            {
                filter.ActorId = actorId;
            }

            return filter;
        }
        #endregion
    }
}
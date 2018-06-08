using System;
using System.Collections.Generic;
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
	// todo Change to user core controllers
	public class LeaderboardControllerTestsFixture : CoreTestFixtureBase
    {
        public LeaderboardControllerTestsFixture(CoreTestFixture fixture) : base(fixture)
	    {
	    }

	    #region Configuration

        private readonly Controllers.LeaderboardController _leaderboardCoreController = ControllerLocator.LeaderboardController;

        private readonly LeaderboardController _leaderboardDbController = DbControllerLocator.LeaderboardController;
        private readonly EvaluationDataController _evaluationDataDbController = DbControllerLocator.EvaluationDataController;
        private readonly UserController _userDbController = DbControllerLocator.UserController;
        private readonly GroupController _groupDbController = DbControllerLocator.GroupController;
        private readonly RelationshipController _relationshipDbController = DbControllerLocator.RelationshipController;
        #endregion

        #region Tests
        [Fact]
        public void GetLeaderboardSumLongSpeedTest()
        {
	        var key = nameof(GetLeaderboardSumLongSpeedTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Cumulative);

	        //CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardSumFloatSpeedTest()
        {
            var key = nameof(GetLeaderboardSumFloatSpeedTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Float, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighLongSpeedTest", EvaluationDataType.Long, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatSpeedTest", EvaluationDataType.Float, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowLongSpeedTest", EvaluationDataType.Long, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatSpeedTest", EvaluationDataType.Float, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountStringSpeedTest", EvaluationDataType.String, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringSpeedTest", EvaluationDataType.String, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringSpeedTest", EvaluationDataType.String, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardNearFilterSpeedTest()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, Fixture.Users.Count, random.Next(1, Fixture.Users.Count + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardFriendFilterSpeedTest()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.Users.Count, random.Next(1, Fixture.Users.Count + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardGroupMemberFilterSpeedTest()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, Fixture.Users.Count, Fixture.Users.Count + Fixture.Groups.Count + 1);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void CreateLeaderboardStandingRequestWithInvalidLeaderboardId()
        {
            var filter = CreateLeaderboardStandingsRequest("", -1, LeaderboardFilterType.Top, Fixture.Users.Count);

            Assert.Throws<MissingRecordException>(() => _leaderboardCoreController.GetStandings(null, filter));
        }

        [Fact]
        public void CreateLeaderboardStandingRequestWithInvalidLimit()
        {
            var leaderboard = CreateLeaderboard("CreateLeaderboardStandingRequestWithInvalidLimit", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, Fixture.Users.Count, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithInvalidActorType()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0, random.Next(Fixture.Users.Count + 1, Fixture.Users.Count + Fixture.Groups.Count + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithNoActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.Users.Count, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithInvalidActorType()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0, random.Next(Fixture.Users.Count + 1, Fixture.Users.Count + Fixture.Groups.Count + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithNoActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, Fixture.Users.Count, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithInvalidActorType()
        {
            var random = new Random();
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0, random.Next(1, Fixture.Users.Count + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithNoActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

		[Fact]
        public void GetLeaderboardSumLongAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardSumLongAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Cumulative, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardSumFloatAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardSumFloatAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Float, LeaderboardType.Cumulative, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Float, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardHighLongAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardHighLongAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Highest, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardHighFloatAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardHighFloatAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Float, LeaderboardType.Highest, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Float, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardLowLongAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardLowLongAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Lowest, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId, true);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardLowFloatAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardLowFloatAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Float, LeaderboardType.Lowest, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Float, leaderboard.GameId, true);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardCountStringAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardCountStringAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.String, LeaderboardType.Count, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.String, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardCountBoolAccuracyTest()
        {
			// Arrange
	        var key = nameof(GetLeaderboardCountBoolAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Boolean, LeaderboardType.Count, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.Boolean, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);
			
			for(var i = 0; i < filter.PageLimit; i++)
	        {
                // Assumptions:
                // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
                // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
				// So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.
				
                Assert.Equal(Fixture.Users[Fixture.Users.Count - (i + 1)].Id, standings[i].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardEarliestStringAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardEarliestStringAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.String, LeaderboardType.Earliest, key);
            CreateEvaluationDataAscending(key, EvaluationDataType.String, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);
            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[i].Id, standings[i].ActorId);
	        }
        }

        [Fact]
        public void GetLeaderboardEarliestBoolAccuracyTest()
        {
			// Arrange
            var key = nameof(GetLeaderboardEarliestBoolAccuracyTest);
            var leaderboard = CreateLeaderboard(key, EvaluationDataType.Boolean, LeaderboardType.Earliest, key);

            CreateEvaluationDataAscending(key, EvaluationDataType.Boolean, leaderboard.GameId);

			// Act
            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

			// Assert
            Assert.Equal(filter.PageLimit, standings.Length);

	        for (var i = 0; i < filter.PageLimit; i++)
	        {
		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        Assert.Equal(Fixture.Users[i].Id, standings[i].ActorId);
	        }
        }

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestStringAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, LeaderboardType.Latest, "GetLeaderboardLatestStringAccuracyTest");

        //	CreateEvaluationDataAscending("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(filter.PageLimit, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, Fixture.Users.Count + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestBoolAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, LeaderboardType.Latest, "GetLeaderboardLatestBoolAccuracyTest");

        //	CreateEvaluationDataAscending("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, Fixture.Users.Count);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(filter.PageLimit, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, Fixture.Users.Count + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}

        [Fact]
        public void GetLeaderboardNearFilterAccuracyTest()
        {
	        // Arrange
	        var key = nameof(GetLeaderboardNearFilterAccuracyTest);
	        var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Lowest, key);
	        CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId, true);

            foreach (var user in Fixture.Users)
	        {
		        // Act
			    var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 10, user.Id);
		        var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

		        // Assert
		        Assert.Equal(filter.PageLimit, standings.Length);

				// Check that this user is included in the standings
				Assert.NotNull(standings.Single(s => s.ActorId == filter.ActorId));

                // Make sure the standings are in the correct order

		        // Assumptions:
		        // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
		        // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
		        // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        var firstStandingActorId = standings[0].ActorId;
		        var firstIndexInUsers = Fixture.Users.ToList().FindIndex(u => u.Id == firstStandingActorId);

		        for (var i = 0; i < standings.Length; i++)
		        {
					Assert.Equal(Fixture.Users[firstIndexInUsers + i].Id, standings[i].ActorId);
		        }
	        }
        }

        [Fact]
        public void GetLeaderboardFriendFilterAccuracyTest()
        {
	        // Arrange
	        var key = nameof(GetLeaderboardFriendFilterAccuracyTest);
	        var leaderboard = CreateLeaderboard(key, EvaluationDataType.Long, LeaderboardType.Lowest, key);
	        CreateEvaluationDataAscending(key, EvaluationDataType.Long, leaderboard.GameId, true);

            foreach (var user in Fixture.Users)
	        {
		        // Act
		        var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, Fixture.Users.Count, user.Id);
		        var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

                // Assert
                // Assumptions: Each user is created with [CoreTestFixture.FriendCount] friends
                Assert.Equal(CoreTestFixture.FriendCount + 1, standings.Length);

                // Assumptions:
                // Each user in Fixture.Users was created in sequence so the ActorId of each user in Fixture.Users is in ascending order.
                // Each user in Fixture.Users has its index in the list + 1 amount of evaluation data - created by CreateEvaluationDataAscending
                // So the leaderboard order should match that of [Fixture.Users reversed] for ascending evaluations.

		        for (var i = 0; i < standings.Length - 1; i++)
		        {
					Assert.True(standings[i].ActorId < standings[i + 1].ActorId);
		        }
	        }
        }

		/* todo: fix
        [Fact]
        public void GetLeaderboardGroupMemberFilterAccuracyTest()
        {
            // this part of the leaderboard logic doesn't currently work correctly
            
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardGroupMemberFilterAccuracyTest");

			var users = CreateUsers(EvaluationDataFixture.Users.Count, "GetLeaderboardGroupMemberFilterAccuracyTest");
			CreateEvaluationDataAscending(users, "GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);
		    CreateGroupMembership("GetLeaderboardGroupMemberFilterAccuracyTest", users);
            
			var pageLimit = EvaluationDataFixture.Users.Count/3;
			var actorId = users.ElementAt(random.Next(0, users.Count())).Id;

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, pageLimit, actorId);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(filter.PageLimit, standings.Length);

			for (int i = 0; i < filter.PageLimit; i++)
			{
				int offset = (filter.ActorId.Value - EvaluationDataFixture.Users.Count - 1) * filter.PageLimit;
				Assert.Equal(offset + i + 1, standings[i].ActorId);
			}
        }*/
        #endregion

        #region Helpers
        private void CreateGroupMembership(string name, IEnumerable<User> users)
        {
            var group = new Group
            {
                Name = name
            };

            _groupDbController.Create(group);

            group = _groupDbController.Get(group.Name).ElementAt(0);

            foreach (var user in users)
            {
                _relationshipDbController.CreateRelationship(new ActorRelationship
                {
                    AcceptorId = group.Id,
                    RequestorId = user.Id,
                });
            }
        }

        private IEnumerable<User> CreateUsers(int count, string uniqueNamePrefix)
        {
            var users = new List<User>();

            for (var i = 0; i < count; i++)
            {
                var user = new User
                {
                    Name = $"{uniqueNamePrefix}_{i}"
                };

                _userDbController.Create(user);

                user = _userDbController.Search(user.Name, true).ElementAt(0);
                users.Add(user);
            }

            return users;
        }


        private Leaderboard CreateLeaderboard(string name, EvaluationDataType dataType, LeaderboardType boardType, string customKey = "")
        {
            var random = new Random();
            var leaderboard = new Leaderboard
            {
                Name = name,
                Token = name,
                EvaluationDataKey = dataType.ToString(),
                GameId = random.Next(1, CoreTestFixture.GameCount + 1),
                ActorType = ActorType.User,
                EvaluationDataType = dataType,
                CriteriaScope = CriteriaScope.Actor,
                LeaderboardType = boardType
            };

            if (customKey.Length > 0)
            {
                leaderboard.EvaluationDataKey = customKey;
            }

            _leaderboardDbController.Create(leaderboard);
            return leaderboard;
        }

        private StandingsRequest CreateLeaderboardStandingsRequest(string token, int gameId, LeaderboardFilterType filterType, int limit, int actorId = 0)
        {
            var filter = new StandingsRequest
            {
                LeaderboardToken = token,
                GameId = gameId,
                LeaderboardFilterType = filterType,
                PageLimit = limit,
                PageOffset = 0
            };

            if (actorId != 0)
            {
                filter.ActorId = actorId;
            }

            return filter;
        }

        private void CreateEvaluationDataAscending(IEnumerable<User> users, string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
            var usersData = new List<EvaluationData>();

            foreach (var user in users)
            {
                CreateEvaluationDataAscending(user.Id, key, type, gameId = 0, singular);
            }
        }

        private void CreateEvaluationDataAscending(int userId, string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
            var random = new Random(userId);

            var data = new List<EvaluationData>();
            for (var j = random.Next(1, 10); j > 0; j--)
            {
                var gameData = new EvaluationData
                {
                    ActorId = userId,
                    GameId = gameId,
                    Key = key,
                    Value = j.ToString(),
                    EvaluationDataType = type
                };

                if (type == EvaluationDataType.Float)
                {
                    gameData.Value = (j * 0.01f).ToString();
                }
                else if (type == EvaluationDataType.Boolean)
                {
                    gameData.Value = (j % 2 == 0 ? true : false).ToString();
                }

                data.Add(gameData);

                if (singular)
                {
                    break;
                }
            }

            _evaluationDataDbController.Create(data.ToList());
        }

        private void CreateEvaluationDataAscending(string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
	        // Each user will have that user's index + 1 amount of EvaluationData (unless singular is defined)
	        // i.e:
	        // users[0] will have 1 EvaluationData
	        // users[1] will have 2 EvaluationData
	        // users[2] will have 3 EvaluationData
            for (var userIndex = 0; userIndex < Fixture.Users.Count; userIndex++)
            {
                var data = new List<EvaluationData>();
                for (var userDataIndex = 0; userDataIndex < userIndex + 1 && (!singular || userDataIndex < 1); userDataIndex++)
                {
                    var gameData = new EvaluationData
                    {
                        ActorId = Fixture.Users[userIndex].Id,
                        GameId = gameId,
                        Key = key,
                        Value = userDataIndex.ToString(),
                        EvaluationDataType = type
                    };

                    if (type == EvaluationDataType.Float)
                    {
                        gameData.Value = (userDataIndex * 0.01f).ToString();
                    }
                    else if (type == EvaluationDataType.Boolean)
                    {
                        gameData.Value = (userDataIndex % 2 == 0).ToString();
                    }

                    data.Add(gameData);
                }

                _evaluationDataDbController.Create(data.ToList());
            }
        }
        #endregion
    }
}
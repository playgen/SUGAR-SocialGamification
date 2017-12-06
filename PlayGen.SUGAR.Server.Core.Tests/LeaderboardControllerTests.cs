using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;
using DbControllerLocator = PlayGen.SUGAR.Server.EntityFramework.Tests.ControllerLocator;

namespace PlayGen.SUGAR.Server.Core.Tests
{
	// todo Change to user core controllers
	// Values ensured to not be nulled by model validation
	[SuppressMessage("ReSharper", "PossibleInvalidOperationException")]
	public class LeaderboardControllerTests : CoreTestBase, IClassFixture<TestDataFixture>
    {
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
            var leaderboard = CreateLeaderboard("GetLeaderboardSumLongSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardSumFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatSpeedTest", EvaluationDataType.Float, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighLongSpeedTest", EvaluationDataType.Long, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatSpeedTest", EvaluationDataType.Float, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowLongSpeedTest", EvaluationDataType.Long, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatSpeedTest", EvaluationDataType.Float, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountStringSpeedTest", EvaluationDataType.String, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringSpeedTest", EvaluationDataType.String, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringSpeedTest", EvaluationDataType.String, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolSpeedTest", EvaluationDataType.Boolean, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardNearFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, TestDataFixture.UserCount, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardFriendFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, TestDataFixture.UserCount, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardGroupMemberFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterSpeedTest", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, TestDataFixture.UserCount, random.Next(TestDataFixture.UserCount + 1, TestDataFixture.UserCount + TestDataFixture.GroupCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void CreateLeaderboardStandingRequestWithInvalidLeaderboardId()
        {
            var filter = CreateLeaderboardStandingsRequest("", -1, LeaderboardFilterType.Top, TestDataFixture.UserCount);

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

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0, random.Next(TestDataFixture.UserCount + 1, TestDataFixture.UserCount + TestDataFixture.GroupCount + 1));

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

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0, random.Next(TestDataFixture.UserCount + 1, TestDataFixture.UserCount + TestDataFixture.GroupCount + 1));

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

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorType", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0, random.Next(1, TestDataFixture.UserCount + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithNoActorId", EvaluationDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        public void GetLeaderboardSumLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumLongAccuracyTest", EvaluationDataType.Long, LeaderboardType.Cumulative, "GetLeaderboardSumLongAccuracyTest");

            CreateEvaluationData("GetLeaderboardSumLongAccuracyTest", EvaluationDataType.Long, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardSumFloatAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatAccuracyTest", EvaluationDataType.Float, LeaderboardType.Cumulative, "GetLeaderboardSumFloatAccuracyTest");

            CreateEvaluationData("GetLeaderboardSumFloatAccuracyTest", EvaluationDataType.Float, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardHighLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighLongAccuracyTest", EvaluationDataType.Long, LeaderboardType.Highest, "GetLeaderboardHighLongAccuracyTest");

            CreateEvaluationData("GetLeaderboardHighLongAccuracyTest", EvaluationDataType.Long, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardHighFloatAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatAccuracyTest", EvaluationDataType.Float, LeaderboardType.Highest, "GetLeaderboardHighFloatAccuracyTest");

            CreateEvaluationData("GetLeaderboardHighFloatAccuracyTest", EvaluationDataType.Float, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardLowLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowLongAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardLowLongAccuracyTest");

            CreateEvaluationData("GetLeaderboardLowLongAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[randomId - 1].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardLowFloatAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatAccuracyTest", EvaluationDataType.Float, LeaderboardType.Lowest, "GetLeaderboardLowFloatAccuracyTest");

            CreateEvaluationData("GetLeaderboardLowFloatAccuracyTest", EvaluationDataType.Float, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[randomId - 1].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardCountStringAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountStringAccuracyTest", EvaluationDataType.String, LeaderboardType.Count, "GetLeaderboardCountStringAccuracyTest");

            CreateEvaluationData("GetLeaderboardCountStringAccuracyTest", EvaluationDataType.String, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardCountBoolAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolAccuracyTest", EvaluationDataType.Boolean, LeaderboardType.Count, "GetLeaderboardCountBoolAccuracyTest");

            CreateEvaluationData("GetLeaderboardCountBoolAccuracyTest", EvaluationDataType.Boolean, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit.Value - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardEarliestStringAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringAccuracyTest", EvaluationDataType.String, LeaderboardType.Earliest, "GetLeaderboardEarliestStringAccuracyTest");

            CreateEvaluationData("GetLeaderboardEarliestStringAccuracyTest", EvaluationDataType.String, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[randomId - 1].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardEarliestBoolAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolAccuracyTest", EvaluationDataType.Boolean, LeaderboardType.Earliest, "GetLeaderboardEarliestBoolAccuracyTest");

            CreateEvaluationData("GetLeaderboardEarliestBoolAccuracyTest", EvaluationDataType.Boolean, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[randomId - 1].ActorId);
            }
        }

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestStringAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, LeaderboardType.Latest, "GetLeaderboardLatestStringAccuracyTest");

        //	CreateEvaluationData("GetLeaderboardLatestStringAccuracyTest", EvaluationDataType.String, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(filter.PageLimit, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, TestDataFixture.UserCount + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}

        // todo fix
        //[Fact]
        //public void GetLeaderboardLatestBoolAccuracyTest()
        //{
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, LeaderboardType.Latest, "GetLeaderboardLatestBoolAccuracyTest");

        //	CreateEvaluationData("GetLeaderboardLatestBoolAccuracyTest", EvaluationDataType.Boolean, leaderboard.GameId);

        //	var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

        //	var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

        //	Assert.Equal(filter.PageLimit, standings.Length);

        //	for (int i = 0; i < 5; i++)
        //	{
        //		Random random = new Random();
        //		int randomId = random.Next(1, TestDataFixture.UserCount + 1);
        //		Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
        //	}
        //}

        [Fact]
        public void GetLeaderboardNearFilterAccuracyTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardNearFilterAccuracyTest");

            CreateEvaluationData("GetLeaderboardNearFilterAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 10, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < filter.PageLimit; i++)
            {
                int offset = ((filter.ActorId.Value - 1) / (TestDataFixture.UserCount / filter.PageLimit.Value)) * filter.PageLimit.Value;
                Assert.Equal(offset + i + 1, standings[i].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardFriendFilterAccuracyTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardFriendFilterAccuracyTest");

            CreateEvaluationData("GetLeaderboardFriendFilterAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, TestDataFixture.UserCount, random.Next(TestDataFixture.FriendCount + 1, TestDataFixture.UserCount - TestDataFixture.FriendCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal((TestDataFixture.FriendCount * 2) + 1, standings.Length);

            for (int i = 0; i < (TestDataFixture.FriendCount * 2) + 1; i++)
            {
                Assert.Equal((filter.ActorId.Value - TestDataFixture.FriendCount) + i, standings[i].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardGroupMemberFilterAccuracyTest()
        {
            /* this part of the leaderboard logic doesn't currently work correctly
            
			Random random = new Random();
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, LeaderboardType.Lowest, "GetLeaderboardGroupMemberFilterAccuracyTest");

			var users = CreateUsers(EvaluationDataFixture.UserCount, "GetLeaderboardGroupMemberFilterAccuracyTest");
			CreateEvaluationData(users, "GetLeaderboardGroupMemberFilterAccuracyTest", EvaluationDataType.Long, leaderboard.GameId, true);
		    CreateGroupMembership("GetLeaderboardGroupMemberFilterAccuracyTest", users);
            
			var pageLimit = EvaluationDataFixture.UserCount/3;
			var actorId = users.ElementAt(random.Next(0, users.Count())).Id;

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, pageLimit, actorId);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(filter.PageLimit, standings.Length);

			for (int i = 0; i < filter.PageLimit; i++)
			{
				int offset = (filter.ActorId.Value - EvaluationDataFixture.UserCount - 1) * filter.PageLimit;
				Assert.Equal(offset + i + 1, standings[i].ActorId);
			}
            */
        }
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
            Random random = new Random();
            var leaderboard = new Leaderboard
            {
                Name = name,
                Token = name,
                EvaluationDataKey = dataType.ToString(),
                GameId = random.Next(1, TestDataFixture.GameCount + 1),
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

        private LeaderboardStandingsRequest CreateLeaderboardStandingsRequest(string token, int gameId, LeaderboardFilterType filterType, int limit, int actorId = 0)
        {
            var filter = new LeaderboardStandingsRequest
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

        private void CreateEvaluationData(IEnumerable<User> users, string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
            List<EvaluationData> usersData = new List<EvaluationData>();

            foreach (var user in users)
            {
                CreateEvaluationData(user.Id, key, type, gameId = 0, singular);
            }
        }

        private void CreateEvaluationData(int userId, string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
            var random = new Random(userId);

            List<EvaluationData> data = new List<EvaluationData>();
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

        private void CreateEvaluationData(string key, EvaluationDataType type, int gameId = 0, bool singular = false)
        {
            for (int i = 1; i <= TestDataFixture.UserCount; i++)
            {
                List<EvaluationData> data = new List<EvaluationData>();
                for (int j = i; j > 0; j--)
                {
                    var gameData = new EvaluationData
                    {
                        ActorId = i,
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
        }
        #endregion
    }
}
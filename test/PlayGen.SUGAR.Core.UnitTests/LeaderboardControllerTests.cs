using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Core.Controllers;
using Xunit;

using DbControllerLocator = PlayGen.SUGAR.Data.EntityFramework.UnitTests.ControllerLocator;

namespace PlayGen.SUGAR.Core.UnitTests
{
    // todo Change to user core controllers
    [Collection("Project Fixture Collection")]
    public class LeaderboardControllerTests : IClassFixture<TestDataFixture>
    {
        #region Configuration

        private readonly Controllers.LeaderboardController _leaderboardCoreController = ControllerLocator.LeaderboardController;

        private readonly Data.EntityFramework.Controllers.LeaderboardController _leaderboardDbController = DbControllerLocator.LeaderboardController;
        private readonly Data.EntityFramework.Controllers.SaveDataController _saveDataDbController = DbControllerLocator.SaveDataController;
        private readonly Data.EntityFramework.Controllers.UserController _userDbController = DbControllerLocator.UserController;
        private readonly Data.EntityFramework.Controllers.GroupController _groupDbController = DbControllerLocator.GroupController;
        private readonly Data.EntityFramework.Controllers.GroupRelationshipController _groupRelationshipDbController = DbControllerLocator.GroupRelationshipController;
        #endregion

        #region Tests
        [Fact]
        public void GetLeaderboardSumLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumLongSpeedTest", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardSumFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatSpeedTest", SaveDataType.Float, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighLongSpeedTest", SaveDataType.Long, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardHighFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatSpeedTest", SaveDataType.Float, LeaderboardType.Highest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowLongSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowLongSpeedTest", SaveDataType.Long, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLowFloatSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatSpeedTest", SaveDataType.Float, LeaderboardType.Lowest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountStringSpeedTest", SaveDataType.String, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardCountBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolSpeedTest", SaveDataType.Boolean, LeaderboardType.Count);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringSpeedTest", SaveDataType.String, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardEarliestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolSpeedTest", SaveDataType.Boolean, LeaderboardType.Earliest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestStringSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringSpeedTest", SaveDataType.String, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardLatestBoolSpeedTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolSpeedTest", SaveDataType.Boolean, LeaderboardType.Latest);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardNearFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterSpeedTest", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, TestDataFixture.UserCount, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);

        }

        [Fact]
        public void GetLeaderboardFriendFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterSpeedTest", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, TestDataFixture.UserCount, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter);
        }

        [Fact]
        public void GetLeaderboardGroupMemberFilterSpeedTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterSpeedTest", SaveDataType.Long, LeaderboardType.Cumulative);

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
            var leaderboard = CreateLeaderboard("CreateLeaderboardStandingRequestWithInvalidLimit", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithInvalidActorType", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0, random.Next(TestDataFixture.UserCount + 1, TestDataFixture.UserCount + TestDataFixture.GroupCount + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateNearLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateNearLeaderboardStandingRequestWithNoActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithInvalidActorType", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0, random.Next(TestDataFixture.UserCount + 1, TestDataFixture.UserCount + TestDataFixture.GroupCount + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateFriendsLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateFriendsLeaderboardStandingRequestWithNoActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Friends, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithInvalidActorId()
        {
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, TestDataFixture.UserCount, -1);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithInvalidActorType()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithInvalidActorType", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0, random.Next(1, TestDataFixture.UserCount + 1));

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        [Fact]
        public void CreateMembersLeaderboardStandingRequestWithNoActorId()
        {
            var leaderboard = CreateLeaderboard("CreateMembersLeaderboardStandingRequestWithNoActorId", SaveDataType.Long, LeaderboardType.Cumulative);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, 0);

            Assert.Throws<ArgumentException>(() => _leaderboardCoreController.GetStandings(leaderboard, filter));

        }

        public void GetLeaderboardSumLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumLongAccuracyTest", SaveDataType.Long, LeaderboardType.Cumulative, "GetLeaderboardSumLongAccuracyTest");

            CreateSaveData("GetLeaderboardSumLongAccuracyTest", SaveDataType.Long, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardSumFloatAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardSumFloatAccuracyTest", SaveDataType.Float, LeaderboardType.Cumulative, "GetLeaderboardSumFloatAccuracyTest");

            CreateSaveData("GetLeaderboardSumFloatAccuracyTest", SaveDataType.Float, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardHighLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighLongAccuracyTest", SaveDataType.Long, LeaderboardType.Highest, "GetLeaderboardHighLongAccuracyTest");

            CreateSaveData("GetLeaderboardHighLongAccuracyTest", SaveDataType.Long, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardHighFloatAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardHighFloatAccuracyTest", SaveDataType.Float, LeaderboardType.Highest, "GetLeaderboardHighFloatAccuracyTest");

            CreateSaveData("GetLeaderboardHighFloatAccuracyTest", SaveDataType.Float, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardLowLongAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardLowLongAccuracyTest", SaveDataType.Long, LeaderboardType.Lowest, "GetLeaderboardLowLongAccuracyTest");

            CreateSaveData("GetLeaderboardLowLongAccuracyTest", SaveDataType.Long, leaderboard.GameId, true);

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
            var leaderboard = CreateLeaderboard("GetLeaderboardLowFloatAccuracyTest", SaveDataType.Float, LeaderboardType.Lowest, "GetLeaderboardLowFloatAccuracyTest");

            CreateSaveData("GetLeaderboardLowFloatAccuracyTest", SaveDataType.Float, leaderboard.GameId, true);

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
            var leaderboard = CreateLeaderboard("GetLeaderboardCountStringAccuracyTest", SaveDataType.String, LeaderboardType.Count, "GetLeaderboardCountStringAccuracyTest");

            CreateSaveData("GetLeaderboardCountStringAccuracyTest", SaveDataType.String, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardCountBoolAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardCountBoolAccuracyTest", SaveDataType.Boolean, LeaderboardType.Count, "GetLeaderboardCountBoolAccuracyTest");

            CreateSaveData("GetLeaderboardCountBoolAccuracyTest", SaveDataType.Boolean, leaderboard.GameId);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Top, TestDataFixture.UserCount);

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                int randomId = random.Next(1, TestDataFixture.UserCount + 1);
                Assert.Equal(randomId, standings[filter.PageLimit - randomId].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardEarliestStringAccuracyTest()
        {
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestStringAccuracyTest", SaveDataType.String, LeaderboardType.Earliest, "GetLeaderboardEarliestStringAccuracyTest");

            CreateSaveData("GetLeaderboardEarliestStringAccuracyTest", SaveDataType.String, leaderboard.GameId);

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
            var leaderboard = CreateLeaderboard("GetLeaderboardEarliestBoolAccuracyTest", SaveDataType.Boolean, LeaderboardType.Earliest, "GetLeaderboardEarliestBoolAccuracyTest");

            CreateSaveData("GetLeaderboardEarliestBoolAccuracyTest", SaveDataType.Boolean, leaderboard.GameId);

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
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestStringAccuracyTest", SaveDataType.String, LeaderboardType.Latest, "GetLeaderboardLatestStringAccuracyTest");

        //	CreateSaveData("GetLeaderboardLatestStringAccuracyTest", SaveDataType.String, leaderboard.GameId);

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
        //	var leaderboard = CreateLeaderboard("GetLeaderboardLatestBoolAccuracyTest", SaveDataType.Boolean, LeaderboardType.Latest, "GetLeaderboardLatestBoolAccuracyTest");

        //	CreateSaveData("GetLeaderboardLatestBoolAccuracyTest", SaveDataType.Boolean, leaderboard.GameId);

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
            var leaderboard = CreateLeaderboard("GetLeaderboardNearFilterAccuracyTest", SaveDataType.Long, LeaderboardType.Lowest, "GetLeaderboardNearFilterAccuracyTest");

            CreateSaveData("GetLeaderboardNearFilterAccuracyTest", SaveDataType.Long, leaderboard.GameId, true);

            var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.Near, 10, random.Next(1, TestDataFixture.UserCount + 1));

            var standings = _leaderboardCoreController.GetStandings(leaderboard, filter).ToArray();

            Assert.Equal(filter.PageLimit, standings.Length);

            for (int i = 0; i < filter.PageLimit; i++)
            {
                int offset = ((filter.ActorId.Value - 1) / (TestDataFixture.UserCount / filter.PageLimit)) * filter.PageLimit;
                Assert.Equal(offset + i + 1, standings[i].ActorId);
            }
        }

        [Fact]
        public void GetLeaderboardFriendFilterAccuracyTest()
        {
            Random random = new Random();
            var leaderboard = CreateLeaderboard("GetLeaderboardFriendFilterAccuracyTest", SaveDataType.Long, LeaderboardType.Lowest, "GetLeaderboardFriendFilterAccuracyTest");

            CreateSaveData("GetLeaderboardFriendFilterAccuracyTest", SaveDataType.Long, leaderboard.GameId, true);

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
			var leaderboard = CreateLeaderboard("GetLeaderboardGroupMemberFilterAccuracyTest", SaveDataType.Long, LeaderboardType.Lowest, "GetLeaderboardGroupMemberFilterAccuracyTest");

			var users = CreateUsers(SaveDataFixture.UserCount, "GetLeaderboardGroupMemberFilterAccuracyTest");
			CreateSaveData(users, "GetLeaderboardGroupMemberFilterAccuracyTest", SaveDataType.Long, leaderboard.GameId, true);
		    CreateGroupMembership("GetLeaderboardGroupMemberFilterAccuracyTest", users);
            
			var pageLimit = SaveDataFixture.UserCount/3;
			var actorId = users.ElementAt(random.Next(0, users.Count())).Id;

			var filter = CreateLeaderboardStandingsRequest(leaderboard.Token, leaderboard.GameId, LeaderboardFilterType.GroupMembers, pageLimit, actorId);

			var standings = _leaderboardEvaulationController.GetStandings(leaderboard, filter).ToArray();

			Assert.Equal(filter.PageLimit, standings.Length);

			for (int i = 0; i < filter.PageLimit; i++)
			{
				int offset = (filter.ActorId.Value - SaveDataFixture.UserCount - 1) * filter.PageLimit;
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
                _groupRelationshipDbController.Create(new UserToGroupRelationship
                {
                    AcceptorId = group.Id,
                    RequestorId = user.Id,
                }, true);
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


        private Leaderboard CreateLeaderboard(string name, SaveDataType dataType, LeaderboardType boardType, string customKey = "")
        {
            Random random = new Random();
            var leaderboard = new Leaderboard
            {
                Name = name,
                Token = name,
                Key = dataType.ToString(),
                GameId = random.Next(1, TestDataFixture.GameCount + 1),
                ActorType = ActorType.User,
                SaveDataType = dataType,
                CriteriaScope = CriteriaScope.Actor,
                LeaderboardType = boardType
            };

            if (customKey.Length > 0)
            {
                leaderboard.Key = customKey;
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

        private void CreateSaveData(IEnumerable<User> users, string key, SaveDataType type, int gameId = 0, bool singular = false)
        {
            List<Data.Model.SaveData> usersData = new List<Data.Model.SaveData>();

            foreach (var user in users)
            {
                CreateSaveData(user.Id, key, type, gameId = 0, singular);
            }
        }

        private void CreateSaveData(int userId, string key, SaveDataType type, int gameId = 0, bool singular = false)
        {
            var random = new Random(userId);

            List<Data.Model.SaveData> data = new List<Data.Model.SaveData>();
            for (var j = random.Next(1, 10); j > 0; j--)
            {
                var gameData = new Data.Model.SaveData
                {
                    ActorId = userId,
                    GameId = gameId,
                    Key = key,
                    Value = j.ToString(),
                    SaveDataType = type
                };

                if (type == SaveDataType.Float)
                {
                    gameData.Value = (j * 0.01f).ToString();
                }
                else if (type == SaveDataType.Boolean)
                {
                    gameData.Value = (j % 2 == 0 ? true : false).ToString();
                }

                data.Add(gameData);

                if (singular)
                {
                    break;
                }
            }

            _saveDataDbController.Create(data.ToList());
        }

        private void CreateSaveData(string key, SaveDataType type, int gameId = 0, bool singular = false)
        {
            for (int i = 1; i <= TestDataFixture.UserCount; i++)
            {
                List<Data.Model.SaveData> data = new List<Data.Model.SaveData>();
                for (int j = i; j > 0; j--)
                {
                    var gameData = new Data.Model.SaveData
                    {
                        ActorId = i,
                        GameId = gameId,
                        Key = key,
                        Value = j.ToString(),
                        SaveDataType = type
                    };
                    if (type == SaveDataType.Float)
                    {
                        gameData.Value = (j * 0.01f).ToString();
                    }
                    else if (type == SaveDataType.Boolean)
                    {
                        gameData.Value = (j % 2 == 0 ? true : false).ToString();
                    }
                    data.Add(gameData);
                    if (singular)
                    {
                        break;
                    }
                }
                _saveDataDbController.Create(data.ToList());
            }
        }
        #endregion
    }
}
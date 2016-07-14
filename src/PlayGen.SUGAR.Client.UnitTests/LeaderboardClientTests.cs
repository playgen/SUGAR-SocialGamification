using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
    public class LeaderboardClientTests
    {
        #region Configuration
        private readonly LeaderboardClient _leaderboardClient;

        public LeaderboardClientTests()
        {
            var testSugarClient = new TestSUGARClient();
            _leaderboardClient = testSugarClient.Leaderboard;

            RegisterAndLogin(testSugarClient.Account);
        }

        private void RegisterAndLogin(AccountClient client)
        {
            var accountRequest = new AccountRequest
            {
                Name = "LeaderboardClientTests",
                Password = "LeaderboardClientTestsPassword",
                AutoLogin = true,
            };

            try
            {
                client.Login(accountRequest);
            }
            catch
            {
                client.Register(accountRequest);
            }
        }
        #endregion

        #region Tests
        [Fact]
        public void CanCreateAndGetLeaderboard()
        {

            var createRequest = new LeaderboardRequest
            {
                Token = "CanCreateAndGetLeaderboardToken",
                GameId = 0,
                Name = "CanCreateAndGetLeaderboardName",
                Key = "CanCreateAndGetLeaderboardKey",
                ActorType = ActorType.User,
                GameDataType = GameDataType.Long,
                CriteriaScope = CriteriaScope.Actor,
                LeaderboardType = LeaderboardType.Highest
            };
            
            var createResponse = _leaderboardClient.Create(createRequest);
            var getResponse = _leaderboardClient.Get(createRequest.Token, createRequest.GameId.Value);

            Assert.Equal(createRequest.Token, createResponse.Token);
            Assert.Equal(createRequest.GameId, createResponse.GameId);
            Assert.Equal(createRequest.Name, createResponse.Name);
            Assert.Equal(createRequest.Key, createResponse.Key);
            Assert.Equal(createRequest.ActorType, createResponse.ActorType);
            Assert.Equal(createRequest.GameDataType, createResponse.GameDataType);
            Assert.Equal(createRequest.CriteriaScope, createResponse.CriteriaScope);
            Assert.Equal(createRequest.LeaderboardType, createResponse.LeaderboardType);

            Assert.Equal(createRequest.Token, getResponse.Token);
            Assert.Equal(createRequest.GameId, getResponse.GameId);
            Assert.Equal(createRequest.Name, getResponse.Name);
            Assert.Equal(createRequest.Key, getResponse.Key);
            Assert.Equal(createRequest.ActorType, getResponse.ActorType);
            Assert.Equal(createRequest.GameDataType, getResponse.GameDataType);
            Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
            Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);
        }
        #endregion
    }
}
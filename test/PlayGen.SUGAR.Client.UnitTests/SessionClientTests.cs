using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Common.Shared.Web;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class SessionClientTests
    {
        private readonly SessionClient _sessionClient;
        private readonly AccountClient _accountClient;
        private readonly GameDataClient _gameDataClient;
        private readonly UserClient _userClient;
        private readonly GameClient _gameClient;

        public SessionClientTests()
        {
            var testClient = new TestSUGARClient();
            _sessionClient = testClient.Session;
            _accountClient = testClient.Account;
            _gameDataClient = testClient.GameData;
            _userClient = testClient.User;
            _gameClient = testClient.Game;

            Helpers.CreateAndLogin(_sessionClient);
        }

        [Test]
        public void CanHeartbeatAndReissueToken()
        {
            // Arrange
            var headers = (Dictionary<string, string>)
                typeof(ClientBase)
                .GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(_sessionClient);

            var originalToken = headers[HeaderKeys.Authorization];

            // Token seems to be seeded by the current time so this is needed to allow enough 
            // time to pass for the re-issued token's seed to be different to that of the original.
            Thread.Sleep(1000);

            // Act
            _sessionClient.Heartbeat();

            // Assert
            var postHeartbeatToken = headers[HeaderKeys.Authorization];
            Assert.AreNotEqual(originalToken, postHeartbeatToken);
        }

        [Test]
        public void CanCreateNewUserAndLogin()
        {
            var accountRequest = new AccountRequest
            {
                Name = "CanCreateNewUserAndLogin",
                Password = "CanCreateNewUserAndLoginPassword",
                SourceToken = "SUGAR",
            };

            var registerResponse = _sessionClient.CreateAndLogin(accountRequest);

            Assert.True(registerResponse.User.Id > 0);
            Assert.AreEqual(accountRequest.Name, registerResponse.User.Name);
        }

        [Test]
        public void CanLoginValidUser()
        {
            var accountRequest = new AccountRequest
            {
                Name = "CanLoginValidUser",
                Password = "CanLoginValidUserPassword",
                SourceToken = "SUGAR"
            };

            _accountClient.Create(accountRequest);

            var logged = _sessionClient.Login(accountRequest);

            Assert.True(logged.User.Id > 0);
            Assert.AreEqual(accountRequest.Name, logged.User.Name);
        }

        [Test]
        public void CannotLoginInvalidUser()
        {
            var accountRequest = new AccountRequest();
            Assert.Throws<ClientException>(() => _sessionClient.Login(accountRequest));
        }

        [Test]
        public void CanLogoutAndInvalidateSessionMethod()
        {
            // Arrange
            _sessionClient.CreateAndLogin(new AccountRequest
            {
                Name = "CanLogoutAndInvalidateSessionMethod",
                Password = "CanLogoutAndInvalidateSessionMethodPassword",
                SourceToken = "SUGAR",
            });

            // Act
            _sessionClient.Logout();

            // Assert
            Assert.Throws<ClientException>(_sessionClient.Heartbeat);
        }

        [Test]
        public void CanLogoutAndInvalidateSessionClass()
        {
            // Arrange
            _sessionClient.CreateAndLogin(new AccountRequest
            {
                Name = "CanLogoutAndInvalidateSessionClass",
                Password = "CanLogoutAndInvalidateSessionClassPassword",
                SourceToken = "SUGAR",
            });

            // Act
            _sessionClient.Logout();

            // Assert
            Assert.Throws<ClientException>(() => _gameDataClient.Add(new SaveDataRequest
            {
                ActorId = Helpers.GetOrCreateUser(_userClient, "CanLogoutAndInvalidateSessionClass").Id,
                GameId = Helpers.GetOrCreateGame(_gameClient, "CanLogoutAndInvalidateSessionClass").Id,
                Key = "CanLogoutAndInvalidateSessionClass",
                SaveDataType = SaveDataType.String,
                Value = "CanLogoutAndInvalidateSessionClass"
            }));
        }
    }
}

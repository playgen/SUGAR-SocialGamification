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
    public class SessionClientTests : ClientTestsBase
    {
        [Test]
        public void CanHeartbeatAndReissueToken()
        {
            // Arrange
            var headers = (Dictionary<string, string>)
                typeof(ClientBase)
                .GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(SUGARClient.Session);

            var originalToken = headers[HeaderKeys.Authorization];

            // Token seems to be seeded by the current time so this is needed to allow enough 
            // time to pass for the re-issued token's seed to be different to that of the original.
            Thread.Sleep(1000);

            // Act
            SUGARClient.Session.Heartbeat();

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

            var registerResponse = SUGARClient.Session.CreateAndLogin(accountRequest);

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

            SUGARClient.Account.Create(accountRequest);

            var logged = SUGARClient.Session.Login(accountRequest);

            Assert.True(logged.User.Id > 0);
            Assert.AreEqual(accountRequest.Name, logged.User.Name);
        }

        [Test]
        public void CannotLoginInvalidUser()
        {
            var accountRequest = new AccountRequest();
            Assert.Throws<ClientException>(() => SUGARClient.Session.Login(accountRequest));
        }

        [Test]
        public void CanLogoutAndInvalidateSessionMethod()
        {
            // Arrange
            SUGARClient.Session.CreateAndLogin(new AccountRequest
            {
                Name = "CanLogoutAndInvalidateSessionMethod",
                Password = "CanLogoutAndInvalidateSessionMethodPassword",
                SourceToken = "SUGAR",
            });

            // Act
            SUGARClient.Session.Logout();

            // Assert
            Assert.Throws<ClientException>(SUGARClient.Session.Heartbeat);
        }

        [Test]
        public void CanLogoutAndInvalidateSessionClass()
        {
            // Arrange
            SUGARClient.Session.CreateAndLogin(new AccountRequest
            {
                Name = "CanLogoutAndInvalidateSessionClass",
                Password = "CanLogoutAndInvalidateSessionClassPassword",
                SourceToken = "SUGAR",
            });

            // Act
            SUGARClient.Session.Logout();

            // Assert
            Assert.Throws<ClientException>(() => SUGARClient.GameData.Add(new SaveDataRequest
            {
                ActorId = Helpers.GetOrCreateUser(SUGARClient.User, "CanLogoutAndInvalidateSessionClass").Id,
                GameId = Helpers.GetOrCreateGame(SUGARClient.Game, "CanLogoutAndInvalidateSessionClass").Id,
                Key = "CanLogoutAndInvalidateSessionClass",
                SaveDataType = SaveDataType.String,
                Value = "CanLogoutAndInvalidateSessionClass"
            }));
        }
    }
}

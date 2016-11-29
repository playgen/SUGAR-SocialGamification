using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using PlayGen.SUGAR.Common.Shared.Web;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class SessionClientTests
    {
        private readonly SessionClient _sessionClient;
        private readonly AccountClient _accountClient;

        public SessionClientTests()
        {
            var testClient = new TestSUGARClient();
            _sessionClient = testClient.Session;
            _accountClient = testClient.Account;

            Helpers.RegisterAndLogin(_accountClient);
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
    }
}

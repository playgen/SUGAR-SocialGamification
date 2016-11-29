using System.Reflection;
using System.Collections.Generic;
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

        public void CanHeartbeatAndReissueToken()
        {
            // Arrange
            var headers = (Dictionary<string, string>)_sessionClient
                .GetType()
                .GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
                .GetValue(_sessionClient);

            var originalToken = headers[HeaderKeys.Authorization];

            // Act
            _sessionClient.Heartbeat();

            // Assert
            var postHeartbeatToken = headers[HeaderKeys.Authorization];
            Assert.AreNotEqual(originalToken, postHeartbeatToken);
        }
    }
}

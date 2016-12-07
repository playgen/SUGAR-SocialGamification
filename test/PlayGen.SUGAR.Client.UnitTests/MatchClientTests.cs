using System;
using NUnit.Framework;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    public class MatchClientTests : ClientTestsBase
    {
        [Test]
        public void CanStart()
        {
            // Assign
            var key = "Match_CanStart";
            var game = SUGARClient.Game.Create(new GameRequest
            {
                Name = key
            });

            var account = SUGARClient.Session.CreateAndLogin(game.Id, new AccountRequest
            {
                Name = key,
                Password = key + "Password",
                SourceToken = "SUGAR"
            });

            // Act
            var match = SUGARClient.Match.Start();
            
            // Assert
            Assert.AreEqual(game.Id, match.Game.Id);
            Assert.AreEqual(account.User.Id, match.Creator.Id);
        }

        [Test]
        public void CanEnd()
        {
            // Assign
            var key = "Match_CanEnd";
            var game = SUGARClient.Game.Create(new GameRequest
            {
                Name = key
            });

            var account = SUGARClient.Session.CreateAndLogin(game.Id, new AccountRequest
            {
                Name = key,
                Password = key + "Password",
                SourceToken = "SUGAR"
            });

            var match = SUGARClient.Match.Start();
            
            // Act
            SUGARClient.Match.End(match.Id);

            // Assert
            Assert.AreNotEqual(match.Ended, default(DateTime));
        }
    }
}

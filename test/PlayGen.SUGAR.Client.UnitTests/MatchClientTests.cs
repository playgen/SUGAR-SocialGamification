using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using PlayGen.SUGAR.Contracts.Shared;

namespace PlayGen.SUGAR.Client.UnitTests
{
    [TestFixture]
    public class MatchClientTests : ClientTestsBase
    {
        private AccountResponse _account;
        private GameResponse _game;

        [SetUp]
        public override void Setup()
        {
            var _game = Helpers.GetOrCreateGame(SUGARClient.Game, "MatchClientTests");

            try
            {
                _account = SUGARClient.Session.Login(_game.Id, new AccountRequest
                {
                    Name = "MatchClientTests",
                    Password = "MatchClientTestsPassword",
                    SourceToken = "SUGAR"
                });
            }
            catch (Exception e)
            {
                _account = SUGARClient.Session.CreateAndLogin(_game.Id, new AccountRequest
                {
                    Name = "MatchClientTests",
                    Password = "MatchClientTestsPassword",
                    SourceToken = "SUGAR"
                });
            }
        }

        [TearDown]
        public override void TearDown()
        {
            SUGARClient.Session.Logout();
        }

        [Test]
        public void CanStart()
        {
            // Act
            var match = SUGARClient.Match.Start();

            // Assert
            Assert.AreEqual(_game.Id, match.Game.Id);
            Assert.AreEqual(_account.User.Id, match.Creator.Id);
        }

        [Test]
        public void CanEnd()
        {
            // Assign
            var match = SUGARClient.Match.Start();

            // Act
            SUGARClient.Match.End(match.Id);

            // Assert
            Assert.AreNotEqual(match.Ended, default(DateTime));
        }

        [Test]
        public void CanGetByTime()
        {
            // Assign
            var shouldntGet = StartMatches(10);

            Thread.Sleep(1000);
            var preTime = DateTime.UtcNow;
            Thread.Sleep(1000);

            var shouldGet = StartMatches(10);
            
            Thread.Sleep(1000);
            var postTime = DateTime.UtcNow;
            Thread.Sleep(1000);

            shouldntGet.AddRange(StartMatches(10));

            // Act
            var got = SUGARClient.Match.GetByTime(preTime, postTime);

            // Assert
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void GetByGame()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByGame()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByCreator()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByCreatorAndTime()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByGameAndCreator()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByGameAndCreatorAndTime()
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private List<MatchResponse> StartMatches(int count)
        {
            var matches = new List<MatchResponse>();
            for (var i = 0; i < count; i++)
            {
                var match = SUGARClient.Match.Start();
                matches.Add(match);
            }

            return matches;
        }

        #endregion
    }
}

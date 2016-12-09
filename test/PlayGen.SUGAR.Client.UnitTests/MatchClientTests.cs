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

        private void LoginUserForGame(string key = "MatchClientTests")
        {
            _game = Helpers.GetOrCreateGame(SUGARClient.Game, key);

            try
            {
                _account = SUGARClient.Session.Login(_game.Id, new AccountRequest
                {
                    Name = key,
                    Password = key + "Password",
                    SourceToken = "SUGAR"
                });
            }
            catch (Exception e)
            {
                _account = SUGARClient.Session.CreateAndLogin(_game.Id, new AccountRequest
                {
                    Name = key,
                    Password = key + "Password",
                    SourceToken = "SUGAR"
                });
            }
        }
        
        [Test]
        public void CanStart()
        {
            // Assign
            LoginUserForGame();

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
            LoginUserForGame();
            var match = SUGARClient.Match.Start();

            // Act
            match = SUGARClient.Match.End(match.Id);

            // Assert
            Assert.AreNotEqual(match.Ended, null);
            Assert.Less(match.Started, match.Ended);
        }

        [Test]
        public void CanGetByTime()
        {
            // Assign
            LoginUserForGame();
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
            LoginUserForGame();
            var got = SUGARClient.Match.GetByGame(_game.Id);

            throw new NotImplementedException();
        }

        [Test]
        public void GetByGameAndTime()
        {
            LoginUserForGame();
            var got = SUGARClient.Match.GetByGame(_game.Id, DateTime.MinValue, DateTime.MaxValue);

            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByCreator()
        {
            LoginUserForGame();
            var got = SUGARClient.Match.GetByCreator(_account.User.Id);

            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByCreatorAndTime()
        {
            LoginUserForGame();
            var got = SUGARClient.Match.GetByCreator(_account.User.Id, DateTime.MinValue, DateTime.MaxValue);
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByGameAndCreator()
        {
            LoginUserForGame();
            var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id);
            throw new NotImplementedException();
        }

        [Test]
        public void CanGetByGameAndCreatorAndTime()
        {
            LoginUserForGame();
            var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id, DateTime.MinValue, DateTime.MaxValue);
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

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Common.Shared;
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
            LoginAdmin();

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
            // Arrange
            LoginUserForGame();

            // Act
            var match = SUGARClient.Match.CreateAndStart();

            // Assert
            Assert.AreEqual(_game.Id, match.Game.Id);
            Assert.AreEqual(_account.User.Id, match.Creator.Id);
        }

        [Test]
        public void CanEnd()
        {
            // Arrange
            LoginUserForGame();
            var match = SUGARClient.Match.CreateAndStart();

            // Act
            match = SUGARClient.Match.End(match.Id);

            // Assert
            Assert.AreNotEqual(match.Ended, null);
            Assert.Less(match.Started, match.Ended);
        }

        [Test]
        public void CanGetByTime()
        {
            // Arrange
            LoginUserForGame();
            var shouldntGet = StartAndEndMatches(10);

            var preTime = DateTime.UtcNow;

            var shouldGet = StartAndEndMatches(10);
            
            var postTime = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            // Act
            var got = SUGARClient.Match.GetByTime(preTime, postTime);

            // Assert
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void GetByGame()
        {
            // Arrange
            LoginUserForGame("GetByGame_ShouldntGet");
            var shouldntGet = StartMatches(10);

            LoginUserForGame("GetByGame_ShouldGet");
            var shouldGet = StartMatches(10);

            // Act
            var got = SUGARClient.Match.GetByGame(_game.Id);

            // Assert
            got.ForEach(m => Assert.AreEqual(_game.Id, m.Game.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void GetByGameAndTime()
        {
            // Arrange
            LoginUserForGame("GetByGameAndTime_ShouldntGet");
            var shouldntGet = StartAndEndMatches(10);

            var pre = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            LoginUserForGame("GetByGameAndTime_ShouldGet");
            var shouldGet = StartAndEndMatches(10);

            var post = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            // Act
            var got = SUGARClient.Match.GetByGame(_game.Id, pre, post);

            // Assert
            got.ForEach(m => Assert.AreEqual(_game.Id, m.Game.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void CanGetByCreator()
        {
            // Arrange
            LoginUserForGame("CanGetByCreator_ShouldntGet");
            var shouldntGet = StartMatches(10);

            LoginUserForGame("CanGetByCreator_ShouldGet");
            var shouldGet = StartMatches(10);

            // Act
            var got = SUGARClient.Match.GetByCreator(_account.User.Id);

            // Assert
            got.ForEach(m => Assert.AreEqual(_account.User.Id, m.Creator.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void CanGetByCreatorAndTime()
        {
            // Arrange
            LoginUserForGame("CanGetByCreatorAndTime_ShouldntGet");
            var shouldntGet = StartAndEndMatches(10);

            var pre = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            LoginUserForGame("CanGetByCreatorAndTime_ShouldGet");
            var shouldGet = StartAndEndMatches(10);

            var post = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            // Act
            var got = SUGARClient.Match.GetByCreator(_account.User.Id, pre, post);

            // Assert
            got.ForEach(m => Assert.AreEqual(_account.User.Id, m.Creator.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void CanGetByGameAndCreator()
        {
            // Arrange
            LoginUserForGame("CanGetByGameAndCreator_ShouldntGet");
            var shouldntGet = StartMatches(10);

            LoginUserForGame("CanGetByGameAndCreator_ShouldGet");
            var shouldGet = StartMatches(10);

            // Act
            var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id);

            // Assert
            got.ForEach(m => Assert.AreEqual(_game.Id, m.Game.Id));
            got.ForEach(m => Assert.AreEqual(_account.User.Id, m.Creator.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void CanGetByGameAndCreatorAndTime()
        {
            // Arrange
            LoginUserForGame("CanGetByGameAndCreatorAndTime_ShouldntGet");
            var shouldntGet = StartAndEndMatches(10);

            var pre = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            LoginUserForGame("CanGetByGameAndCreatorAndTime_ShouldGet");
            var shouldGet = StartAndEndMatches(10);

            var post = DateTime.UtcNow;

            shouldntGet.AddRange(StartAndEndMatches(10));

            // Act
            var got = SUGARClient.Match.GetByGameAndCreator(_game.Id, _account.User.Id, pre, post);

            // Assert
            got.ForEach(m => Assert.AreEqual(_game.Id, m.Game.Id));
            got.ForEach(m => Assert.AreEqual(_account.User.Id, m.Creator.Id));
            shouldGet.ForEach(m => Assert.IsTrue(got.Any(g => g.Id == m.Id)));
            shouldntGet.ForEach(m => Assert.IsFalse(got.Any(g => g.Id == m.Id)));
        }

        [Test]
        public void CanAddAndGetData()
        {
            // Arrange
            LoginUserForGame("CanAddAndGetData");

            var match = SUGARClient.Match.CreateAndStart();

            var datas = new List<EvaluationDataResponse>();

            // Act
            for (var i = 0; i < 10; i++)
            {
                datas.Add(SUGARClient.Match.AddData(new EvaluationDataRequest
                {
                    RelatedEntityId = match.Id,
                    GameId = _game.Id,
                    CreatingActorId = _account.User.Id,
                    EvaluationDataType = EvaluationDataType.Long,
                    Key = "CanAddAndGetData",
                    Value = i.ToString()
                }));
            }

            var got = SUGARClient.Match.GetData(match.Id);

            // Assert
            datas.ForEach(a => Assert.True(got.Any(g => g.GameId == a.GameId
                && g.RelatedEntityId == a.RelatedEntityId
                && g.CreatingActorId == a.CreatingActorId
                && g.Key == a.Key
                && g.Value == a.Value)));
        }

        #region Helpers
        private List<MatchResponse> StartMatches(int count)
        {
            var matches = new List<MatchResponse>();
            for (var i = 0; i < count; i++)
            {
                var match = SUGARClient.Match.CreateAndStart();
                matches.Add(match);
            }

            return matches;
        }

        private List<MatchResponse> StartAndEndMatches(int count)
        {
            var matches = StartMatches(count);
            matches = matches.Select(m => SUGARClient.Match.End(m.Id)).ToList();
            return matches;
        }

        #endregion
    }
}

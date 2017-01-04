using System;
using System.Collections.Generic;
using System.Threading;
using PlayGen.SUGAR.Data.Model;
using Xunit;
using System.Linq;

namespace PlayGen.SUGAR.Core.UnitTests
{
    public class MatchControllerTests : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed) return;

            _isDisposed = true;
        }

        [Fact]
        public void CanStart()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("CanStart");
            var user = Helpers.GetOrCreateUser("CanStart");

            var preCreateTime = DateTime.UtcNow;

            // Act
            var match = ControllerLocator.MatchController.Create(game.Id, user.Id);
            match = ControllerLocator.MatchController.Start(match.Id);

            // Assert
            var postCreateTime = DateTime.UtcNow;

            Assert.Equal(game.Id, match.GameId);
            Assert.Equal(user.Id, match.CreatorId);
            Assert.True(preCreateTime <= match.Started && match.Started <= postCreateTime);
        }

        [Fact]
        public void CanEnd()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("CanEnd");
            var user = Helpers.GetOrCreateUser("CanEnd");

            var match = ControllerLocator.MatchController.Create(game.Id, user.Id);
            match = ControllerLocator.MatchController.Start(match.Id);

            // Act
            match = ControllerLocator.MatchController.End(match.Id);

            // Assert
            Assert.NotEqual(default(DateTime), match.Ended);
            Assert.NotNull(match.Ended);
            Assert.True(match.Started <= match.Ended);
        }

        [Fact]
        public void CanGetByTime()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("CanGetByTime");
            var user = Helpers.GetOrCreateUser("CanGetByTime");

            var shouldntGet = Create(10, game.Id, user.Id);

            Thread.Sleep(1000);
            var startTime = DateTime.UtcNow;
            Thread.Sleep(1000);

            var shouldGet = Create(10, game.Id, user.Id);

            Thread.Sleep(1000);
            var endTime = DateTime.UtcNow;
            Thread.Sleep(1000);

            shouldntGet.AddRange(Create(10, game.Id, user.Id));

            // Act
            var got = ControllerLocator.MatchController.GetByTime(startTime, endTime);

            // Assert
            shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));    // Make sure all matches created during specified time were returned
            shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));    // Make sure matches that weren't created during the specified time aren't returned
        }

        [Fact]
        public void CanGetByGame()
        {
            // Arrange
            var shouldGetGame = Helpers.GetOrCreateGame("CanGetByGame_ShouldGet");
            var shouldntGetGame = Helpers.GetOrCreateGame("CanGetByGame_ShouldNotGet");
            var user = Helpers.GetOrCreateUser("CanGetByGame");

            var shouldntGet = Create(10, shouldntGetGame.Id, user.Id);
            var shouldGet = Create(10, shouldGetGame.Id, user.Id);

            // Act
            var got = ControllerLocator.MatchController.GetByGame(shouldGetGame.Id);

            // Assert
            shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));    // Make sure all matches created during specified time were returned
            shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));    // Make sure matches that weren't created during the specified time aren't returned
        }

        [Fact]
        public void GetByCreator()
        {
            // Arrange
            var shouldGetUser = Helpers.GetOrCreateUser("GetByCreator_ShouldGet");
            var shouldntGetUser = Helpers.GetOrCreateUser("GetByCreator_ShouldNotGet");
            var game = Helpers.GetOrCreateGame("CanGetByGame");

            var shouldntGet = Create(10, game.Id, shouldntGetUser.Id);
            var shouldGet = Create(10, game.Id, shouldGetUser.Id);

            // Act
            var got = ControllerLocator.MatchController.GetByCreator(shouldGetUser.Id);

            // Assert
            shouldGet.ForEach(m => Assert.True(got.Any(g => g.Id == m.Id)));    // Make sure all matches created during specified time were returned
            shouldntGet.ForEach(m => Assert.False(got.Any(g => g.Id == m.Id)));    // Make sure matches that weren't created during the specified time aren't returned
        }

        [Fact]
        public void CanAddAndGetData()
        {
            // Arrange
            var game = Helpers.GetOrCreateGame("CanAddAndGetData");
            var user = Helpers.GetOrCreateUser("CanAddAndGetData");
            var match = ControllerLocator.MatchController.Create(game.Id, user.Id);

            // Act
            var datas = new List<EvaluationData>();
            for (var i = 0; i < 10; i++)
            {
                datas.Add(ControllerLocator.MatchController.AddData(new EvaluationData
                {
                    GameId = game.Id,
                    ActorId = user.Id,
                    EntityId = match.Id,
                    Category = EvaluationDataCategory.MatchData,
                    Key = "CanAddAndGetData",
                    Value = i.ToString(),
                    EvaluationDataType = EvaluationDataType.Long
                }));
            }

            var got = ControllerLocator.MatchController.GetData(match.Id);

            // Assert
            datas.ForEach(a => Assert.True(got.Any(g => g.GameId == a.GameId
                && g.EntityId == a.EntityId
                && g.ActorId == a.ActorId
                && g.Key == a.Key
                && g.Value == a.Value)));
        }

        #region Helpers

        private List<Match> Create(int count, int gameId, int userId)
        {
            var matches = new List<Match>();
            for (var i = 0; i < 10; i++)
            {
                var match = ControllerLocator.MatchController.Create(gameId, userId);
                match = ControllerLocator.MatchController.Start(match.Id);
                match = ControllerLocator.MatchController.End(match.Id);
                matches.Add(match);
            }

            return matches;
        }
        #endregion
    }
}

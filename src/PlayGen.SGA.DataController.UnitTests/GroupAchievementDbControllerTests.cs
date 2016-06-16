using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class GroupAchievementDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly GroupAchievementDbController _groupAchievementDbController;

        public GroupAchievementDbControllerTests()
        {
            _groupAchievementDbController = new GroupAchievementDbController(_nameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetGroupAchievement()
        {
            string gameAchievementName = "CreateGroupAchievement";

            var newAchievement = CreateGroupAchievement(gameAchievementName);

            var gameAchievements = _groupAchievementDbController.Get(new int[] { newAchievement.GameId });

            int matches = gameAchievements.Count(g => g.Name == gameAchievementName && g.GameId == newAchievement.GameId);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateDuplicateGroupAchievement()
        {
            string gameAchievementName = "CreateDuplicateGroupAchievement";

            var firstachievement = CreateGroupAchievement(gameAchievementName);

            bool hadDuplicateException = false;

            try
            {
                CreateGroupAchievement(gameAchievementName, firstachievement.GameId);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void GetMultipleGroupAchievements()
        {
            string[] gameAchievementNames = new[]
            {
                "GetMultipleGroupAchievements1",
                "GetMultipleGroupAchievements2",
                "GetMultipleGroupAchievements3",
                "GetMultipleGroupAchievements4",
            };

            IList<int> gameIds = new List<int>();
            foreach (var gameAchievementName in gameAchievementNames)
            {
                gameIds.Add(CreateGroupAchievement(gameAchievementName).GameId);
            }

            CreateGroupAchievement("GetMultipleGroupAchievements_DontGetThis");

            var gameAchievements = _groupAchievementDbController.Get(gameIds.ToArray());

            var matchingGroupAchievements = gameAchievements.Select(g => gameAchievementNames.Contains(g.Name));

            Assert.Equal(matchingGroupAchievements.Count(), gameAchievementNames.Length);
        }

        [Fact]
        public void GetNonExistingGroupAchievements()
        {
            var gameAchievements = _groupAchievementDbController.Get(new int[] { -1 });

            Assert.Empty(gameAchievements);
        }

        [Fact]
        public void DeleteExistingGroupAchievement()
        {
            string gameAchievementName = "DeleteExistingGroupAchievement";

            var gameAchievement = CreateGroupAchievement(gameAchievementName);
            var gameId = gameAchievement.GameId;

            var gameAchievements = _groupAchievementDbController.Get(new int[] { gameId });
            Assert.Equal(gameAchievements.Count(), 1);
            Assert.Equal(gameAchievements.ElementAt(0).Name, gameAchievementName);

            _groupAchievementDbController.Delete(new[] { gameAchievement.Id });
            gameAchievements = _groupAchievementDbController.Get(new int[] { gameId });

            Assert.Empty(gameAchievements);
        }

        [Fact]
        public void DeleteNonExistingGroupAchievement()
        {
            bool hadExeption = false;

            try
            {
                _groupAchievementDbController.Delete(new int[] { -1 });
            }
            catch (Exception)
            {
                hadExeption = true;
            }

            Assert.False(hadExeption);
        }
        #endregion

        #region Helpers
        private GroupAchievement CreateGroupAchievement(string name, int gameId = 0)
        {
            GameDbController gameAchievementDbController = new GameDbController(_nameOrConnectionString);
            if (gameId == 0) {
                Game newgame = new Game
                {
                    Name = name
                };
                gameId = gameAchievementDbController.Create(newgame).Id;
            }

            var newGroupAchievement = new GroupAchievement
            {
                Name = name,
                GameId = gameId,
                CompletionCriteria = name
            };

            return _groupAchievementDbController.Create(newGroupAchievement);
        }
        #endregion
    }
}

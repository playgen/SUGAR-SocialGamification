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
            string groupAchievementName = "CreateGroupAchievement";

            var newAchievement = CreateGroupAchievement(groupAchievementName);

            var groupAchievements = _groupAchievementDbController.Get(new int[] { newAchievement.GameId });

            int matches = groupAchievements.Count(g => g.Name == groupAchievementName && g.GameId == newAchievement.GameId);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateGroupAchievementWithNonExistingGame()
        {
            string groupAchievementName = "CreateGroupAchievementWithNonExistingGame";

            bool hadException = false;

            try
            {
                CreateGroupAchievement(groupAchievementName, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateDuplicateGroupAchievement()
        {
            string groupAchievementName = "CreateDuplicateGroupAchievement";

            var firstachievement = CreateGroupAchievement(groupAchievementName);

            bool hadDuplicateException = false;

            try
            {
                CreateGroupAchievement(groupAchievementName, firstachievement.GameId);
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
            string[] groupAchievementNames = new[]
            {
                "GetMultipleGroupAchievements1",
                "GetMultipleGroupAchievements2",
                "GetMultipleGroupAchievements3",
                "GetMultipleGroupAchievements4",
            };

            IList<int> gameIds = new List<int>();
            foreach (var groupAchievementName in groupAchievementNames)
            {
                gameIds.Add(CreateGroupAchievement(groupAchievementName).GameId);
            }

            CreateGroupAchievement("GetMultipleGroupAchievements_DontGetThis");

            var groupAchievements = _groupAchievementDbController.Get(gameIds.ToArray());

            var matchingGroupAchievements = groupAchievements.Select(g => groupAchievementNames.Contains(g.Name));

            Assert.Equal(matchingGroupAchievements.Count(), groupAchievementNames.Length);
        }

        [Fact]
        public void GetNonExistingGroupAchievements()
        {
            var groupAchievements = _groupAchievementDbController.Get(new int[] { -1 });

            Assert.Empty(groupAchievements);
        }

        [Fact]
        public void DeleteExistingGroupAchievement()
        {
            string groupAchievementName = "DeleteExistingGroupAchievement";

            var groupAchievement = CreateGroupAchievement(groupAchievementName);
            var groupId = groupAchievement.GameId;

            var groupAchievements = _groupAchievementDbController.Get(new int[] { groupId });
            Assert.Equal(groupAchievements.Count(), 1);
            Assert.Equal(groupAchievements.ElementAt(0).Name, groupAchievementName);

            _groupAchievementDbController.Delete(new[] { groupAchievement.Id });
            groupAchievements = _groupAchievementDbController.Get(new int[] { groupId });

            Assert.Empty(groupAchievements);
        }

        [Fact]
        public void DeleteNonExistingGroupAchievement()
        {
            bool hadException = false;

            try
            {
                _groupAchievementDbController.Delete(new int[] { -1 });
            }
            catch (Exception)
            {
                hadException = true;
            }

            Assert.False(hadException);
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

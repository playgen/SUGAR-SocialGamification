using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class UserAchievementDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly UserAchievementDbController _userAchievementDbController;

        public UserAchievementDbControllerTests()
        {
            _userAchievementDbController = new UserAchievementDbController(_nameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetUserAchievement()
        {
            string userAchievementName = "CreateUserAchievement";

            var newAchievement = CreateUserAchievement(userAchievementName);

            var userAchievements = _userAchievementDbController.Get(new int[] { newAchievement.GameId });

            int matches = userAchievements.Count(g => g.Name == userAchievementName && g.GameId == newAchievement.GameId);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateUserAchievementWithNonExistingGame()
        {
            string userAchievementName = "CreateUserAchievementWithNonExistingGame";

            bool hadException = false;

            try
            {
                CreateUserAchievement(userAchievementName, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateDuplicateUserAchievement()
        {
            string userAchievementName = "CreateDuplicateUserAchievement";

            var firstachievement = CreateUserAchievement(userAchievementName);

            bool hadDuplicateException = false;

            try
            {
                CreateUserAchievement(userAchievementName, firstachievement.GameId);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void GetMultipleUserAchievements()
        {
            string[] userAchievementNames = new[]
            {
                "GetMultipleUserAchievements1",
                "GetMultipleUserAchievements2",
                "GetMultipleUserAchievements3",
                "GetMultipleUserAchievements4",
            };

            IList<int> gameIds = new List<int>();
            foreach (var userAchievementName in userAchievementNames)
            {
                gameIds.Add(CreateUserAchievement(userAchievementName).GameId);
            }

            CreateUserAchievement("GetMultipleUserAchievements_DontGetThis");

            var userAchievements = _userAchievementDbController.Get(gameIds.ToArray());

            var matchingUserAchievements = userAchievements.Select(g => userAchievementNames.Contains(g.Name));

            Assert.Equal(matchingUserAchievements.Count(), userAchievementNames.Length);
        }

        [Fact]
        public void GetNonExistingUserAchievements()
        {
            var userAchievements = _userAchievementDbController.Get(new int[] { -1 });

            Assert.Empty(userAchievements);
        }

        [Fact]
        public void DeleteExistingUserAchievement()
        {
            string userAchievementName = "DeleteExistingUserAchievement";

            var userAchievement = CreateUserAchievement(userAchievementName);
            var userId = userAchievement.GameId;

            var userAchievements = _userAchievementDbController.Get(new int[] { userId });
            Assert.Equal(userAchievements.Count(), 1);
            Assert.Equal(userAchievements.ElementAt(0).Name, userAchievementName);

            _userAchievementDbController.Delete(new[] { userAchievement.Id });
            userAchievements = _userAchievementDbController.Get(new int[] { userId });

            Assert.Empty(userAchievements);
        }

        [Fact]
        public void DeleteNonExistingUserAchievement()
        {
            bool hadException = false;

            try
            {
                _userAchievementDbController.Delete(new int[] { -1 });
            }
            catch (Exception)
            {
                hadException = true;
            }

            Assert.False(hadException);
        }
        #endregion

        #region Helpers
        private UserAchievement CreateUserAchievement(string name, int gameId = 0)
        {
            GameDbController gameAchievementDbController = new GameDbController(_nameOrConnectionString);
            if (gameId == 0)
            {
                Game newgame = new Game
                {
                    Name = name
                };
                gameId = gameAchievementDbController.Create(newgame).Id;
            }

            var newUserAchievement = new UserAchievement
            {
                Name = name,
                GameId = gameId,
                CompletionCriteria = name
            };

            return _userAchievementDbController.Create(newUserAchievement);
        }
        #endregion
    }
}
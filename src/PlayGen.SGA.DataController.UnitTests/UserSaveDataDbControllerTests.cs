using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class UserSaveDataDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly UserSaveDataDbController _userDataDbController;

        public UserSaveDataDbControllerTests()
        {
            _userDataDbController = new UserSaveDataDbController(NameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetUserSaveData()
        {
            string userDataName = "CreateUserData";

            var newSaveData = CreateUserData(userDataName);

            var userDatas = _userDataDbController.Get(newSaveData.GameId, newSaveData.UserId, new string[] { newSaveData.Key });

            int matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newSaveData.GameId && g.UserId == newSaveData.UserId);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateUserDataWithNonExistingGame()
        {
            string userDataName = "CreateUserDataWithNonExistingGame";

            bool hadException = false;

            try
            {
                CreateUserData(userDataName, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateUserDataWithNonExistingUser()
        {
            string userDataName = "CreateUserDataWithNonExistingUser";

            bool hadException = false;

            try
            {
                CreateUserData(userDataName, 0, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void GetMultipleUserSaveDatas()
        {
            string[] userDataNames = new[]
            {
                "GetMultipleUserSaveDatas1",
                "GetMultipleUserSaveDatas2",
                "GetMultipleUserSaveDatas3",
                "GetMultipleUserSaveDatas4",
            };

            var doNotFind = CreateUserData("GetMultipleUserSaveDatas_DontGetThis");
            var gameId = doNotFind.GameId;
            var userId = doNotFind.UserId;

            foreach (var userDataName in userDataNames)
            {
                CreateUserData(userDataName, gameId, userId);
            }

            var userDatas = _userDataDbController.Get(gameId, userId, userDataNames);

            var matchingUserSaveDatas = userDatas.Select(g => userDataNames.Contains(g.Key));

            Assert.Equal(matchingUserSaveDatas.Count(), userDataNames.Length);
        }

        [Fact]
        public void GetUserSaveDatasWithNonExistingKey()
        {
            string userDataName = "GetUserSaveDatasWithNonExistingKey";

            var newSaveData = CreateUserData(userDataName);

            var userDatas = _userDataDbController.Get(newSaveData.GameId, newSaveData.UserId, new string[] { "null key" });

            Assert.Empty(userDatas);
        }

        [Fact]
        public void GetUserSaveDatasWithNonExistingGame()
        {
            string userDataName = "GetUserSaveDatasWithNonExistingGame";

            var newSaveData = CreateUserData(userDataName);

            var userDatas = _userDataDbController.Get(-1, newSaveData.UserId, new string[] { userDataName });

            Assert.Empty(userDatas);
        }

        [Fact]
        public void GetUserSaveDatasWithNonExistingUser()
        {
            string userDataName = "GetUserSaveDatasWithNonExistingUser";

            var newSaveData = CreateUserData(userDataName);

            var userDatas = _userDataDbController.Get(newSaveData.GameId, -1, new string[] { userDataName });

            Assert.Empty(userDatas);
        }
        #endregion

        #region Helpers
        private UserData CreateUserData(string key, int gameId = 0, int userId = 0)
        {
            GameDbController gameDbController = new GameDbController(NameOrConnectionString);
            if (gameId == 0)
            {
                Game newgame = new Game
                {
                    Name = key
                };
                gameId = gameDbController.Create(newgame).Id;
            }

            UserDbController userDbController = new UserDbController(NameOrConnectionString);
            if (userId == 0)
            {
                User newuser = new User
                {
                    Name = key
                };
                userId = userDbController.Create(newuser).Id;
            }

            var newUserData = new UserData
            {
                Key = key,
                GameId = gameId,
                UserId = userId,
                Value = key + " value",
                DataType = 0
            };

            return _userDataDbController.Create(newUserData);
        }
        #endregion
    }
}

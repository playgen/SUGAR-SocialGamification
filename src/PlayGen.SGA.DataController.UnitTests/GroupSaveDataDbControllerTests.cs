using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class GroupSaveDataDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly GroupSaveDataDbController _groupDataDbController;

        public GroupSaveDataDbControllerTests()
        {
            _groupDataDbController = new GroupSaveDataDbController(NameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetGroupSaveData()
        {
            string groupDataName = "CreateGroupData";

            var newSaveData = CreateGroupData(groupDataName);

            var groupDatas = _groupDataDbController.Get(newSaveData.GameId, newSaveData.GroupId, new string[] { newSaveData.Key });

            int matches = groupDatas.Count(g => g.Key == groupDataName && g.GameId == newSaveData.GameId && g.GroupId == newSaveData.GroupId);

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateGroupDataWithNonExistingGame()
        {
            string groupDataName = "CreateGroupDataWithNonExistingGame";

            bool hadException = false;

            try
            {
                CreateGroupData(groupDataName, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateGroupDataWithNonExistingGroup()
        {
            string groupDataName = "CreateGroupDataWithNonExistingGroup";

            bool hadException = false;

            try
            {
                CreateGroupData(groupDataName, 0, -1);
            }
            catch (DuplicateRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void GetMultipleGroupSaveDatas()
        {
            string[] groupDataNames = new[]
            {
                "GetMultipleGroupSaveDatas1",
                "GetMultipleGroupSaveDatas2",
                "GetMultipleGroupSaveDatas3",
                "GetMultipleGroupSaveDatas4",
            };

            var doNotFind = CreateGroupData("GetMultipleGroupSaveDatas_DontGetThis");
            var gameId = doNotFind.GameId;
            var groupId = doNotFind.GroupId;

            foreach (var groupDataName in groupDataNames)
            {
                CreateGroupData(groupDataName, gameId, groupId);
            }

            var groupDatas = _groupDataDbController.Get(gameId, groupId, groupDataNames);

            var matchingGroupSaveDatas = groupDatas.Select(g => groupDataNames.Contains(g.Key));

            Assert.Equal(matchingGroupSaveDatas.Count(), groupDataNames.Length);
        }

        [Fact]
        public void GetGroupSaveDatasWithNonExistingKey()
        {
            string groupDataName = "GetGroupSaveDatasWithNonExistingKey";

            var newSaveData = CreateGroupData(groupDataName);

            var groupDatas = _groupDataDbController.Get(newSaveData.GameId, newSaveData.GroupId, new string[] { "null key" });

            Assert.Empty(groupDatas);
        }

        [Fact]
        public void GetGroupSaveDatasWithNonExistingGame()
        {
            string groupDataName = "GetGroupSaveDatasWithNonExistingGame";

            var newSaveData = CreateGroupData(groupDataName);

            var groupDatas = _groupDataDbController.Get(-1, newSaveData.GroupId, new string[] { groupDataName });

            Assert.Empty(groupDatas);
        }

        [Fact]
        public void GetGroupSaveDatasWithNonExistingGroup()
        {
            string groupDataName = "GetGroupSaveDatasWithNonExistingGroup";

            var newSaveData = CreateGroupData(groupDataName);

            var groupDatas = _groupDataDbController.Get(newSaveData.GameId, -1, new string[] { groupDataName });

            Assert.Empty(groupDatas);
        }
        #endregion

        #region Helpers
        private GroupData CreateGroupData(string key, int gameId = 0, int groupId = 0)
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

            GroupDbController groupDbController = new GroupDbController(NameOrConnectionString);
            if (groupId == 0)
            {
                Group newgroup = new Group
                {
                    Name = key
                };
                groupId = groupDbController.Create(newgroup).Id;
            }

            var newGroupData = new GroupData
            {
                Key = key,
                GameId = gameId,
                GroupId = groupId,
                Value = key + " value",
                DataType = 0
            };

            return _groupDataDbController.Create(newGroupData);
        }
        #endregion
    }
}

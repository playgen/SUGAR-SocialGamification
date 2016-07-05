using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GameDataControllerTests : IClassFixture<TestController>
	{
		private TestController _testController;

		#region Configuration
		private readonly GameDataController _gameDataController;
		private readonly GameController _gameController;
		private readonly UserController _userController;

		public GameDataControllerTests(TestController testController)
		{
			_gameDataController = testController.GameDataController;
			_gameController = testController.GameController;
			_userController = testController.UserController;			
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroupSaveData()
		{
			string groupDataName = "CreateGroupData";

			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			int matches = groupDatas.Count(g => g.Key == groupDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupDataWithNonExistingGame()
		{
			string groupDataName = "CreateGroupDataWithNonExistingGame";

			Assert.Throws<MissingRecordException>(() => CreateGroupData(groupDataName, -1));
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
			catch (MissingRecordException)
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
			var groupId = doNotFind.ActorId;

			foreach (var groupDataName in groupDataNames)
			{
				CreateGroupData(groupDataName, gameId.Value, groupId.Value);
			}

			var groupDatas = _gameDataController.Get(gameId, groupId, groupDataNames);

			var matchingGroupSaveDatas = groupDatas.Select(g => groupDataNames.Contains(g.Key));

			Assert.Equal(matchingGroupSaveDatas.Count(), groupDataNames.Length);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingKey()
		{
			string groupDataName = "GetGroupSaveDatasWithNonExistingKey";

			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _gameDataController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { "null key" });

			Assert.Empty(groupDatas);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingGame()
		{
			string groupDataName = "GetGroupSaveDatasWithNonExistingGame";

			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _gameDataController.Get(-1, newSaveData.ActorId, new string[] { groupDataName });

			Assert.Empty(groupDatas);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingGroup()
		{
			string groupDataName = "GetGroupSaveDatasWithNonExistingGroup";

			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _gameDataController.Get(newSaveData.GameId, -1, new string[] { groupDataName });

			Assert.Empty(groupDatas);
		}
		#endregion

		#region Helpers
		private GameData CreateGroupData(string key, int gameId = 0, int groupId = 0)
		{
			if (gameId == 0)
			{
				Game game = new Game
				{
					Name = key
				};
				_gameController.Create(game);
				gameId = game.Id;
			}

			if (groupId == 0)
			{
				Group group = new Group
				{
					Name = key
				};
				_groupDbController.Create(group);
				groupId = group.Id;
			}

			var groupData = new GameData
			{
				Key = key,
				GameId = gameId,
				ActorId = groupId,
				Value = key + " value",
				DataType = 0
			};
			_gameDataController.Create(groupData);

			return groupData;
		}
		#endregion
	}
}

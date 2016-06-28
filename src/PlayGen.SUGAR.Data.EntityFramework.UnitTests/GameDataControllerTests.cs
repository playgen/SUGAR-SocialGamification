using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class GameDataControllerTests : TestController
	{
		#region Configuration
		private readonly GameDataController _groupDataDbController;

		public GameDataControllerTests()
		{
			_groupDataDbController = new GameDataController(NameOrConnectionString);
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetGroupSaveData()
		{
			const string groupDataName = "CreateGroupData";

			var newSaveData = CreateGroupData(groupDataName);
			var groupDatas = _groupDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { newSaveData.Key });

			var matches = groupDatas.Count(g => g.Key == groupDataName && g.GameId == newSaveData.GameId && g.ActorId == newSaveData.ActorId);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateGroupDataWithNonExistingGame()
		{
			const string groupDataName = "CreateGroupDataWithNonExistingGame";
			Assert.Throws<MissingRecordException>(() => CreateGroupData(groupDataName, -1));
		}

		[Fact]
		public void CreateGroupDataWithNonExistingGroup()
		{
			const string groupDataName = "CreateGroupDataWithNonExistingGroup";
			Assert.Throws<MissingRecordException>(() => CreateGroupData(groupDataName, 0, -1));
		}

		[Fact]
		public void GetMultipleGroupSaveDatas()
		{
			var groupDataNames = new[]
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
				CreateGroupData(groupDataName, gameId, groupId);
			}

			var groupDatas = _groupDataDbController.Get(gameId, groupId, groupDataNames);

			var matchingGroupSaveDatas = groupDatas.Select(g => groupDataNames.Contains(g.Key));

			Assert.Equal(matchingGroupSaveDatas.Count(), groupDataNames.Length);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingKey()
		{
			const string groupDataName = "GetGroupSaveDatasWithNonExistingKey";
			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _groupDataDbController.Get(newSaveData.GameId, newSaveData.ActorId, new string[] { "null key" });
			Assert.Empty(groupDatas);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingGame()
		{
			const string groupDataName = "GetGroupSaveDatasWithNonExistingGame";
			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _groupDataDbController.Get(-1, newSaveData.ActorId, new string[] { groupDataName });
			Assert.Empty(groupDatas);
		}

		[Fact]
		public void GetGroupSaveDatasWithNonExistingGroup()
		{
			const string groupDataName = "GetGroupSaveDatasWithNonExistingGroup";
			var newSaveData = CreateGroupData(groupDataName);

			var groupDatas = _groupDataDbController.Get(newSaveData.GameId, -1, new string[] { groupDataName });
			Assert.Empty(groupDatas);
		}
		#endregion

		#region Helpers
		private GameData CreateGroupData(string key, int? gameId = null, int? groupId = null)
		{
			var gameDbController = new GameController(NameOrConnectionString);
			if (gameId.HasValue == false)
			{
				var game = new Game
				{
					Name = key
				};
				gameDbController.Create(game);
				gameId = game.Id;
			}

			var groupDbController = new GroupController(NameOrConnectionString);
			if (groupId.HasValue == false)
			{
				var group = new Group
				{
					Name = key
				};
				groupDbController.Create(group);
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
			_groupDataDbController.Create(groupData);

			return groupData;
		}
		#endregion
	}
}

using System.Collections.Generic;
using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	public class EvaluationDataControllerTests : EntityFrameworkTestBase
	{
		#region Configuration
		private readonly EvaluationDataController _evaluationDataController = ControllerLocator.EvaluationDataController;
		private readonly GameController _gameController = ControllerLocator.GameController;
		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUserGameEvaluationData()
		{
			var userDataName = "CreateAndGetUserGameEvaluationData";

			var newEvaluationData = CreateEvaluationData(userDataName, createNewGame: true, createNewUser: true);

			var userDatas = _evaluationDataController.Get(newEvaluationData.GameId, newEvaluationData.ActorId, new List<string> { newEvaluationData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == newEvaluationData.GameId && g.ActorId == newEvaluationData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetUserGlobalEvaluationData()
		{
			var userDataName = "CreateAndGetUserGlobalEvaluationData";

			var newEvaluationData = CreateEvaluationData(userDataName, createNewUser: true);

			var userDatas = _evaluationDataController.Get(newEvaluationData.GameId, newEvaluationData.ActorId, new List<string> { newEvaluationData.Key });

			var matches = userDatas.Count(g => g.Key == userDataName && g.GameId == Platform.GlobalGameId && g.ActorId == newEvaluationData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateAndGetGameGlobalEvaluationData()
		{
			var userDataName = "CreateAndGetGameGlobalEvaluationData";

			var newEvaluationData = CreateEvaluationData(userDataName, createNewGame: true);

			var userDatas = _evaluationDataController.Get(newEvaluationData.GameId, newEvaluationData.ActorId, new List<string> { newEvaluationData.Key });

			var matches = userDatas.Count(g => g.Key == newEvaluationData.Key && g.GameId == newEvaluationData.GameId && g.ActorId == newEvaluationData.ActorId);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void GetMultipleUserEvaluationDatas()
		{
			var userDataNames = new List<string>
			{
				"GetMultipleUserEvaluationDatas1",
				"GetMultipleUserEvaluationDatas2",
				"GetMultipleUserEvaluationDatas3",
				"GetMultipleUserEvaluationDatas4",
			};

			var doNotFind = CreateEvaluationData("GetMultipleUserEvaluationDatas_DontGetThis");
			var gameId = doNotFind.GameId;
			var userId = doNotFind.ActorId;

			foreach (var userDataName in userDataNames)
			{
				CreateEvaluationData(userDataName, gameId, userId);
			}

			var userDatas = _evaluationDataController.Get(gameId, userId, userDataNames);

			var matchingUserEvaluationDatas = userDatas.Select(g => userDataNames.Contains(g.Key));

			Assert.Equal(matchingUserEvaluationDatas.Count(), userDataNames.Count);
		}

		[Fact]
		public void GetUserEvaluationDatasWithNonExistingKey()
		{
			var userDataName = "GetUserEvaluationDatasWithNonExistingKey";

			var newEvaluationData = CreateEvaluationData(userDataName);

			var userDatas = _evaluationDataController.Get(newEvaluationData.GameId, newEvaluationData.ActorId, new List<string> { "null key" });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserEvaluationDatasWithNonExistingGame()
		{
			var userDataName = "GetUserEvaluationDatasWithNonExistingGame";

			var newEvaluationData = CreateEvaluationData(userDataName);

			var userDatas = _evaluationDataController.Get(-1, newEvaluationData.ActorId, new List<string> { userDataName });

			Assert.Empty(userDatas);
		}

		[Fact]
		public void GetUserEvaluationDatasWithNonExistingUser()
		{
			var userDataName = "GetUserEvaluationDatasWithNonExistingUser";

			var newEvaluationData = CreateEvaluationData(userDataName, Platform.GlobalGameId, Platform.GlobalActorId);

			var userDatas = _evaluationDataController.Get(newEvaluationData.GameId, -1, new List<string> { userDataName });

			Assert.Empty(userDatas);
		}

		[Theory]
		[InlineData(EvaluationDataCategory.GameData)]
		[InlineData(EvaluationDataCategory.MatchData)]
		[InlineData(EvaluationDataCategory.Achievement)]
		[InlineData(EvaluationDataCategory.Resource)]
		[InlineData(EvaluationDataCategory.Skill)]
        public void DoesDeleteEvaluationDataWhenUserDeleted(EvaluationDataCategory category)
		{
			// Arrange
			var user = Helpers.CreateUser($"{nameof(DoesDeleteEvaluationDataWhenUserDeleted)}_{category}");

			var datas = Enumerable.Range(0, 10).Select(i => new EvaluationData
			{
				Key = nameof(DoesDeleteEvaluationDataWhenUserDeleted),
				ActorId = user.Id,
				Category = category,
				EvaluationDataType = EvaluationDataType.Long
			}).ToList();

			_evaluationDataController.Create(datas);

			// Act
			_userController.Delete(user.Id);

			// Assert
			var createdDatas = _evaluationDataController.GetActorData(user.Id);
			Assert.Empty(createdDatas);
		}
        #endregion

        #region Helpers
        private Model.EvaluationData CreateEvaluationData(string key, int? gameId = null, int? userId = null,
			bool createNewGame = false, bool createNewUser = false)
		{
			if (createNewGame)
			{
				var game = new Game {
					Name = key
				};
				_gameController.Create(game);
				gameId = game.Id;
			}

			if (createNewUser)
			{
				var user = new User {
					Name = key
				};
				_userController.Create(user);
				userId = user.Id;
			}

			var userData = new Model.EvaluationData {
				Key = key,
				GameId = gameId ?? Platform.GlobalGameId,
				ActorId = userId ?? Platform.GlobalActorId,
				Value = key + " value",
				EvaluationDataType = 0
			};
			_evaluationDataController.Create(userData);
			return userData;
		}
		#endregion
	}
}

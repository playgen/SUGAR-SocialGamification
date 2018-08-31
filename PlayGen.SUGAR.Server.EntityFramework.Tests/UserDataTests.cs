using System;
using System.Collections.Generic;
using System.Text;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
    public class UserDataTests : EntityFrameworkTestBase
	{
		private readonly ActorDataController _actorDataController = ControllerLocator.ActorDataController;
		private readonly UserController _userController = ControllerLocator.UserController;

        [Fact]
		public void DoesDeleteUserDataWhenUserDeleted()
		{
			// Arrange
			var user = Helpers.CreateUser($"{nameof(DoesDeleteUserDataWhenUserDeleted)} User");
			var game = Helpers.CreateGame($"{nameof(DoesDeleteUserDataWhenUserDeleted)} Game");

			_actorDataController.Create(new ActorData
			{
				GameId = game.Id,
				ActorId = user.Id,
				EvaluationDataType = EvaluationDataType.String,
				Value = nameof(DoesDeleteUserDataWhenUserDeleted)
			});
			
			// Act
			_userController.Delete(user.Id);

			// Assert
			var datas = _actorDataController.Get(game.Id, user.Id);
			Assert.Empty(datas);
		}
    }
}

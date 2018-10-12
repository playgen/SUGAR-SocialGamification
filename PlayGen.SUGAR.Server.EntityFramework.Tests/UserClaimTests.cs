using System;
using System.Collections.Generic;
using System.Text;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
    public class UserClaimTests : EntityFrameworkTestBase
	{
		private readonly ActorClaimController _actorClaimController = ControllerLocator.ActorClaimController;
		private readonly UserController _userController = ControllerLocator.UserController;

		[Fact]
		public void DoesDeleteUserClaimsWhenUserDeleted()
		{
			// Arrange
			var user = Helpers.CreateUser(nameof(DoesDeleteUserClaimsWhenUserDeleted));

			var coreClaimController = new Core.Authorization.ClaimController(
				ControllerLocator.ClaimController, 
				ControllerLocator.RoleController, 
				ControllerLocator.RoleClaimController);

			coreClaimController.GetAuthorizationClaims();

            // Act
            _userController.Delete(user.Id);

			// Assert
			var userClaims = _actorClaimController.GetActorClaims(user.Id);
			Assert.Empty(userClaims);
        }
    }
}

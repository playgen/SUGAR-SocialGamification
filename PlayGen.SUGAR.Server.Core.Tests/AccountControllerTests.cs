using System.Linq;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Authorization;
using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Core.Extensions;
using PlayGen.SUGAR.Server.Model;
using Xunit;


namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class AccountControllerTests : CoreTestBase
    {
        #region Configuration
        private readonly AccountController _accountController = ControllerLocator.AccountController;
        private readonly AccountSourceController _accountSourceController = ControllerLocator.AccountSourceController;

	    private readonly UserController _userController = ControllerLocator.UserController;
		private readonly ActorClaimController _actorClaimController = ControllerLocator.ActorClaimController;

        //private readonly SeededData.EntityFramework.Controllers.UserController _userController = DbControllerLocator.UserController;
        //private readonly SeededData.EntityFramework.Controllers.GameController _gameController = DbControllerLocator.GameController;
        #endregion

        #region Tests

        [Fact]
        public void CanAutoRegisterOnNonExistentUserLogin()
        {
            var source = _accountSourceController.Create(new AccountSource()
            {
                RequiresPassword = false,
                Token = "testSource",
                ApiSecret = "",
                UsernamePattern = "",
                Description = "",
                AutoRegister = true
            }); 
			
            var toVerify = new Account()
            {
                 Name = "testUser",
                 Password = "TestPass"
            };

            var response = _accountController.Authenticate(toVerify, source.Token);

            Assert.Equal(toVerify.Name, response.Name);
            Assert.True(response.Id > 0);
        }


	    [Fact]
	    public void CanCreatePrivateUser()
	    {
		    var userName = "CanCreatePrivateUser";

		    CreateUser(userName, true);

		    var user = _userController.GetExistingUser(userName);

		    Assert.Equal(userName, user.Name);

		    Assert.Equal(true, user.Private);
	    }

	    [Fact]
	    public void CanGetPrivateUsersWithGlobalClaims()
	    {
		    var userName = "CanGetPrivateUsersWithGlobalClaims";
		    var privateUserName = userName + "_private";
			var newUser = CreateUser(userName, false);

			
		    //_actorRoleController.Create(ClaimScope.Global, newUser.Id, -1);

			// add private user 
		    var newPrivateUser = CreateUser(privateUserName, true);

		    // Assign claim to user
		    var claim = new ActorClaim()
		    {
			    ActorId = newUser.Id,
			    Claim = new Claim { ClaimScope = ClaimScope.User },
			    EntityId = newPrivateUser.Id
		    };
		    _actorClaimController.Create(claim);


			var user = _userController.Get(newPrivateUser.Id, ActorVisibilityFilter.All);

			Assert.NotNull(user);
			Assert.Equal(privateUserName, user.Name);
	    }

	    [Fact]
	    public void CannotFindPrivateUserInSearch()
	    {
		    var userName = "CannotFindPrivateUserInSearch";

		    CreateUser(userName, true);

			// sending with id -1 as should not be able to retriev newUser
		    var allUsers = _userController.GetAll(ActorVisibilityFilter.Public);

		    var matches = allUsers.Count(u => u.Name == userName);

		    Assert.Equal(0, matches);
	    }

	    [Fact]
	    public void CanRetrievePrivateSelfFromSearch()
	    {
		    var userName = "CanRetrievePrivateSelfFromSearch";

		    var newUser = CreateUser(userName, true);
		    var allUsers = _userController.GetAll(ActorVisibilityFilter.Private);

		    var matches = allUsers.Count(u => u.Name == userName);

			Assert.Equal(1, matches);
	    }

		#endregion

		#region Helpers

		private User CreateUser(string name, bool privateUser)
		{
			var user = new User
			{
				Name = name,
				Private = privateUser
			};

			_userController.Create(user);

			return user;
		}

		#endregion
	}
}

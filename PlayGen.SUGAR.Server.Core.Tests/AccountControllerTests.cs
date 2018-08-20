using System.Linq;
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
	    private readonly ActorRoleController _actorRoleController = ControllerLocator.ActorRoleController;

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
	    public void CannotFindPrivateUserInSearch()
	    {
		    var userName = "CannotFindPrivateUserInSearch";

		    CreateUser(userName, true);

			// sending with id -1 as should not be able to retriev newUser
		    var allUsers = _userController.GetAll(-1);
			allUsers = allUsers.FilterPrivate(_actorRoleController, -1);

		    var matches = allUsers.Count(u => u.Name == userName);

		    Assert.Equal(0, matches);
	    }

	    [Fact]
	    public void CanRetrievePrivateSelfFromSearch()
	    {
		    var userName = "CanRetrievePrivateSelfFromSearch";

		    var newUser = CreateUser(userName, true);
		    var allUsers = _userController.GetAll(newUser.Id);
		    allUsers = allUsers.FilterPrivate(_actorRoleController, newUser.Id);

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

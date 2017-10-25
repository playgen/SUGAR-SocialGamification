using PlayGen.SUGAR.Server.Core.Controllers;
using PlayGen.SUGAR.Server.Model;
using Xunit;


namespace PlayGen.SUGAR.Server.Core.Tests
{
    public class AccountControllerTests : CoreTestBase
    {
        #region Configuration
        private readonly AccountController _accountController = ControllerLocator.AccountController;
        private readonly AccountSourceController _accountSourceController = ControllerLocator.AccountSourceController;
        //private readonly Data.EntityFramework.Controllers.UserController _userController = DbControllerLocator.UserController;
        //private readonly Data.EntityFramework.Controllers.GameController _gameController = DbControllerLocator.GameController;
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

        #endregion
    }
}

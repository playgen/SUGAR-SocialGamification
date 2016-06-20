using PlayGen.SGA.DataController;
using PlayGen.SGA.DataController.UnitTests;
using PlayGen.SGA.ServerAuthentication;
using PlayGen.SGA.WebAPI.Controllers;
using Xunit;

namespace PlayGen.SGA.WebAPI.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        #region Configuration
        private readonly AccountController _accountController;
        private readonly UserDbController _userDbController;

        public AccountControllerTests()
        {
            _accountController = new AccountController(
                new AccountDbController(TestDbController.NameOrConnectionString),
                new UserDbController(TestDbController.NameOrConnectionString),
                new PasswordEncryption(),
                new JsonWebTokenUtility("5Y2gQ33IrRffE66030Dy1om5nk4HI58V"));

            _userDbController = new UserDbController(TestDbController.NameOrConnectionString);
        }
        #endregion

        [Fact]
        public void Test()
        {
            Assert.True(true);  
        }
    }
}

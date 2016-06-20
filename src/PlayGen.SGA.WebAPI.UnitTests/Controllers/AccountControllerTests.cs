using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataController;
using PlayGen.SGA.DataController.UnitTests;
using PlayGen.SGA.DataModel;
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

        #region Tests
        [Fact]
        public void InvalidRegisterName()
        {
            var accountRequest = new AccountRequest
            {
                Password = "InvalidRegisterNamePassword",
            };

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 
        }

        [Fact]
        public void InvalidRegisterPassword()
        {
            var accountRequest = new AccountRequest
            {
                Name = "InvalidRegisterPassword",
            };

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 
        }

        [Fact]
        public void RegisterAndLogin()
        {
            var accountRequest = CreatAccountRequest("RegisterAndLogin", "RegisterAndLoginPassword");

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 

            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void RegisterInvalidUser()
        {
            int userId = -1;

            var accountRequest = CreatAccountRequest("RegisterInvalidUser", "RegisterInvalidUser");

            var response = _accountController.Register(userId, accountRequest);

            // Todo Validate Response 

            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void RegisterUser()
        {
            var accountRequest = CreatAccountRequest("RegisterInvalidUser", "RegisterInvalidUser");

            var user = new User
            {
                Name = accountRequest.Name,
            };
            user = _userDbController.Create(user);

            var response = _accountController.Register(user.Id, accountRequest);

            // Todo Validate Response 

            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void InvalidLoginName()
        {
            var accountRequest = CreatAccountRequest("InvalidLoginName", "InvalidLoginName" );

            var response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void InvalidLoginPassword()
        {
            var accountRequest = CreatAccountRequest("InvalidLoginPassword", "InvalidLoginPasswordPassword");

            var response = _accountController.Register(accountRequest);

            accountRequest.Password = "ThisPasswordShouldFail";
            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }
        #endregion

        #region helpers

        private AccountRequest CreatAccountRequest(string name, string password)
        {
            return new AccountRequest
            {
                Name = name,
                Password = password,
            };
        }
        #endregion
    }
}

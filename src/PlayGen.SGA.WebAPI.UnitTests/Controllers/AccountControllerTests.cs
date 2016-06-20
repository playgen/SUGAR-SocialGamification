using System;
using System.Net.Configuration;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.DataController;
using PlayGen.SGA.DataController.UnitTests;
using PlayGen.SGA.DataModel;
using PlayGen.SGA.ServerAuthentication;
using PlayGen.SGA.WebAPI.Controllers;
using PlayGen.SGA.WebAPI.Exceptions;
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
            InitializeEnvironment();

            _accountController = new AccountController(
                new AccountDbController(TestDbController.NameOrConnectionString),
                new UserDbController(TestDbController.NameOrConnectionString),
                new PasswordEncryption(),
                new JsonWebTokenUtility("5Y2gQ33IrRffE66030Dy1om5nk4HI58V"));

            _userDbController = new UserDbController(TestDbController.NameOrConnectionString);
        }

        private void InitializeEnvironment()
        {
            TestDbController.DeleteDatabase();
        }
        #endregion

        #region Tests
        [Fact]
        public void RegisterInvalidAccountName()
        {
            var accountRequest = new AccountRequest
            {
                Password = "RegisterInvalidAccountNamePassword",
            };

            bool hadException = false;

            try
            {
                var response = _accountController.Register(accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }
            
            Assert.True(hadException);
        }

        [Fact]
        public void RegisterInvalidAccountPassword()
        {
            var accountRequest = new AccountRequest
            {
                Name = "RegisterInvalidAccountPassword",
            };

            bool hadException = false;

            try
            {
                var response = _accountController.Register(accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void RegisterNewUserAndLogin()
        {
            var accountRequest = CreatAccountRequest("RegisterNewUserAndLogin", "RegisterNewUserAndLoginPassword");

            var response = _accountController.Register(accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);

            response = _accountController.Login(accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);
        }

        [Fact]
        public void RegisterInvalidUser()
        {
            int userId = -1;

            var accountRequest = CreatAccountRequest("RegisterInvalidUser", "RegisterInvalidUser");

            bool hadException = false;

            try
            {
                var response = _accountController.Register(userId, accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }
            
            Assert.True(hadException);
        }

        [Fact]
        public void RegisterExistingUserAndLogin()
        {
            var accountRequest = CreatAccountRequest("RegisterExistingUserAndLogin", "RegisterExistingUserAndLoginPassword");

            var user = new User
            {
                Name = accountRequest.Name,
            };
            _userDbController.Create(user);

            var response = _accountController.Register(user.Id, accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);

            response = _accountController.Login(accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);
        }

        [Fact]
        public void LoginInvalidAccountName()
        {
            var accountRequest = CreatAccountRequest("LoginInvalidAccountName", "LoginInvalidAccountNamePassword");

            _accountController.Register(accountRequest);

            accountRequest.Name = "ThisAccountNameShouldFail";

            bool hadException = false;

            try
            {
                _accountController.Login(accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }
            
            Assert.True(hadException);
        }

        [Fact]
        public void LoginInvalidAccountPassword()
        {
            var accountRequest = CreatAccountRequest("LoginInvalidAccountPassword", "LoginInvalidAccountPasswordPassword");

            _accountController.Register(accountRequest);

            accountRequest.Password = "ThisPasswordShouldFail";

            bool hadException = false;

            try
            {
                _accountController.Login(accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }

            Assert.True(hadException);
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

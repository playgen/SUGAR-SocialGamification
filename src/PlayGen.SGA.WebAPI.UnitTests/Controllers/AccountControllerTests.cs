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
        public void InvalidRegisterName()
        {
            var accountRequest = new AccountRequest
            {
                Password = "InvalidRegisterNamePassword",
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
        public void InvalidRegisterPassword()
        {
            var accountRequest = new AccountRequest
            {
                Name = "InvalidRegisterPassword",
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
        public void RegisterAndLogin()
        {
            var accountRequest = CreatAccountRequest("RegisterAndLogin", "RegisterAndLoginPassword");

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
        public void RegisterUser()
        {
            var accountRequest = CreatAccountRequest("RegisterInvalidUser", "RegisterInvalidUserPassword");

            var user = new User
            {
                Name = accountRequest.Name,
            };
            user = _userDbController.Create(user);

            var response = _accountController.Register(user.Id, accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);

            response = _accountController.Login(accountRequest);

            Assert.NotEqual(null, response.User);
            Assert.NotEqual(null, response.Token);
        }

        [Fact]
        public void InvalidLoginName()
        {
            var accountRequest = CreatAccountRequest("InvalidLoginName", "InvalidLoginName" );

            bool hadException = false;

            try
            {
                var response = _accountController.Login(accountRequest);
            }
            catch (InvalidAccountDetailsException)
            {
                hadException = true;
            }
            
            Assert.True(hadException);
        }

        [Fact]
        public void InvalidLoginPassword()
        {
            var accountRequest = CreatAccountRequest("InvalidLoginPassword", "InvalidLoginPasswordPassword");

            var response = _accountController.Register(accountRequest);

            accountRequest.Password = "ThisPasswordShouldFail";

            bool hadException = false;

            try
            {
                response = _accountController.Login(accountRequest);
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

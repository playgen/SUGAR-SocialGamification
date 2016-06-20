using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var accountRequest = new AccountRequest();
            accountRequest.Password = "InvalidRegisterNamePassword";

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 
        }

        [Fact]
        public void InvalidRegisterPassword()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "InvalidRegisterPassword";

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 
        }

        [Fact]
        public void RegisterAndLogin()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "RegisterAndLogin";
            accountRequest.Password = "RegisterAndLoginPassword";

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 

            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void RegisterInvalidUser()
        {
            int userId = -1;

            var accountRequest = new AccountRequest();
            accountRequest.Name = "RegisterInvalidUser";
            accountRequest.Password = "RegisterInvalidUser";

            var response = _accountController.Register(userId, accountRequest);

            // Todo Validate Response 

            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void RegisterUser()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "RegisterInvalidUser";
            accountRequest.Password = "RegisterInvalidUser";

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
            var accountRequest = new AccountRequest();
            accountRequest.Name = "InvalidLoginName";
            accountRequest.Password = "InvalidLoginName";

            var response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }

        [Fact]
        public void InvalidLoginPassword()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "InvalidLoginPassword";
            accountRequest.Password = "InvalidLoginPasswordPassword";

            var response = _accountController.Register(accountRequest);

            accountRequest.Password = "ThisPasswordShouldFail";
            response = _accountController.Login(accountRequest);

            // Todo Validate Response
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.Contracts;
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

        public AccountControllerTests()
        {
            _accountController = new AccountController(
                new AccountDbController(TestDbController.NameOrConnectionString), 
                new PasswordEncryption(),
                new JsonWebTokenUtility("5Y2gQ33IrRffE66030Dy1om5nk4HI58V"));
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

        public void InvalidRegisterPassword()
        {
            var accountRequest = new AccountRequest();
            accountRequest.Name = "InvalidRegisterPassword";

            var response = _accountController.Register(accountRequest);

            // Todo Validate Response 
        }

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

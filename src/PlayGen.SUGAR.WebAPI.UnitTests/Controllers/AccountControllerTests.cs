using System;
using System.Net.Configuration;
using PlayGen.SUGAR.Contracts;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.ServerAuthentication;
using PlayGen.SUGAR.WebAPI.Controllers;
using PlayGen.SUGAR.WebAPI.Exceptions;
using PlayGen.SUGAR.Data.EntityFramework.UnitTests;
using Xunit;

namespace PlayGen.SUGAR.WebAPI.UnitTests.Controllers
{
	public class AccountControllerTests
	{
		#region Configuration
		private readonly AccountController _accountController;
		private readonly Data.EntityFramework.Controllers.UserController _userDbController;

		public AccountControllerTests()
		{
			InitializeEnvironment();

			_accountController = new AccountController(
				new Data.EntityFramework.Controllers.AccountController(TestController.NameOrConnectionString),
				new Data.EntityFramework.Controllers.UserController(TestController.NameOrConnectionString),
				new PasswordEncryption(),
				new JsonWebTokenUtility("5Y2gQ33IrRffE66030Dy1om5nk4HI58V"));

			_userDbController = new Data.EntityFramework.Controllers.UserController(TestController.NameOrConnectionString);
		}

		private void InitializeEnvironment()
		{
			TestController.DeleteDatabase();
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

			Assert.Throws<InvalidAccountDetailsException>(() => _accountController.Register(accountRequest));
		}

		[Fact]
		public void RegisterInvalidAccountPassword()
		{
			var accountRequest = new AccountRequest
			{
				Name = "RegisterInvalidAccountPassword",
			};

			Assert.Throws<InvalidAccountDetailsException>(() => _accountController.Register(accountRequest));
		}

		[Fact]
		public void RegisterNewUser()
		{
			var accountRequest = CreatAccountRequest("RegisterNewUser", "RegisterNewUserPassword");

			var response = _accountController.Register(accountRequest);

			// Todo modify to evaluate new type returned by _accountController
		}

		[Fact]
		public void RegisterNewUserAndLogin()
		{
			var accountRequest = CreatAccountRequest("RegisterNewUserAndLogin", "RegisterNewUserAndLoginPassword");
			accountRequest.AutoLogin = true;

			var response = _accountController.Register(accountRequest);

			// Todo modify to evaluate new type returned by _accountController
		}

		[Fact]
		public void LoginUser()
		{
			var accountRequest = CreatAccountRequest("LoginUser", "LoginUserPassword");

			_accountController.Register(accountRequest);
			var response = _accountController.Login(accountRequest);

			// Todo modify to evaluate new type returned by _accountController
		}

		[Fact]
		public void RegisterInvalidUser()
		{
			int userId = -1;

			var accountRequest = CreatAccountRequest("RegisterInvalidUser", "RegisterInvalidUser");

			Assert.Throws<InvalidAccountDetailsException>(() => _accountController.Register(accountRequest));
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

			var response = _accountController.Register(accountRequest);

			// Todo modify to evaluate new type returned by _accountController

			response = _accountController.Login(accountRequest);

			// Todo modify to evaluate new type returned by _accountController
		}

		[Fact]
		public void LoginInvalidAccountName()
		{
			var accountRequest = CreatAccountRequest("LoginInvalidAccountName", "LoginInvalidAccountNamePassword");

			_accountController.Register(accountRequest);

			accountRequest.Name = "ThisAccountNameShouldFail";

			Assert.Throws<InvalidAccountDetailsException>(() => _accountController.Login(accountRequest));
		}

		[Fact]
		public void LoginInvalidAccountPassword()
		{
			var accountRequest = CreatAccountRequest("LoginInvalidAccountPassword", "LoginInvalidAccountPasswordPassword");

			_accountController.Register(accountRequest);

			accountRequest.Password = "ThisPasswordShouldFail";

			Assert.Throws<InvalidAccountDetailsException>(() => _accountController.Login(accountRequest));
		}
		#endregion

		#region helpers
		private AccountRequest CreatAccountRequest(string name, string password)
		{
			return new AccountRequest
			{
				Name = name,
				Password = password,
				AutoLogin = false,
			};
		}
		#endregion
	}
}

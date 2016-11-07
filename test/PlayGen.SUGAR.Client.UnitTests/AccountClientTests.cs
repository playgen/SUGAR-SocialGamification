using PlayGen.SUGAR.Contracts.Shared;
using Xunit;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class AccountClientTests
	{
		#region Configuration
		private readonly AccountClient _accountClient;

		public AccountClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_accountClient = testSugarClient.Account;
		}
		#endregion

		#region Tests
		[Fact]
		public void CanRegisterNewUserAndLogin()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CanRegisterNewUserAndLogin",
				Password = "CanRegisterNewUserAndLoginPassword",
				AutoLogin = true,
			};

			var registerResponse = _accountClient.Register(accountRequest);

			Assert.True(registerResponse.User.Id > 0);
			Assert.Equal(accountRequest.Name, registerResponse.User.Name);
		}
		/*
		[Fact]
		public void CannotRegisterDuplicate()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CannotRegisterDuplicate",
				Password = "CannotRegisterDuplicatePassword",
			};

			var registerResponse = _accountClient.Register(accountRequest);

			Assert.Throws<ClientException>(() => _accountClient.Register(accountRequest));
		}

		[Fact]
		public void CannotRegisterInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientException>(() => _accountClient.Register(accountRequest));
		}

		[Fact]
		public void CanLoginValidUser()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CanLoginValidUser",
				Password = "CanLoginValidUserPassword",
			};

			var registerResponse = _accountClient.Register(accountRequest);

			var logged = _accountClient.Login(accountRequest);

			Assert.True(logged.User.Id > 0);
			Assert.Equal(accountRequest.Name, logged.User.Name);
		}

		[Fact]
		public void CannotLoginInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientException>(() => _accountClient.Login(accountRequest));
		}*/
		#endregion
	}
}

using System;
using PlayGen.SUGAR.Contracts;
using Xunit;
using System.Net;

namespace PlayGen.SUGAR.Client.IntegrationTests
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
		public void CanRegisterNewUser()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CanRegisterNewUser",
				Password = "CanRegisterNewUserPassword",
				AutoLogin = false,
			};

			var registerResponse = _accountClient.Register(accountRequest);

			Assert.True(registerResponse.User.Id > 0);
			Assert.Equal(accountRequest.Name, registerResponse.User.Name);
		}

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

		[Fact]
		public void CantRegisterInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<WebException>(() => _accountClient.Register(accountRequest));
		}

		// TODO: Test logging in an existing user

		[Fact]
		public void CantLoginInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<WebException>(() => _accountClient.Login(accountRequest));
		}
		#endregion
	}
}

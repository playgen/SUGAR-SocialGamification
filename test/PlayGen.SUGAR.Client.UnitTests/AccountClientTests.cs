using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;

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
        [Test]
		public void CannotCreateDuplicate()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CannotCreateDuplicate",
				Password = "CannotCreateDuplicatePassword",
                SourceToken = "SUGAR"
            };

			var registerResponse = _accountClient.Create(accountRequest);

			Assert.Throws<ClientException>(() => _accountClient.Create(accountRequest));
		}

		[Test]
		public void CannotCreateInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientException>(() => _accountClient.Create(accountRequest));
		}
		#endregion
	}
}

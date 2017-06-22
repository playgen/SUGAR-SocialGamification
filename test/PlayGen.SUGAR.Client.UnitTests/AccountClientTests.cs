using PlayGen.SUGAR.Contracts;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class AccountClientTests : ClientTestsBase
	{
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

			var registerResponse = SUGARClient.Account.Create(accountRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Account.Create(accountRequest));
		}

		[Test]
		public void CannotCreateInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientHttpException>(() => SUGARClient.Account.Create(accountRequest));
		}
		#endregion
	}
}

using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class AccountTests : ClientTestBase
	{
		[Fact]
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

		[Fact]
		public void CannotCreateInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientHttpException>(() => SUGARClient.Account.Create(accountRequest));
		}
	}
}

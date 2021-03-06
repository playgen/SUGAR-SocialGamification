﻿using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class AccountClientTests : ClientTestBase
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

			Fixture.SUGARClient.Account.Create(accountRequest);
			
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Account.Create(accountRequest));
		}

		[Fact]
		public void CannotCreateInvalidUser()
		{
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Account.Create(accountRequest));
		}

		public AccountClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}

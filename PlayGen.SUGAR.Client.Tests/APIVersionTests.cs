using System.Collections.Generic;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class APIVersionTests : ClientTestBase
	{
		[Fact]
		public void CanAccessWithMatchingAPIVersion()
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = $"{nameof(CanAccessWithMatchingAPIVersion)}",
				Password = $"{nameof(CanAccessWithMatchingAPIVersion)}Password",
				SourceToken = "SUGAR"
			};

			// Act
			var accountResponse = Fixture.SUGARClient.Account.Create(accountRequest);

			// Asssert
			Assert.NotNull(accountResponse);
		}

		[Fact]
		public void CantAccessWithMismatchedAPIVersion()
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = $"{nameof(CantAccessWithMismatchedAPIVersion)}",
				Password = $"{nameof(CantAccessWithMismatchedAPIVersion)}Password",
				SourceToken = "SUGAR"
			};

			var sugarClient = Fixture.CreateSugarClient(new Dictionary<string, string> { { "APIVersion", "0.0.0" } });

			// Act & Assert
			Assert.Throws<ClientHttpException>(() => sugarClient.Account.Create(accountRequest));
		}

		[Fact]
		public void CantAccessWithNoAPIVersion()
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = $"{nameof(CantAccessWithNoAPIVersion)}",
				Password = $"{nameof(CantAccessWithNoAPIVersion)}Password",
				SourceToken = "SUGAR"
			};

			var sugarClient = Fixture.CreateSugarClient(new Dictionary<string, string>());

			// Act & Assert
			Assert.Throws<ClientHttpException>(() => sugarClient.Account.Create(accountRequest));
		}

		[Fact]
		public void CanGetAPIVersion()
		{
			// Arrange
			var sugarClient = Fixture.CreateSugarClient(new Dictionary<string, string>());

			// Act
			var version = sugarClient.APIVersion.Get();

			// Assert
			Assert.NotEmpty(version);
			Assert.Contains(APIVersion.Version, version);
		}

		[Theory]
		[InlineData(0, 0)]
		[InlineData(0, 1)]
		[InlineData(2, 8)]
		[InlineData(35, 2387)]
		public void CanAccessWithMatchingMajorButMismatchedMinor(int minor, int build)
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = $"{nameof(CanAccessWithMatchingMajorButMismatchedMinor)}_{minor}.{build}",
				Password = $"{nameof(CanAccessWithMatchingMajorButMismatchedMinor)}Password",
				SourceToken = "SUGAR"
			};

			var sugarClient = Fixture.CreateSugarClient(new Dictionary<string, string> { { APIVersion.Key, $"{APIVersion.Major}.{minor}.{build}" } });

			// Act
			var response = sugarClient.Account.Create(accountRequest);

			// Assert
			Assert.NotNull(response);
		}

		public APIVersionTests(ClientTestsFixture fixture) : base(fixture)
		{
		}
	}
}

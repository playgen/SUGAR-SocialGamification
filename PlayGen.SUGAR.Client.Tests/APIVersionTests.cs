using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
				Name = "CanAccessWithMatchingAPIVersion",
				Password = "CanAccessWithMatchingAPIVersionPassword",
				SourceToken = "SUGAR"
			};

			// Act
			var accountResponse = SUGARClient.Account.Create(accountRequest);

			// Asssert
			Assert.NotNull(accountResponse);
		}

		[Fact]
		public void CantAccessWithMismatchedAPIVersion()
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = "CantAccessWithMismatchedAPIVersion",
				Password = "CantAccessWithMismatchedAPIVersionPassword",
				SourceToken = "SUGAR"
			};

			var sugarClient = CreateSugarClient();

			var type = typeof(ClientBase);
			var fieldInfo = type.GetField("PersistentHeaders", BindingFlags.NonPublic | BindingFlags.Static);
			var persistentHeaders = (Dictionary<string, string>)fieldInfo.GetValue(SUGARClient.Account);

			persistentHeaders["APIVersion"] = "0.0.0";

			// Act & Assert
			Assert.Throws<ClientHttpException>(() => sugarClient.Account.Create(accountRequest));
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
				Name = $"CanAccessWithMatchingMajorButMismatchedMinor_{minor}.{build}",
				Password = "CanAccessWithMatchingMajorButMismatchedMinorPassword",
				SourceToken = "SUGAR"
			};

			var sugarClient = CreateSugarClient();

			var type = typeof(ClientBase);
			var fieldInfo = type.GetField("PersistentHeaders", BindingFlags.NonPublic | BindingFlags.Static);
			var persistentHeaders = (Dictionary<string, string>)fieldInfo.GetValue(SUGARClient.Account);

			persistentHeaders["APIVersion"] = $"{APIVersion.Major}.{minor}.{build}";

			// Act
			var response = sugarClient.Account.Create(accountRequest);

			// Assert
			Assert.NotNull(response);
		}
	}
}

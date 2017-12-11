using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Common.Web;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class SessionClientTests : ClientTestBase
	{
		[Fact]
		public void CanHeartbeatAndReissueToken()
		{
			var key = "Session_CanHeartbeatAndReissueToken";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			// Arrange
			var headers = (Dictionary<string, string>)
				typeof(ClientBase)
				.GetField("_persistentHeaders", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(Fixture.SUGARClient.Session);

			var originalToken = headers[HeaderKeys.Authorization];
			
			// Act
			Thread.Sleep(1 * 1000);
			Fixture.SUGARClient.Session.Heartbeat();

			// Assert
			var postHeartbeatToken = headers[HeaderKeys.Authorization];
			Assert.NotEqual(originalToken, postHeartbeatToken);
		}

		[Fact]
		public void NewTokenForUserLogin()
		{
			var key = "Session_NewTokenForUserLogin";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			// Arrange
			var headers = (Dictionary<string, string>)
				typeof(ClientBase)
				.GetField("_persistentHeaders", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(Fixture.SUGARClient.Session);

			var originalToken = headers[HeaderKeys.Authorization];

			// Act
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key + "_New");

			// Assert
			var newToken = headers[HeaderKeys.Authorization];
			Assert.NotEqual(originalToken, newToken);
		}

		[Fact]
		public void CanCreateNewUserAndLogin()
		{
			var key = "Session_CanCreateNewUserAndLogin";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, "Global");

			var accountRequest = new AccountRequest
			{
				Name = key + "_New",
				Password = key + "_New" + "Password",
				SourceToken = "SUGAR"
			};

			var registerResponse = Fixture.SUGARClient.Session.CreateAndLogin(game.Id, accountRequest);

			Assert.True(registerResponse.User.Id > 0);
			Assert.Equal(accountRequest.Name, registerResponse.User.Name);
		}

		[Fact]
		public void CanLoginUser()
		{
			var key = "Session_CanLoginUser";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, "Global");

			var accountRequest = new AccountRequest
			{
				Name = key,
				Password = "ThisIsTheTestingPassword",
				SourceToken = "SUGAR"
			};

			var logged = Fixture.SUGARClient.Session.Login(game.Id, accountRequest);

			Assert.True(logged.User.Id > 0);
			Assert.Equal(accountRequest.Name, logged.User.Name);
		}

		[Fact]
		public void CanLoginUserAsync()
		{
			var key = "Session_CanLoginUserAsync";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, "Global");

			var accountRequest = new AccountRequest
			{
				Name = key,
				Password = "ThisIsTheTestingPassword",
				SourceToken = "SUGAR"
			};

			AccountResponse response = null;
			Exception exception = null;

			// Act
			Fixture.SUGARClient.Session.LoginAsync(game.Id,
				accountRequest,
				r => response = r,
				e => exception = e);

			// Assert
			var executionCount = 0;
			while (executionCount < 1)
			{
				if (Fixture.SUGARClient.TryExecuteResponse())
				{
					executionCount++;
				}
			}

			Assert.NotNull(response);
			Assert.Null(exception);
			Assert.Equal(accountRequest.Name.ToLower(), response.User.Name.ToLower());
			Assert.True(response.User.Id >= 1);
		}

		[Fact]
		public void CannotLoginInvalidUser()
		{
			var key = "Session_CannotLoginInvalidUser";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, "Global");

			var accountRequest = new AccountRequest();
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Session.Login(game.Id, accountRequest));
		}

		[Fact]
		public void CanLogoutAndInvalidateSessionMethod()
		{
			var key = "Session_CanLogoutAndInvalidateSessionMethod";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			// Act
			Fixture.SUGARClient.Session.Logout();

			// Assert
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Session.Heartbeat());
		}

		[Fact]
		public void CanLogoutAndInvalidateSessionClass()
		{
			var key = "Session_CanLogoutAndInvalidateSessionClass";
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);
			var game = Helpers.GetGame(Fixture.SUGARClient.Game, "Global");

			// Act
			Fixture.SUGARClient.Session.Logout();

			// Assert
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.GameData.Add(new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				EvaluationDataType = EvaluationDataType.String,
				Value = key
			}));
		}

		public SessionClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}

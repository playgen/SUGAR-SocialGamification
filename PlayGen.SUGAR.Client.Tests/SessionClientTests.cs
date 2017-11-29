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
			// Arrange
			var headers = (Dictionary<string, string>)
				typeof(ClientBase)
				.GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
				.GetValue(SUGARClient.Session);

			var originalToken = headers[HeaderKeys.Authorization];
			
			// Act
			Thread.Sleep(1 * 1000);
			SUGARClient.Session.Heartbeat();

			// Assert
			var postHeartbeatToken = headers[HeaderKeys.Authorization];
			Assert.NotEqual(originalToken, postHeartbeatToken);
		}

		[Fact]
		public void NewTokenForUserLogin()
		{
			// Arrange
			var headers = (Dictionary<string, string>)
				typeof(ClientBase)
				.GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
				.GetValue(SUGARClient.Session);

			var originalToken = headers[HeaderKeys.Authorization];

			// Act
			Helpers.Login(SUGARClient.Session, new AccountRequest
			{
				Name = "NewTokenForUserLogin",
				Password = "NewTokenForUserLoginPassword",
				SourceToken = "SUGAR"
			});

			// Assert
			var newToken = headers[HeaderKeys.Authorization];
			Assert.NotEqual(originalToken, newToken);
		}

		[Fact]
		public void NewTokenForGameLogin()
		{
			// Arrange
			var headers = (Dictionary<string, string>)
				typeof(ClientBase)
				.GetField("PersistentHeaders", BindingFlags.Static | BindingFlags.NonPublic)
				.GetValue(SUGARClient.Session);

			var originalToken = headers[HeaderKeys.Authorization];

			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(SessionClientTests)}_NewTokenForGameLogin_Original");

			Helpers.Login(SUGARClient.Session, game.Id, new AccountRequest
			{
				Name = "NewTokenForGameLogin",
				Password = "NewTokenForGameLoginPassword",
				SourceToken = "SUGAR"
			});

			LoginAdmin();

			var newGame = Helpers.GetGame(SUGARClient.Game, $"{nameof(SessionClientTests)}_NewTokenForGameLogin_New");

			// Act
			Helpers.Login(SUGARClient.Session, newGame.Id, new AccountRequest
			{
				Name = "NewTokenForGameLogin",
				Password = "NewTokenForGameLoginPassword",
				SourceToken = "SUGAR"
			});

			// Assert
			var newToken = headers[HeaderKeys.Authorization];
			Assert.NotEqual(originalToken, newToken);
		}

		[Fact]
		public void CanCreateNewUserAndLogin()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CanCreateNewUserAndLogin",
				Password = "CanCreateNewUserAndLoginPassword",
				SourceToken = "SUGAR"
			};

			var registerResponse = SUGARClient.Session.CreateAndLogin(accountRequest);

			Assert.True(registerResponse.User.Id > 0);
			Assert.Equal(accountRequest.Name, registerResponse.User.Name);
		}

		[Fact]
		public void CanLoginUser()
		{
			var accountRequest = new AccountRequest
			{
				Name = "CanLoginUser",
				Password = "CanLoginUserPassword",
				SourceToken = "SUGAR"
			};

			SUGARClient.Account.Create(accountRequest);

			var logged = SUGARClient.Session.Login(accountRequest);

			Assert.True(logged.User.Id > 0);
			Assert.Equal(accountRequest.Name, logged.User.Name);
		}

		[Fact]
		public void CanLoginUserAsync()
		{
			// Arrange
			var accountRequest = new AccountRequest
			{
				Name = "CanLoginUserAsync",
				Password = "CanLoginUserAsyncPassword",
				SourceToken = "SUGAR"
			};

			var game = Helpers.GetGame(SUGARClient.Game, $"{nameof(SessionClientTests)}_CanLoginUserAsync");

			SUGARClient.Account.Create(accountRequest);

			AccountResponse response = null;
			Exception exception = null;

			// Act
			SUGARClient.Session.LoginAsync(game.Id,
				accountRequest,
				r => response = r,
				e => exception = e);

			// Assert
			var executionCount = 0;
			while (executionCount < 1)
			{
				if (SUGARClient.TryExecuteResponse())
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
			var accountRequest = new AccountRequest();
			Assert.Throws<ClientHttpException>(() => SUGARClient.Session.Login(accountRequest));
		}

		[Fact]
		public void CanLogoutAndInvalidateSessionMethod()
		{
			// Arrange
			SUGARClient.Session.CreateAndLogin(new AccountRequest
			{
				Name = "CanLogoutAndInvalidateSessionMethod",
				Password = "CanLogoutAndInvalidateSessionMethodPassword",
				SourceToken = "SUGAR"
			});

			// Act
			SUGARClient.Session.Logout();

			// Assert
			Assert.Throws<ClientHttpException>(() => SUGARClient.Session.Heartbeat());
		}

		[Fact]
		public void CanLogoutAndInvalidateSessionClass()
		{
			// Arrange
			SUGARClient.Session.CreateAndLogin(new AccountRequest
			{
				Name = "CanLogoutAndInvalidateSessionClass",
				Password = "CanLogoutAndInvalidateSessionClassPassword",
				SourceToken = "SUGAR"
			});

			// Act
			SUGARClient.Session.Logout();

			// Assert
			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(new EvaluationDataRequest
			{
				CreatingActorId = Helpers.GetUser(SUGARClient.User, $"{nameof(SessionClientTests)}_CanLogoutAndInvalidateSessionClass").Id,
				GameId = Helpers.GetGame(SUGARClient.Game, $"{nameof(SessionClientTests)}_CanLogoutAndInvalidateSessionClass").Id,
				Key = "CanLogoutAndInvalidateSessionClass",
				EvaluationDataType = EvaluationDataType.String,
				Value = "CanLogoutAndInvalidateSessionClass"
			}));
		}
	}
}

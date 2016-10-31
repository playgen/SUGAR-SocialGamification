using System;
using System.Linq;
using PlayGen.SUGAR.Contracts;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class LeaderboardClientTests
	{
		#region Configuration
		private readonly LeaderboardClient _leaderboardClient;
		private readonly GameDataClient _gameDataClient;
		private readonly UserClient _userClient;
		private readonly GroupClient _groupClient;
		private readonly GameClient _gameClient;

		public LeaderboardClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_leaderboardClient = testSugarClient.Leaderboard;
			_gameDataClient = testSugarClient.GameData;
			_userClient = testSugarClient.User;
			_groupClient = testSugarClient.Group;
			_gameClient = testSugarClient.Game;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "LeaderboardClientTests",
				Password = "LeaderboardClientTestsPassword",
				AutoLogin = true,
			};

			try
			{
				client.Login(accountRequest);
			}
			catch
			{
				client.Register(accountRequest);
			}
		}
		#endregion

		#region Tests
		[Test]
		public void CanCreateAndGetGlobalLeaderboard()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGlobalLeaderboard",
				Name = "CanCreateAndGetGlobalLeaderboard",
				Key = "CanCreateAndGetGlobalLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};
			
			var createResponse = _leaderboardClient.Create(createRequest);
			var getResponse = _leaderboardClient.GetGlobal(createRequest.Token);

			Assert.AreEqual(createRequest.Token, createResponse.Token);
			Assert.AreEqual(createRequest.Name, createResponse.Name);
			Assert.AreEqual(createRequest.Key, createResponse.Key);
			Assert.AreEqual(createRequest.ActorType, createResponse.ActorType);
			Assert.AreEqual(createRequest.GameDataType, createResponse.GameDataType);
			Assert.AreEqual(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.GameDataType, getResponse.GameDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CanCreateAndGetGameLeaderboard()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGameLeaderboard",
				GameId = game.Id,		
				Name = "CanCreateAndGetGameLeaderboard",
				Key = "CanCreateAndGetGameLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);
			var getResponse = _leaderboardClient.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(createRequest.Token, createResponse.Token);
			Assert.AreEqual(createRequest.GameId, createResponse.GameId);
			Assert.AreEqual(createRequest.Name, createResponse.Name);
			Assert.AreEqual(createRequest.Key, createResponse.Key);
			Assert.AreEqual(createRequest.ActorType, createResponse.ActorType);
			Assert.AreEqual(createRequest.GameDataType, createResponse.GameDataType);
			Assert.AreEqual(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.GameId, getResponse.GameId);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.GameDataType, getResponse.GameDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CannotCreateDuplicateLeaderboard()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateDuplicateLeaderboard",
				GameId = game.Id,
				Name = "CannotCreateDuplicateLeaderboard",
				Key = "CannotCreateDuplicateLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutToken()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutName()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutName",
				GameId = game.Id,
				Key = "CannotCreateLeaderboardWithoutName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutKey()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithEmptyName()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "",
				Key = "CannotCreateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithEmptyToken()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithEmptyToken",
				Key = "CannotCreateLeaderboardWithEmptyToken",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithTypeMismatch()
		{
			var game = GetOrCreateGame("Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithTypeMismatch",
				Key = "CannotCreateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.GameDataType = GameDataType.Float;
			createRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.GameDataType = GameDataType.String;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.GameDataType = GameDataType.Boolean;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientException>(() => _leaderboardClient.Create(createRequest));
		}

		[Test]
		public void CanGetLeaderboardsByGame()
		{
			var game = GetOrCreateGame("GameGet");

			var leaderboardNames = new string[]
			{
				"CanGetLeaderboardsByGame1",
				"CanGetLeaderboardsByGame2",
				"CanGetLeaderboardsByGame3",
			};

			foreach (var name in leaderboardNames)
			{
				var createRequest = new LeaderboardRequest
				{
					Token = name,
					GameId = game.Id,
					Name = name,
					Key = name,
					ActorType = ActorType.User,
					GameDataType = GameDataType.Long,
					CriteriaScope = CriteriaScope.Actor,
					LeaderboardType = LeaderboardType.Highest
				};

				var createResponse = _leaderboardClient.Create(createRequest);
			}

			var getResponse = _leaderboardClient.Get(game.Id);

			Assert.AreEqual(3, getResponse.Count());

			var getCheck = getResponse.Select(g => leaderboardNames.Contains(g.Name));

			Assert.AreEqual(3, getCheck.Count());
		}

		[Test]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var getResponse = _leaderboardClient.Get(-1);

			Assert.IsEmpty(getResponse);
		}

		[Test]
		public void CannotGetNotExistingLeaderboard()
		{
			var getResponse = _leaderboardClient.Get("CannotGetNotExistingLeaderboard", -1);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var game = GetOrCreateGame("Get");

			Assert.Throws<ClientException>(() => _leaderboardClient.Get("", game.Id));
		}

		[Test]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var getResponse = _leaderboardClient.GetGlobal("CannotGetNotExistingGlobalLeaderboard");

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			Assert.Throws<ClientException>(() => _leaderboardClient.GetGlobal(""));
		}

		[Test]
		public void CanUpdateLeaderboard()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard",
				Key = "CanUpdateLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var getResponse = _leaderboardClient.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.GameId, getResponse.GameId);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.GameDataType, getResponse.GameDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard Updated",
				Key = "CanUpdateLeaderboard Updated",
				ActorType = ActorType.Group,
				GameDataType = GameDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			_leaderboardClient.Update(updateRequest);

			getResponse = _leaderboardClient.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(updateRequest.Token, getResponse.Token);
			Assert.AreEqual(updateRequest.GameId, getResponse.GameId);
			Assert.AreEqual(updateRequest.Name, getResponse.Name);
			Assert.AreEqual(updateRequest.Key, getResponse.Key);
			Assert.AreEqual(updateRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(updateRequest.GameDataType, getResponse.GameDataType);
			Assert.AreEqual(updateRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(updateRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CannotUpdateToDuplicateLeaderboard()
		{
			var game = GetOrCreateGame("Update");

			var createRequestOne = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardOne",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardOne",
				Key = "CannotUpdateToDuplicateLeaderboardOne",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponseOne = _leaderboardClient.Create(createRequestOne);

			var createRequestTwo = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardTwo",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardTwo",
				Key = "CannotUpdateToDuplicateLeaderboardTwo",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponseTwo = _leaderboardClient.Create(createRequestTwo);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardTwo",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardOne",
				Key = "CannotUpdateToDuplicateLeaderboardTwo",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateNotExistingLeaderboard()
		{
			var game = GetOrCreateGame("Update");

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateNotExistingLeaderboard",
				GameId = game.Id,
				Name = "CannotUpdateNotExistingLeaderboard",
				Key = "CannotUpdateNotExistingLeaderboard",
				ActorType = ActorType.Group,
				GameDataType = GameDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutToken()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutToken",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutName()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutName",
				Key = "CannotUpdateLeaderboardWithoutName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Token = "CannotUpdateLeaderboardWithoutName",
				Key = "CannotUpdateLeaderboardWithoutName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutKey()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutKey",
				Key = "CannotUpdateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Token = "CannotUpdateLeaderboardWithoutKey",
				Name = "CannotUpdateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithEmptyName()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithEmptyName",
				Key = "CannotUpdateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "",
				Key = "CannotUpdateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithTypeMismatch()
		{
			var game = GetOrCreateGame("Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithTypeMismatch",
				Key = "CannotUpdateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			_leaderboardClient.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithTypeMismatch",
				Key = "CannotCreateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.GameDataType = GameDataType.Float;
			updateRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.GameDataType = GameDataType.String;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.GameDataType = GameDataType.Boolean;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientException>(() => _leaderboardClient.Update(updateRequest));
		}

		[Test]
		public void CanDeleteGlobalLeaderboard()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanDeleteGlobalLeaderboard",
				Name = "CanDeleteGlobalLeaderboard",
				Key = "CanDeleteGlobalLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);
			var getResponse = _leaderboardClient.GetGlobal(createRequest.Token);

			Assert.NotNull(getResponse);

			_leaderboardClient.DeleteGlobal(createRequest.Token);

			getResponse = _leaderboardClient.GetGlobal(createRequest.Token);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotDeleteNonExistingGlobalLeaderboard()
		{
			_leaderboardClient.DeleteGlobal("CannotDeleteNonExistingGlobalLeaderboard");
		}

		[Test]
		public void CannotDeleteGlobalLeaderboardByEmptyToken()
		{
			Assert.Throws<ClientException>(() => _leaderboardClient.DeleteGlobal(""));
		}

		[Test]
		public void CanDeleteLeaderboard()
		{
			var game = GetOrCreateGame("Delete");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanDeleteLeaderboard",
				GameId = game.Id,
				Name = "CanDeleteLeaderboard",
				Key = "CanDeleteLeaderboard",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);
			var getResponse = _leaderboardClient.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.NotNull(getResponse);

			_leaderboardClient.Delete(createRequest.Token, createRequest.GameId.Value);

			getResponse = _leaderboardClient.GetGlobal(createRequest.Token);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotDeleteNonExistingLeaderboard()
		{
			var game = GetOrCreateGame("Delete");

			_leaderboardClient.Delete("CannotDeleteNonExistingLeaderboard", game.Id);
		}

		[Test]
		public void CannotDeleteLeaderboardByEmptyToken()
		{
			var game = GetOrCreateGame("Delete");

			Assert.Throws<ClientException>(() => _leaderboardClient.Delete("", game.Id));
		}

		[Test]
		public void CanGetGlobalLeaderboardStandings()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetGlobalLeaderboardStandings",
				Name = "CanGetGlobalLeaderboardStandings",
				Key = "CanGetGlobalLeaderboardStandings",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var user = GetOrCreateUser("Standings");
			var gameData = new GameDataRequest
			{
				Key = createRequest.Key,
				GameDataType = createRequest.GameDataType,
				ActorId = user.Id,
				Value = "5"
			};

			_gameDataClient.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest);

			Assert.AreEqual(1, standingsResponse.Count());
			Assert.AreEqual(user.Name, standingsResponse.First().ActorName);
		}

		[Test]
		public void CanGetLeaderboardStandings()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetLeaderboardStandings",
				GameId = game.Id,
				Name = "CanGetLeaderboardStandings",
				Key = "CanGetLeaderboardStandings",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var user = GetOrCreateUser("Standings");
			var gameData = new GameDataRequest
			{
				Key = createRequest.Key,
				GameDataType = createRequest.GameDataType,
				ActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			_gameDataClient.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest);

			Assert.AreEqual(1, standingsResponse.Count());
			Assert.AreEqual(user.Name, standingsResponse.First().ActorName);
		}

		[Test]
		public void CannotGetNotExistingLeaderboardStandings()
		{
			var game = GetOrCreateGame("Standings");

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = "CannotGetNotExistingLeaderboardStandings",
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetLeaderboardStandingsWithIncorrectActorType()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var group = GetOrCreateGroup("Standings");
			var gameData = new GameDataRequest
			{
				Key = createRequest.Key,
				GameDataType = createRequest.GameDataType,
				ActorId = group.Id,
				GameId = game.Id,
				Value = "5"
			};

			_gameDataClient.Add(gameData);

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = group.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetLeaderboardStandingsWithZeroPageLimit()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				Key = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 0,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetNearLeaderboardStandingsWithoutActorId()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetNearLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetNearLeaderboardStandingsWithoutActorId",
				Key = "CannotGetNearLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetFriendsLeaderboardStandingsWithoutActorId()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				Key = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Friends,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetGroupMemberLeaderboardStandingsWithoutActorId()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				Key = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType()
		{
			var game = GetOrCreateGame("Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				GameDataType = GameDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var user = GetOrCreateUser("Standings");
			var gameData = new GameDataRequest
			{
				Key = createRequest.Key,
				GameDataType = createRequest.GameDataType,
				ActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			_gameDataClient.Add(gameData);

			var createResponse = _leaderboardClient.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = user.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientException>(() => _leaderboardClient.CreateGetLeaderboardStandings(standingsRequest));
		}

		#endregion

		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "LeaderboardControllerTests" + suffix ?? $"_{suffix}";
			var users = _userClient.Get(name, true);
			ActorResponse user;

			if (users.Any())
			{
				user = users.Single();
			}
			else
			{
				user = _userClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return user;
		}

		private ActorResponse GetOrCreateGroup(string suffix)
		{
			string name = "LeaderboardControllerTests" + suffix ?? $"_{suffix}";
			var groups = _groupClient.Get(name);
			ActorResponse group;

			if (groups.Any())
			{
				group = groups.Single();
			}
			else
			{
				group = _groupClient.Create(new ActorRequest
				{
					Name = name
				});
			}

			return group;
		}

		private GameResponse GetOrCreateGame(string suffix)
		{
			string name = "LeaderboardControllerTests" + suffix ?? $"_{suffix}";
			var games = _gameClient.Get(name);
			GameResponse game;

			if (games.Any())
			{
				game = games.Single();
			}
			else
			{
				game = _gameClient.Create(new GameRequest
				{
					Name = name
				});
			}

			return game;
		}
		#endregion
	}
}
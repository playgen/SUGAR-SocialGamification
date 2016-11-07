using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using Xunit;

namespace PlayGen.SUGAR.Client.UnitTests
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
		[Fact]
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

			Assert.Equal(createRequest.Token, createResponse.Token);
			Assert.Equal(createRequest.Name, createResponse.Name);
			Assert.Equal(createRequest.Key, createResponse.Key);
			Assert.Equal(createRequest.ActorType, createResponse.ActorType);
			Assert.Equal(createRequest.GameDataType, createResponse.GameDataType);
			Assert.Equal(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.GameDataType, getResponse.GameDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
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

			Assert.Equal(createRequest.Token, createResponse.Token);
			Assert.Equal(createRequest.GameId, createResponse.GameId);
			Assert.Equal(createRequest.Name, createResponse.Name);
			Assert.Equal(createRequest.Key, createResponse.Key);
			Assert.Equal(createRequest.ActorType, createResponse.ActorType);
			Assert.Equal(createRequest.GameDataType, createResponse.GameDataType);
			Assert.Equal(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.GameId, getResponse.GameId);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.GameDataType, getResponse.GameDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

			Assert.Equal(3, getResponse.Count());

			var getCheck = getResponse.Select(g => leaderboardNames.Contains(g.Name));

			Assert.Equal(3, getCheck.Count());
		}

		[Fact]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var getResponse = _leaderboardClient.Get(-1);

			Assert.Empty(getResponse);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboard()
		{
			var getResponse = _leaderboardClient.Get("CannotGetNotExistingLeaderboard", -1);

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var game = GetOrCreateGame("Get");

			Assert.Throws<ClientException>(() => _leaderboardClient.Get("", game.Id));
		}

		[Fact]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var getResponse = _leaderboardClient.GetGlobal("CannotGetNotExistingGlobalLeaderboard");

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			Assert.Throws<ClientException>(() => _leaderboardClient.GetGlobal(""));
		}

		[Fact]
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

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.GameId, getResponse.GameId);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.GameDataType, getResponse.GameDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);

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

			Assert.Equal(updateRequest.Token, getResponse.Token);
			Assert.Equal(updateRequest.GameId, getResponse.GameId);
			Assert.Equal(updateRequest.Name, getResponse.Name);
			Assert.Equal(updateRequest.Key, getResponse.Key);
			Assert.Equal(updateRequest.ActorType, getResponse.ActorType);
			Assert.Equal(updateRequest.GameDataType, getResponse.GameDataType);
			Assert.Equal(updateRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(updateRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
		public void CannotDeleteNonExistingGlobalLeaderboard()
		{
			_leaderboardClient.DeleteGlobal("CannotDeleteNonExistingGlobalLeaderboard");
		}

		[Fact]
		public void CannotDeleteGlobalLeaderboardByEmptyToken()
		{
			Assert.Throws<ClientException>(() => _leaderboardClient.DeleteGlobal(""));
		}

		[Fact]
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

		[Fact]
		public void CannotDeleteNonExistingLeaderboard()
		{
			var game = GetOrCreateGame("Delete");

			_leaderboardClient.Delete("CannotDeleteNonExistingLeaderboard", game.Id);
		}

		[Fact]
		public void CannotDeleteLeaderboardByEmptyToken()
		{
			var game = GetOrCreateGame("Delete");

			Assert.Throws<ClientException>(() => _leaderboardClient.Delete("", game.Id));
		}

		[Fact]
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

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
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

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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

		[Fact]
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
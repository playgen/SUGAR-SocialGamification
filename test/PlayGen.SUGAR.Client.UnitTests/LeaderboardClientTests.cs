using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Shared;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class LeaderboardClientTests : ClientTestsBase
	{
		[Test]
		public void CanCreateAndGetGlobalLeaderboard()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGlobalLeaderboard",
				Name = "CanCreateAndGetGlobalLeaderboard",
				Key = "CanCreateAndGetGlobalLeaderboard",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};
			
			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.GetGlobal(createRequest.Token);

			Assert.AreEqual(createRequest.Token, createResponse.Token);
			Assert.AreEqual(createRequest.Name, createResponse.Name);
			Assert.AreEqual(createRequest.Key, createResponse.Key);
			Assert.AreEqual(createRequest.ActorType, createResponse.ActorType);
			Assert.AreEqual(createRequest.SaveDataType, createResponse.SaveDataType);
			Assert.AreEqual(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.SaveDataType, getResponse.SaveDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CanCreateAndGetGameLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGameLeaderboard",
				GameId = game.Id,		
				Name = "CanCreateAndGetGameLeaderboard",
				Key = "CanCreateAndGetGameLeaderboard",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(createRequest.Token, createResponse.Token);
			Assert.AreEqual(createRequest.GameId, createResponse.GameId);
			Assert.AreEqual(createRequest.Name, createResponse.Name);
			Assert.AreEqual(createRequest.Key, createResponse.Key);
			Assert.AreEqual(createRequest.ActorType, createResponse.ActorType);
			Assert.AreEqual(createRequest.SaveDataType, createResponse.SaveDataType);
			Assert.AreEqual(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.GameId, getResponse.GameId);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.SaveDataType, getResponse.SaveDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CannotCreateDuplicateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateDuplicateLeaderboard",
				GameId = game.Id,
				Name = "CannotCreateDuplicateLeaderboard",
				Key = "CannotCreateDuplicateLeaderboard",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutName",
				GameId = game.Id,
				Key = "CannotCreateLeaderboardWithoutName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithoutKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithEmptyName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "",
				Key = "CannotCreateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithEmptyToken",
				Key = "CannotCreateLeaderboardWithEmptyToken",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CannotCreateLeaderboardWithTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithTypeMismatch",
				Key = "CannotCreateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.SaveDataType = SaveDataType.Float;
			createRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.SaveDataType = SaveDataType.String;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.SaveDataType = SaveDataType.Boolean;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Test]
		public void CanGetLeaderboardsByGame()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "GameGet");

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
					SaveDataType = SaveDataType.Long,
					CriteriaScope = CriteriaScope.Actor,
					LeaderboardType = LeaderboardType.Highest
				};

				var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			}

			var getResponse = SUGARClient.Leaderboard.Get(game.Id);

			Assert.AreEqual(3, getResponse.Count());

			var getCheck = getResponse.Where(g => leaderboardNames.Any(ln => g.Name.Contains(ln)));

			Assert.AreEqual(3, getCheck.Count());
		}

		[Test]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var getResponse = SUGARClient.Leaderboard.Get(-1);

			Assert.IsEmpty(getResponse);
		}

		[Test]
		public void CannotGetNotExistingLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.Get("CannotGetNotExistingLeaderboard", -1);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.Get("", game.Id));
		}

		[Test]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.GetGlobal("CannotGetNotExistingGlobalLeaderboard");

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.GetGlobal(""));
		}

		[Test]
		public void CanUpdateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard",
				Key = "CanUpdateLeaderboard",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(createRequest.Token, getResponse.Token);
			Assert.AreEqual(createRequest.GameId, getResponse.GameId);
			Assert.AreEqual(createRequest.Name, getResponse.Name);
			Assert.AreEqual(createRequest.Key, getResponse.Key);
			Assert.AreEqual(createRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(createRequest.SaveDataType, getResponse.SaveDataType);
			Assert.AreEqual(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(createRequest.LeaderboardType, getResponse.LeaderboardType);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard Updated",
				Key = "CanUpdateLeaderboard Updated",
				ActorType = ActorType.Group,
				SaveDataType = SaveDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			SUGARClient.Leaderboard.Update(updateRequest);

			getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.AreEqual(updateRequest.Token, getResponse.Token);
			Assert.AreEqual(updateRequest.GameId, getResponse.GameId);
			Assert.AreEqual(updateRequest.Name, getResponse.Name);
			Assert.AreEqual(updateRequest.Key, getResponse.Key);
			Assert.AreEqual(updateRequest.ActorType, getResponse.ActorType);
			Assert.AreEqual(updateRequest.SaveDataType, getResponse.SaveDataType);
			Assert.AreEqual(updateRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.AreEqual(updateRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Test]
		public void CannotUpdateToDuplicateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequestOne = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardOne",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardOne",
				Key = "CannotUpdateToDuplicateLeaderboardOne",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponseOne = SUGARClient.Leaderboard.Create(createRequestOne);

			var createRequestTwo = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardTwo",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardTwo",
				Key = "CannotUpdateToDuplicateLeaderboardTwo",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponseTwo = SUGARClient.Leaderboard.Create(createRequestTwo);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardTwo",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardOne",
				Key = "CannotUpdateToDuplicateLeaderboardTwo",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateNotExistingLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateNotExistingLeaderboard",
				GameId = game.Id,
				Name = "CannotUpdateNotExistingLeaderboard",
				Key = "CannotUpdateNotExistingLeaderboard",
				ActorType = ActorType.Group,
				SaveDataType = SaveDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutToken",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutName",
				Key = "CannotUpdateLeaderboardWithoutName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Token = "CannotUpdateLeaderboardWithoutName",
				Key = "CannotUpdateLeaderboardWithoutName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithoutKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutKey",
				Key = "CannotUpdateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Token = "CannotUpdateLeaderboardWithoutKey",
				Name = "CannotUpdateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithEmptyName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithEmptyName",
				Key = "CannotUpdateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "",
				Key = "CannotUpdateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Test]
		public void CannotUpdateLeaderboardWithTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithTypeMismatch",
				Key = "CannotUpdateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			SUGARClient.Leaderboard.Create(createRequest);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithTypeMismatch",
				Key = "CannotCreateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.SaveDataType = SaveDataType.Float;
			updateRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.SaveDataType = SaveDataType.String;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.SaveDataType = SaveDataType.Boolean;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
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
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.GetGlobal(createRequest.Token);

			Assert.NotNull(getResponse);

			SUGARClient.Leaderboard.DeleteGlobal(createRequest.Token);

			getResponse = SUGARClient.Leaderboard.GetGlobal(createRequest.Token);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotDeleteNonExistingGlobalLeaderboard()
		{
			SUGARClient.Leaderboard.DeleteGlobal("CannotDeleteNonExistingGlobalLeaderboard");
		}

		[Test]
		public void CannotDeleteGlobalLeaderboardByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.DeleteGlobal(""));
		}

		[Test]
		public void CanDeleteLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanDeleteLeaderboard",
				GameId = game.Id,
				Name = "CanDeleteLeaderboard",
				Key = "CanDeleteLeaderboard",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.NotNull(getResponse);

			SUGARClient.Leaderboard.Delete(createRequest.Token, createRequest.GameId.Value);

			getResponse = SUGARClient.Leaderboard.GetGlobal(createRequest.Token);

			Assert.Null(getResponse);
		}

		[Test]
		public void CannotDeleteNonExistingLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			SUGARClient.Leaderboard.Delete("CannotDeleteNonExistingLeaderboard", game.Id);
		}

		[Test]
		public void CannotDeleteLeaderboardByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Delete");

			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.Delete("", game.Id));
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
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Standings");
			var gameData = new SaveDataRequest
			{
				Key = createRequest.Key,
				SaveDataType = createRequest.SaveDataType,
				ActorId = user.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.AreEqual(1, standingsResponse.Count());
			Assert.AreEqual(user.Name, standingsResponse.First().ActorName);
		}

		[Test]
		public void CanGetLeaderboardStandings()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetLeaderboardStandings",
				GameId = game.Id,
				Name = "CanGetLeaderboardStandings",
				Key = "CanGetLeaderboardStandings",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Standings");
			var gameData = new SaveDataRequest
			{
				Key = createRequest.Key,
				SaveDataType = createRequest.SaveDataType,
				ActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			var standingsResponse = SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest);

			Assert.AreEqual(1, standingsResponse.Count());
			Assert.AreEqual(user.Name, standingsResponse.First().ActorName);
		}

		[Test]
		public void CannotGetNotExistingLeaderboardStandings()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = "CannotGetNotExistingLeaderboardStandings",
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var group = Helpers.GetOrCreateGroup(SUGARClient.Group, "Standings");
			var gameData = new SaveDataRequest
			{
				Key = createRequest.Key,
				SaveDataType = createRequest.SaveDataType,
				ActorId = group.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = group.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetLeaderboardStandingsWithZeroPageLimit()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				Key = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Top,
				PageLimit = 0,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetNearLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetNearLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetNearLeaderboardStandingsWithoutActorId",
				Key = "CannotGetNearLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Near,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetFriendsLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				Key = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.Friends,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetGroupMemberLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				Key = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}

		[Test]
		public void CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				SaveDataType = SaveDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Standings");
			var gameData = new SaveDataRequest
			{
				Key = createRequest.Key,
				SaveDataType = createRequest.SaveDataType,
				ActorId = user.Id,
				GameId = game.Id,
				Value = "5"
			};

			SUGARClient.GameData.Add(gameData);

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var standingsRequest = new LeaderboardStandingsRequest
			{
				LeaderboardToken = createResponse.Token,
				GameId = game.Id,
				ActorId = user.Id,
				LeaderboardFilterType = LeaderboardFilterType.GroupMembers,
				PageLimit = 10,
				PageOffset = 0
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.CreateGetLeaderboardStandings(standingsRequest));
		}
	}
}
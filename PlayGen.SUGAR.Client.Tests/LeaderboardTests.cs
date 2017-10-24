using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class LeaderboardTests : ClientTestBase
	{
		[Fact]
		public void CanCreateAndGetGlobalLeaderboard()
		{
			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGlobalLeaderboard",
				Name = "CanCreateAndGetGlobalLeaderboard",
				Key = "CanCreateAndGetGlobalLeaderboard",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};
			
			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.GetGlobal(createRequest.Token);

			Assert.Equal(createRequest.Token, createResponse.Token);
			Assert.Equal(createRequest.Name, createResponse.Name);
			Assert.Equal(createRequest.Key, createResponse.Key);
			Assert.Equal(createRequest.ActorType, createResponse.ActorType);
			Assert.Equal(createRequest.EvaluationDataType, createResponse.EvaluationDataType);
			Assert.Equal(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.EvaluationDataType, getResponse.EvaluationDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
		public void CanCreateAndGetGameLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanCreateAndGetGameLeaderboard",
				GameId = game.Id,		
				Name = "CanCreateAndGetGameLeaderboard",
				Key = "CanCreateAndGetGameLeaderboard",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			var getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.Equal(createRequest.Token, createResponse.Token);
			Assert.Equal(createRequest.GameId, createResponse.GameId);
			Assert.Equal(createRequest.Name, createResponse.Name);
			Assert.Equal(createRequest.Key, createResponse.Key);
			Assert.Equal(createRequest.ActorType, createResponse.ActorType);
			Assert.Equal(createRequest.EvaluationDataType, createResponse.EvaluationDataType);
			Assert.Equal(createRequest.CriteriaScope, createResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, createResponse.LeaderboardType);

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.GameId, getResponse.GameId);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.EvaluationDataType, getResponse.EvaluationDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
		public void CannotCreateDuplicateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateDuplicateLeaderboard",
				GameId = game.Id,
				Name = "CannotCreateDuplicateLeaderboard",
				Key = "CannotCreateDuplicateLeaderboard",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithoutToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithoutName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutName",
				GameId = game.Id,
				Key = "CannotCreateLeaderboardWithoutName",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithoutKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithEmptyName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "",
				Key = "CannotCreateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithEmptyToken",
				Key = "CannotCreateLeaderboardWithEmptyToken",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CannotCreateLeaderboardWithTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Create");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithTypeMismatch",
				Key = "CannotCreateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.EvaluationDataType = EvaluationDataType.Float;
			createRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.EvaluationDataType = EvaluationDataType.String;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.EvaluationDataType = EvaluationDataType.Boolean;
			createRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
			createRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Create(createRequest));
		}

		[Fact]
		public void CanGetLeaderboardsByGame()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_GameGet");

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
					EvaluationDataType = EvaluationDataType.Long,
					CriteriaScope = CriteriaScope.Actor,
					LeaderboardType = LeaderboardType.Highest
				};

				var createResponse = SUGARClient.Leaderboard.Create(createRequest);
			}

			var getResponse = SUGARClient.Leaderboard.Get(game.Id);

			Assert.Equal(3, getResponse.Count());

			var getCheck = getResponse.Where(g => leaderboardNames.Any(ln => g.Name.Contains(ln)));

			Assert.Equal(3, getCheck.Count());
		}

		[Fact]
		public void CannotGetLeaderboardsByNotExistingGame()
		{
			var getResponse = SUGARClient.Leaderboard.Get(-1);

			Assert.Empty(getResponse);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.Get("CannotGetNotExistingLeaderboard", -1);

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetLeaderboardWithEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Get");

			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.Get("", game.Id));
		}

		[Fact]
		public void CannotGetNotExistingGlobalLeaderboard()
		{
			var getResponse = SUGARClient.Leaderboard.GetGlobal("CannotGetNotExistingGlobalLeaderboard");

			Assert.Null(getResponse);
		}

		[Fact]
		public void CannotGetGlobalLeaderboardWithEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.GetGlobal(""));
		}

		[Fact]
		public void CanUpdateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard",
				Key = "CanUpdateLeaderboard",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.Equal(createRequest.Token, getResponse.Token);
			Assert.Equal(createRequest.GameId, getResponse.GameId);
			Assert.Equal(createRequest.Name, getResponse.Name);
			Assert.Equal(createRequest.Key, getResponse.Key);
			Assert.Equal(createRequest.ActorType, getResponse.ActorType);
			Assert.Equal(createRequest.EvaluationDataType, getResponse.EvaluationDataType);
			Assert.Equal(createRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(createRequest.LeaderboardType, getResponse.LeaderboardType);

			var updateRequest = new LeaderboardRequest
			{
				Token = "CanUpdateLeaderboard",
				GameId = game.Id,
				Name = "CanUpdateLeaderboard Updated",
				Key = "CanUpdateLeaderboard Updated",
				ActorType = ActorType.Group,
				EvaluationDataType = EvaluationDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			SUGARClient.Leaderboard.Update(updateRequest);

			getResponse = SUGARClient.Leaderboard.Get(createRequest.Token, createRequest.GameId.Value);

			Assert.Equal(updateRequest.Token, getResponse.Token);
			Assert.Equal(updateRequest.GameId, getResponse.GameId);
			Assert.Equal(updateRequest.Name, getResponse.Name);
			Assert.Equal(updateRequest.Key, getResponse.Key);
			Assert.Equal(updateRequest.ActorType, getResponse.ActorType);
			Assert.Equal(updateRequest.EvaluationDataType, getResponse.EvaluationDataType);
			Assert.Equal(updateRequest.CriteriaScope, getResponse.CriteriaScope);
			Assert.Equal(updateRequest.LeaderboardType, getResponse.LeaderboardType);
		}

		[Fact]
		public void CannotUpdateToDuplicateLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequestOne = new LeaderboardRequest
			{
				Token = "CannotUpdateToDuplicateLeaderboardOne",
				GameId = game.Id,
				Name = "CannotUpdateToDuplicateLeaderboardOne",
				Key = "CannotUpdateToDuplicateLeaderboardOne",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateNotExistingLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var updateRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateNotExistingLeaderboard",
				GameId = game.Id,
				Name = "CannotUpdateNotExistingLeaderboard",
				Key = "CannotUpdateNotExistingLeaderboard",
				ActorType = ActorType.Group,
				EvaluationDataType = EvaluationDataType.Float,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Lowest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateLeaderboardWithoutToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotCreateLeaderboardWithoutToken",
				GameId = game.Id,
				Name = "CannotCreateLeaderboardWithoutToken",
				Key = "CannotCreateLeaderboardWithoutToken",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateLeaderboardWithoutName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutName",
				Key = "CannotUpdateLeaderboardWithoutName",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateLeaderboardWithoutKey()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithoutKey",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithoutKey",
				Key = "CannotUpdateLeaderboardWithoutKey",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateLeaderboardWithEmptyName()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithEmptyName",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithEmptyName",
				Key = "CannotUpdateLeaderboardWithEmptyName",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
		}

		[Fact]
		public void CannotUpdateLeaderboardWithTypeMismatch()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Update");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotUpdateLeaderboardWithTypeMismatch",
				GameId = game.Id,
				Name = "CannotUpdateLeaderboardWithTypeMismatch",
				Key = "CannotUpdateLeaderboardWithTypeMismatch",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Count
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.EvaluationDataType = EvaluationDataType.Float;
			updateRequest.LeaderboardType = LeaderboardType.Count;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Earliest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Latest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.EvaluationDataType = EvaluationDataType.String;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.EvaluationDataType = EvaluationDataType.Boolean;
			updateRequest.LeaderboardType = LeaderboardType.Highest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Lowest;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
			updateRequest.LeaderboardType = LeaderboardType.Cumulative;
			Assert.Throws<ClientHttpException>(() => SUGARClient.Leaderboard.Update(updateRequest));
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
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotDeleteNonExistingGlobalLeaderboard()
		{
			SUGARClient.Leaderboard.DeleteGlobal("CannotDeleteNonExistingGlobalLeaderboard");
		}

		[Fact]
		public void CannotDeleteGlobalLeaderboardByEmptyToken()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.DeleteGlobal(""));
		}

		[Fact]
		public void CanDeleteLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Delete");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanDeleteLeaderboard",
				GameId = game.Id,
				Name = "CanDeleteLeaderboard",
				Key = "CanDeleteLeaderboard",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotDeleteNonExistingLeaderboard()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Delete");

			SUGARClient.Leaderboard.Delete("CannotDeleteNonExistingLeaderboard", game.Id);
		}

		[Fact]
		public void CannotDeleteLeaderboardByEmptyToken()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Delete");

			Assert.Throws<ClientException>(() => SUGARClient.Leaderboard.Delete("", game.Id));
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
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(LeaderboardTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
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

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CanGetLeaderboardStandings()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CanGetLeaderboardStandings",
				GameId = game.Id,
				Name = "CanGetLeaderboardStandings",
				Key = "CanGetLeaderboardStandings",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var createResponse = SUGARClient.Leaderboard.Create(createRequest);

			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(LeaderboardTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
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

			Assert.Equal(1, standingsResponse.Count());
			Assert.Equal(user.Name, standingsResponse.First().ActorName);
		}

		[Fact]
		public void CannotGetNotExistingLeaderboardStandings()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

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

		[Fact]
		public void CannotGetLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var group = Helpers.GetOrCreateGroup(SUGARClient.Group, $"{nameof(LeaderboardTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = group.Id,
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

		[Fact]
		public void CannotGetLeaderboardStandingsWithZeroPageLimit()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				GameId = game.Id,
				Name = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				Key = "CannotGetLeaderboardStandingsWithZeroPageLimit",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotGetNearLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetNearLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetNearLeaderboardStandingsWithoutActorId",
				Key = "CannotGetNearLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotGetFriendsLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				Key = "CannotGetFriendsLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotGetGroupMemberLeaderboardStandingsWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				GameId = game.Id,
				Name = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				Key = "CannotGetGroupMemberLeaderboardStandingsWithoutActorId",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
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

		[Fact]
		public void CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(LeaderboardTests)}_Standings");

			var createRequest = new LeaderboardRequest
			{
				Token = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				GameId = game.Id,
				Name = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				Key = "CannotGetGroupMembersLeaderboardStandingsWithIncorrectActorType",
				ActorType = ActorType.User,
				EvaluationDataType = EvaluationDataType.Long,
				CriteriaScope = CriteriaScope.Actor,
				LeaderboardType = LeaderboardType.Highest
			};

			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(LeaderboardTests)}_Standings");
			var gameData = new EvaluationDataRequest
			{
				Key = createRequest.Key,
				EvaluationDataType = createRequest.EvaluationDataType,
				CreatingActorId = user.Id,
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
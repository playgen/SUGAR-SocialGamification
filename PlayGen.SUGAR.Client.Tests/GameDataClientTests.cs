﻿using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GameDataClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreate()
		{
			var key = "GameData_CanCreate";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(evaluationDataRequest);

			Assert.Equal(evaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(evaluationDataRequest.GameId, response.GameId);
			Assert.Equal(evaluationDataRequest.Key, response.Key);
			Assert.Equal(evaluationDataRequest.Value, response.Value);
			Assert.Equal(evaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanCreateWithoutGameId()
		{
			var key = "GameData_CanCreateWithoutGameId";
			Helpers.Login(SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(evaluationDataRequest);

			Assert.Equal(evaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.Equal(evaluationDataRequest.GameId, response.GameId);
			Assert.Equal(evaluationDataRequest.Key, response.Key);
			Assert.Equal(evaluationDataRequest.Value, response.Value);
			Assert.Equal(evaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CannotCreateWithoutActorId()
		{
			var key = "GameData_CannotCreateWithoutActorId";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutKey()
		{
			var key = "GameData_CannotCreateWithoutKey";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithoutValue()
		{
			var key = "GameData_CannotCreateWithoutValue";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CannotCreateWithMismatchedData()
		{
			var key = "GameData_CannotCreateWithMismatchedData";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.Float
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Float;
			evaluationDataRequest.Value = "True";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));

			evaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;
			evaluationDataRequest.Value = "2";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(evaluationDataRequest));
		}

		[Fact]
		public void CanGetGameData()
		{
			var key = "GameData_CanGetGameData";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(evaluationDataRequest);

			var get = SUGARClient.GameData.Get(loggedInAccount.User.Id, game.Id, new [] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CannotGetGameDataWithoutActorId()
		{
			var key = "GameData_CannotGetGameDataWithoutActorId";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(evaluationDataRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Get(0, game.Id, new[] { key }));
		}

		[Fact]
		public void CanGetGameDataWithoutGameId()
		{
			var key = "GameData_CanGetGameDataWithoutGameId";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				Key = key,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(evaluationDataRequest);

			var get = SUGARClient.GameData.Get(loggedInAccount.User.Id, null, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(get.First().CreatingActorId, response.CreatingActorId);
			Assert.Equal(get.First().GameId, response.GameId);
			Assert.Equal(get.First().Key, response.Key);
			Assert.Equal(get.First().Value, response.Value);
			Assert.Equal(get.First().EvaluationDataType, response.EvaluationDataType);
		}

		[Fact]
		public void CanGetGameDataByMultipleKeys()
		{
			var key = "GameData_CanGetGameDataByMultipleKeys";
			Helpers.Login(SUGARClient, key, key, out var game, out var loggedInAccount);

			var evaluationDataRequestOne = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "1",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var evaluationDataRequestTwo = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "2",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var evaluationDataRequestThree = new EvaluationDataRequest
			{
				CreatingActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "3",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			SUGARClient.GameData.Add(evaluationDataRequestOne);
			SUGARClient.GameData.Add(evaluationDataRequestTwo);
			SUGARClient.GameData.Add(evaluationDataRequestThree);

			var get = SUGARClient.GameData.Get(loggedInAccount.User.Id, game.Id, new[] { evaluationDataRequestOne.Key, evaluationDataRequestTwo.Key, evaluationDataRequestThree.Key });

			Assert.Equal(3, get.Count());
			foreach (var g in get)
			{
				Assert.Equal("Test Value", g.Value);
			}
		}
	}
}
using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class GameDataTests : ClientTestsBase
	{
		[Test]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.AreEqual(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.AreEqual(EvaluationDataRequest.GameId, response.GameId);
			Assert.AreEqual(EvaluationDataRequest.Key, response.Key);
			Assert.AreEqual(EvaluationDataRequest.Value, response.Value);
			Assert.AreEqual(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.AreEqual(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.AreEqual(EvaluationDataRequest.GameId, response.GameId);
			Assert.AreEqual(EvaluationDataRequest.Key, response.Key);
			Assert.AreEqual(EvaluationDataRequest.Value, response.Value);
			Assert.AreEqual(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			Assert.AreEqual(EvaluationDataRequest.CreatingActorId, response.CreatingActorId);
			Assert.AreEqual(EvaluationDataRequest.GameId, response.GameId);
			Assert.AreEqual(EvaluationDataRequest.Key, response.Key);
			Assert.AreEqual(EvaluationDataRequest.Value, response.Value);
			Assert.AreEqual(EvaluationDataRequest.EvaluationDataType, response.EvaluationDataType);
		}

		[Test]
		public void CannotCreateWithoutKey()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Test]
		public void CannotCreateWithoutValue()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithoutKey",
				EvaluationDataType = EvaluationDataType.String
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Test]
		public void CannotCreateWithMismatchedData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CannotCreateWithMismatchedData",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.Float
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Float;
			EvaluationDataRequest.Value = "True";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Long;

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));

			EvaluationDataRequest.EvaluationDataType = EvaluationDataType.Boolean;
			EvaluationDataRequest.Value = "2";

			Assert.Throws<ClientHttpException>(() => SUGARClient.GameData.Add(EvaluationDataRequest));
		}

		[Test]
		public void CanGetGameData()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "CanGetGameData");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "CanGetGameData");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameData",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, game.Id, new[] {"CanGetGameData"});

			Assert.AreEqual(1, get.Items.Count());
			Assert.AreEqual(get.Items.First()
					.CreatingActorId,
				response.CreatingActorId);
			Assert.AreEqual(get.Items.First()
					.GameId,
				response.GameId);
			Assert.AreEqual(get.Items.First()
					.Key,
				response.Key);
			Assert.AreEqual(get.Items.First()
					.Value,
				response.Value);
			Assert.AreEqual(get.Items.First()
					.EvaluationDataType,
				response.EvaluationDataType);
		}

		[Test]
		public void CanGetGameDataWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				GameId = game.Id,
				Key = "CanGetGameDataWithoutActorId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(null, game.Id, new[] {"CanGetGameDataWithoutActorId"});

			Assert.AreEqual(1, get.Items.Count());
			Assert.AreEqual(get.Items.First()
					.CreatingActorId,
				response.CreatingActorId);
			Assert.AreEqual(get.Items.First()
					.GameId,
				response.GameId);
			Assert.AreEqual(get.Items.First()
					.Key,
				response.Key);
			Assert.AreEqual(get.Items.First()
					.Value,
				response.Value);
			Assert.AreEqual(get.Items.First()
					.EvaluationDataType,
				response.EvaluationDataType);
		}

		[Test]
		public void CanGetGameDataWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");

			var EvaluationDataRequest = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				Key = "CanGetGameDataWithoutGameId",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var response = SUGARClient.GameData.Add(EvaluationDataRequest);

			var get = SUGARClient.GameData.Get(user.Id, null, new[] {"CanGetGameDataWithoutGameId"});

			Assert.AreEqual(1, get.Items.Count());
			Assert.AreEqual(get.Items.First()
					.CreatingActorId,
				response.CreatingActorId);
			Assert.AreEqual(get.Items.First()
					.GameId,
				response.GameId);
			Assert.AreEqual(get.Items.First()
					.Key,
				response.Key);
			Assert.AreEqual(get.Items.First()
					.Value,
				response.Value);
			Assert.AreEqual(get.Items.First()
					.EvaluationDataType,
				response.EvaluationDataType);
		}

		[Test]
		public void CanGetGameDataByMultipleKeys()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var EvaluationDataRequestOne = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys1",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var EvaluationDataRequestTwo = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys2",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var EvaluationDataRequestThree = new EvaluationDataRequest
			{
				CreatingActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetGameDatByMultipleKeys3",
				Value = "Test Value",
				EvaluationDataType = EvaluationDataType.String
			};

			var responseOne = SUGARClient.GameData.Add(EvaluationDataRequestOne);
			var responseTwo = SUGARClient.GameData.Add(EvaluationDataRequestTwo);
			var responseThree = SUGARClient.GameData.Add(EvaluationDataRequestThree);

			var get = SUGARClient.GameData.Get(user.Id,
				game.Id,
				new[] {"CanGetGameDatByMultipleKeys1", "CanGetGameDatByMultipleKeys2", "CanGetGameDatByMultipleKeys3"});

			Assert.AreEqual(3, get.Items.Count());
			foreach (var g in get.Items)
				Assert.AreEqual("Test Value", g.Value);
		}
	}
}
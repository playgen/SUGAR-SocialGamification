using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts.Shared;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class ResourceClientTests : ClientTestsBase
	{
		[Test]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Create");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.AreEqual(resourceRequest.Key, response.Key);
			Assert.AreEqual(resourceRequest.Quantity, response.Quantity);
		}

		[Test]
		public void CanUpdateExisting()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CanUpdateExisting",
				Quantity = 100,
			};

			var createdResource = SUGARClient.Resource.AddOrUpdate(resourceRequest);
			var createdQuantity = createdResource.Quantity;
			var updatedQuantity = createdQuantity + 9000;

			var resourceRequestUpdated = new ResourceAddRequest
			{
				Key = createdResource.Key,
				Quantity = updatedQuantity
			};

			SUGARClient.Resource.AddOrUpdate(resourceRequestUpdated);

			var updatedResource = SUGARClient.Resource.Get(createdResource.GameId, createdResource.ActorId,
				new[] {createdResource.Key}).Single();

			Assert.AreEqual(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.AreEqual(createdResource.Id, updatedResource.Id);
		}

		[Test]
		public void CanTransferCreateResource_FromUserToUser()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, "From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, "To");

			var fromResource = SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferCreateResource_FromUserToUser",
				Quantity = 100,
			});

			var originalQuantity = fromResource.Quantity;
			var transferQuantity = originalQuantity/3;

			var transferResponse = SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.AreEqual(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.AreEqual(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.AreEqual(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.AreEqual(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.AreEqual(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Test]
		public void CanTransferUpdateResource_FromUserToUser()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, "From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, "To");

			var fromResource = SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferUpdateResource_FromUserToUser",
				Quantity = 100,
			});

			var toResource = SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				GameId = fromResource.GameId,
				ActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = 50,
			});

			var originalFrmoQuantity = fromResource.Quantity;
			var originalToQuantity = toResource.Quantity;
			var transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.AreEqual(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.AreEqual(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.AreEqual(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.AreEqual(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.AreEqual(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Test]
		public void CannotTransferNonExistingResource()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, "From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, "To");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{

				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = new Guid().ToString(),
				GameId = null,
				Quantity = 100,
			}));
		}

		[TestCase(0)]
		[TestCase(-1)]
		[TestCase(-2000)]
		public void CannotTransfer_WithLessThan1Quantity(long transferQuantity)
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, "From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, "To");

			var fromResource = SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CannotTransfer_WithLessThan1Quantity" + transferQuantity,
				Quantity = 100,
			});		

			Assert.Throws<ClientHttpException>(() => SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				GameId = fromResource.GameId,
				Quantity = transferQuantity
			}));
		}

		[Test]
		public void CannotTransfer_WithOutOfRangeQuantity()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, "From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, "To");

			var fromResource = SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CannotTransfer_WithOutOfRangeQuantity",
				Quantity = 100,
			});

			var transferQuantity = fromResource.Quantity*2;

			Assert.Throws<ClientHttpException>(() => SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity,
			}));
		}

		[Test]
		public void CanGetResource()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResource",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(game.Id, user.Id, new string[] { "CanGetResource" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanGetResourceWithoutActorId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(game.Id, null, new string[] { "CanGetResourceWithoutActorId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanGetResourceWithoutGameId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(null, user.Id, new string[] { "CanGetResourceWithoutGameId" });

			Assert.AreEqual(1, get.Count());
			Assert.AreEqual(resourceRequest.Key, get.First().Key);
			Assert.AreEqual(resourceRequest.Quantity, get.First().Quantity);
		}

		[Test]
		public void CanGetResourceByMultipleKeys()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, "Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, "Get");

			var resourceRequestOne = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys1",
				Quantity = 100,
			};

			var resourceRequestTwo = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys2",
				Quantity = 100,
			};

			var resourceRequestThree = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResourceByMultipleKeys3",
				Quantity = 100,
			};

			var responseOne = SUGARClient.Resource.AddOrUpdate(resourceRequestOne);
			var responseTwo = SUGARClient.Resource.AddOrUpdate(resourceRequestTwo);
			var responseThree = SUGARClient.Resource.AddOrUpdate(resourceRequestThree);

			var get = SUGARClient.Resource.Get(game.Id, user.Id, new string[] { "CanGetResourceByMultipleKeys1", "CanGetResourceByMultipleKeys2", "CanGetResourceByMultipleKeys3" });

			Assert.AreEqual(3, get.Count());
			foreach (var r in get)
			{
				Assert.AreEqual(100, r.Quantity);
			}
		}
	}
}
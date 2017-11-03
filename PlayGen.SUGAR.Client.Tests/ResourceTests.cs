using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class ResourceTests : ClientTestBase
	{
		[Fact]
		public void CanCreate()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanCreate",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CanCreateWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CanCreateWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Create");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanCreateWithoutActorId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
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

			Assert.Equal(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.Equal(createdResource.Id, updatedResource.Id);
		}

		[Fact]
		public void CanTransferCreateResource_FromUserToUser()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_To");

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

			Assert.Equal(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CanTransferUpdateResource_FromUserToUser()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_To");

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

			Assert.Equal(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(toUser.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CannotTransferNonExistingResource()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_To");

			Assert.Throws<ClientHttpException>(() => SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{

				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = new Guid().ToString(),
				GameId = null,
				Quantity = 100,
			}));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2000)]
		public void CannotTransfer_WithLessThan1Quantity(long transferQuantity)
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_To");

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

		[Fact]
		public void CannotTransfer_WithOutOfRangeQuantity()
		{
			var fromUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_From");
			var toUser = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_To");

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

		[Fact]
		public void CanGetResource()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				GameId = game.Id,
				Key = "CanGetResource",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(game.Id, user.Id, new string[] { "CanGetResource" });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CanGetResourceWithoutActorId()
		{
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = "CanGetResourceWithoutActorId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(game.Id, null, new string[] { "CanGetResourceWithoutActorId" });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CanGetResourceWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = user.Id,
				Key = "CanGetResourceWithoutGameId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = SUGARClient.Resource.Get(null, user.Id, new string[] { "CanGetResourceWithoutGameId" });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CanGetResourceByMultipleKeys()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Get");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

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

			Assert.Equal(3, get.Count());
			foreach (var r in get)
			{
				Assert.Equal(100, r.Quantity);
			}
		}

		[Fact]
		public void CanAddWithoutGameId()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");

			var resourceRequest = new ResourceAddRequest {
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.Add(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Resource.Key);
			Assert.Equal(resourceRequest.Quantity, response.Resource.Quantity);
		}

		[Fact]
		public void CanAddToAnExistingUsersResources()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = 100,
			};

			var resourceRequestTwo = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = 100,
			};

			var response = SUGARClient.Resource.Add(resourceRequest);
			var responseTwo = SUGARClient.Resource.Add(resourceRequestTwo);

			Assert.Equal(resourceRequest.Key, response.Resource.Key);
			Assert.Equal(resourceRequestTwo.Key, responseTwo.Resource.Key);
			Assert.Equal(resourceRequestTwo.Quantity + resourceRequest.Quantity, response.Resource.Quantity + responseTwo.Resource.Quantity);
		}

		[Fact]
		public void CanMinusResources()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = 100,
			};

			var resourceRequestTwo = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = -30,
			};

			var response = SUGARClient.Resource.Add(resourceRequest);
			var responseTwo = SUGARClient.Resource.Add(resourceRequestTwo);

			Assert.Equal(resourceRequest.Key, response.Resource.Key);
			Assert.Equal(resourceRequestTwo.Key, responseTwo.Resource.Key);
			Assert.Equal(resourceRequestTwo.Quantity + resourceRequest.Quantity, responseTwo.Resource.Quantity);
		}

		[Fact]
		public void CantMinusPastZero()
		{
			var user = Helpers.GetOrCreateUser(SUGARClient.User, $"{nameof(ResourceTests)}_Create");
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = 30,
			};

			var resourceRequestTwo = new ResourceAddRequest {
				ActorId = user.Id,
				GameId = game.Id,
				Key = "Multiple",
				Quantity = -100,
			};

			var response = SUGARClient.Resource.Add(resourceRequest);
			var responseTwo = SUGARClient.Resource.Add(resourceRequestTwo);

			Assert.Equal(resourceRequest.Key, response.Resource.Key);
			Assert.Equal(resourceRequestTwo.Key, responseTwo.Resource.Key);
			Assert.Equal((long) 0, responseTwo.Resource.Quantity);
		}

		[Fact]
		public void CantAddToAUserThatDoesntExist()
		{
			var user = new ActorResponse();
			var game = Helpers.GetOrCreateGame(SUGARClient.Game, $"{nameof(ResourceTests)}_Get");

			var resourceRequest = new ResourceAddRequest {
				ActorId = user.Id,
				Key = "CanCreateWithoutGameId",
				GameId = game.Id,
				Quantity = 100,
			};

			var response = SUGARClient.Resource.Add(resourceRequest);

			Assert.NotEqual(resourceRequest.Key, response.Resource.Key);
			Assert.NotEqual(resourceRequest.Quantity, response.Resource.Quantity);
		}
	}
}
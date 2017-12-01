using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class ResourceClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreate()
		{
			var key = "Resource_CanCreate";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			var response = Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CanCreateWithoutGameId()
		{
			var key = "Resource_CanCreateWithoutGameId";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100
			};

			var response = Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CannotCreateWithoutActorId()
		{
			var key = "Resource_CannotCreateWithoutActorId";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var _);

			var resourceRequest = new ResourceAddRequest
			{
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CanUpdateExisting()
		{
			var key = "Resource_CanUpdateExisting";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100,
				GameId = game.Id
			};

			var createdResource = Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);
			var createdQuantity = createdResource.Quantity;
			var updatedQuantity = createdQuantity + 9000;

			var resourceRequestUpdated = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = createdResource.Key,
				Quantity = updatedQuantity,
				GameId = game.Id
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestUpdated);

			var updatedResource = Fixture.SUGARClient.Resource.Get(createdResource.GameId, createdResource.ActorId,
				new[] {createdResource.Key}).Single();

			Assert.Equal(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.Equal(createdResource.Id, updatedResource.Id);
		}

		[Fact]
		public void CanTransferCreateResourceFromUserToUser()
		{
			var key = "Resource_CanTransferCreateResourceFromUserToUser";
			var recipient = CreateUser(key);
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 300
			});

			var originalQuantity = fromResource.Quantity;
			var transferQuantity = originalQuantity/3;

			var transferResponse = Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.Equal(originalQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(recipient.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CanTransferUpdateResourceFromUserToUser()
		{
			var key = "Resource_CanTransferUpdateResourceFromUserToUser";
			var recipient = CreateUser(key);
			var toResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = recipient.Id,
				Key = key,
				Quantity = 150
			});

			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 300
			});

			var originalFrmoQuantity = fromResource.Quantity;
			var originalToQuantity = toResource.Quantity;
			var transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity
			});

			Assert.Equal(originalFrmoQuantity - transferQuantity, transferResponse.FromResource.Quantity);
			Assert.Equal(originalToQuantity + transferQuantity, transferResponse.ToResource.Quantity);
			Assert.Equal(recipient.Id, transferResponse.ToResource.ActorId);
			Assert.Equal(fromResource.GameId, transferResponse.FromResource.GameId);
			Assert.Equal(fromResource.GameId, transferResponse.ToResource.GameId);
		}

		[Fact]
		public void CannotTransferNonExistingResource()
		{
			var key = "Resource_CannotTransferNonExistingResource";
			var recipient = CreateUser(key);
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = 100
			}));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2000)]
		public void CannotTransferWithLessThan1Quantity(long transferQuantity)
		{
			var key = "Resource_CannotTransferWithLessThan1Quantity";
			var recipient = CreateUser(key);
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = (key + transferQuantity).Replace('-', '_'),
				Quantity = 300
			});		

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = transferQuantity
			}));
		}

		[Fact]
		public void CannotTransferWithOutOfRangeQuantity()
		{
			var key = "Resource_CannotTransferWithOutOfRangeQuantity";
			var recipient = CreateUser(key);
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var fromResource = Fixture.SUGARClient.Resource.AddOrUpdate(new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100
			});

			var transferQuantity = fromResource.Quantity*2;

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Transfer(new ResourceTransferRequest
			{
				SenderActorId = loggedInAccount.User.Id,
				RecipientActorId = recipient.Id,
				Key = key,
				Quantity = transferQuantity
			}));
		}

		[Fact]
		public void CanGetResource()
		{
			var key = "Resource_CanGetResource";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key,
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = Fixture.SUGARClient.Resource.Get(game.Id, loggedInAccount.User.Id, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CannotGetResourceWithoutActorId()
		{
			var key = "Resource_CannotGetResourceWithoutActorId";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var _);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Resource.Get(game.Id, null, new[] { key }));
		}

		[Fact]
		public void CanGetResourceWithoutGameId()
		{
			var key = "Resource_CanGetResourceWithoutGameId";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var resourceRequest = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				Key = key,
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequest);

			var get = Fixture.SUGARClient.Resource.Get(null, loggedInAccount.User.Id, new[] { key });

			Assert.Equal(1, get.Count());
			Assert.Equal(resourceRequest.Key, get.First().Key);
			Assert.Equal(resourceRequest.Quantity, get.First().Quantity);
		}

		[Fact]
		public void CanGetResourceByMultipleKeys()
		{
			var key = "Resource_CanGetResourceByMultipleKeys";
			Helpers.Login(Fixture.SUGARClient, key, key, out var game, out var loggedInAccount);

			var resourceRequestOne = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "1",
				Quantity = 100
			};

			var resourceRequestTwo = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "2",
				Quantity = 100
			};

			var resourceRequestThree = new ResourceAddRequest
			{
				ActorId = loggedInAccount.User.Id,
				GameId = game.Id,
				Key = key + "3",
				Quantity = 100
			};

			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestOne);
			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestTwo);
			Fixture.SUGARClient.Resource.AddOrUpdate(resourceRequestThree);

			var get = Fixture.SUGARClient.Resource.Get(game.Id, loggedInAccount.User.Id, new[] { resourceRequestOne.Key, resourceRequestTwo.Key, resourceRequestThree.Key });

			Assert.Equal(3, get.Count());
			foreach (var r in get)
			{
				Assert.Equal(100, r.Quantity);
			}
		}

		#region Helpers
		private GroupResponse CreateGroup(string key)
		{
			Helpers.Login(Fixture.SUGARClient, "Global", key + "_Creator", out var _, out var _);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			return Fixture.SUGARClient.Group.Create(groupRequest);
		}

		private UserResponse CreateUser(string key)
		{
			Helpers.Login(Fixture.SUGARClient, "Global", key + "_User", out var _, out var friendAccount);
			return friendAccount.User;
		}
		#endregion

		public ResourceClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
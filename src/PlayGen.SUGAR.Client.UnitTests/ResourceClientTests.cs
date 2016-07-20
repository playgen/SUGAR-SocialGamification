using System;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;
using System.Linq;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class ResourceClientTests
	{
		#region Configuration
		private readonly ResourceClient _resourceClient;
		private readonly UserClient _userClient;

		public ResourceClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_resourceClient = testSugarClient.Resource;
			_userClient = testSugarClient.User;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "ResourceClientTests",
				Password = "ResourceClientTestsPassword",
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
		public void CanCreate()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CanCreate",
				Quantity = 100,
			};

			var response = _resourceClient.AddOrUpdate(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CannotCreateDuplicate()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CannotCreateDuplicate",
				Quantity = 100,
			};

			_resourceClient.AddOrUpdate(resourceRequest);

			Assert.Throws<Exception>(() => _resourceClient.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CanUpdateExisting()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CanUpdateExisting",
				Quantity = 100,
			};

			var createdResource = _resourceClient.AddOrUpdate(resourceRequest);
			var createdQuantity = createdResource.Quantity;
			var updatedQuantity = createdQuantity + 9000;

			//resourceRequest.Quantity = updatedQuantity;

			var resourceRequestUpdated = new ResourceAddRequest
			{
				Key = createdResource.Key,
				Quantity = updatedQuantity
			};

			_resourceClient.AddOrUpdate(resourceRequestUpdated);

			var updatedResource = _resourceClient.Get(createdResource.GameId, createdResource.ActorId,
				new[] {createdResource.Key}).Single();

			Assert.Equal(createdQuantity + updatedQuantity, updatedResource.Quantity);
			Assert.Equal(createdResource.Id, updatedResource.Id);
		}

		[Fact]
		public void CantUpdateNonexisting()
		{
			var resourceRequest = new ResourceAddRequest
			{
				Key = "CantUpdateNonexisting",
				Quantity = 100,
			};

			Assert.Throws<Exception>(() => _resourceClient.AddOrUpdate(resourceRequest));
		}

		[Fact]
		public void CanTransferCreateResource_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferCreateResource_FromUserToUser",
				Quantity = 100,
			});

			var originalQuantity = fromResource.Quantity;
			var transferQuantity = originalQuantity/3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
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
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferUpdateResource_FromUserToUser",
				Quantity = 100,
			});

			var toResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = fromResource.GameId,
				ActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = 50,
			});

			var originalFrmoQuantity = fromResource.Quantity;
			var originalToQuantity = toResource.Quantity;
			var transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
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
		public void CantTransferNonExistingResource()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
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
		public void CantTransfer_WithLessThan1Quantity(long transferQuantity)
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CantTransfer_WithLessThan1Quantity" + transferQuantity,
				Quantity = 100,
			});		

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				GameId = fromResource.GameId,
				Quantity = transferQuantity
			}));
		}

		[Fact]
		public void CantTransfer_WithOutOfRangeQuantity()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.AddOrUpdate(new ResourceAddRequest
			{
				GameId = null,
				ActorId = null,
				Key = "CantTransfer_WithOutOfRangeQuantity",
				Quantity = 100,
			});

			long transferQuantity = fromResource.Quantity*2;

			Assert.Throws<Exception>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				GameId = fromResource.GameId,
				SenderActorId = fromUser.Id,
				RecipientActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = transferQuantity,
			}));
		}
		#endregion

		#region Helpers
		private ActorResponse GetOrCreateUser(string suffix)
		{
			string name = "ResourceControllerTests" + suffix ?? $"_{suffix}";
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
		#endregion
	}
}
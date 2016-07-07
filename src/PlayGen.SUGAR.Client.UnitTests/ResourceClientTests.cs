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
		public void CanCreateResource()
		{
			var resourceRequest = new ResourceRequest
			{
				Key = "CanCreateResource",
				Quantity = 100,
			};

			var response = _resourceClient.Add(resourceRequest);

			Assert.Equal(resourceRequest.Key, response.Key);
			Assert.Equal(resourceRequest.Quantity, response.Quantity);
		}

		[Fact]
		public void CannotCreateDuplicateResource()
		{
			var resourceRequest = new ResourceRequest
			{
				Key = "CannotCreateDuplicateResource",
				Quantity = 100,
			};

			_resourceClient.Add(resourceRequest);

			Assert.Throws<WebException>(() => _resourceClient.Add(resourceRequest));
		}

		[Fact]
		public void CanTransferCreateResource_FromUserToUser()
		{
			var fromUser = GetOrCreateUser("From");
			var toUser = GetOrCreateUser("To");

			var fromResource = _resourceClient.Add(new ResourceRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferCreateResource_FromUserToUser",
				Quantity = 100,
			});

			long originalQuantity = fromResource.Quantity;
			long transferQuantity = originalQuantity/3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
			{
				ResourceId = fromResource.Id,
				GameId = fromResource.GameId,
				Quantity = transferQuantity,
				RecipientId = toUser.Id,
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

			var fromResource = _resourceClient.Add(new ResourceRequest
			{
				GameId = null,
				ActorId = fromUser.Id,
				Key = "CanTransferCreateResource_FromUserToUser",
				Quantity = 100,
			});

			var toResource = _resourceClient.Add(new ResourceRequest
			{
				GameId = fromResource.GameId,
				ActorId = toUser.Id,
				Key = fromResource.Key,
				Quantity = 50,
			});

			long originalFrmoQuantity = fromResource.Quantity;
			long originalToQuantity = toResource.Quantity;
			long transferQuantity = originalFrmoQuantity / 3;

			var transferResponse = _resourceClient.Transfer(new ResourceTransferRequest
			{
				ResourceId = fromResource.Id,
				GameId = fromResource.GameId,
				Quantity = transferQuantity,
				RecipientId = toUser.Id,
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
			Assert.Throws<WebException>(() => _resourceClient.Transfer(new ResourceTransferRequest
			{
				ResourceId = -1,
				GameId = null,
				Quantity = 100,
				RecipientId = null,
			}));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2000)]
		public void CantTransfer_FromUserToUserWithLessThan1Quantity(long transferQuantity)
		{
			throw new NotImplementedException();
		}

		[Fact]
		public void CantTransfer_FromUserToUserWithOutOfRangeQuantity()
		{
			throw new NotImplementedException();
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
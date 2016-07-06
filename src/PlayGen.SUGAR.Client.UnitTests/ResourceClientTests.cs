using System.Diagnostics.Eventing.Reader;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class ResourceClientTests
	{
		#region Configuration
		private readonly ResourceClient _resourceClient;

		public ResourceClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_resourceClient = testSugarClient.Resource;

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

		#endregion
	}
}
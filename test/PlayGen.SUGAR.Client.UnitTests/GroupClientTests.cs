using System;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using Xunit;
using PlayGen.SUGAR.Client.Exceptions;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class GroupClientTests
	{
		#region Configuration
		private readonly GroupClient _groupClient;

		public GroupClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_groupClient = testSugarClient.Group;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "GroupClientTests",
				Password = "GroupClientTestsPassword",
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
		public void CanCreateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanCreateGroup",
			};

			var response = _groupClient.Create(groupRequest);

			Assert.Equal(groupRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Fact]
		public void CannotCreateDuplicateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CannotCreateDuplicateGroup",
			};

			_groupClient.Create(groupRequest);

			Assert.Throws<ClientException>(() => _groupClient.Create(groupRequest));
		}

		[Fact]
		public void CannotCreateGroupWithNoName()
		{
			var groupRequest = new ActorRequest { };

			Assert.Throws<ClientException>(() => _groupClient.Create(groupRequest));
		}

		[Fact]
		public void CanGetGroupsByName()
		{
			var groupRequestOne = new ActorRequest
			{
				Name = "CanGetGroupsByName 1",
			};

			var responseOne = _groupClient.Create(groupRequestOne);

			var groupRequestTwo = new ActorRequest
			{
				Name = "CanGetGroupsByName 2",
			};

			var responseTwo = _groupClient.Create(groupRequestTwo);

			var getGroups = _groupClient.Get("CanGetGroupsByName");

			Assert.Equal(2, getGroups.Count());
		}

		[Fact]
		public void CannotGetNotExistingGroupByName()
		{
			var getGroups = _groupClient.Get("CannotGetNotExistingGroupByName");

			Assert.Empty(getGroups);
		}

		[Fact]
		public void CannotGetGroupByEmptyName()
		{
			Assert.Throws<ClientException>(() => _groupClient.Get(""));
		}

		[Fact]
		public void CanGetGroupById()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanGetGroupById",
			};

			var response = _groupClient.Create(groupRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.Equal(response.Name, getGroup.Name);
			Assert.Equal(groupRequest.Name, getGroup.Name);
		}

		[Fact]
		public void CannotGetNotExistingGroupById()
		{
			var getGroup = _groupClient.Get(-1);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CanUpdateGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanUpdateGroup",
			};

			var response = _groupClient.Create(groupRequest);

			var updateRequest = new ActorRequest
			{
				Name = "CanUpdateGroup Updated"
			};

			_groupClient.Update(response.Id, updateRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal("CanUpdateGroup Updated", getGroup.Name);
		}

		[Fact]
		public void CannotUpdateGroupToDuplicateName()
		{
			var groupRequestOne = new ActorRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 1"
			};

			var responseOne = _groupClient.Create(groupRequestOne);

			var groupRequestTwo = new ActorRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 2"
			};

			var responseTwo = _groupClient.Create(groupRequestTwo);

			var updateGroup = new ActorRequest
			{
				Name = groupRequestOne.Name
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(responseTwo.Id, updateGroup));
		}

		[Fact]
		public void CannotUpdateNonExistingGroup()
		{
			var updateGroup = new ActorRequest
			{
				Name = "CannotUpdateNonExistingGroup"
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(-1, updateGroup));
		}

		[Fact]
		public void CannotUpdateGroupToNoName()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CannotUpdateGroupToNoName",
			};

			var response = _groupClient.Create(groupRequest);

			var updateRequest = new ActorRequest
			{
			};

			Assert.Throws<ClientException>(() => _groupClient.Update(response.Id, updateRequest));
		}

		[Fact]
		public void CanDeleteGroup()
		{
			var groupRequest = new ActorRequest
			{
				Name = "CanDeleteGroup",
			};

			var response = _groupClient.Create(groupRequest);

			var getGroup = _groupClient.Get(response.Id);

			Assert.NotNull(getGroup);

			_groupClient.Delete(response.Id);

			getGroup = _groupClient.Get(response.Id);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CannotDeleteNonExistingGroup()
		{
			_groupClient.Delete(-1);
		}


		#endregion
	}
}
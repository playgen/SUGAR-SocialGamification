using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GroupTests : ClientTestBase
	{
		[Fact]
		public void CanCreateGroup()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CanCreateGroup",
			};

			var response = SUGARClient.Group.Create(groupRequest);

			Assert.Equal(groupRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Fact]
		public void CannotCreateDuplicateGroup()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CannotCreateDuplicateGroup",
			};

			SUGARClient.Group.Create(groupRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.Group.Create(groupRequest));
		}

		[Fact]
		public void CannotCreateGroupWithNoName()
		{
			var groupRequest = new GroupRequest { };

			Assert.Throws<ClientHttpException>(() => SUGARClient.Group.Create(groupRequest));
		}

		[Fact]
		public void CanGetGroupsByName()
		{
			var groupRequestOne = new GroupRequest
			{
				Name = "CanGetGroupsByName 1",
			};

			var responseOne = SUGARClient.Group.Create(groupRequestOne);

			var groupRequestTwo = new GroupRequest
			{
				Name = "CanGetGroupsByName 2",
			};

			var responseTwo = SUGARClient.Group.Create(groupRequestTwo);

			var getGroups = SUGARClient.Group.Get("CanGetGroupsByName");

			Assert.Equal(2, getGroups.Count());
		}

		[Fact]
		public void CannotGetNotExistingGroupByName()
		{
			var getGroups = SUGARClient.Group.Get("CannotGetNotExistingGroupByName");

			Assert.Empty(getGroups);
		}

		[Fact]
		public void CannotGetGroupByEmptyName()
		{
			Assert.Throws<ClientException>(() => SUGARClient.Group.Get(""));
		}

		[Fact]
		public void CanGetGroupById()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CanGetGroupById",
			};

			var response = SUGARClient.Group.Create(groupRequest);

			var getGroup = SUGARClient.Group.Get((int) response.Id);

			Assert.Equal(response.Name, getGroup.Name);
			Assert.Equal(groupRequest.Name, getGroup.Name);
		}

		[Fact]
		public void CannotGetNotExistingGroupById()
		{
			var getGroup = SUGARClient.Group.Get(-1);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CanUpdateGroup()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CanUpdateGroup",
			};

			var response = SUGARClient.Group.Create(groupRequest);

			var updateRequest = new GroupRequest
			{
				Name = "CanUpdateGroup Updated"
			};

			SUGARClient.Group.Update(response.Id, updateRequest);

			var getGroup = SUGARClient.Group.Get((int) response.Id);

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal("CanUpdateGroup Updated", getGroup.Name);
		}

		[Fact]
		public void CannotUpdateGroupToDuplicateName()
		{
			var groupRequestOne = new GroupRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 1"
			};

			var responseOne = SUGARClient.Group.Create(groupRequestOne);

			var groupRequestTwo = new GroupRequest
			{
				Name = "CannotUpdateGroupToDuplicateName 2"
			};

			var responseTwo = SUGARClient.Group.Create(groupRequestTwo);

			var updateGroup = new GroupRequest
			{
				Name = groupRequestOne.Name
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Group.Update(responseTwo.Id, updateGroup));
		}

		[Fact]
		public void CannotUpdateNonExistingGroup()
		{
			var updateGroup = new GroupRequest
			{
				Name = "CannotUpdateNonExistingGroup"
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Group.Update(-1, updateGroup));
		}

		[Fact]
		public void CannotUpdateGroupToNoName()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CannotUpdateGroupToNoName",
			};

			var response = SUGARClient.Group.Create(groupRequest);

			var updateRequest = new GroupRequest
			{
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.Group.Update(response.Id, updateRequest));
		}

		[Fact]
		public void CanDeleteGroup()
		{
			var groupRequest = new GroupRequest
			{
				Name = "CanDeleteGroup",
			};

			var response = SUGARClient.Group.Create(groupRequest);

			var getGroup = SUGARClient.Group.Get((int) response.Id);

			Assert.NotNull(getGroup);

			SUGARClient.Group.Delete(response.Id);

			getGroup = SUGARClient.Group.Get((int) response.Id);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CannotDeleteNonExistingGroup()
		{
			SUGARClient.Group.Delete(-1);
		}
	}
}
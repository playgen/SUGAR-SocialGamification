using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class GroupClientTests : ClientTestBase
	{
		[Fact]
		public void CanCreateGroup()
		{
			var key = "Group_CanCreateGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			var response = Fixture.SUGARClient.Group.Create(groupRequest);

			Assert.Equal(groupRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Fact]
		public void CannotCreateDuplicateGroup()
		{
			var key = "Group_CannotCreateDuplicateGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			Fixture.SUGARClient.Group.Create(groupRequest);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Create(groupRequest));
		}

		[Fact]
		public void CannotCreateGroupWithNoName()
		{
			var key = "Group_CannotCreateGroupWithNoName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest();

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Create(groupRequest));
		}

		[Fact]
		public void CanGetGroupsByName()
		{
			var key = "Group_CanGetGroupsByName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequestOne = new GroupRequest
			{
				Name = key + "_Group1"
			};

			Fixture.SUGARClient.Group.Create(groupRequestOne);

			var groupRequestTwo = new GroupRequest
			{
				Name = key + "_Group2"
			};

			Fixture.SUGARClient.Group.Create(groupRequestTwo);

			var getGroups = Fixture.SUGARClient.Group.Get("CanGetGroupsByName");

			Assert.Equal(2, getGroups.Count());
		}

		[Fact]
		public void CannotGetNotExistingGroupByName()
		{
			var key = "Group_CannotGetNotExistingGroupByName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGroups = Fixture.SUGARClient.Group.Get(key);

			Assert.Empty(getGroups);
		}

		[Fact]
		public void CannotGetGroupByEmptyName()
		{
			var key = "Group_CannotGetGroupByEmptyName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Group.Get(string.Empty));
		}

		[Fact]
		public void CanGetGroupById()
		{
			var key = "Group_CanGetGroupById";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			var response = Fixture.SUGARClient.Group.Create(groupRequest);

			var getGroup = Fixture.SUGARClient.Group.Get(response.Id);

			Assert.Equal(response.Name, getGroup.Name);
			Assert.Equal(groupRequest.Name, getGroup.Name);
		}

		[Fact]
		public void CannotGetNotExistingGroupById()
		{
			var key = "Group_CannotGetNotExistingGroupById";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var getGroup = Fixture.SUGARClient.Group.Get(-1);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CanUpdateGroup()
		{
			var key = "Group_CanUpdateGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			var response = Fixture.SUGARClient.Group.Create(groupRequest);

			var updateRequest = new GroupRequest
			{
				Name = groupRequest.Name + " Updated"
			};

			Fixture.SUGARClient.Group.Update(response.Id, updateRequest);

			var getGroup = Fixture.SUGARClient.Group.Get(response.Id);

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal(key + "_Group" + " Updated", getGroup.Name);
		}

		[Fact]
		public void CannotUpdateGroupToDuplicateName()
		{
			var key = "Group_CannotUpdateGroupToDuplicateName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequestOne = new GroupRequest
			{
				Name = key + "_Group1"
			};

			Fixture.SUGARClient.Group.Create(groupRequestOne);

			var groupRequestTwo = new GroupRequest
			{
				Name = key + "_Group2"
			};

			var responseTwo = Fixture.SUGARClient.Group.Create(groupRequestTwo);

			var updateGroup = new GroupRequest
			{
				Name = groupRequestOne.Name
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Update(responseTwo.Id, updateGroup));
		}

		[Fact]
		public void CannotUpdateNonExistingGroup()
		{
			var key = "Group_CannotUpdateNonExistingGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var updateGroup = new GroupRequest
			{
				Name = key + "_Group"
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Update(-1, updateGroup));
		}

		[Fact]
		public void CannotUpdateGroupToNoName()
		{
			var key = "Group_CannotUpdateGroupToNoName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			var response = Fixture.SUGARClient.Group.Create(groupRequest);

			var updateRequest = new GroupRequest();

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Update(response.Id, updateRequest));
		}

		[Fact]
		public void CanDeleteGroup()
		{
			var key = "Group_CanDeleteGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			var groupRequest = new GroupRequest
			{
				Name = key + "_Group"
			};

			var response = Fixture.SUGARClient.Group.Create(groupRequest);

			var getGroup = Fixture.SUGARClient.Group.Get(response.Id);

			Assert.NotNull(getGroup);

			Fixture.SUGARClient.Group.Delete(response.Id);

			getGroup = Fixture.SUGARClient.Group.Get(response.Id);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CannotDeleteNonExistingGroup()
		{
			var key = "Group_CannotDeleteNonExistingGroup";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var game, out var loggedInAccount);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Delete(-1));
		}

		public GroupClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class UserTests : ClientTestBase
	{
		[Fact]
		public void CanGetUsersByName()
		{
			var userRequestOne = new UserRequest
			{
				Name = "CanGetUsersByName 1",
			};

			var responseOne = SUGARClient.User.Create(userRequestOne);

			var userRequestTwo = new UserRequest
			{
				Name = "CanGetUsersByName 2",
			};

			var responseTwo = SUGARClient.User.Create(userRequestTwo);

			var getUsers = SUGARClient.User.Get("CanGetUsersByName");

			Assert.Equal(2, getUsers.Count());
		}

		[Fact]
		public void CannotGetNotExistingUserByName()
		{
			var getUsers = SUGARClient.User.Get("CannotGetNotExistingUserByName");

			Assert.Empty(getUsers);
		}

		[Fact]
		public void CannotGetUserByEmptyName()
		{
			Assert.Throws<ClientException>(() => SUGARClient.User.Get(""));
		}

		[Fact]
		public void CanGetUserById()
		{
			var userRequest = new UserRequest
			{
				Name = "CanGetUserById",
			};

			var response = SUGARClient.User.Create(userRequest);

			var getUser = SUGARClient.User.Get((int) response.Id);

			Assert.Equal(response.Name, getUser.Name);
			Assert.Equal(userRequest.Name, getUser.Name);
		}

		[Fact]
		public void CannotGetNotExistingUserById()
		{
			var getUser = SUGARClient.User.Get(-1);

			Assert.Null(getUser);
		}

		[Fact]
		public void CanUpdateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanUpdateUser",
			};

			var response = SUGARClient.User.Create(userRequest);

			var updateRequest = new UserRequest
			{
				Name = "CanUpdateUser Updated"
			};

			SUGARClient.User.Update(response.Id, updateRequest);

			var getUser = SUGARClient.User.Get((int) response.Id);

			Assert.NotEqual(response.Name, updateRequest.Name);
			Assert.Equal("CanUpdateUser Updated", getUser.Name);
		}

		[Fact]
		public void CannotUpdateUserToDuplicateName()
		{
			var userRequestOne = new UserRequest
			{
				Name = "CannotUpdateUserToDuplicateName 1"
			};

			var responseOne = SUGARClient.User.Create(userRequestOne);

			var userRequestTwo = new UserRequest
			{
				Name = "CannotUpdateUserToDuplicateName 2"
			};

			var responseTwo = SUGARClient.User.Create(userRequestTwo);

			var updateUser = new UserRequest
			{
				Name = userRequestOne.Name
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Update(responseTwo.Id, updateUser));
		}

		[Fact]
		public void CannotUpdateNonExistingUser()
		{
			var updateUser = new UserRequest
			{
				Name = "CannotUpdateNonExistingUser"
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Update(-1, updateUser));
		}

		[Fact]
		public void CannotUpdateUserToNoName()
		{
			var userRequest = new UserRequest
			{
				Name = "CannotUpdateUserToNoName",
			};

			var response = SUGARClient.User.Create(userRequest);

			var updateRequest = new UserRequest
			{
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Update(response.Id, updateRequest));
		}
	}
}
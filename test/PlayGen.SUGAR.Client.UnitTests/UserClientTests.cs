using System.Linq;
using NUnit.Framework;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;

namespace PlayGen.SUGAR.Client.UnitTests
{
	public class UserClientTests : ClientTestsBase
	{
		[Test]
		public void CanCreateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanCreateUser"
			};

			var response = SUGARClient.User.Create(userRequest);

			Assert.AreEqual(userRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Test]
		public void CannotCreateDuplicateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CannotCreateDuplicateUser"
			};

			SUGARClient.User.Create(userRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Create(userRequest));
		}

		[Test]
		public void CannotCreateUserWithNoName()
		{
			var userRequest = new UserRequest();

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Create(userRequest));
		}

		[Test]
		public void CanGetUsersByName()
		{
			var userRequestOne = new UserRequest
			{
				Name = "CanGetUsersByName 1"
			};

			var responseOne = SUGARClient.User.Create(userRequestOne);

			var userRequestTwo = new UserRequest
			{
				Name = "CanGetUsersByName 2"
			};

			var responseTwo = SUGARClient.User.Create(userRequestTwo);

			var getUsers = SUGARClient.User.Get("CanGetUsersByName");

			Assert.AreEqual(2, getUsers.Items.Count());
		}

		[Test]
		public void CannotGetNotExistingUserByName()
		{
			var getUsers = SUGARClient.User.Get("CannotGetNotExistingUserByName");

			Assert.IsEmpty(getUsers.Items);
		}

		[Test]
		public void CannotGetUserByEmptyName()
		{
			Assert.Throws<ClientException>(() => SUGARClient.User.Get(""));
		}

		[Test]
		public void CanGetUserById()
		{
			var userRequest = new UserRequest
			{
				Name = "CanGetUserById"
			};

			var response = SUGARClient.User.Create(userRequest);

			var getUser = SUGARClient.User.Get(response.Id);

			Assert.AreEqual(response.Name, getUser.Name);
			Assert.AreEqual(userRequest.Name, getUser.Name);
		}

		[Test]
		public void CannotGetNotExistingUserById()
		{
			var getUser = SUGARClient.User.Get(-1);

			Assert.Null(getUser);
		}

		[Test]
		public void CanUpdateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanUpdateUser"
			};

			var response = SUGARClient.User.Create(userRequest);

			var updateRequest = new UserRequest
			{
				Name = "CanUpdateUser Updated"
			};

			SUGARClient.User.Update(response.Id, updateRequest);

			var getUser = SUGARClient.User.Get(response.Id);

			Assert.AreNotEqual(response.Name, updateRequest.Name);
			Assert.AreEqual("CanUpdateUser Updated", getUser.Name);
		}

		[Test]
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

		[Test]
		public void CannotUpdateNonExistingUser()
		{
			var updateUser = new UserRequest
			{
				Name = "CannotUpdateNonExistingUser"
			};

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Update(-1, updateUser));
		}

		[Test]
		public void CannotUpdateUserToNoName()
		{
			var userRequest = new UserRequest
			{
				Name = "CannotUpdateUserToNoName"
			};

			var response = SUGARClient.User.Create(userRequest);

			var updateRequest = new UserRequest();

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Update(response.Id, updateRequest));
		}

		[Test]
		public void CanDeleteUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanDeleteUser"
			};

			var response = SUGARClient.User.Create(userRequest);

			var getUser = SUGARClient.User.Get(response.Id);

			Assert.NotNull(getUser);

			SUGARClient.User.Delete(response.Id);

			getUser = SUGARClient.User.Get(response.Id);

			Assert.Null(getUser);
		}

		[Test]
		public void CannotDeleteNonExistingUser()
		{
			SUGARClient.User.Delete(-1);
		}
	}
}
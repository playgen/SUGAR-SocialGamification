using System;
using System.Linq;
using System.Net;
using PlayGen.SUGAR.Contracts;
using NUnit.Framework;

namespace PlayGen.SUGAR.Client.IntegrationTests
{
	public class UserClientTests
	{
		#region Configuration
		private readonly UserClient _userClient;
		
		public UserClientTests()
		{
			var testSugarClient = new TestSUGARClient();
			_userClient = testSugarClient.User;

			RegisterAndLogin(testSugarClient.Account);
		}

		private void RegisterAndLogin(AccountClient client)
		{
			var accountRequest = new AccountRequest
			{
				Name = "UserClientTests",
				Password = "UserClientTestsPassword",
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
		[Test]
		public void CanCreateUser()
		{
			var userRequest = new ActorRequest
			{
				Name = "CanCreateUser",
			};

			var response = _userClient.Create(userRequest);

			Assert.AreEqual(userRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Test]
		public void CannotCreateDuplicateUser()
		{
			var userRequest = new ActorRequest
			{
				Name = "CannotCreateDuplicateUser",
			};

			_userClient.Create(userRequest);

			Assert.Throws<Exception>(() => _userClient.Create(userRequest));
		}

		[Test]
		public void CannotCreateUserWithNoName()
		{
			var userRequest = new ActorRequest { };

			Assert.Throws<Exception>(() => _userClient.Create(userRequest));
		}

		[Test]
		public void CanGetUsersByName()
		{
			var userRequestOne = new ActorRequest
			{
				Name = "CanGetUsersByName 1",
			};

			var responseOne = _userClient.Create(userRequestOne);

			var userRequestTwo = new ActorRequest
			{
				Name = "CanGetUsersByName 2",
			};

			var responseTwo = _userClient.Create(userRequestTwo);

			var getUsers = _userClient.Get("CanGetUsersByName");

			Assert.AreEqual(2, getUsers.Count());
		}

		[Test]
		public void CannotGetNotExistingUserByName()
		{
			var getUsers = _userClient.Get("CannotGetNotExistingUserByName");

			Assert.IsEmpty(getUsers);
		}

		[Test]
		public void CannotGetUserByEmptyName()
		{
			Assert.Throws<Exception>(() => _userClient.Get(""));
		}

		[Test]
		public void CanGetUserById()
		{
			var userRequest = new ActorRequest
			{
				Name = "CanGetUserById",
			};

			var response = _userClient.Create(userRequest);

			var getUser = _userClient.Get(response.Id);

			Assert.AreEqual(response.Name, getUser.Name);
			Assert.AreEqual(userRequest.Name, getUser.Name);
		}

		[Test]
		public void CannotGetNotExistingUserById()
		{
			var getUser = _userClient.Get(-1);

			Assert.Null(getUser);
		}

		[Test]
		public void CanUpdateUser()
		{
			var userRequest = new ActorRequest
			{
				Name = "CanUpdateUser",
			};

			var response = _userClient.Create(userRequest);

			var updateRequest = new ActorRequest
			{
				Name = "CanUpdateUser Updated"
			};

			_userClient.Update(response.Id, updateRequest);

			var getUser = _userClient.Get(response.Id);

			Assert.AreNotEqual(response.Name, updateRequest.Name);
			Assert.AreEqual("CanUpdateUser Updated", getUser.Name);
		}

		[Test]
		public void CannotUpdateUserToDuplicateName()
		{
			var userRequestOne = new ActorRequest
			{
				Name = "CannotUpdateUserToDuplicateName 1"
			};

			var responseOne = _userClient.Create(userRequestOne);

			var userRequestTwo = new ActorRequest
			{
				Name = "CannotUpdateUserToDuplicateName 2"
			};

			var responseTwo = _userClient.Create(userRequestTwo);

			var updateUser = new ActorRequest
			{
				Name = userRequestOne.Name
			};

			Assert.Throws<Exception>(() => _userClient.Update(responseTwo.Id, updateUser));
		}

		[Test]
		public void CannotUpdateNonExistingUser()
		{
			var updateUser = new ActorRequest
			{
				Name = "CannotUpdateNonExistingUser"
			};

			Assert.Throws<Exception>(() => _userClient.Update(-1, updateUser));
		}

		[Test]
		public void CannotUpdateUserToNoName()
		{
			var userRequest = new ActorRequest
			{
				Name = "CannotUpdateUserToNoName",
			};

			var response = _userClient.Create(userRequest);

			var updateRequest = new ActorRequest
			{
			};

			Assert.Throws<Exception>(() => _userClient.Update(response.Id, updateRequest));
		}

		[Test]
		public void CanDeleteUser()
		{
			var userRequest = new ActorRequest
			{
				Name = "CanDeleteUser",
			};

			var response = _userClient.Create(userRequest);

			var getUser = _userClient.Get(response.Id);

			Assert.NotNull(getUser);

			_userClient.Delete(response.Id);

			getUser = _userClient.Get(response.Id);

			Assert.Null(getUser);
		}

		[Test]
		public void CannotDeleteNonExistingUser()
		{
			_userClient.Delete(-1);
		}


		#endregion
	}
}
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class UserClientTests : ClientTestBase
	{
		[Fact]
		public void CanGetUsersByName()
		{
			var key = "User_CanGetUsersByName";
			CreateUser(key + "_Extra1");
			CreateUser(key + "_Extra2");
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);

			var getUsers = Fixture.SUGARClient.User.Get(key + "_Extra");

			Assert.Equal(2, getUsers.Count());
		}

		[Fact]
		public void CannotGetNotExistingUserByName()
		{
			var key = "User_CannotGetNotExistingUserByName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);

			var getUsers = Fixture.SUGARClient.User.Get(key + "_Extra");

			Assert.Empty(getUsers);
		}

		[Fact]
		public void CannotGetUserByEmptyName()
		{
			var key = "User_CannotGetUserByEmptyName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.User.Get(string.Empty));
		}

		[Fact]
		public void CanGetUserById()
		{
			var key = "User_CanGetUserById";
			var newUser = CreateUser(key + "_Extra1");
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);

			var getUser = Fixture.SUGARClient.User.Get(newUser.Id);

			Assert.Equal(newUser.Name, getUser.Name);
		}

		[Fact]
		public void CannotGetNotExistingUserById()
		{
			var key = "User_CannotGetNotExistingUserById";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);
			var getUser = Fixture.SUGARClient.User.Get(-1);

			Assert.Null(getUser);
		}

		[Fact]
		public void CanUpdateUser()
		{
			var key = "User_CanUpdateUser";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var updateRequest = new UserRequest
			{
				Name = loggedInAccount.User.Name + "_Updated"
			};

			Fixture.SUGARClient.User.Update(loggedInAccount.User.Id, updateRequest);

			var getUser = Fixture.SUGARClient.User.Get(loggedInAccount.User.Id);

			Assert.NotEqual(key, updateRequest.Name);
			Assert.Equal(loggedInAccount.User.Name + "_Updated", getUser.Name);
		}

		[Fact]
		public void CannotUpdateUserToDuplicateName()
		{
			var key = "User_CannotUpdateUserToDuplicateName";
			var extra = CreateUser(key + "_Extra");
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var updateUser = new UserRequest
			{
				Name = extra.Name
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.User.Update(loggedInAccount.User.Id, updateUser));
		}

		[Fact]
		public void CannotUpdateNonExistingUser()
		{
			var key = "User_CannotUpdateNonExistingUser";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var _);

			var updateUser = new UserRequest
			{
				Name = key + "_Updated"
			};

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.User.Update(-1, updateUser));
		}

		[Fact]
		public void CannotUpdateUserToNoName()
		{
			var key = "User_CannotUpdateUserToNoName";
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var loggedInAccount);

			var updateRequest = new UserRequest();

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.User.Update(loggedInAccount.User.Id, updateRequest));
		}

		#region Helpers
		private UserResponse CreateUser(string key)
		{
			Helpers.Login(Fixture.SUGARClient, "Global", key, out var _, out var friendAccount);
			return friendAccount.User;
		}
		#endregion

		public UserClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
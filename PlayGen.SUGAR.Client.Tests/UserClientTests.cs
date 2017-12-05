using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Exceptions;
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

		[Theory]
		[InlineData(null)]
		[InlineData("Short Description")]
		[InlineData("Medium Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi sodales nisl quis neque tempus bibendum. Nunc ut sem vel metus venenatis euismod sed id enim. Sed faucibus erat eget sapien fringilla, id malesuada nunc convallis. Nam dapibus cursus accumsan. Sed in fermentum libero. Proin quis felis turpis. Etiam feugiat scelerisque metus id pretium. Morbi lacus purus, ornare eget libero nec, fringilla laoreet dui. Suspendisse dictum a turpis eu vulputate. Vestibulum ante ipsum primis in cras amet.")]
		[InlineData("Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut in venenatis augue, quis hendrerit justo. Praesent malesuada leo vel nisl aliquet, id efficitur nibh tincidunt. Donec laoreet orci vel sollicitudin rutrum. Maecenas lacinia libero nec finibus porttitor. Donec tempus tincidunt felis in viverra. Aliquam eleifend ex sem, at porta sapien malesuada ac. Praesent auctor ligula dictum odio gravida tempor. Fusce malesuada malesuada magna non pretium. Mauris lobortis accumsan porttitor. Donec commodo risus at neque porttitor volutpat. Phasellus et ullamcorper libero, vel euismod massa. Vivamus ullamcorper nibh a dolor accumsan tempor. Curabitur laoreet accumsan nisi non porttitor. Donec tincidunt sapien eu ligula consequat fringilla sed eu augue. Nulla sed convallis lacus, eu venenatis urna. Duis eu purus tempor dolor porttitor tincidunt eu quis libero. Nunc sit amet aliquet eros.Sed sed vestibulum nulla.Phasellus suscipit arcu vel neque egestas, eget rutrum est sollicitudin.Curabitur amet.")]
		public void CanCreateWithValidDescription(string description)
		{
			// Arrange 
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, $"CanCreateValid_{Guid.NewGuid()}");

			var userRequest = new UserRequest
			{
				Name = loggedInAccount.User.Name,
				Description = description
			};

			// Act
			var userResponse = Fixture.SUGARClient.User.Update(loggedInAccount.User.Id, userRequest);

			// Assert
			Assert.NotNull(userResponse);
			Assert.Equal(userRequest.Name, userResponse.Name);
			Assert.Equal(userRequest.Description, userResponse.Description);
		}

		[Theory]
		[InlineData("Over Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam vel pulvinar lorem. Nulla dictum tincidunt sem sed ornare. Etiam convallis gravida dolor et euismod. Cras vitae mattis felis, vitae placerat tortor. Suspendisse ultrices sagittis blandit. Vestibulum aliquet odio ut erat ornare, at scelerisque sapien iaculis. Donec commodo volutpat sapien, viverra imperdiet odio vestibulum at. Nullam aliquet gravida felis egestas mollis. Donec sodales, lorem ac lobortis feugiat, dolor neque iaculis leo, et finibus felis lorem sit amet odio. Fusce sodales risus in accumsan volutpat.In elementum scelerisque ex non mattis.Cras in malesuada metus.Suspendisse vitae volutpat velit.Nulla nibh lectus, elementum quis libero quis, iaculis pharetra erat.Nam in tortor tincidunt, volutpat neque vel, vestibulum nisi.Curabitur imperdiet mattis maximus.Ut posuere volutpat vehicula.Nunc interdum arcu nisl, nec vehicula tellus ultrices non.Fusce in placerat lectus.Etiam gravida vitae nibh eget interdum.Proin sagittis sem non tempor luctus.Morbi tempor, risus nec pretium aliquet, felis ante metus.")]
		public void CantCreateWithInvalidDescription(string description)
		{
			// Arrange 
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, $"CantCreateInvalid_{Guid.NewGuid()}");
			
			var userRequest = new UserRequest
			{
				Name = loggedInAccount.User.Name,

				Description = description
			};

			// Act Assert
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.User.Update(loggedInAccount.User.Id, userRequest));
		}

		public UserClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}
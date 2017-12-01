using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Exceptions;
using PlayGen.SUGAR.Contracts;
using Xunit;

namespace PlayGen.SUGAR.Client.Tests
{
	public class UserTests : ClientTestBase
	{
		[Fact]
		public void CanCreateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanCreateUser",
			};

			var response = SUGARClient.User.Create(userRequest);

			Assert.Equal(userRequest.Name, response.Name);
			Assert.True(response.Id > 0);
		}

		[Fact]
		public void CannotCreateDuplicateUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CannotCreateDuplicateUser",
			};

			SUGARClient.User.Create(userRequest);

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Create(userRequest));
		}

		[Fact]
		public void CannotCreateUserWithNoName()
		{
			var userRequest = new UserRequest { };

			Assert.Throws<ClientHttpException>(() => SUGARClient.User.Create(userRequest));
		}

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

		[Fact]
		public void CanDeleteUser()
		{
			var userRequest = new UserRequest
			{
				Name = "CanDeleteUser",
			};

			var response = SUGARClient.User.Create(userRequest);

			var getUser = SUGARClient.User.Get((int) response.Id);

			Assert.NotNull(getUser);

			SUGARClient.User.Delete(response.Id);

			getUser = SUGARClient.User.Get((int) response.Id);

			Assert.Null(getUser);
		}

		[Fact]
		public void CannotDeleteNonExistingUser()
		{
			SUGARClient.User.Delete(-1);
		}

		[Theory]
		[InlineData("")]
		[InlineData("Short Description")]
		[InlineData("Medium Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi sodales nisl quis neque tempus bibendum. Nunc ut sem vel metus venenatis euismod sed id enim. Sed faucibus erat eget sapien fringilla, id malesuada nunc convallis. Nam dapibus cursus accumsan. Sed in fermentum libero. Proin quis felis turpis. Etiam feugiat scelerisque metus id pretium. Morbi lacus purus, ornare eget libero nec, fringilla laoreet dui. Suspendisse dictum a turpis eu vulputate. Vestibulum ante ipsum primis in cras amet.")]
		[InlineData("Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut in venenatis augue, quis hendrerit justo. Praesent malesuada leo vel nisl aliquet, id efficitur nibh tincidunt. Donec laoreet orci vel sollicitudin rutrum. Maecenas lacinia libero nec finibus porttitor. Donec tempus tincidunt felis in viverra. Aliquam eleifend ex sem, at porta sapien malesuada ac. Praesent auctor ligula dictum odio gravida tempor. Fusce malesuada malesuada magna non pretium. Mauris lobortis accumsan porttitor. Donec commodo risus at neque porttitor volutpat. Phasellus et ullamcorper libero, vel euismod massa. Vivamus ullamcorper nibh a dolor accumsan tempor. Curabitur laoreet accumsan nisi non porttitor. Donec tincidunt sapien eu ligula consequat fringilla sed eu augue. Nulla sed convallis lacus, eu venenatis urna. Duis eu purus tempor dolor porttitor tincidunt eu quis libero. Nunc sit amet aliquet eros.Sed sed vestibulum nulla.Phasellus suscipit arcu vel neque egestas, eget rutrum est sollicitudin.Curabitur amet.")]
		public void CanCreateWithValidDescription(string description)
		{
			// Arrange 
			var request = new UserRequest
			{
				Name = $"CanCreateWithValidDescription_{Guid.NewGuid()}",

				Description = description
			};

			// Act
			var response = SUGARClient.User.Create(request);

			// Assert
			Assert.NotNull(response);
			Assert.Equal(request.Name, response.Name);
			Assert.Equal(request.Description, response.Description);
		}

		[Theory]
		[InlineData("Over Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam vel pulvinar lorem. Nulla dictum tincidunt sem sed ornare. Etiam convallis gravida dolor et euismod. Cras vitae mattis felis, vitae placerat tortor. Suspendisse ultrices sagittis blandit. Vestibulum aliquet odio ut erat ornare, at scelerisque sapien iaculis. Donec commodo volutpat sapien, viverra imperdiet odio vestibulum at. Nullam aliquet gravida felis egestas mollis. Donec sodales, lorem ac lobortis feugiat, dolor neque iaculis leo, et finibus felis lorem sit amet odio. Fusce sodales risus in accumsan volutpat.In elementum scelerisque ex non mattis.Cras in malesuada metus.Suspendisse vitae volutpat velit.Nulla nibh lectus, elementum quis libero quis, iaculis pharetra erat.Nam in tortor tincidunt, volutpat neque vel, vestibulum nisi.Curabitur imperdiet mattis maximus.Ut posuere volutpat vehicula.Nunc interdum arcu nisl, nec vehicula tellus ultrices non.Fusce in placerat lectus.Etiam gravida vitae nibh eget interdum.Proin sagittis sem non tempor luctus.Morbi tempor, risus nec pretium aliquet, felis ante metus.")]
		public void CantCreateWithInvalidDescription(string description)
		{
			// Arrange 
			var userRequest = new UserRequest
			{
				Name = $"CanCreateWithValidDescription_{Guid.NewGuid()}",

				Description = description
			};

			// Act Assert
			Assert.Throws<SUGARException>(() => SUGARClient.User.Create(userRequest));
		}
	}
}
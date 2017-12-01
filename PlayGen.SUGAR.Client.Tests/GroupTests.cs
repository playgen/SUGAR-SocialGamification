using System;
using System.Linq;
using PlayGen.SUGAR.Client.Exceptions;
using PlayGen.SUGAR.Common.Exceptions;
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

		[Theory]
		[InlineData("")]
		[InlineData("Short Description")]
		[InlineData("Medium Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi sodales nisl quis neque tempus bibendum. Nunc ut sem vel metus venenatis euismod sed id enim. Sed faucibus erat eget sapien fringilla, id malesuada nunc convallis. Nam dapibus cursus accumsan. Sed in fermentum libero. Proin quis felis turpis. Etiam feugiat scelerisque metus id pretium. Morbi lacus purus, ornare eget libero nec, fringilla laoreet dui. Suspendisse dictum a turpis eu vulputate. Vestibulum ante ipsum primis in cras amet.")]
		[InlineData("Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut in venenatis augue, quis hendrerit justo. Praesent malesuada leo vel nisl aliquet, id efficitur nibh tincidunt. Donec laoreet orci vel sollicitudin rutrum. Maecenas lacinia libero nec finibus porttitor. Donec tempus tincidunt felis in viverra. Aliquam eleifend ex sem, at porta sapien malesuada ac. Praesent auctor ligula dictum odio gravida tempor. Fusce malesuada malesuada magna non pretium. Mauris lobortis accumsan porttitor. Donec commodo risus at neque porttitor volutpat. Phasellus et ullamcorper libero, vel euismod massa. Vivamus ullamcorper nibh a dolor accumsan tempor. Curabitur laoreet accumsan nisi non porttitor. Donec tincidunt sapien eu ligula consequat fringilla sed eu augue. Nulla sed convallis lacus, eu venenatis urna. Duis eu purus tempor dolor porttitor tincidunt eu quis libero. Nunc sit amet aliquet eros.Sed sed vestibulum nulla.Phasellus suscipit arcu vel neque egestas, eget rutrum est sollicitudin.Curabitur amet.")]
		public void CanCreateWithValidDescription(string description)
		{
			// Arrange 
			var request = new GroupRequest
			{
				Name = $"CanCreateWithValidDescription_{Guid.NewGuid()}",

				Description = description
			};

			// Act
			var response = SUGARClient.Group.Create(request);

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
			var request = new GroupRequest
			{
				Name = $"CanCreateWithValidDescription_{Guid.NewGuid()}",

				Description = description
			};

			// Act Assert
			Assert.Throws<SUGARException>(() => SUGARClient.Group.Create(request));
		}
	}
}
using System;
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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var groupRequest = new GroupRequest();

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Create(groupRequest));
		}

		[Fact]
		public void CanGetGroupsByName()
		{
			var key = "Group_CanGetGroupsByName";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGroups = Fixture.SUGARClient.Group.Get(key);

			Assert.Empty(getGroups);
		}

		[Fact]
		public void CannotGetGroupByEmptyName()
		{
			var key = "Group_CannotGetGroupByEmptyName";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientException>(() => Fixture.SUGARClient.Group.Get(string.Empty));
		}

		[Fact]
		public void CanGetGroupById()
		{
			var key = "Group_CanGetGroupById";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGroup = Fixture.SUGARClient.Group.Get(-1);

			Assert.Null(getGroup);
		}

		[Fact]
		public void CanUpdateGroup()
		{
			var key = "Group_CanUpdateGroup";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

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
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Delete(-1));
		}

		[Fact]
		public void CanCreateGroupForGame()
		{
			var key = "Group_CanCreateGroupForGame";

			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGames = Fixture.SUGARClient.Game.Get(key);
			var game = getGames.First();
			Assert.NotNull(game);

			var groupRequest = new GroupRequest { Name = key + "_Group", GameId = game.Id};

			Fixture.SUGARClient.Group.Create(groupRequest);
			var groups = Fixture.SUGARClient.Group.GetByGame(game.Id);

			Assert.NotNull(groups);
			Assert.Equal(key+"_Group", groups.First().Name);
		}

		[Fact]
		public void CannotCreateGroupForInvalidGame()
		{
			var key = "Group_CannotCreateGroupForInvalidGame";
			Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, key);

			var getGames = Fixture.SUGARClient.Game.Get(key);
			var game = getGames.First();
			Assert.NotNull(game);

			var groupRequest = new GroupRequest { Name = key + "_Group" , GameId = -1};
			Fixture.SUGARClient.Group.Create(groupRequest);
			var groups = Fixture.SUGARClient.Group.GetByGame(game.Id);
			Assert.Equal(0, groups.Count());
		}

		[Theory]
		[InlineData("")]
		[InlineData("Short Description")]
		[InlineData("Medium Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi sodales nisl quis neque tempus bibendum. Nunc ut sem vel metus venenatis euismod sed id enim. Sed faucibus erat eget sapien fringilla, id malesuada nunc convallis. Nam dapibus cursus accumsan. Sed in fermentum libero. Proin quis felis turpis. Etiam feugiat scelerisque metus id pretium. Morbi lacus purus, ornare eget libero nec, fringilla laoreet dui. Suspendisse dictum a turpis eu vulputate. Vestibulum ante ipsum primis in cras amet.")]
		[InlineData("Max Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut in venenatis augue, quis hendrerit justo. Praesent malesuada leo vel nisl aliquet, id efficitur nibh tincidunt. Donec laoreet orci vel sollicitudin rutrum. Maecenas lacinia libero nec finibus porttitor. Donec tempus tincidunt felis in viverra. Aliquam eleifend ex sem, at porta sapien malesuada ac. Praesent auctor ligula dictum odio gravida tempor. Fusce malesuada malesuada magna non pretium. Mauris lobortis accumsan porttitor. Donec commodo risus at neque porttitor volutpat. Phasellus et ullamcorper libero, vel euismod massa. Vivamus ullamcorper nibh a dolor accumsan tempor. Curabitur laoreet accumsan nisi non porttitor. Donec tincidunt sapien eu ligula consequat fringilla sed eu augue. Nulla sed convallis lacus, eu venenatis urna. Duis eu purus tempor dolor porttitor tincidunt eu quis libero. Nunc sit amet aliquet eros.Sed sed vestibulum nulla.Phasellus suscipit arcu vel neque egestas, eget rutrum est sollicitudin.Curabitur amet.")]
		public void CanCreateWithValidDescription(string description)
		{
			// Arrange 
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, $"CanCreateValid_{Guid.NewGuid()}");

			var request = new GroupRequest
			{
				Name = $"{loggedInAccount.User.Name}_Group",

				Description = description
			};

			// Act
			var response = Fixture.SUGARClient.Group.Create(request);

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
			var loggedInAccount = Helpers.CreateAndLoginGlobal(Fixture.SUGARClient, $"CantCreateInvalid_{Guid.NewGuid()}");

			var request = new GroupRequest
			{
				Name = $"{loggedInAccount.User.Name}_Group",

				Description = description
			};

			// Act Assert
			Assert.Throws<ClientHttpException>(() => Fixture.SUGARClient.Group.Create(request));
		}

		public GroupClientTests(ClientTestsFixture fixture)
			: base(fixture)
		{
		}
	}
}

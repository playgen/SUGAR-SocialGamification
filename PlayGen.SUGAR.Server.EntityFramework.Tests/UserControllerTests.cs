using System.Linq;
using PlayGen.SUGAR.Server.EntityFramework.Controllers;
using PlayGen.SUGAR.Server.EntityFramework.Exceptions;
using PlayGen.SUGAR.Server.Model;
using Xunit;

namespace PlayGen.SUGAR.Server.EntityFramework.Tests
{
	[Collection("Project Fixture Collection")]
	public class UserControllerTests
	{
		#region Configuration

		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUser()
		{
			var userName = "CreateUser";

			CreateUser(userName);

			var users = _userController.Search(userName);

			var matches = users.Count(g => g.Name == userName);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateUser()
		{
			var userName = "CreateDuplicateUser";

			CreateUser(userName);

			Assert.Throws<DuplicateRecordException>(() => CreateUser(userName));
		}

		[Fact]
		public void GetMultipleUsersByName()
		{
			var userNames = new[]
			{
				"GetMultipleUsersByName1",
				"GetMultipleUsersByName2",
				"GetMultipleUsersByName3",
				"GetMultipleUsersByName4",
			};

			foreach (var userName in userNames)
			{
				CreateUser(userName);
			}

			CreateUser("GetMultiple_UsersByName_DontGetThis");

			var users = _userController.Search("GetMultipleUsersByName");

			var matchingUsers = users.Select(g => userNames.Contains(g.Name));

			Assert.Equal(matchingUsers.Count(), userNames.Length);
		}

		[Fact]
		public void GetNonExistingUser()
		{
			var users = _userController.Search("GetNonExsitingUsers");

			Assert.Empty(users);
		}

		[Fact]
		public void GetUserById()
		{
			var newUser = CreateUser("GetUserById");

			var id = newUser.Id;

			var user = _userController.Get(id);

			Assert.NotNull(user);
			Assert.Equal(newUser.Name, user.Name);
		}

		[Fact]
		public void GetNonExistingUserById()
		{
			var user = _userController.Get(-1);

			Assert.Null(user);
		}

		[Fact]
		public void UpdateUser()
		{
			var userName = "UpdateExistingUser";

			var newUser = CreateUser(userName);

			var users = _userController.Search(userName);

			var matches = users.Count(g => g.Name == userName);

			Assert.Equal(1, matches);

			var updateUser = new User {
				Id = newUser.Id,
				Name = "UpdateExistingUserProof"
			};

			_userController.Update(updateUser);

			var updatedUser = _userController.Get(newUser.Id);

			Assert.Equal("UpdateExistingUserProof", updatedUser.Name);
		}

		[Fact]
		public void UpdateUserToDuplicateName()
		{
			var userName = "UpdateUserToDuplicateName";

			var newUser = CreateUser(userName);

			var newUserDuplicate = CreateUser(userName + " Two");

			var updateUser = new User {
				Id = newUserDuplicate.Id,
				Name = newUser.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _userController.Update(updateUser));
		}

		[Fact]
		public void UpdateNonExistingUser()
		{
			var user = new User {
				Id = -1,
				Name = "UpdateNonExistingUser"
			};

			Assert.Throws<MissingRecordException>(() => _userController.Update(user));
		}

		[Fact]
		public void DeleteExistingUser()
		{
			var userName = "DeleteExistingUser";

			var user = CreateUser(userName);

			var users = _userController.Search(userName);
			Assert.Equal(users.Count(), 1);
			Assert.Equal(users.ElementAt(0).Name, userName);

			_userController.Delete(user.Id);
			users = _userController.Search(userName);

			Assert.Empty(users);
		}

		[Fact]
		public void DeleteNonExistingUser()
		{
			_userController.Delete(-1);
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User {
				Name = name,
			};

			_userController.Create(user);

			return user;
		}
		#endregion
	}
}
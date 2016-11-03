using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserControllerTests
	{
		#region Configuration
		private readonly UserController _userDbController;

		public UserControllerTests()
		{
			_userDbController = TestEnvironment.UserController;
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUser()
		{
			var userName = "CreateUser";

			CreateUser(userName);

			var users = _userDbController.Search(userName);

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

			var users = _userDbController.Search("GetMultipleUsersByName");

			var matchingUsers = users.Select(g => userNames.Contains(g.Name));

			Assert.Equal(matchingUsers.Count(), userNames.Length);
		}

		[Fact]
		public void GetNonExistingUser()
		{
			var users = _userDbController.Search("GetNonExsitingUsers");

			Assert.Empty(users);
		}

		[Fact]
		public void GetUserById()
		{
			var newUser = CreateUser("GetUserById");

			var id = newUser.Id;

			var user = _userDbController.Search(id);

			Assert.NotNull(user);
			Assert.Equal(newUser.Name, user.Name);
		}

		[Fact]
		public void GetNonExistingUserById()
		{
			var user = _userDbController.Search(-1);

			Assert.Null(user);
		}

		[Fact]
		public void UpdateUser()
		{
			var userName = "UpdateExistingUser";

			var newUser = CreateUser(userName);

			var users = _userDbController.Search(userName);

			var matches = users.Count(g => g.Name == userName);

			Assert.Equal(1, matches);

			var updateUser = new User
			{
				Id = newUser.Id,
				Name = "UpdateExistingUserProof"
			};

			_userDbController.Update(updateUser);

			var updatedUser = _userDbController.Search(newUser.Id);

			Assert.Equal("UpdateExistingUserProof", updatedUser.Name);
		}

		[Fact]
		public void UpdateUserToDuplicateName()
		{
			var userName = "UpdateUserToDuplicateName";

			var newUser = CreateUser(userName);

			var newUserDuplicate = CreateUser(userName + " Two");

			var updateUser = new User
			{
				Id = newUserDuplicate.Id,
				Name = newUser.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _userDbController.Update(updateUser));
		}

		[Fact]
		public void UpdateNonExistingUser()
		{
			var user = new User
			{
				Id = -1,
				Name = "UpdateNonExistingUser"
			};

			Assert.Throws<MissingRecordException>(() => _userDbController.Update(user));
		}

		[Fact]
		public void DeleteExistingUser()
		{
			var userName = "DeleteExistingUser";

			var user = CreateUser(userName);

			var users = _userDbController.Search(userName);
			Assert.Equal(users.Count(), 1);
			Assert.Equal(users.ElementAt(0).Name, userName);

			_userDbController.Delete(user.Id);
			users = _userDbController.Search(userName);

			Assert.Empty(users);
		}

		[Fact]
		public void DeleteNonExistingUser()
		{
			_userDbController.Delete(-1);
		}
		#endregion

		#region Helpers
		private User CreateUser(string name)
		{
			var user = new User
			{
				Name = name,
			};

			_userDbController.Create(user);

			return user;
		}
		#endregion
	}
}
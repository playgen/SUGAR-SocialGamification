using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using NUnit.Framework;

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
		[Test]
		public void CreateAndGetUser()
		{
			string userName = "CreateUser";

			CreateUser(userName);

			var users = _userDbController.Search(userName);

			int matches = users.Count(g => g.Name == userName);

			Assert.AreEqual(matches, 1);
		}

		[Test]
		public void CreateDuplicateUser()
		{
			string userName = "CreateDuplicateUser";

			CreateUser(userName);

			Assert.Throws<DuplicateRecordException>(() => CreateUser(userName));
		}

		[Test]
		public void GetMultipleUsersByName()
		{
			string[] userNames = new[]
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

			Assert.AreEqual(matchingUsers.Count(), userNames.Length);
		}

		[Test]
		public void GetNonExistingUser()
		{
			var users = _userDbController.Search("GetNonExsitingUsers");

			Assert.IsEmpty(users);
		}

		[Test]
		public void GetUserById()
		{
			User newUser = CreateUser("GetUserById");

			int id = newUser.Id;

			var user = _userDbController.Search(id);

			Assert.NotNull(user);
			Assert.AreEqual(newUser.Name, user.Name);
		}

		[Test]
		public void GetNonExistingUserById()
		{
			var user = _userDbController.Search(-1);

			Assert.Null(user);
		}

		[Test]
		public void UpdateUser()
		{
			string userName = "UpdateExistingUser";

			User newUser = CreateUser(userName);

			var users = _userDbController.Search(userName);

			int matches = users.Count(g => g.Name == userName);

			Assert.AreEqual(1, matches);

			var updateUser = new User
			{
				Id = newUser.Id,
				Name = "UpdateExistingUserProof"
			};

			_userDbController.Update(updateUser);

			var updatedUser = _userDbController.Search(newUser.Id);

			Assert.AreEqual("UpdateExistingUserProof", updatedUser.Name);
		}

		[Test]
		public void UpdateUserToDuplicateName()
		{
			string userName = "UpdateUserToDuplicateName";

			User newUser = CreateUser(userName);

			User newUserDuplicate = CreateUser(userName + " Two");

			var updateUser = new User
			{
				Id = newUserDuplicate.Id,
				Name = newUser.Name
			};

			Assert.Throws<DuplicateRecordException>(() => _userDbController.Update(updateUser));
		}

		[Test]
		public void UpdateNonExistingUser()
		{
			var user = new User
			{
				Id = -1,
				Name = "UpdateNonExistingUser"
			};

			Assert.Throws<MissingRecordException>(() => _userDbController.Update(user));
		}

		[Test]
		public void DeleteExistingUser()
		{
			string userName = "DeleteExistingUser";

			var user = CreateUser(userName);

			var users = _userDbController.Search(userName);
			Assert.AreEqual(users.Count(), 1);
			Assert.AreEqual(users.ElementAt(0).Name, userName);

			_userDbController.Delete(user.Id);
			users = _userDbController.Search(userName);

			Assert.IsEmpty(users);
		}

		[Test]
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
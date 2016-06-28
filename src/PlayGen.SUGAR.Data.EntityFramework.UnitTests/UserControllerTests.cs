using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class UserControllerTests : TestController
	{
		#region Configuration
		private readonly UserController _userDbController;

		public UserControllerTests()
		{
			_userDbController = new UserController(NameOrConnectionString);
		}
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetUser()
		{
			string userName = "CreateUser";

			CreateUser(userName);

			var users = _userDbController.Search(userName);

			int matches = users.Count(g => g.Name == userName);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateUser()
		{
			string userName = "CreateDuplicateUser";

			CreateUser(userName);

			Assert.Throws<DuplicateRecordException>(() => CreateUser(userName));
		}

		[Fact]
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

			Assert.Equal(matchingUsers.Count(), userNames.Length);
		}

		[Fact]
		public void GetNonExistingUser()
		{
			var users = _userDbController.Search("GetNonExsitingUsers");

			Assert.Empty(users);
		}

		[Fact]
		public void DeleteExistingUser()
		{
			string userName = "DeleteExistingUser";

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

			var users = _userDbController.Get(new string[] { userName });

			int matches = users.Count(g => g.Name == userName);

			Assert.Equal(matches, 1);
		}

		[Fact]
		public void CreateDuplicateUser()
		{
			string userName = "CreateDuplicateUser";

			CreateUser(userName);

			bool hadDuplicateException = false;

			try
			{
				CreateUser(userName);
			}
			catch (DuplicateRecordException)
			{
				hadDuplicateException = true;
			}

			Assert.True(hadDuplicateException);
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

			CreateUser("GetMultipleUsersByName_DontGetThis");

			var users = _userDbController.Get(userNames);

			var matchingUsers = users.Select(g => userNames.Contains(g.Name));

			Assert.Equal(matchingUsers.Count(), userNames.Length);
		}

		[Fact]
		public void GetMultipleUsersById()
		{
			string[] userNames = new[]
			{
				"GetMultipleUsersById1",
				"GetMultipleUsersById2",
				"GetMultipleUsersById3",
				"GetMultipleUsersById4",
			};

			List<int> userIds = new List<int>();

			foreach (var userName in userNames)
			{
				var user = CreateUser(userName);
				userIds.Add(user.Id);
			}

			CreateUser("GetMultipleUsersById_DontGetThis");

			var users = _userDbController.Get(userIds.ToArray());

			var matchingUsers = users.Select(u => userIds.Contains(u.Id));

			Assert.Equal(matchingUsers.Count(), userIds.Count);
		}

		[Fact]
		public void GetNonExistingUsers()
		{
			var users = _userDbController.Get(new string[] { "GetNonExsitingUsers" });

			Assert.Empty(users);
		}

		[Fact]
		public void DeleteExistingUser()
		{
			string userName = "DeleteExistingUser";

			var user = CreateUser(userName);

			var users = _userDbController.Get(new string[] { userName });
			Assert.Equal(users.Count(), 1);
			Assert.Equal(users.ElementAt(0).Name, userName);

			_userDbController.Delete(new[] { user.Id });
			users = _userDbController.Get(new string[] { userName });

			Assert.Empty(users);
		}

		[Fact]
		public void DeleteNonExistingUser()
		{
			bool hadException = false;

			try
			{
				_userDbController.Delete(new int[] { -1 });
			}
			catch (Exception)
			{
				hadException = true;
			}

			Assert.False(hadException);
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

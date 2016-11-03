using System.Linq;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;
using PlayGen.SUGAR.Data.Model;
using Xunit;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
    [Collection("Project Fixture Collection")]
    public class AccountControllerTests
	{
        #region Configuration
	    private readonly AccountController _accountController = ControllerLocator.AccountController;
	    private readonly UserController _userController = ControllerLocator.UserController;
        #endregion

        #region Tests
        [Fact]
		public void CreateAndGetAccount()
		{
			var name = "CreateAndGetAccount";
			string password = $"{name}Password";

			CreateAccount(name, password);

			var accounts = _accountController.Get(new string[] { name });

			var matches = accounts.Count(a => a.Name == name);

			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateDuplicateAccount()
		{
			var name = "CreateDuplicateAccount";
			string password = $"{name}Password";

			CreateAccount(name, password);

			Assert.Throws<DuplicateRecordException>(() => CreateAccount(name, password));
		}

		[Fact]
		public void GetMultipleAccountsByName()
		{
			var names = new[]
			{
				"GetMultipleAccountsByName1",
				"GetMultipleAccountsByName2",
				"GetMultipleAccountsByName3",
				"GetMultipleAccountsByName4",
			};

			foreach (var name in names)
			{
				CreateAccount(name, $"{name}Password");
			}

			CreateAccount("GetMultipleAccountsByName_DontGetThis", "GetMultipleAccountsByName_DontGetThisPassword");

			var accounts = _accountController.Get(names);

			var matchingAccounts = accounts.Select(a => names.Contains(a.Name));

			Assert.Equal(names.Length, matchingAccounts.Count());
		}

		[Fact]
		public void GetNonExistingAccounts()
		{
			var accounts = _accountController.Get(new string[] { "GetNonExsitingAccounts" });

			Assert.Empty(accounts);
		}

		[Fact]
		public void DeleteExistingAccount()
		{
			var name = "DeleteExistingAccount";
			string password = $"{name}Password";

			var account = CreateAccount(name, password);

			var accounts = _accountController.Get(new string[] { name });
			Assert.Equal(accounts.Count(), 1);
			Assert.Equal(accounts.ElementAt(0).Name, name);

			_accountController.Delete(account.Id);
			accounts = _accountController.Get(new string[] { name });

			Assert.Empty(accounts);
		}

		[Fact]
		public void DeleteNonExistingAccount()
		{
			Assert.Throws<MissingRecordException>(() => _accountController.Delete(-1));
		}
		#endregion

		#region Helpers
		private Account CreateAccount(string name, string password)
		{
			var user = CreateUser(name);

			var account = new Account
			{
				Name = name,
				Password = password,
				UserId = user.Id,
				User = user
			};
			
			return _accountController.Create(account);
		}

		private User CreateUser(string name)
		{
			var user = new User()
			{
				Name = name,
			};

			_userController.Create(user);

			return user;
		}
		#endregion
	}
}
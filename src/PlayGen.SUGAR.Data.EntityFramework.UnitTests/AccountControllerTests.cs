using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SUGAR.Data.EntityFramework.Controllers;
using PlayGen.SUGAR.Data.Model;
using Xunit;
using PlayGen.SUGAR.Data.EntityFramework.Exceptions;

namespace PlayGen.SUGAR.Data.EntityFramework.UnitTests
{
	public class AccountControllerTests : TestController
	{
		#region Configuration

		private readonly AccountController _accountDbController;
		private readonly UserController _userDbController; 

		public AccountControllerTests()
		{
			_accountDbController = new AccountController(NameOrConnectionString);
			_userDbController = new UserController(NameOrConnectionString);
		}

		#endregion

		#region Tests

		[Fact]
		public void CreateAndGetAccount()
		{
			const string name = "CreateAndGetAccount";
			var password = $"{name}Password";
			CreateAccount(name, password);
			var accounts = _accountDbController.Get(new string[] { name });

			var matches = accounts.Count(a => a.Name == name);
			Assert.Equal(1, matches);
		}

		[Fact]
		public void CreateDuplicateAccount()
		{
			const string name = "CreateDuplicateAccount";
			var password = $"{name}Password";
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

			var accounts = _accountDbController.Get(names);

			var matchingAccounts = accounts.Select(a => names.Contains(a.Name));

			Assert.Equal(names.Length, matchingAccounts.Count());
		}

		[Fact]
		public void GetNonExistingAccounts()
		{
			var accounts = _accountDbController.Get(new string[] { "GetNonExsitingAccounts" });

			Assert.Empty(accounts);
		}

		[Fact]
		public void DeleteExistingAccount()
		{
			const string name = "DeleteExistingAccount";
			var password = $"{name}Password";

			var account = CreateAccount(name, password);

			var accounts = _accountDbController.Get(new string[] { name });
			Assert.NotNull(accounts);
			Assert.Equal(accounts.Count(), 1);
			Assert.Equal(accounts.ElementAt(0).Name, name);

			_accountDbController.Delete(account.Id);
			accounts = _accountDbController.Get(new string[] { name });

			Assert.Empty(accounts);
		}

		[Fact]
		public void DeleteNonExistingAccount()
		{
			//TODO: make exception type specific
			Assert.Throws<Exception>(() => _accountDbController.Delete(-1));
		}
		#endregion

		#region Helpers
		private Account CreateAccount(string name, string password)
		{
			var user = CreateUser(name);

			var account = new Account
			{
				Name = name,
				PasswordHash = password,
				UserId = user.Id,
				User = user,
			};
			
			return _accountDbController.Create(account);
		}

		private User CreateUser(string name)
		{
			var user = new User()
			{
				Name = name,
			};

			_userDbController.Create(user);

			return user;
		}
		#endregion
	}
}

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
		private readonly AccountSourceController _accountSourceController = ControllerLocator.AccountSourceController;
		private readonly UserController _userController = ControllerLocator.UserController;
		#endregion

		#region Tests
		[Fact]
		public void CreateAndGetAccount()
		{
			var name = "CreateAndGetAccount";
			string password = $"{name}Password";

			var source = CreateAccountSource(name);
			CreateAccount(name, password, source.Id);

			Account account;
			Assert.True(_accountController.TryGet(name, source.Id, out account));
		}

		[Fact]
		public void CreateDuplicateAccount()
		{
			var name = "CreateDuplicateAccount";
			string password = $"{name}Password";

			var source = CreateAccountSource(name);
			CreateAccount(name, password, source.Id);

			Assert.Throws<DuplicateRecordException>(() => CreateAccount(name, password, source.Id));
		}

		[Fact]
		public void GetAccountByName()
		{
			var name = "GetAccountsByName";

			var source = CreateAccountSource("GetMultipleAccountsByName");

			CreateAccount(name, $"{name}Password", source.Id);

			CreateAccount("GetMultipleAccountsByName_DontGetThis", "GetMultipleAccountsByName_DontGetThisPassword", source.Id);

			Account account;
				
			Assert.True(_accountController.TryGet(name, source.Id, out account));
		}

		[Fact]
		public void GetNonExistingAccounts()
		{
			var source = CreateAccountSource("GetNonExistingAccounts");

			Account account;

			Assert.False(_accountController.TryGet("GetNonExsitingAccounts", source.Id, out account));
		}

		[Fact]
		public void DeleteExistingAccount()
		{
			var name = "DeleteExistingAccount";
			string password = $"{name}Password";

			var source = CreateAccountSource(name);

			Account account = CreateAccount(name, password, source.Id);

			var found = _accountController.TryGet(name, source.Id, out account);

			Assert.True(found);

			_accountController.Delete(account.Id);
			Assert.False(_accountController.TryGet(name, source.Id, out account));

		}

		[Fact]
		public void DeleteNonExistingAccount()
		{
			Assert.Throws<MissingRecordException>(() => _accountController.Delete(-1));
		}
		#endregion

		#region Helpers
		private Account CreateAccount(string name, string password, int sourceId)
		{
			var user = CreateUser(name);

			var account = new Account
			{
				Name = name,
				Password = password,
				UserId = user.Id,
				User = user,
				AccountSourceId = sourceId
			};
			
			return _accountController.Create(account);
		}

		private AccountSource CreateAccountSource(string name)
		{
			var source = new AccountSource
			{
				Description = name,
				Token = name,
				RequiresPassword = true,
			};

			return _accountSourceController.Create(source);
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
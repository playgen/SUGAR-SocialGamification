using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using Xunit;
using PlayGen.SGA.DataController.Exceptions;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class AccountDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly AccountDbController _accountDbController;

        public AccountDbControllerTests()
        {
            _accountDbController = new AccountDbController(_nameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetAccount()
        {
            string name = "CreateAndGetAccount";
            string password = $"{name}Password";
            Account.Permissions permissions = Account.Permissions.Default;

            var account = CreateAccount(name, password, permissions);

            var accounts = _accountDbController.Get(new string[] { name });

            int matches = accounts.Count(a => a.Name == name);

            Assert.Equal(1, matches);
        }

        [Fact]
        public void CreateDuplicateAccount()
        {
            string name = "CreateDuplicateAccount";
            string password = $"{name}Password";
            Account.Permissions permissions = Account.Permissions.Default;

            CreateAccount(name, password, permissions);

            bool hadDuplicateException = false;

            try
            {
                CreateAccount(name, password, permissions);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void GetMultipleAccounts()
        {
            string[] names = new[]
            {
                "GetMultipleAccounts1",
                "GetMultipleAccounts2",
                "GetMultipleAccounts3",
                "GetMultipleAccounts4",
            };

            foreach (var name in names)
            {
                CreateAccount(name, $"{name}Password", Account.Permissions.Default);
            }

            CreateAccount("GetMultipleAccounts_DontGetThis", "GetMultipleAccounts_DontGetThisPassword", Account.Permissions.Default);

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
            string name = "DeleteExistingAccount";
            string password = $"{name}Password";
            Account.Permissions permissions = Account.Permissions.Default;

            var account = CreateAccount(name, password, permissions);

            var accounts = _accountDbController.Get(new string[] { name });
            Assert.Equal(accounts.Count(), 1);
            Assert.Equal(accounts.ElementAt(0).Name, name);

            _accountDbController.Delete(new[] { account.Id });
            accounts = _accountDbController.Get(new string[] { name });

            Assert.Empty(accounts);
        }

        [Fact]
        public void DeleteNonExistingAccount()
        {
            bool hadExeption = false;

            try
            {
                _accountDbController.Delete(new int[] { -1 });
            }
            catch (Exception)
            {
                hadExeption = true;
            }

            Assert.False(hadExeption);
        }
        #endregion

        #region Helpers
        private Account CreateAccount(string name, string password, Account.Permissions permission)
        {
            var account = new Account
            {
                Name = name,
                Password = password,
                Permission = permission,
            };

            return _accountDbController.Create(account);
        }
        #endregion
    }
}

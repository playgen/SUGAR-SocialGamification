using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataModel;
using Xunit;

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
        public void CreateAndGetGame()
        {
            string name = "Bob";
            string password = "bobber";
            Account.Permissions permissions = Account.Permissions.Default;

            var account = CreateAccount(name, password, permissions);

            var accounts = _accountDbController.Get(new string[] { name });

            int matches = accounts.Count(a => a.Name == name);

            Assert.Equal(1, matches);
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

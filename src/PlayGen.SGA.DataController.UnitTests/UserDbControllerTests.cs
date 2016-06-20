using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class UserDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly UserDbController _userDbController;

        public UserDbControllerTests()
        {
            _userDbController = new UserDbController(NameOrConnectionString);
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
        public void GetMultipleUsers()
        {
            string[] userNames = new[]
            {
                "GetMultipleUsers1",
                "GetMultipleUsers2",
                "GetMultipleUsers3",
                "GetMultipleUsers4",
            };

            foreach (var userName in userNames)
            {
                CreateUser(userName);
            }

            CreateUser("GetMultipleUsers_DontGetThis");

            var users = _userDbController.Get(userNames);

            var matchingUsers = users.Select(g => userNames.Contains(g.Name));

            Assert.Equal(matchingUsers.Count(), userNames.Length);
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
            var newUser = new User
            {
                Name = name,
            };

            return _userDbController.Create(newUser);
        }
        #endregion
    }
}

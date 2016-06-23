using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlayGen.SGA.DataController.Exceptions;
using PlayGen.SGA.DataModel;
using Xunit;

namespace PlayGen.SGA.DataController.UnitTests
{
    public class UserFriendDbControllerTests : TestDbController
    {
        #region Configuration
        private readonly UserFriendDbController _userFriendDbController;

        public UserFriendDbControllerTests()
        {
            _userFriendDbController = new UserFriendDbController(NameOrConnectionString);
        }
        #endregion


        #region Tests
        [Fact]
        public void CreateAndGetUserFriendRequest()
        {
            string userFriendName = "CreateUserFriendRequest";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            var userRequests = _userFriendDbController.GetRequests(newFriend.AcceptorId);

            int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void CreateUserFriendWithNonExistingRequestor()
        {
            string userFriendName = "CreateUserFriendWithNonExistingRequestor";
            var acceptor = CreateUser(userFriendName);
            bool hadException = false;

            try
            {
                CreateUserFriend(-1, acceptor.Id);
            }
            catch (MissingRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateUserFriendWithNonExistingAcceptor()
        {
            string userFriendName = "CreateUserFriendWithNonExistingAcceptor";
            var requestor = CreateUser(userFriendName);
            bool hadException = false;

            try
            {
                CreateUserFriend(requestor.Id, -1);
            }
            catch (MissingRecordException)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void CreateDuplicateUserFriend()
        {
            string userFriendName = "CreateDuplicateUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            bool hadDuplicateException = false;

            try
            {
                CreateUserFriend(requestor.Id, acceptor.Id);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void CreateDuplicateReversedUserFriend()
        {
            string userFriendName = "CreateDuplicateReversedUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            bool hadDuplicateException = false;

            try
            {
                CreateUserFriend(acceptor.Id, requestor.Id);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void GetNonExistingUserFriendRequests()
        {
            var userFriends = _userFriendDbController.GetRequests(-1);

            Assert.Empty(userFriends);
        }

        [Fact]
        public void AcceptUserFriendRequest()
        {
            string userFriendName = "AcceptUserFriendRequest";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            _userFriendDbController.UpdateRequest(newFriend, true);

            var userRequests = _userFriendDbController.GetRequests(newFriend.AcceptorId);

            int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

            Assert.Equal(matches, 0);

            var userFriends = _userFriendDbController.GetFriends(newFriend.RequestorId);

            matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

            Assert.Equal(matches, 1);
        }

        [Fact]
        public void RejectUserFriendRequest()
        {
            string userFriendName = "RejectUserFriendRequest";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            _userFriendDbController.UpdateRequest(newFriend, false);

            var userRequests = _userFriendDbController.GetRequests(newFriend.AcceptorId);

            int matches = userRequests.Count(g => g.Name == userFriendName + " Requestor");

            Assert.Equal(matches, 0);

            var userFriends = _userFriendDbController.GetFriends(newFriend.RequestorId);

            matches = userFriends.Count(g => g.Name == userFriendName + " Acceptor");

            Assert.Equal(matches, 0);
        }

        [Fact]
        public void UpdateNonExistingUserFriendRequest()
        {
            string userFriendName = "UpdateNonExistingUserFriendRequest";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = new UserToUserRelationship
            {
                RequestorId = requestor.Id,
                AcceptorId = acceptor.Id
            };

            bool hadException = false;

            try
            {
                _userFriendDbController.UpdateRequest(newFriend, true);
            }
            catch (Exception)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }

        [Fact]
        public void GetNonExistingUserFriends()
        {
            var userFriends = _userFriendDbController.GetFriends(-1);

            Assert.Empty(userFriends);
        }

        [Fact]
        public void CreateDuplicateAcceptedUserFriend()
        {
            string userFriendName = "CreateDuplicateAcceptedUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            _userFriendDbController.UpdateRequest(newFriend, true);

            bool hadDuplicateException = false;

            try
            {
                CreateUserFriend(requestor.Id, acceptor.Id);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void CreateDuplicateReversedAcceptedUserFriend()
        {
            string userFriendName = "CreateDuplicateReversedAcceptedUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            _userFriendDbController.UpdateRequest(newFriend, true);

            bool hadDuplicateException = false;

            try
            {
                CreateUserFriend(acceptor.Id, requestor.Id);
            }
            catch (DuplicateRecordException)
            {
                hadDuplicateException = true;
            }

            Assert.True(hadDuplicateException);
        }

        [Fact]
        public void UpdateUserFriend()
        {
            string userFriendName = "UpdateUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = CreateUserFriend(requestor.Id, acceptor.Id);

            _userFriendDbController.UpdateRequest(newFriend, true);

            _userFriendDbController.Update(newFriend);
            var friends = _userFriendDbController.GetFriends(acceptor.Id);

            Assert.Empty(friends);
        }

        [Fact]
        public void UpdateNonExistingUserFriend()
        {
            string userFriendName = "UpdateNonExistingUserFriend";

            var requestor = CreateUser(userFriendName + " Requestor");
            var acceptor = CreateUser(userFriendName + " Acceptor");

            var newFriend = new UserToUserRelationship
            {
                RequestorId = requestor.Id,
                AcceptorId = acceptor.Id
            };

            bool hadException = false;

            try
            {
                _userFriendDbController.Update(newFriend);
            }
            catch (Exception)
            {
                hadException = true;
            }

            Assert.True(hadException);
        }
        #endregion

        #region Helpers
        private User CreateUser(string name)
        {
            UserDbController userDbController = new UserDbController(NameOrConnectionString);
            var user = new User
            {
                Name = name,
            };

            userDbController.Create(user);

            return user;
        }

        private UserToUserRelationship CreateUserFriend(int requestor, int acceptor)
        {
            var userFriend = new UserToUserRelationship
            {
                RequestorId = requestor,
                AcceptorId = acceptor
            };
            _userFriendDbController.Create(userFriend, false);

            return userFriend;
        }
        #endregion
    }
}
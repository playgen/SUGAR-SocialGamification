using System.Collections.Generic;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserFriendController
    {
        private readonly Data.EntityFramework.Controllers.UserRelationshipController _userRelationshipDbController;

        public UserFriendController(Data.EntityFramework.Controllers.UserRelationshipController userRelationshipDbController)
        {
            _userRelationshipDbController = userRelationshipDbController;
        }
        
        public IEnumerable<User> GetFriendRequests(int userId)
        {
            var requestors = _userRelationshipDbController.GetRequests(userId);
            return requestors;
        }
        
        public IEnumerable<User> GetSentRequests(int userId)
        {
            var requests = _userRelationshipDbController.GetSentRequests(userId);
            return requests;
        }
        
        public IEnumerable<User> GetFriends(int userId)
        {
            var friends = _userRelationshipDbController.GetFriends(userId);
            return friends;
        }
        
        public void CreateFriendRequest(UserToUserRelationship newRelationship, bool autoAccept)
        {
            _userRelationshipDbController.Create(newRelationship, autoAccept);
        }
        
        public void UpdateFriendRequest(UserToUserRelationship relationship, bool accepted)
        {
            _userRelationshipDbController.UpdateRequest(relationship, accepted);
        }
        
        public void UpdateFriend(UserToUserRelationship relationship)
        {
            _userRelationshipDbController.Update(relationship);
        }
    }
}
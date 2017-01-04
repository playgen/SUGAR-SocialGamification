using System.Collections.Generic;
using NLog;
using PlayGen.SUGAR.Data.Model;

namespace PlayGen.SUGAR.Core.Controllers
{
    public class UserFriendController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Data.EntityFramework.Controllers.UserRelationshipController _userRelationshipDbController;

        public UserFriendController(Data.EntityFramework.Controllers.UserRelationshipController userRelationshipDbController)
        {
            _userRelationshipDbController = userRelationshipDbController;
        }
        
        public List<User> GetFriendRequests(int userId)
        {
            var requestors = _userRelationshipDbController.GetRequests(userId);

            Logger.Info($"{requestors.Count} Requestors for UserId: {userId}");

            return requestors;
        }
        
        public List<User> GetSentRequests(int userId)
        {
            var requests = _userRelationshipDbController.GetSentRequests(userId);

            Logger.Info($"{requests.Count} Requests for UserId: {userId}");

            return requests;
        }
        
        public List<User> GetFriends(int userId)
        {
            var friends = _userRelationshipDbController.GetFriends(userId);

            Logger.Info($"{friends.Count} Friends for UserId: {userId}");

            return friends;
        }
        
        public void CreateFriendRequest(UserToUserRelationship newRelationship, bool autoAccept)
        {
            _userRelationshipDbController.Create(newRelationship, autoAccept);

            Logger.Info($"{newRelationship.RequestorId} -> {newRelationship.AcceptorId}, AutoAccept: {autoAccept}");
        }
        
        public void UpdateFriendRequest(UserToUserRelationship relationship, bool accepted)
        {
            _userRelationshipDbController.UpdateRequest(relationship, accepted);

            Logger.Info($"{relationship.RequestorId} -> {relationship.AcceptorId}, Accepted: {accepted}");
        }
        
        public void UpdateFriend(UserToUserRelationship relationship)
        {
            _userRelationshipDbController.Update(relationship);

            Logger.Info($"{relationship.RequestorId} -> {relationship.AcceptorId}");
        }
    }
}
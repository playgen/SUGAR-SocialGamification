using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserFriendController
    {
        RelationshipResponse CreateFriendRequest(RelationshipRequest relationship);

        IEnumerable<ActorResponse> GetFriendRequests(int userId);

        void UpdateFriendRequest(RelationshipStatusUpdate relationship);

        IEnumerable<ActorResponse> GetFriends(int userId);

        void UpdateFriend(RelationshipStatusUpdate relationship);
    }
}
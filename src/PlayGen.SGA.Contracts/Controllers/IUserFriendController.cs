using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserFriendController
    {
        void CreateFriendRequest(int requestorId, int acceptorId);

        IEnumerable<Actor> GetFriendRequests(int userId);

        void UpdateFriendRequest(int acceptorId, Relationship relationship);

        IEnumerable<Actor> GetFriends(int userId);

        void UpdateFriend(int userId, Relationship relationship);
    }
}
using System.Collections.Generic;

namespace PlayGen.SGA.Contracts.Controllers
{
    public interface IUserFriendController
    {
        void CreateFriendRequest(Relationship relationship);

        IEnumerable<Actor> GetFriendRequests(int userId);

        void UpdateFriendRequest(Relationship relationship);

        IEnumerable<Actor> GetFriends(int userId);

        void UpdateFriend(Relationship relationship);
    }
}
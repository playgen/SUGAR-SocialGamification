using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    public class UserFriendClientProxy : ClientProxy, IUserFriendController
    {
        public void CreateFriendRequest(Relationship relationship)
        {
            var query = GetUriBuilder("api/userfriend").ToString();
            Post<Relationship, int>(query, relationship);
        }

        public IEnumerable<Actor> GetFriendRequests(int userId)
        {
            var query = GetUriBuilder("api/userfriend/requests")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public void UpdateFriendRequest(Relationship relationship)
        {
            var query = GetUriBuilder("api/userfriend/request").ToString();
            Put<Relationship, int>(query, relationship);
        }

        public IEnumerable<Actor> GetFriends(int userId)
        {
            var query = GetUriBuilder("api/userfriend/friends")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<Actor>>(query);
        }

        public void UpdateFriend(Relationship relationship)
        {
            var query = GetUriBuilder("api/userfriend/").ToString();
            Put<Relationship, int>(query, relationship);
        }
    }
}

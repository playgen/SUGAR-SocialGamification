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
        public RelationshipResponse CreateFriendRequest(RelationshipRequest relationship)
        {
            var query = GetUriBuilder("api/userfriend").ToString();
            return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
        }

        public IEnumerable<ActorResponse> GetFriendRequests(int userId)
        {
            var query = GetUriBuilder("api/userfriend/requests")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        public void UpdateFriendRequest(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/userfriend/request").ToString();
            Put(query, relationship);
        }

        public IEnumerable<ActorResponse> GetFriends(int userId)
        {
            var query = GetUriBuilder("api/userfriend/friends")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        public void UpdateFriend(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/userfriend/").ToString();
            Put(query, relationship);
        }
    }
}

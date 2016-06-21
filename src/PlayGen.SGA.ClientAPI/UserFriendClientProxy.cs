using System;
using System.Collections.Generic;
using System.Linq;
using PlayGen.SGA.ClientAPI.Extensions;
using PlayGen.SGA.Contracts;
using PlayGen.SGA.Contracts.Controllers;

namespace PlayGen.SGA.ClientAPI
{
    /// <summary>
    /// Controller that facilitates User to User relationship specific operations.
    /// </summary>
    public class UserFriendClientProxy : ClientProxyBase, IUserFriendController
    {
        public UserFriendClientProxy(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Get a list of all Users that have relationship requests for this <param name="userId"/>.
        /// </summary>
        /// <param name="userId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> GetFriendRequests(int userId)
        {
            var query = GetUriBuilder("api/userfriend/requests")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Get a list of all Users that have relationships with this <param name="userId"/>.
        /// </summary>
        /// <param name="userId">ID of the group.</param>
        /// <returns>A list of <see cref="ActorResponse"/> which match the search criteria.</returns>
        public IEnumerable<ActorResponse> GetFriends(int userId)
        {
            var query = GetUriBuilder("api/userfriend/friends")
                .AppendQueryParameters(new int[] { userId }, "userId={0}")
                .ToString();
            return Get<IEnumerable<ActorResponse>>(query);
        }

        /// <summary>
        /// Create a new relationship request between two Users.
        /// Requires a relationship between the two to not already exist.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipRequest"/> object that holds the details of the new relationship request.</param>
        /// <returns>A <see cref="RelationshipResponse"/> containing the new Relationship details.</returns>
        public RelationshipResponse CreateFriendRequest(RelationshipRequest relationship)
        {
            var query = GetUriBuilder("api/userfriend").ToString();
            return Post<RelationshipRequest, RelationshipResponse>(query, relationship);
        }

        /// <summary>
        /// Update an existing relationship request between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
        /// Requires the relationship request to already exist between the two Users.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        public void UpdateFriendRequest(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/userfriend/request").ToString();
            Put(query, relationship);
        }

        /// <summary>
        /// Update an existing relationship between <param name="relationship.RequestorId"/> and <param name="relationship.AcceptorId"/>.
        /// Requires the relationship to already exist between the two Users.
        /// </summary>
        /// <param name="relationship"><see cref="RelationshipStatusUpdate"/> object that holds the details of the relationship.</param>
        public void UpdateFriend(RelationshipStatusUpdate relationship)
        {
            var query = GetUriBuilder("api/userfriend/").ToString();
            Put(query, relationship);
        }
    }
}
